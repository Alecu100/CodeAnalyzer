namespace CodeEvaluator.Evaluation.Common
{
    using System.Collections.Generic;

    using global::CodeEvaluator.Evaluation.Interfaces;

    public class KeywordToTypeInfoRemapper : IKeywordToTypeInfoRemapper
    {
        private readonly Dictionary<string, string> _keywordToTypeInfoMappings = new Dictionary<string, string>();

        public KeywordToTypeInfoRemapper()
        {
            _keywordToTypeInfoMappings["object"] = "System.Object";
            _keywordToTypeInfoMappings["short"] = "System.Int16";
            _keywordToTypeInfoMappings["int"] = "System.Int32";
            _keywordToTypeInfoMappings["long"] = "System.Int64";
            _keywordToTypeInfoMappings["ushort"] = "System.UInt16";
            _keywordToTypeInfoMappings["uint"] = "System.UInt32";
            _keywordToTypeInfoMappings["ulong"] = "System.UInt64";
            _keywordToTypeInfoMappings["string"] = "System.String";
            _keywordToTypeInfoMappings["bool"] = "System.Boolean";
            _keywordToTypeInfoMappings["float"] = "System.Single";
            _keywordToTypeInfoMappings["double"] = "System.Double";
            _keywordToTypeInfoMappings["decimal"] = "System.Decimal";
            _keywordToTypeInfoMappings["byte"] = "System.Byte";
            _keywordToTypeInfoMappings["sbyte"] = "System.SByte";
            _keywordToTypeInfoMappings["char"] = "System.Char";
        }

        public bool IsKeywordTypeInfo(string keywordTypeInfo)
        {
            return _keywordToTypeInfoMappings.ContainsKey(keywordTypeInfo);
        }

        public string GetMappedTypeInfo(string keywordTypeInfo)
        {
            return _keywordToTypeInfoMappings[keywordTypeInfo];
        }
    }
}