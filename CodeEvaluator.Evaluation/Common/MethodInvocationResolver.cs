using System.Collections.Generic;
using CodeEvaluator.Evaluation.Interfaces;
using CodeEvaluator.Evaluation.Members;
using StructureMap;

namespace CodeEvaluator.Evaluation.Common
{
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

            return new MethodInvocationResolverResult {CanInvokeMethod = false};
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

                if (currentParameter.HasDefault == false && mandatoryParameters.Count >= parameterIndex)
                    return false;

                EvaluatedObjectReferenceBase currentParameterValue = null;

                if (parameterIndex < mandatoryParameters.Count)
                {
                    currentParameterValue = mandatoryParameters[parameterIndex];
                }
                else
                {
                    if (!optionalParameters.ContainsKey(currentParameter.IdentifierText))
                        return false;

                    currentParameterValue = optionalParameters[currentParameter.IdentifierText];
                }

                if (NoInheritanceChainFound(currentParameter, currentParameterValue))
                    return false;

                var currentParameterPassedValue =
                    GetParameterReference(currentParameter, currentParameterValue);

                methodParametersToAssign[parameterIndex] = currentParameterPassedValue;
            }

            return true;
        }

        private EvaluatedObjectReferenceBase GetParameterReference(EvaluatedMethodParameter currentParameter,
            EvaluatedObjectReferenceBase parameterValue)
        {
            if (currentParameter.IsByReference)
                return new EvaluatedObjectIndirectReference(parameterValue);

            return new EvaluatedObjectDirectReference(parameterValue);
        }

        private bool NoInheritanceChainFound(
            EvaluatedMethodParameter currentParameter,
            EvaluatedObjectReferenceBase parameterValue)
        {
            var inheritanceChainResolverResult =
                InheritanceChainResolver.ResolveInheritanceChain(parameterValue.TypeInfo, currentParameter.TypeInfo);

            return inheritanceChainResolverResult.IsValid;
        }
    }
}