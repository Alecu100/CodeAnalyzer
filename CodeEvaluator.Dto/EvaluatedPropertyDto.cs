using System;

namespace CodeEvaluator.Dto
{
    [Serializable]
    public class EvaluatedPropertyDto : EvaluatedTypedMemberDto
    {
        public EvaluatedMethodDto Getter { get; set; }
        public EvaluatedMethodDto Setter { get; set; }
    }
}