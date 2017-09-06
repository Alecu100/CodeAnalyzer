namespace CodeEvaluator.Evaluation.Members
{
    using System.Collections.Generic;

    public class EvaluatedObjectIndirectReference : EvaluatedObjectReferenceBase
    {
        private readonly EvaluatedObjectReferenceBase _internalReference;

        public EvaluatedObjectIndirectReference(EvaluatedObjectReferenceBase internalReference)
        {
            _internalReference = internalReference;
        }

        public override IReadOnlyList<EvaluatedObject> EvaluatedObjects
        {
            get
            {
                return _internalReference.EvaluatedObjects;
            }
        }

        public override void AssignEvaluatedObject(EvaluatedObject evaluatedObject)
        {
            _internalReference.AssignEvaluatedObject(evaluatedObject);
        }

        public override void AssignEvaluatedObject(EvaluatedObjectReferenceBase evaluatedObjectReference)
        {
            _internalReference.AssignEvaluatedObject(evaluatedObjectReference);
        }

        public override void AssignEvaluatedObjects(IEnumerable<EvaluatedObject> evaluatedObjects)
        {
            _internalReference.AssignEvaluatedObjects(evaluatedObjects);
        }
    }
}