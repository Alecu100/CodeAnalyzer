using System.Collections.Generic;
using CodeEvaluator.Evaluation.Common;
using CodeEvaluator.Evaluation.Members;

namespace CodeEvaluator.Evaluation.Interfaces
{
    public interface IMethodInvocationResolver
    {
        MethodInvocationResolverResult ResolveMethodInvocation(
            List<EvaluatedMethodBase> targetMethodGroup,
            EvaluatedObject targetObject,
            EvaluatedTypeInfo referenceType,
            List<EvaluatedObjectReference> mandatoryParameters,
            Dictionary<string, EvaluatedObjectReference> optionalParameters);
    }
}