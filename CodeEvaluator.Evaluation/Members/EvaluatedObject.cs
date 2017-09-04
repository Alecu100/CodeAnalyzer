namespace CodeEvaluator.Evaluation.Members
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using CodeEvaluator.Evaluation.Common;
    using CodeEvaluator.Evaluation.Interfaces;

    using StructureMap;

    #region Using

    #endregion

    public class EvaluatedObject
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Pushes the history.
        /// </summary>
        /// <param name="executionStack">The execution frame.</param>
        public void PushHistory(CodeEvaluatorExecutionStack executionStack)
        {
            var evaluatedObjectHistory = new EvaluatedObjectHistory
                                             {
                                                 SyntaxNode =
                                                     executionStack.CurrentExecutionFrame
                                                     .CurrentSyntaxNode,
                                                 SyntaxNodeStackSnaphot =
                                                     executionStack.SyntaxNodeStack.Skip(
                                                         Math.Max(
                                                             0,
                                                             executionStack.SyntaxNodeStack.Count
                                                             - executionStack.Parameters
                                                                   .EvaluatedObjectsHistoryLength))
                                                     .ToList()
                                             };

            _history.Add(evaluatedObjectHistory);
        }

        #endregion

        #region Constructors and Destructors

        public EvaluatedObject(EvaluatedTypeInfo typeInfo, List<EvaluatedObjectReference> fields)
        {
            ObjectFactory.BuildUp(this);

            _typeInfo = typeInfo;
            _fields.AddRange(fields);
        }

        protected EvaluatedObject(EvaluatedTypeInfo typeInfo)
        {
            ObjectFactory.BuildUp(this);

            _typeInfo = typeInfo;
        }

        protected EvaluatedObject()
        {
            ObjectFactory.BuildUp(this);
        }

        #endregion

        #region SpecificFields

        protected readonly List<EvaluatedObjectHistory> _history = new List<EvaluatedObjectHistory>();

        protected readonly List<EvaluatedObjectReference> _fields = new List<EvaluatedObjectReference>();

        protected EvaluatedTypeInfo _typeInfo;

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets the member variables.
        /// </summary>
        /// <value>
        ///     The member variables.
        /// </value>
        public virtual IReadOnlyList<EvaluatedObjectReference> Fields
        {
            get
            {
                return _fields.Union(_typeInfo.SharedStaticObject._fields).ToList();
            }
        }

        /// <summary>
        ///     Gets the history.
        /// </summary>
        /// <value>
        ///     The history.
        /// </value>
        public virtual List<EvaluatedObjectHistory> History
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
        public IEvaluatedObjectsHeap ParentHeap { get; set; }

        /// <summary>
        ///     Gets or sets the type information.
        /// </summary>
        /// <value>
        ///     The type information.
        /// </value>
        public virtual EvaluatedTypeInfo TypeInfo
        {
            get
            {
                return _typeInfo;
            }
        }

        #endregion
    }
}