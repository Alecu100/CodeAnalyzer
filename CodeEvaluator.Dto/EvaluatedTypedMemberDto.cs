using System;

namespace CodeEvaluator.Dto
{
    [Serializable]
    public class EvaluatedTypedMemberDto : EvaluatedMemberDto
    {
        [NonSerialized]
        public Type OriginalType;

        public EvaluatedTypeInfoDto TypeInfo { get; set; }
    }
}