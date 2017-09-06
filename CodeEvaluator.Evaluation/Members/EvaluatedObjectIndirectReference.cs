using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeEvaluator.Evaluation.Members
{
    public class EvaluatedObjectIndirectReference : EvaluatedObjectReference
    {
        private EvaluatedObjectReference _internalReference;

        public EvaluatedObjectIndirectReference(EvaluatedObjectReference internalReference)
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

        public override void AssignEvaluatedObject(EvaluatedObjectReference evaluatedObjectReference)
        {
            _internalReference.AssignEvaluatedObject(evaluatedObjectReference);
        }

        public override void AssignEvaluatedObjects(IEnumerable<EvaluatedObject> evaluatedObjects)
        {
            _internalReference.AssignEvaluatedObjects(evaluatedObjects);
        }
    }
}
