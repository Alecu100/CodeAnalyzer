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

        public string ObjectTypeName
        {
            get
            {
                return "System.Object";
            }
        }

        public string ShortTypeName
        {
            get
            {
                return "System.Int16";
            }
        }

        public string IntTypeName
        {
            get
            {
                return "System.Int32";
            }
        }

        public string LongTypeName
        {
            get
            {
                return "System.Int64";
            }
        }

        public string UShortTypeName
        {
            get
            {
                return "System.UInt16";
            }
        }

        public string UIntTypeName
        {
            get
            {
                return "System.UInt32";
            }
        }

        public string ULongTypeName
        {
            get
            {
                return "System.UInt64";
            }
        }

        public string StringTypeName
        {
            get
            {
                return "System.String";
            }
        }

        public string BoolTypeName
        {
            get
            {
                return "System.Boolean";
            }
        }

        public string FloatTypeName
        {
            get
            {
                return "System.Single";
            }
        }

        public string DoubleTypeName
        {
            get
            {
                return "System.Double";
            }
        }

        public string DecimalTypeName
        {
            get
            {
                return "System.Decimal";
            }
        }

        public string ByteTypeName
        {
            get
            {
                return "System.Byte";
            }
        }

        public string SByteTypeName
        {
            get
            {
                return "System.SByte";
            }
        }

        public string CharTypeName
        {
            get
            {
                return "System.Char";
            }
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