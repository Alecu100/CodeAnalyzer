namespace CodeEvaluator.Evaluation.Interfaces
{
    public interface IKeywordToTypeInfoRemapper
    {
        bool IsKeywordTypeInfo(string keywordTypeInfo);

        string GetMappedTypeInfo(string keywordTypeInfo);
    }
}