using System;
using System.Collections.Generic;

namespace CodeEvaluator.Dto
{
    [Serializable]
    public class EvaluatedMethodDto : EvaluatedTypedMemberDto
    {
        public List<EvaluatedTypedMemberDto> Parameters { get; } = new List<EvaluatedTypedMemberDto>();
    }
}