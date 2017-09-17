using System.Collections.Generic;

namespace CodeEvaluator.Evaluation.Members
{
    public class EvaluatedObjectDirectReference : EvaluatedObjectReferenceBase
    {
        #region Public Properties

        /// <summary>
        ///     Gets or sets the evaluatedObject.
        /// </summary>
        /// <value>
        ///     The evaluatedObject.
        /// </value>
        public override IReadOnlyList<EvaluatedObject> EvaluatedObjects => _evaluatedObjects;

        #endregion

        #region SpecificFields

        private readonly List<EvaluatedObject> _evaluatedObjects = new List<EvaluatedObject>();

        public EvaluatedObjectDirectReference()
        {
        }

        public EvaluatedObjectDirectReference(EvaluatedObjectReferenceBase referenceToCopy)
        {
            TypeInfo = referenceToCopy.TypeInfo;
            Identifier = referenceToCopy.Identifier;
            FullIdentifierText = referenceToCopy.FullIdentifierText;
            IdentifierText = referenceToCopy.IdentifierText;

            AssignEvaluatedObject(referenceToCopy);
        }

        public EvaluatedObjectDirectReference(EvaluatedObject evaluatedObject)
        {
            AssignEvaluatedObject(evaluatedObject);
        }

        #endregion

        #region Protected Methods and Operators

        #endregion

        #region Private Methods and Operators

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Adds the evaluatedObject.
        /// </summary>
        /// <param name="evaluatedObject">The evaluatedObject.</param>
        public sealed override void AssignEvaluatedObject(EvaluatedObject evaluatedObject)
        {
            AssignTypeInfoIfMissing(evaluatedObject);
            _evaluatedObjects.Add(evaluatedObject);
        }

        public sealed override void AssignEvaluatedObject(EvaluatedObjectReferenceBase evaluatedObjectReference)
        {
            foreach (var evaluatedObject in evaluatedObjectReference.EvaluatedObjects)
            {
                AssignTypeInfoIfMissing(evaluatedObject);
                _evaluatedObjects.Add(evaluatedObject);
            }
        }

        public sealed override void AssignEvaluatedObjects(IEnumerable<EvaluatedObject> evaluatedObjects)
        {
            foreach (var evaluatedObject in evaluatedObjects)
            {
                AssignTypeInfoIfMissing(evaluatedObject);
                _evaluatedObjects.Add(evaluatedObject);
            }
        }

        #endregion
    }
}