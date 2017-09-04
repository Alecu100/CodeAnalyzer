using System.Collections.Generic;
using CodeEvaluator.Evaluation.Common;
using CodeEvaluator.Evaluation.Members;

namespace CodeEvaluator.Evaluation.Interfaces
{
    public interface IMethodInvocationResolver
    {
        MethodInvocationResolverResult ResolveMethodInvocation(EvaluatedObject targetObject,
            List<EvaluatedMethodBase> availableMethods,
            EvaluatedTypeInfo referenceType, EvaluatedMethodPassedParameters passedParameters);
    }
}