namespace CodeEvaluator.Evaluation.Members
{
    using System;
    using System.Collections.Generic;

    using Microsoft.CodeAnalysis.CSharp.Syntax;

    #region Using

    #endregion

    [Serializable]
    public abstract class EvaluatedObjectReferenceBase : EvaluatedMember
    {
        #region SpecificFields

        private VariableDeclaratorSyntax _declarator;

        #endregion

        #region Protected Methods and Operators

        #endregion

        #region Private Methods and Operators

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets or sets the declarator.
        /// </summary>
        /// <value>
        ///     The declarator.
        /// </value>
        public VariableDeclaratorSyntax Declarator
        {
            get
            {
                return _declarator;
            }
            set
            {
                _declarator = value;
            }
        }

        /// <summary>
        ///     Gets or sets the type information.
        /// </summary>
        /// <value>
        ///     The type information.
        /// </value>
        public EvaluatedTypeInfo TypeInfo { get; set; }

        /// <summary>
        ///     Gets or sets the evaluatedObject.
        /// </summary>
        /// <value>
        ///     The evaluatedObject.
        /// </value>
        public abstract IReadOnlyList<EvaluatedObject> EvaluatedObjects { get; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Adds the evaluatedObject.
        /// </summary>
        /// <param name="evaluatedObject">The evaluatedObject.</param>
        public abstract void AssignEvaluatedObject(EvaluatedObject evaluatedObject);

        public abstract void AssignEvaluatedObject(EvaluatedObjectReferenceBase evaluatedObjectReference);

        public abstract void AssignEvaluatedObjects(IEnumerable<EvaluatedObject> evaluatedObjects);

        protected void AssignTypeInfoIfMissing(EvaluatedObject evaluatedObject)
        {
            if (TypeInfo == null)
            {
                TypeInfo = evaluatedObject.TypeInfo;
            }
        }

        #endregion
    }
}