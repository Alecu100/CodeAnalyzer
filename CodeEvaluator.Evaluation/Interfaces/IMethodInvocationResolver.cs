namespace CodeEvaluator.Evaluation.Interfaces
{
    using System.Collections.Generic;

    using CodeEvaluator.Evaluation.Common;
    using CodeEvaluator.Evaluation.Members;

    public interface IMethodInvocationResolver
    {
        MethodInvocationResolverResult ResolveMethodInvocation(
            EvaluatedInvokableObject methodInvokableObject,
            List<EvaluatedObjectReference> mandatoryParameters,
            Dictionary<string, EvaluatedObjectReference> optionalParameters);
    }
}