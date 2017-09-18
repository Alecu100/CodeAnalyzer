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
            var assembliesQueue = new List<string>();
            var shouldStillTryToLoadAssemblies = true;

            assemblyNames.Sort();
            searchDirectories = searchDirectories.Distinct().ToList();
            assemblyNames = assemblyNames.Distinct().ToList();

            foreach (var assemblyName in assemblyNames)
            {
                assembliesQueue.Add(assemblyName);
            }

            while (assembliesQueue.Count > 0 && shouldStillTryToLoadAssemblies)
            {
                shouldStillTryToLoadAssemblies = false;

                var assembliesToRemove = new List<string>();

                foreach (var assembly in assembliesQueue)
                {
                    if (TryToLoadAssembly(searchDirectories, assembly))
                    {
                        shouldStillTryToLoadAssemblies = true;

                        assembliesToRemove.Add(assembly);
                    }
                }

                foreach (var assemblyToRemove in assembliesToRemove)
                {
                    assembliesQueue.Remove(assemblyToRemove);
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
                    searchDirectories.Remove(searchDirectory);
                    searchDirectories.Insert(0, searchDirectory);
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
                    }
                }

                var directoryInfos = directoryInfo.GetDirectories();

                foreach (var newDirectryInfo in directoryInfos)
                {
                    directoriesQueue.Enqueue(newDirectryInfo);
                }
            }

            return false;
        }
    }
}