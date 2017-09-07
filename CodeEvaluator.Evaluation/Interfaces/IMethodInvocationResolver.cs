namespace CodeEvaluator.Evaluation.Interfaces
{
    using System.Collections.Generic;

    using CodeEvaluator.Evaluation.Common;
    using CodeEvaluator.Evaluation.Members;

    public interface IMethodInvocationResolver
    {
        MethodInvocationResolverResult ResolveMethodInvocation(
            EvaluatedDelegate methodDelegate,
            List<EvaluatedObjectReferenceBase> mandatoryParameters,
            Dictionary<string, EvaluatedObjectReferenceBase> optionalParameters);
    }
}