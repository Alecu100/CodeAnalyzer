namespace CodeEvaluator.Evaluation.Interfaces
{
    public interface IKeywordToTypeInfoRemapper
    {
        string ObjectTypeName { get; }

        string ShortTypeName { get; }

        string IntTypeName { get; }

        string LongTypeName { get; }

        string UShortTypeName { get; }

        string UIntTypeName { get; }

        string ULongTypeName { get; }

        string StringTypeName { get; }

        string BoolTypeName { get; }

        string FloatTypeName { get; }

        string DoubleTypeName { get; }

        string DecimalTypeName { get; }

        string ByteTypeName { get; }

        string SByteTypeName { get; }

        string CharTypeName { get; }

        bool IsKeywordTypeInfo(string keywordTypeInfo);

        string GetMappedTypeInfo(string keywordTypeInfo);
    }
}