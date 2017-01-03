//  Project              : GLP
//  Module               : RomSoft.Client.Debug.dll
//  File                 : TrackedVariable.cs
//  Author               : Alecsandru
//  Last Updated         : 14/12/2015 at 18:00
//  
// 
//  Contains             : Implementation of the TrackedVariable.cs class.
//  Classes              : TrackedVariable.cs
// 
//  
//  ----------------------------------------------------------------------- 
//   <copyright file="TrackedVariable.cs" company="Sysmex"> 
//       Copyright (c) Sysmex. All rights reserved. 
//   </copyright> 
//  -----------------------------------------------------------------------

namespace RomSoft.Client.Debug.Library.Members
{
    #region Using

    using System;
    using System.Collections.Generic;

    using Microsoft.CodeAnalysis;

    using RomSoft.Client.Debug.Library.Common;
    using RomSoft.Client.Debug.Library.Interfaces;

    using StructureMap;

    #endregion

    public class TrackedVariable : IDisposable
    {
        #region Fields

        private bool _isDisposed;

        private ITrackedVariablesHeap _parentHeap;

        private TrackedTypeInfo _typeInfo;

        private readonly List<TrackedVariableReference> _fields = new List<TrackedVariableReference>();

        private readonly List<TrackedVariableHistory> _history = new List<TrackedVariableHistory>();

        private readonly List<TrackedVariableReference> _references = new List<TrackedVariableReference>();

        private List<TrackedProperty> _properties;

        #endregion

        #region Constructors and Destructors

        public TrackedVariable()
        {
            ObjectFactory.BuildUp(this);
        }

        ~TrackedVariable()
        {
            DisposeInternal(false);
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets the member variables.
        /// </summary>
        /// <value>
        ///     The member variables.
        /// </value>
        public List<TrackedVariableReference> Fields
        {
            get
            {
                return _fields;
            }
        }

        /// <summary>
        /// Gets the properties.
        /// </summary>
        /// <value>
        /// The properties.
        /// </value>
        public List<TrackedProperty> Properties
        {
            get
            {
                if (_properties == null)
                {
                    _properties = new List<TrackedProperty>();
                }

                return _properties;
            }
        }

        /// <summary>
        ///     Gets the history.
        /// </summary>
        /// <value>
        ///     The history.
        /// </value>
        public IReadOnlyList<TrackedVariableHistory> History
        {
            get
            {
                return _history;
            }
        }

        /// <summary>
        ///     Gets or sets the parent heap.
        /// </summary>
        /// <value>
        ///     The parent heap.
        /// </value>
        public ITrackedVariablesHeap ParentHeap
        {
            get
            {
                return _parentHeap;
            }
            set
            {
                _parentHeap = value;
            }
        }

        /// <summary>
        ///     Gets or sets the references.
        /// </summary>
        /// <value>
        ///     The references.
        /// </value>
        public IReadOnlyList<TrackedVariableReference> References
        {
            get
            {
                return _references;
            }
        }

        /// <summary>
        ///     Gets or sets the type information.
        /// </summary>
        /// <value>
        ///     The type information.
        /// </value>
        public TrackedTypeInfo TypeInfo
        {
            get
            {
                return _typeInfo;
            }
            set
            {
                _typeInfo = value;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Adds the reference.
        /// </summary>
        /// <param name="reference">The reference.</param>
        public void AddReference(TrackedVariableReference reference)
        {
            _references.Add(reference);
        }

        /// <summary>
        ///     Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            DisposeInternal(true);
        }

        /// <summary>
        ///     Pushes the history.
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <param name="executionFrame">The execution frame.</param>
        public void PushHistory(SyntaxNode expression, StaticWorkflowEvaluatorExecutionFrame executionFrame)
        {
            var trackedVariableHistory = new TrackedVariableHistory
                                             {
                                                 ExecutionFrame = executionFrame,
                                                 SyntaxNode = expression
                                             };
            _history.Add(trackedVariableHistory);
        }

        /// <summary>
        ///     Removes the reference.
        /// </summary>
        /// <param name="reference">The reference.</param>
        public void RemoveReference(TrackedVariableReference reference)
        {
            _references.Remove(reference);

            if (_references.Count == 0)
            {
                Dispose();

                _parentHeap.Remove(this);
            }
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
                    foreach (var trackedVariable in _references)
                    {
                        trackedVariable.Dispose();
                    }
                }

                _isDisposed = true;
            }
        }

        #endregion
    }
}