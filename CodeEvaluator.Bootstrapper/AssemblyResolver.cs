using System;
using System.IO;
using System.Reflection;

namespace CodeEvaluator.Bootstrapper
{
    public static class AssemblyResolver
    {
        public static Assembly AppDomain_AssemblyResolve(object sender, ResolveEventArgs args)
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
    }
}