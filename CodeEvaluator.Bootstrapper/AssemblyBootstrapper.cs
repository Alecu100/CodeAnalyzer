using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace CodeEvaluator.Bootstrapper
{
    public class AssemblyBootstrapper : MarshalByRefObject
    {
        public void LoadAssemblies(List<string> searchDirectories, List<string> assemblyNames)
        {
            foreach (var assemblyName in assemblyNames)
            {
                TryToLoadAssembly(searchDirectories, assemblyName);
            }

            var assembliesQueue = new Queue<string>();
            var shouldStillTryToLoadAssemblies = true;

            foreach (var assemblyName in assemblyNames)
            {
                assembliesQueue.Enqueue(assemblyName);
            }

            while (assembliesQueue.Count > 0 && shouldStillTryToLoadAssemblies)
            {
                shouldStillTryToLoadAssemblies = false;

                var assemblyName = assembliesQueue.Dequeue();

                if (TryToLoadAssembly(searchDirectories, assemblyName))
                {
                    shouldStillTryToLoadAssemblies = true;
                }
                else
                {
                    assembliesQueue.Enqueue(assemblyName);
                }
            }
        }

        private bool TryToLoadAssembly(List<string> searchDirectories, string assemblyName)
        {
            if (!assemblyName.EndsWith(".dll", StringComparison.InvariantCultureIgnoreCase))
            {
                assemblyName = assemblyName + ".dll";
            }

            foreach (var searchDirectory in searchDirectories)
            {
                if (TryToLoadAssemblyFromDirectory(searchDirectory, assemblyName))
                {
                    return true;
                }
            }

            return false;
        }

        private bool TryToLoadAssemblyFromDirectory(string searchDirectory, string assemblyName)
        {
            var startDirectory = new DirectoryInfo(searchDirectory);
            var directoriesQueue = new Queue<DirectoryInfo>();
            directoriesQueue.Enqueue(startDirectory);

            while (directoriesQueue.Count > 0)
            {
                var directoryInfo = directoriesQueue.Dequeue();
                var allFiles = directoryInfo.GetFiles();

                var assemblyFile = allFiles.FirstOrDefault(file => string.Equals(file.Name, assemblyName));

                if (assemblyFile != null)
                {
                    try
                    {
                        Assembly.LoadFile(assemblyFile.FullName);
                        return true;
                    }
                    catch (Exception)
                    {
                        var directoryInfos = directoryInfo.GetDirectories();

                        foreach (var newDirectryInfo in directoryInfos)
                        {
                            directoriesQueue.Enqueue(newDirectryInfo);
                        }
                    }
                }
            }

            return false;
        }
    }
}