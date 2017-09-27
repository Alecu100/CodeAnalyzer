namespace CodeEvaluator.Evaluation.Extensions
{
    using System.Linq;

    using CodeEvaluator.Evaluation.Members;

    public static class EvaluatedObjectReferenceExtensions
    {
        public static bool IsNull(this EvaluatedObjectReference evaluatedObjectReference)
        {
            return evaluatedObjectReference == null || !evaluatedObjectReference.EvaluatedObjects.Any();
        }

        public static bool IsNotNull(this EvaluatedObjectReference evaluatedObjectReference)
        {
            return evaluatedObjectReference != null && evaluatedObjectReference.EvaluatedObjects.Any();
        }
    }
}