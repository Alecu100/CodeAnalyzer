//  Project              : GLP
//  Module               : RomSoft.Client.Debug.dll
//  File                 : EvaluatedObjectReference.cs
//  Author               : Alecsandru
//  Last Updated         : 18/12/2015 at 16:51
//  
// 
//  Contains             : Implementation of the EvaluatedObjectReference.cs class.
//  Classes              : EvaluatedObjectReference.cs
// 
//  
//  ----------------------------------------------------------------------- 
//   <copyright file="EvaluatedObjectReference.cs" company="Sysmex"> 
//       Copyright (c) Sysmex. All rights reserved. 
//   </copyright> 
//  -----------------------------------------------------------------------

using System.Collections.Generic;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CodeAnalysis.Core.Members
{

    #region Using

    #endregion

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