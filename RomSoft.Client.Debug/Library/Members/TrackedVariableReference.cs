//  Project              : GLP
//  Module               : RomSoft.Client.Debug.dll
//  File                 : TrackedVariableReference.cs
//  Author               : Alecsandru
//  Last Updated         : 18/12/2015 at 16:51
//  
// 
//  Contains             : Implementation of the TrackedVariableReference.cs class.
//  Classes              : TrackedVariableReference.cs
// 
//  
//  ----------------------------------------------------------------------- 
//   <copyright file="TrackedVariableReference.cs" company="Sysmex"> 
//       Copyright (c) Sysmex. All rights reserved. 
//   </copyright> 
//  -----------------------------------------------------------------------

namespace RomSoft.Client.Debug.Library.Members
{
    #region Using

    using System.Collections.Generic;

    using Microsoft.CodeAnalysis.CSharp.Syntax;

    #endregion

    public class TrackedVariableReference : TrackedMember
    {
        #region Fields

        private readonly List<TrackedVariable> _variables = new List<TrackedVariable>();

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets or sets the declarator.
        /// </summary>
        /// <value>
        ///     The declarator.
        /// </value>
        public VariableDeclaratorSyntax Declarator { get; set; }

        /// <summary>
        ///     Gets or sets the type information.
        /// </summary>
        /// <value>
        ///     The type information.
        /// </value>
        public TrackedTypeInfo TypeInfo { get; set; }

        /// <summary>
        ///     Gets or sets the variable.
        /// </summary>
        /// <value>
        ///     The variable.
        /// </value>
        public IReadOnlyList<TrackedVariable> Variables
        {
            get
            {
                return _variables;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Adds the variable.
        /// </summary>
        /// <param name="variable">The variable.</param>
        public TrackedVariableReference AddVariable(TrackedVariable variable)
        {
            var trackedVariableReference = Copy();

            trackedVariableReference.AddVariableInternal(variable);

            Dispose();

            return trackedVariableReference;
        }

        public TrackedVariableReference AddVariables(IEnumerable<TrackedVariable> variables)
        {
            var trackedVariableReference = Copy();

            foreach (var trackedVariable in variables)
            {
                trackedVariableReference.AddVariableInternal(trackedVariable);
            }

            Dispose();

            return trackedVariableReference;
        }

        public TrackedVariableReference Copy()
        {
            var trackedVariableReference = new TrackedVariableReference();

            foreach (var trackedVariable in _variables)
            {
                trackedVariableReference.AddVariableInternal(trackedVariable);
            }

            trackedVariableReference.TypeInfo = TypeInfo;
            trackedVariableReference.Declaration = Declaration;
            trackedVariableReference.Declarator = Declarator;
            trackedVariableReference.Identifier = Identifier;
            trackedVariableReference.IdentifierText = IdentifierText;
            trackedVariableReference.FullIdentifierText = FullIdentifierText;

            return trackedVariableReference;
        }

        public TrackedVariableReference Merge(TrackedVariableReference reference)
        {
            var trackedVariableReferenceCopy = Copy();

            trackedVariableReferenceCopy.AddVariables(reference.Variables);

            Dispose();

            return trackedVariableReferenceCopy;
        }

        public TrackedVariableReference Move()
        {
            var trackedVariableReferenceCopy = Copy();

            Dispose();

            return trackedVariableReferenceCopy;
        }

        public TrackedVariableReference SelectVariable(TrackedVariable variable)
        {
            var trackedVariableReference = new TrackedVariableReference();

            trackedVariableReference.AddVariableInternal(variable);
            trackedVariableReference.TypeInfo = TypeInfo;
            trackedVariableReference.Declaration = Declaration;
            trackedVariableReference.Declarator = Declarator;
            trackedVariableReference.Identifier = Identifier;
            trackedVariableReference.IdentifierText = IdentifierText;
            trackedVariableReference.FullIdentifierText = FullIdentifierText;

            return trackedVariableReference;
        }

        #endregion

        #region Protected Methods and Operators

        /// <summary>
        ///     Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        protected override void DisposeInternal(bool isDisposing)
        {
            if (_isDisposed == false)
            {
                if (isDisposing)
                {
                    foreach (var trackedVariable in _variables)
                    {
                        trackedVariable.RemoveReference(this);
                    }
                }

                _isDisposed = true;
            }
        }

        #endregion

        #region Private Methods and Operators

        private void AddVariableInternal(TrackedVariable variable)
        {
            variable.AddReference(this);
            _variables.Add(variable);
        }

        #endregion
    }
}