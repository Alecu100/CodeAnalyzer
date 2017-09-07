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
            foreach (var evaluatedMethodBase in methodDelegate.TargetMethodGroup)
            {
                var methodParametersToAssign = new Dictionary<int, EvaluatedObjectReferenceBase>();

                if (
                    !TryToAssignParameters(
                        evaluatedMethodBase,
                        mandatoryParameters,
                        optionalParameters,
                        methodParametersToAssign)) continue;

                EvaluatedMethodBase resolvedTargetMethod = null;

                if (!TryToResolveTargetMethod(
                    evaluatedMethodBase,
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
            EvaluatedMethodBase evaluatedMethodBase,
            EvaluatedObject targetObject,
            EvaluatedTypeInfo referenceType,
            ref EvaluatedMethodBase resolvedTargetMethod)
        {
            var inheritanceChainResolverResult =
                InheritanceChainResolver.ResolveInheritanceChain(referenceType, targetObject.TypeInfo);

            if (inheritanceChainResolverResult.IsValid == false)
                return false;

            var classInheritanceChain = inheritanceChainResolverResult.ResolvedInheritanceChain;

            resolvedTargetMethod = evaluatedMethodBase;

            if (classInheritanceChain.Count == 1)
                return true;

            for (var inheritanceIndex = 0; inheritanceIndex < classInheritanceChain.Count; inheritanceIndex++)
            {
                var currentMethod = classInheritanceChain[inheritanceIndex]
                    .SpecificMethods.FirstOrDefault(
                        method => MethodSignatureComparer.HaveSameSignature(evaluatedMethodBase, method));

                if (currentMethod == null)
                    continue;

                var derivedMethod = currentMethod;

                if (inheritanceIndex < classInheritanceChain.Count - 1)
                {
                    derivedMethod = classInheritanceChain[inheritanceIndex + 1]
                        .SpecificMethods.FirstOrDefault(
                            method => MethodSignatureComparer.HaveSameSignature(evaluatedMethodBase, method));

                    if (derivedMethod != null &&
                        classInheritanceChain[inheritanceIndex].IsInterfaceType &&
                        classInheritanceChain[inheritanceIndex + 1].IsInterfaceType == false &&
                        !derivedMethod.IsVirtualOrAbstract())
                        return true;

                    if (!currentMethod.IsVirtualOrAbstract() &&
                        classInheritanceChain[inheritanceIndex].IsInterfaceType == false)
                        return true;

                    if ((currentMethod.IsVirtualOrAbstract() || currentMethod.IsOverride()) && derivedMethod != null &&
                        !(derivedMethod.IsOverride() || derivedMethod.IsNew()))
                        return true;
                }

                resolvedTargetMethod = currentMethod;
            }

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