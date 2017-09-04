using System.Collections.Generic;
using CodeEvaluator.Evaluation.Interfaces;
using CodeEvaluator.Evaluation.Members;
using StructureMap;

namespace CodeEvaluator.Evaluation.Common
{
    public class MethodInvocationResolver : IMethodInvocationResolver
    {
        public MethodInvocationResolverResult ResolveMethodInvocation(EvaluatedObject targetObject,
            List<EvaluatedMethodBase> availableMethods, EvaluatedTypeInfo referenceType,
            EvaluatedMethodPassedParameters passedParameters)
        {
            foreach (var evaluatedMethodBase in availableMethods)
            {
                var methodParametersToAssign = new EvaluatedMethodPassedParameters();

                if (!TryToAssignParameters(evaluatedMethodBase, passedParameters, methodParametersToAssign))
                    continue;

                var resolvedTargetMethod = ResolveTargetMethod(evaluatedMethodBase, targetObject, referenceType);

                return new MethodInvocationResolverResult
                {
                    CanInvokeMethod = true,
                    ResolvedMethod = resolvedTargetMethod,
                    ResolvedPassedParameters = methodParametersToAssign
                };
            }

            return new MethodInvocationResolverResult {CanInvokeMethod = false};
        }

        private EvaluatedMethodBase ResolveTargetMethod(EvaluatedMethodBase evaluatedMethodBase,
            EvaluatedObject targetObject, EvaluatedTypeInfo referenceType)
        {
            var inheritanceChainResolver = ObjectFactory.GetInstance<IInheritanceChainResolver>();

            return evaluatedMethodBase;
        }

        private bool TryToAssignParameters(EvaluatedMethodBase evaluatedMethodBase,
            EvaluatedMethodPassedParameters passedParameters, EvaluatedMethodPassedParameters methodParametersToAssign)
        {
            return true;
        }
    }
}