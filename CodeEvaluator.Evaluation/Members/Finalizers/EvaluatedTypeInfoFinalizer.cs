using System.Linq;
using CodeEvaluator.Evaluation.Interfaces;

namespace CodeEvaluator.Evaluation.Members.Finalizers
{
    public abstract class EvaluatedTypeInfoFinalizer : IEvaluatedTypeInfoFinalizer
    {
        public abstract void FinalizeTypeInfo(EvaluatedTypeInfo evaluatedTypeInfo);
        public abstract int Priority { get; }

        protected string GetTypeKind(EvaluatedTypeInfo evaluatedTypeInfo)
        {
            return evaluatedTypeInfo.IsValueType ? "struct" : "class";
        }

        protected string GetNamespace(string identifier, string fullIdentifier)
        {
            return fullIdentifier.TrimEnd(identifier.ToArray()).TrimEnd('.');
        }
    }
}