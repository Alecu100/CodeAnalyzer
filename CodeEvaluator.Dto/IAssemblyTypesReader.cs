using System.Collections.Generic;

namespace CodeEvaluator.Dto
{
    public interface IAssemblyTypesReader
    {
        List<EvaluatedTypeInfoDto> ReadTypeInfos(List<string> assemblyFileNames);
    }
}