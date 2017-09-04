using CodeEvaluator.Evaluation.Interfaces;
using CodeEvaluator.Evaluation.Members;

namespace CodeEvaluator.Evaluation.Common
{
    public class MethodInvocationResolver : IMethodInvocationResolver
    {
        public MethodInvocationResolverResult ResolveMethodInvocation(EvaluatedObject targetObject, EvaluatedTypeInfo referenceType,
            EvaluatedMethodPassedParameters passedParameters)
        {
            throw new System.NotImplementedException();
        }
    }
}