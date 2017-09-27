namespace CodeEvaluator.Evaluation.Members
{
    using System.Collections.Generic;
    using System.Linq;

    public class EvaluatedObjectIndirectReference : EvaluatedObjectReference
    {
        private readonly List<EvaluatedObjectReference> _internalReferences = new List<EvaluatedObjectReference>();

        public EvaluatedObjectIndirectReference(IEnumerable<EvaluatedObjectReference> internalReferences)
        {
            _internalReferences.AddRange(internalReferences);

            AddTypeInfoIfMissing(internalReferences.First());
        }

        public EvaluatedObjectIndirectReference()
        {
        }

        public EvaluatedObjectIndirectReference(EvaluatedObjectReference internalReference)
        {
            _internalReferences.Add(internalReference);

            AddTypeInfoIfMissing(internalReference);
        }

        public override IReadOnlyList<EvaluatedObject> EvaluatedObjects
        {
            get
            {
                return _internalReferences.SelectMany(reference => reference.EvaluatedObjects).ToList();
            }
        }

        public override void AssignEvaluatedObject(EvaluatedObject evaluatedObject)
        {
            foreach (var internalReference in _internalReferences) internalReference.AssignEvaluatedObject(evaluatedObject);
        }

        public override void AssignEvaluatedObject(EvaluatedObjectReference evaluatedObjectReference)
        {
            foreach (var internalReference in _internalReferences) internalReference.AssignEvaluatedObject(evaluatedObjectReference);
        }

        public override void AssignEvaluatedObjects(IEnumerable<EvaluatedObject> evaluatedObjects)
        {
            foreach (var internalReference in _internalReferences) internalReference.AssignEvaluatedObjects(evaluatedObjects);
        }

        public void AssignEvaluatedObjectReference(EvaluatedObjectReference evaluatedObjectReference)
        {
            _internalReferences.Add(evaluatedObjectReference);

            AddTypeInfoIfMissing(evaluatedObjectReference);
        }

        private void AddTypeInfoIfMissing(EvaluatedObjectReference evaluatedObjectReference)
        {
            if (TypeInfo == null)
            {
                TypeInfo = evaluatedObjectReference.TypeInfo;
                Identifier = evaluatedObjectReference.Identifier;
                FullIdentifierText = evaluatedObjectReference.FullIdentifierText;
                IdentifierText = evaluatedObjectReference.IdentifierText;
            }
        }
    }
}