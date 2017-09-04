using CodeEvaluator.Evaluation.Members;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CodeEvaluator.Evaluation.Common
{
    public class MethodInvocationResolverResult
    {
        public bool CanInvokeMethod { get; set; }

        public EvaluatedMethodBase ResolvedMethod { get; set; }

        public EvaluatedMethodPassedParameters ResolvedPassedParameters { get; set; }
    }
}