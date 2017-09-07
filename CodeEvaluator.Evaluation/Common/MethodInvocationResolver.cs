namespace CodeEvaluator.Evaluation.Common
{
    using System.Collections.Generic;

    using global::CodeEvaluator.Evaluation.Interfaces;
    using global::CodeEvaluator.Evaluation.Members;

    using StructureMap;

    public class MethodInvocationResolver : IMethodInvocationResolver
    {
        public MethodInvocationResolver()
        {
            ObjectFactory.BuildUp(this);
        }

        public IInheritanceChainResolver InheritanceChainResolver { get; set; }

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
            return evaluatedMethodBase;
        }

        private bool TryToAssignParameters(
            EvaluatedMethodBase evaluatedMethodBase,
            List<EvaluatedObjectReferenceBase> mandatoryParameters,
            Dictionary<string, EvaluatedObjectReferenceBase> optionalParameters,
            Dictionary<int, EvaluatedObjectReferenceBase> methodParametersToAssign)
        {
            for (var parameterIndex = 0; parameterIndex < evaluatedMethodBase.Parameters.Count; parameterIndex++)
            {
                var currentParameter = evaluatedMethodBase.Parameters[parameterIndex];

                if (currentParameter.HasDefault == false)
                {
                    if (mandatoryParameters.Count >= parameterIndex)
                    {
                        return false;
                    }

                    if (NoInheritanceChainFound(currentParameter, mandatoryParameters[parameterIndex]))
                    {
                        return false;
                    }

                    var evaluatedObjectReference = GetParameterReference(currentParameter, mandatoryParameters[parameterIndex]);

                    methodParametersToAssign[parameterIndex] = evaluatedObjectReference;
                }
                else
                {
                    
                }
            }

            return true;
        }

        private EvaluatedObjectReferenceBase GetParameterReference(EvaluatedMethodParameter currentParameter, EvaluatedObjectReferenceBase parameterValue)
        {
            if (currentParameter.IsByReference)
            {
                return new EvaluatedObjectIndirectReference(parameterValue);
            }

            return new EvaluatedObjectDirectReference(parameterValue);
        }

        private bool NoInheritanceChainFound(
            EvaluatedMethodParameter currentParameter,
            EvaluatedObjectReferenceBase mandatoryParameter)
        {
            var inheritanceChainResolverResult =
                InheritanceChainResolver.ResolveInheritanceChain(mandatoryParameter.TypeInfo, currentParameter.TypeInfo);

            return inheritanceChainResolverResult.IsValid;
        }
    }
}