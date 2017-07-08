using System;
using System.Collections.Generic;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CodeAnalysis.Core.Members
{

    #region Using

    #endregion

    [Serializable]
    public class EvaluatedObjectReference : EvaluatedMember
    {
        #region SpecificFields

        private readonly List<EvaluatedObject> _evaluatedObjects = new List<EvaluatedObject>();
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
            get { return _declarator; }
            set { _declarator = value; }
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
        public IReadOnlyList<EvaluatedObject> EvaluatedObjects
        {
            get { return _evaluatedObjects; }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Adds the evaluatedObject.
        /// </summary>
        /// <param name="evaluatedObject">The evaluatedObject.</param>
        public void AssignEvaluatedObject(EvaluatedObject evaluatedObject)
        {
            AssignTypeInfoIfMissing(evaluatedObject);
            _evaluatedObjects.Add(evaluatedObject);
        }

        public void AssignEvaluatedObject(EvaluatedObjectReference evaluatedObjectReference)
        {
            foreach (var evaluatedObject in evaluatedObjectReference.EvaluatedObjects)
            {
                AssignTypeInfoIfMissing(evaluatedObject);
                _evaluatedObjects.Add(evaluatedObject);
            }
        }


        public void AssignEvaluatedObjects(IEnumerable<EvaluatedObject> evaluatedObjects)
        {
            foreach (var evaluatedObject in evaluatedObjects)
            {
                AssignTypeInfoIfMissing(evaluatedObject);
                _evaluatedObjects.Add(evaluatedObject);
            }
        }

        private void AssignTypeInfoIfMissing(EvaluatedObject evaluatedObject)
        {
            if (TypeInfo == null)
            {
                TypeInfo = evaluatedObject.TypeInfo;
            }
        }

        #endregion
    }
}