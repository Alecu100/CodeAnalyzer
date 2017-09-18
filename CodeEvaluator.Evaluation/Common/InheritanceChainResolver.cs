using CodeEvaluator.Evaluation.Interfaces;
using CodeEvaluator.Evaluation.Members;

namespace CodeEvaluator.Evaluation.Common
{
    public class InheritanceChainResolver : IInheritanceChainResolver
    {
        public InheritanceChainResolverResult ResolveInheritanceChain(EvaluatedTypeInfo baseType,
            EvaluatedTypeInfo derivedType)
        {
            var inheritanceChainResolverResult = new InheritanceChainResolverResult();

            ResolveInheritanceChainRecursive(baseType,
                derivedType, inheritanceChainResolverResult);

            if (inheritanceChainResolverResult.IsValid)
            {
                inheritanceChainResolverResult.ResolvedInheritanceChain.Reverse();
            }

            return inheritanceChainResolverResult;
        }

        private void ResolveInheritanceChainRecursive(EvaluatedTypeInfo baseType, EvaluatedTypeInfo derivedType,
            InheritanceChainResolverResult inheritanceChainResolverResult)
        {
            inheritanceChainResolverResult.ResolvedInheritanceChain.Add(derivedType);

            if (baseType == derivedType)
            {
                inheritanceChainResolverResult.IsValid = true;

                return;
            }

            foreach (var derivedTypeBaseTypeInfo in derivedType.BaseTypeInfos)
            {
                ResolveInheritanceChainRecursive(baseType, derivedTypeBaseTypeInfo, inheritanceChainResolverResult);

                if (inheritanceChainResolverResult.IsValid)
                    return;
            }

            inheritanceChainResolverResult.ResolvedInheritanceChain.Remove(derivedType);
        }
    }
}