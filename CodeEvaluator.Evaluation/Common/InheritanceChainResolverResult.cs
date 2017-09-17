using System.Collections.Generic;
using CodeEvaluator.Evaluation.Members;

namespace CodeEvaluator.Evaluation.Common
{
    public class InheritanceChainResolverResult
    {
        public bool IsValid { get; set; }

        public List<EvaluatedTypeInfo> ResolvedInheritanceChain { get; set; } = new List<EvaluatedTypeInfo>();
    }
}