using System;
using System.Collections.Generic;
using CodeAnalysis.Core.Interfaces;
using CodeAnalysis.Core.Members;
using CodeEvaluator.Bootstrapper;

namespace CodeEvaluator.ExternalTypesReader
{
    public class AssemblyTypesReader : MarshalByRefObject, IAssemblyTypesReader
    {
        public List<EvaluatedTypeInfo> ReadTypeInfos(List<string> assemblyFileNames)
        {
            var domaininfo = new AppDomainSetup();
            var assemblyLocation =
                typeof(AssemblyBootstrapper).Assembly.Location.Replace(
                    typeof(AssemblyBootstrapper).Assembly.GetName().Name + ".dll", "");

            domaininfo.ApplicationBase = assemblyLocation;
            domaininfo.ConfigurationFile = AppDomain.CurrentDomain.SetupInformation.ConfigurationFile;

            var assembliesToLoad = new List<string>();

            var referencedAssemblies = GetType().Assembly.GetReferencedAssemblies();

            foreach (var referencedAssembly in referencedAssemblies)
            {
                assembliesToLoad.Add(referencedAssembly.Name);
            }

            referencedAssemblies = typeof(CodeAnalysis.Core.Common.CodeEvaluator).Assembly.GetReferencedAssemblies();

            foreach (var referencedAssembly in referencedAssemblies)
            {
                assembliesToLoad.Add(referencedAssembly.Name);
            }

            var adEvidence = AppDomain.CurrentDomain.Evidence;

            var remoteAppDomain = AppDomain.CreateDomain(Guid.NewGuid().ToString(), adEvidence, domaininfo);

            try
            {
                remoteAppDomain.Load(typeof(AssemblyBootstrapper).Assembly.GetName());
            }
            catch (Exception)
            {
            }

            AppDomain.CurrentDomain.AssemblyResolve += AssemblyResolver.AppDomain_AssemblyResolve;

            var remoteAssemblyResolver =
                (AssemblyBootstrapper)
                    remoteAppDomain.CreateInstanceAndUnwrap(typeof(AssemblyBootstrapper).Assembly.FullName,
                        typeof(AssemblyBootstrapper).FullName);

            remoteAssemblyResolver.LoadAssemblies(new List<string> {assemblyLocation, AppDomain.CurrentDomain.BaseDirectory},
                assembliesToLoad);

            var instanceFrom =
                (AssemblyTypesReader)
                    remoteAppDomain.CreateInstanceAndUnwrap(GetType().Assembly.GetName().Name, GetType().FullName);

            var readTypeInfosRemote = instanceFrom.ReadTypeInfosRemote(
                assemblyFileNames);

            AppDomain.Unload(remoteAppDomain);

            AppDomain.CurrentDomain.AssemblyResolve -= AssemblyResolver.AppDomain_AssemblyResolve;

            return readTypeInfosRemote;
        }

        public
            List<EvaluatedTypeInfo> ReadTypeInfosRemote
            (List<string>
                assemblyFileNames)
        {
            var evaluatedTypeInfos = new List<EvaluatedTypeInfo>();

            var assemblyBootstrapper = new AssemblyBootstrapper();

            var assemblyDirectories = new List<string>();

            foreach (var assemblyFileName in assemblyFileNames)
            {
                var nameParts = assemblyFileName.Split('\\', '/');

                var assemblyDirectory = assemblyFileName.Replace(nameParts[nameParts.Length - 1], "");

                assemblyDirectories.Add(assemblyDirectory);
            }

            assemblyBootstrapper.LoadAssemblies(assemblyDirectories, assemblyFileNames);

            return evaluatedTypeInfos;
        }
    }
}