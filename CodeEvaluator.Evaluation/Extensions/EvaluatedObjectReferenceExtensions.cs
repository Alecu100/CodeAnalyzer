using System.Linq;
using CodeEvaluator.Evaluation.Members;

namespace CodeEvaluator.Evaluation.Extensions
{
    public static class EvaluatedObjectReferenceExtensions
    {
        public static bool IsNull(this EvaluatedObjectReference evaluatedObjectReference)
        {
            return evaluatedObjectReference == null || !evaluatedObjectReference.EvaluatedObjects.Any();
        }

        public static bool IsNotNull(this EvaluatedObjectReference evaluatedObjectReference)
        {
            if (evaluatedObjectReference == null)
                return false;

            if (evaluatedObjectReference is EvaluatedPropertyObjectReference)
            {
                var evaluatedPropertyObjectReference = (EvaluatedPropertyObjectReference) evaluatedObjectReference;

                return evaluatedPropertyObjectReference.EvaluatedPropertyObjects.Any();
            }

            return true;
        }
    }
}