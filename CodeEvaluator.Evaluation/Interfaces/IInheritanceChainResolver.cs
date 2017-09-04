using CodeEvaluator.Evaluation.Common;
using CodeEvaluator.Evaluation.Members;

namespace CodeEvaluator.Evaluation.Interfaces
{
    public interface IInheritanceChainResolver
    {
        InheritanceChainResolverResult ResolveInheritanceChain(EvaluatedTypeInfo baseType,
            EvaluatedTypeInfo derivedType);
    }
}