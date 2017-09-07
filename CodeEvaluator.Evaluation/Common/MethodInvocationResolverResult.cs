using CodeEvaluator.Evaluation.Members;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CodeEvaluator.Evaluation.Common
{
    using System.Collections.Generic;

    public class MethodInvocationResolverResult
    {
        public bool CanInvokeMethod { get; set; }

        public EvaluatedMethodBase ResolvedMethod { get; set; }

        public Dictionary<int, EvaluatedObjectReferenceBase> ResolvedPassedParameters { get; set; }
    }
}