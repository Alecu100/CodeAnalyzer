using CodeEvaluator.Evaluation.Common;
using CodeEvaluator.Evaluation.Members;

namespace CodeEvaluator.Evaluation.Interfaces
{
    public interface IMethodInvocationResolver
    {
        MethodInvocationResolverResult ResolveMethodInvocation(EvaluatedObject targetObject,
            EvaluatedTypeInfo referenceType, EvaluatedMethodPassedParameters passedParameters);
    }
}