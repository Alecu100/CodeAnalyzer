using CodeEvaluator.Evaluation.Members;

namespace CodeEvaluator.Evaluation.Interfaces
{
    public interface IEvaluatedTypeInfoFinalizer
    {
        void FinalizeTypeInfo(EvaluatedTypeInfo evaluatedTypeInfo);

        int Priority { get; }
    }
}