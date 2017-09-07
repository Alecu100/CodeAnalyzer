using CodeEvaluator.Evaluation.Members;

namespace CodeEvaluator.Evaluation.Interfaces
{
    public interface IMethodSignatureComparer
    {
        bool HaveSameSignature(EvaluatedMethodBase methodtoCompareAgainst, EvaluatedMethodBase methodToCompare);
    }
}