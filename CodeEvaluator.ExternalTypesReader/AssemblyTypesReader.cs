using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using CodeAnalysis.Core.Interfaces;
using CodeAnalysis.Core.Members;

namespace CodeEvaluator.ExternalTypesReader
{
    public class AssemblyTypesReader : MarshalByRefObject, IAssemblyTypesReader
    {
        public List<EvaluatedTypeInfo> ReadTypeInfos(IList<string> assemblyFileNames)
        {
            var domaininfo = new AppDomainSetup();
            var assemblyLocation = GetType().Assembly.Location.Replace(GetType().Assembly.GetName().Name + ".dll", "");

            domaininfo.ApplicationBase = assemblyLocation;

            var adEvidence = AppDomain.CurrentDomain.Evidence;

            var remoteAppDomain = AppDomain.CreateDomain(Guid.NewGuid().ToString(), adEvidence, domaininfo);

            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
            remoteAppDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;

            var instanceFrom =
                (AssemblyTypesReader)
                    remoteAppDomain.CreateInstanceAndUnwrap(GetType().Assembly.FullName, GetType().FullName);


            var readTypeInfosRemote = instanceFrom.ReadTypeInfosRemote(
                assemblyFileNames);

            AppDomain.Unload(remoteAppDomain);

            AppDomain.CurrentDomain.AssemblyResolve -= CurrentDomain_AssemblyResolve;
            remoteAppDomain.AssemblyResolve -= CurrentDomain_AssemblyResolve;

            return readTypeInfosRemote;
        }

        private Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            try
            {
                var assembly = Assembly.Load(args.Name);
                if (assembly != null)
                    return assembly;
            }
            catch
            {
                // ignore load error }

                // *** Try to load by filename - split out the filename of the full assembly name
                // *** and append the base path of the original assembly (ie. look in the same dir)
                // *** NOTE: this doesn't account for special search paths but then that never
                //           worked before either.
                var Parts = args.Name.Split(',');
                var File = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\" + Parts[0].Trim() +
                           ".dll";

                return Assembly.LoadFrom(File);
            }

            return null;
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