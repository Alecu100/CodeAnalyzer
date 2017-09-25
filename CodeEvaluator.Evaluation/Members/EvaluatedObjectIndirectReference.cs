﻿using System.Collections.Generic;
using System.Linq;

namespace CodeEvaluator.Evaluation.Members
{
    public class EvaluatedObjectIndirectReference : EvaluatedObjectReference
    {
        private readonly List<EvaluatedObjectReference> _internalReferences = new List<EvaluatedObjectReference>();

        public EvaluatedObjectIndirectReference(IEnumerable<EvaluatedObjectReference> internalReferences)
        {
            _internalReferences.AddRange(internalReferences);

            TypeInfo = internalReferences.First().TypeInfo;
            Identifier = internalReferences.First().Identifier;
            FullIdentifierText = internalReferences.First().FullIdentifierText;
            IdentifierText = internalReferences.First().IdentifierText;
        }

        public EvaluatedObjectIndirectReference(EvaluatedObjectReference internalReference)
        {
            _internalReferences.Add(internalReference);

            TypeInfo = internalReference.TypeInfo;
            Identifier = internalReference.Identifier;
            FullIdentifierText = internalReference.FullIdentifierText;
            IdentifierText = internalReference.IdentifierText;
        }


        public override IReadOnlyList<EvaluatedObject> EvaluatedObjects
        {
            get { return _internalReferences.SelectMany(reference => reference.EvaluatedObjects).ToList(); }
        }

        public override void AssignEvaluatedObject(EvaluatedObject evaluatedObject)
        {
            foreach (var internalReference in _internalReferences)
                internalReference.AssignEvaluatedObject(evaluatedObject);
        }

        public override void AssignEvaluatedObject(EvaluatedObjectReference evaluatedObjectReference)
        {
            foreach (var internalReference in _internalReferences)
                internalReference.AssignEvaluatedObject(evaluatedObjectReference);
        }

        public override void AssignEvaluatedObjects(IEnumerable<EvaluatedObject> evaluatedObjects)
        {
            foreach (var internalReference in _internalReferences)
                internalReference.AssignEvaluatedObjects(evaluatedObjects);
        }
    }
}