using System.Collections.Generic;
using CodeAnalysis.Core.Members;

namespace CodeAnalysis.Core.Interfaces
{
    public interface IAssemblyTypesReader
    {
        List<EvaluatedTypeInfo> ReadTypeInfos(IList<string> assemblyFileNames);
    }
}