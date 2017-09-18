using System.Collections.Generic;
using System.Linq;
using CodeEvaluator.Evaluation.Extensions;
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

        public IMethodSignatureComparer MethodSignatureComparer { get; set; }

        public MethodInvocationResolverResult ResolveMethodInvocation(
            EvaluatedDelegate methodDelegate,
            List<EvaluatedObjectReferenceBase> mandatoryParameters,
            Dictionary<string, EvaluatedObjectReferenceBase> optionalParameters)
        {
            foreach (var evaluatedMethodDerived in methodDelegate.TargetMethodGroup)
            {
                var methodParametersToAssign = new Dictionary<int, EvaluatedObjectReferenceBase>();

                if (
                    !TryToAssignParameters(
                        evaluatedMethodDerived,
                        mandatoryParameters,
                        optionalParameters,
                        methodParametersToAssign)) continue;

                EvaluatedMethodBase resolvedTargetMethod = null;

                methodParametersToAssign[-1] = new EvaluatedObjectDirectReference();
                methodParametersToAssign[-1].AssignEvaluatedObject(methodDelegate.TargetObject);

                if (methodDelegate.TypeInfo == methodDelegate.TargetObject.TypeInfo)
                    resolvedTargetMethod = evaluatedMethodDerived;
                else if (!TryToResolveTargetMethod(
                    evaluatedMethodDerived,
                    methodDelegate.TargetObject,
                    methodDelegate.TypeInfo,
                    ref resolvedTargetMethod))
                    return new MethodInvocationResolverResult {CanInvokeMethod = false};

                return new MethodInvocationResolverResult
                {
                    CanInvokeMethod = true,
                    ResolvedMethod = resolvedTargetMethod,
                    ResolvedPassedParameters = methodParametersToAssign
                };
            }

            return new MethodInvocationResolverResult {CanInvokeMethod = false};
        }

        private bool TryToResolveTargetMethod(
            EvaluatedMethodBase derivedSignatureMethod,
            EvaluatedObject targetObject,
            EvaluatedTypeInfo referenceType,
            ref EvaluatedMethodBase resolvedTargetMethod)
        {
            var inheritanceChainResolverResult =
                InheritanceChainResolver.ResolveInheritanceChain(referenceType, targetObject.TypeInfo);

            if (inheritanceChainResolverResult.IsValid == false)
                return false;

            var classInheritanceChain = inheritanceChainResolverResult.ResolvedInheritanceChain;

            if (classInheritanceChain.Count == 1)
            {
                resolvedTargetMethod = derivedSignatureMethod;
                return true;
            }

            EvaluatedMethodBase lastResolvedMethod = null;
            EvaluatedTypeInfo lastResolvedMethodType = null;

            for (var inheritanceIndex = 1; inheritanceIndex < classInheritanceChain.Count; inheritanceIndex++)
            {
                var currentResolvedMethod = classInheritanceChain[inheritanceIndex]
                    .SpecificMethods.FirstOrDefault(
                        method => MethodSignatureComparer.HaveSameSignature(derivedSignatureMethod, method));

                var currentResolvedMethodType = classInheritanceChain[inheritanceIndex];

                if (currentResolvedMethod == null)
                    continue;

                if (lastResolvedMethod == null)
                {
                    lastResolvedMethod = currentResolvedMethod;
                    lastResolvedMethodType = currentResolvedMethodType;

                    continue;
                }

                if ((lastResolvedMethod.IsVirtualOrAbstract() || lastResolvedMethod.IsOverride()) &&
                    (!currentResolvedMethod.IsOverride() || currentResolvedMethod.IsNew()))
                    break;

                if (currentResolvedMethodType.IsInterfaceType == false &&
                    lastResolvedMethodType.IsInterfaceType && !currentResolvedMethod.IsVirtualOrAbstract())
                {
                    lastResolvedMethod = currentResolvedMethod;
                    break;
                }

                lastResolvedMethod = currentResolvedMethod;
                lastResolvedMethodType = currentResolvedMethodType;
            }

            resolvedTargetMethod = lastResolvedMethod;
            return true;
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

                if (currentParameter.HasDefault == false && mandatoryParameters.Count <= parameterIndex)
                    return false;

                EvaluatedObjectReferenceBase currentParameterValue = null;

                if (parameterIndex < mandatoryParameters.Count)
                {
                    currentParameterValue = mandatoryParameters[parameterIndex];
                }
                else
                {
                    if (!optionalParameters.ContainsKey(currentParameter.IdentifierText))
                    {
                        currentParameterValue = new EvaluatedObjectDirectReference();

                        currentParameterValue.TypeInfo = currentParameter.TypeInfo;
                        currentParameterValue.IdentifierText = currentParameter.IdentifierText;
                    }

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