namespace CodeEvaluator.Evaluation.Common
{
    using System.Collections.Generic;

    using global::CodeEvaluator.Evaluation.Interfaces;
    using global::CodeEvaluator.Evaluation.Members;

    using StructureMap;

    public class MethodInvocationResolver : IMethodInvocationResolver
    {
        public MethodInvocationResolverResult ResolveMethodInvocation(
            EvaluatedDelegate methodDelegate,
            List<EvaluatedObjectReferenceBase> mandatoryParameters,
            Dictionary<string, EvaluatedObjectReferenceBase> optionalParameters)
        {
            foreach (var evaluatedMethodBase in methodDelegate.TargetMethodGroup)
            {
                var methodParametersToAssign = new Dictionary<int, EvaluatedObjectReferenceBase>();

                if (
                    !TryToAssignParameters(
                        evaluatedMethodBase,
                        mandatoryParameters,
                        optionalParameters,
                        methodParametersToAssign)) continue;

                var resolvedTargetMethod = ResolveTargetMethod(
                    evaluatedMethodBase,
                    methodDelegate.TargetObject,
                    methodDelegate.TypeInfo);

                return new MethodInvocationResolverResult
                           {
                               CanInvokeMethod = true,
                               ResolvedMethod = resolvedTargetMethod,
                               ResolvedPassedParameters = methodParametersToAssign
                           };
            }

            return new MethodInvocationResolverResult { CanInvokeMethod = false };
        }

        private EvaluatedMethodBase ResolveTargetMethod(
            EvaluatedMethodBase evaluatedMethodBase,
            EvaluatedObject targetObject,
            EvaluatedTypeInfo referenceType)
        {
            var inheritanceChainResolver = ObjectFactory.GetInstance<IInheritanceChainResolver>();

            return evaluatedMethodBase;
        }

        private bool TryToAssignParameters(
            EvaluatedMethodBase evaluatedMethodBase,
            List<EvaluatedObjectReferenceBase> mandatoryParameters,
            Dictionary<string, EvaluatedObjectReferenceBase> optionalParameters,
            Dictionary<int, EvaluatedObjectReferenceBase> methodParametersToAssign)
        {
            return true;
        }
    }
}