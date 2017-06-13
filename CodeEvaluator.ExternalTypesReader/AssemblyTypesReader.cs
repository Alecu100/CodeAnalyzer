using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using CodeAnalysis.Core.Interfaces;
using CodeAnalysis.Core.Members;
using CodeEvaluator.Bootstrapper;

namespace CodeEvaluator.ExternalTypesReader
{
    public class AssemblyTypesReader : MarshalByRefObject, IAssemblyTypesReader
    {
        public List<EvaluatedTypeInfo> ReadTypeInfos(IList<string> assemblyFileNames)
        {
            var domaininfo = new AppDomainSetup();
            var assemblyLocation =
                typeof(AssemblyBootstrapper).Assembly.Location.Replace(typeof(AssemblyBootstrapper).Assembly.GetName().Name + ".dll", "");

            domaininfo.ApplicationBase = assemblyLocation;

            var assembliesToLoad = new List<string>();

            var referencedAssemblies = GetType().Assembly.GetReferencedAssemblies();

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
                    remoteAppDomain.CreateInstanceAndUnwrap(typeof(AssemblyBootstrapper).Assembly.FullName, typeof(AssemblyBootstrapper).FullName);

            remoteAssemblyResolver.LoadAssemblies(new List<string> {assemblyLocation, Environment.CurrentDirectory},
                assembliesToLoad);

            var instanceFrom =
                (AssemblyTypesReader)
                    remoteAppDomain.CreateInstanceAndUnwrap(GetType().Assembly.FullName, GetType().FullName);


            var readTypeInfosRemote = instanceFrom.ReadTypeInfosRemote(
                assemblyFileNames);

            AppDomain.Unload(remoteAppDomain);

            AppDomain.CurrentDomain.AssemblyResolve -= AssemblyResolver.AppDomain_AssemblyResolve;

            return readTypeInfosRemote;
        }

        public
            List<EvaluatedTypeInfo> ReadTypeInfosRemote
            (IList<string>
                assemblyFileNames)
        {
            var evaluatedTypeInfos = new List<EvaluatedTypeInfo>();

            return evaluatedTypeInfos;
        }
    }
}