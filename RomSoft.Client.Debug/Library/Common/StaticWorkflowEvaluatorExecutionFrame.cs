//  Project              : GLP
//  Module               : RomSoft.Client.Debug.dll
//  File                 : StaticWorkflowEvaluatorExecutionFrame.cs
//  Author               : Alecsandru
//  Last Updated         : 18/12/2015 at 22:29
//  
// 
//  Contains             : Implementation of the StaticWorkflowEvaluatorExecutionFrame.cs class.
//  Classes              : StaticWorkflowEvaluatorExecutionFrame.cs
// 
//  
//  ----------------------------------------------------------------------- 
//   <copyright file="StaticWorkflowEvaluatorExecutionFrame.cs" company="Sysmex"> 
//       Copyright (c) Sysmex. All rights reserved. 
//   </copyright> 
//  -----------------------------------------------------------------------

namespace RomSoft.Client.Debug.Library.Common
{
    #region Using

    using System;
    using System.Collections.Generic;

    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp.Syntax;

    using RomSoft.Client.Debug.Library.Members;

    #endregion

    public class StaticWorkflowEvaluatorExecutionFrame : IDisposable
    {
        #region Fields

        private bool _isDisposed;

        private readonly List<TrackedVariableReference> _localReferences = new List<TrackedVariableReference>();

        private readonly Dictionary<int, TrackedVariableReference> _passedMethodParameters =
            new Dictionary<int, TrackedVariableReference>();

        private readonly List<TrackedVariableReference> _returnedMethodParameters = new List<TrackedVariableReference>();

        private readonly List<TrackedVariableReference> _returningMethodParameters =
            new List<TrackedVariableReference>();

        #endregion

        #region Constructors and Destructors

        ~StaticWorkflowEvaluatorExecutionFrame()
        {
            DisposeInternal(false);
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets or sets the accessed reference.
        /// </summary>
        /// <value>
        ///     The accessed reference.
        /// </value>
        public TrackedVariableReference AccessedReference { get; set; }

        /// <summary>
        ///     Gets or sets the accessed reference member.
        /// </summary>
        /// <value>
        ///     The accessed reference member.
        /// </value>
        public SimpleNameSyntax AccessedReferenceMember { get; set; }

        /// <summary>
        ///     Gets or sets the current method.
        /// </summary>
        /// <value>
        ///     The current method.
        /// </value>
        public TrackedMethodBase CurrentMethod { get; set; }

        /// <summary>
        ///     Gets or sets the current syntax node.
        /// </summary>
        /// <value>
        ///     The current syntax node.
        /// </value>
        public SyntaxNode CurrentSyntaxNode { get; set; }

        /// <summary>
        ///     Gets the local variables.
        /// </summary>
        /// <value>
        ///     The local variables.
        /// </value>
        public List<TrackedVariableReference> LocalReferences
        {
            get
            {
                return _localReferences;
            }
        }

        /// <summary>
        ///     Gets or sets the stack variables.
        /// </summary>
        /// <value>
        ///     The stack variables.
        /// </value>
        public Dictionary<int, TrackedVariableReference> PassedMethodParameters
        {
            get
            {
                return _passedMethodParameters;
            }
        }

        /// <summary>
        ///     Gets the returned method parameters.
        /// </summary>
        /// <value>
        ///     The returned method parameters.
        /// </value>
        public List<TrackedVariableReference> ReturnedMethodParameters
        {
            get
            {
                return _returnedMethodParameters;
            }
        }

        /// <summary>
        /// Gets the returning method parameters.
        /// </summary>
        /// <value>
        /// The returning method parameters.
        /// </value>
        public List<TrackedVariableReference> ReturningMethodParameters
        {
            get
            {
                return _returningMethodParameters;
            }
        }

        /// <summary>
        ///     Gets or sets the this reference.
        /// </summary>
        /// <value>
        ///     The this reference.
        /// </value>
        public TrackedVariableReference ThisReference { get; set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            DisposeInternal(true);
        }

        #endregion

        #region Protected Methods and Operators

        /// <summary>
        ///     Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        protected virtual void DisposeInternal(bool isDisposing)
        {
            if (_isDisposed == false)
            {
                if (isDisposing)
                {
                    foreach (var localReference in _localReferences)
                    {
                        localReference.Dispose();
                    }

                    ThisReference.Dispose();
                }

                _isDisposed = true;
            }
        }

        #endregion
    }
}