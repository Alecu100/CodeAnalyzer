using System;
using CodeEvaluator.Evaluation.Interfaces;
using CodeEvaluator.Evaluation.Members;

namespace CodeEvaluator.Evaluation.Common
{
    public class MethodSignatureComparer : IMethodSignatureComparer
    {
        public bool HaveSameSignature(EvaluatedMethodBase methodtoCompareAgainst, EvaluatedMethodBase methodToCompare)
        {
            if (!string.Equals(methodtoCompareAgainst.IdentifierText, methodToCompare.IdentifierText,
                StringComparison.Ordinal))
                return false;

            if (methodtoCompareAgainst.Parameters.Count != methodToCompare.Parameters.Count)
                return false;

            if (methodtoCompareAgainst.ReturnType != methodToCompare.ReturnType)
                return false;

            for (var i = 0; i < methodToCompare.Parameters.Count; i++)
                if (methodToCompare.Parameters[i].TypeInfo != methodtoCompareAgainst.Parameters[i].TypeInfo)
                    return false;

            return true;
        }
    }
}