namespace CodeEvaluator.Evaluation.Common
{
    using System.Collections.Generic;

    using global::CodeEvaluator.Evaluation.Members;

    using Microsoft.CodeAnalysis;

    #region Using

    #endregion

    public class CodeEvaluatorExecutionFrame
    {
        private readonly EvaluatedObjectReference _returningMethodParameters = new EvaluatedObjectReference();

        private EvaluatedObjectReference _memberAccessReference;

        #region Public Properties

        /// <summary>
        ///     Gets or sets the accessed reference.
        /// </summary>
        /// <value>
        ///     The accessed reference.
        /// </value>
        public EvaluatedObjectReference MemberAccessReference
        {
            get { return _memberAccessReference; }
            set { _memberAccessReference = value; }
        }

        /// <summary>
        ///     Gets or sets the current method.
        /// </summary>
        /// <value>
        ///     The current method.
        /// </value>
        public EvaluatedMethodBase CurrentMethod { get; set; }

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
        public List<EvaluatedObjectReference> LocalReferences { get; } = new List<EvaluatedObjectReference>();

        /// <summary>
        ///     Gets or sets the stack variables.
        /// </summary>
        /// <value>
        ///     The stack variables.
        /// </value>
        public Dictionary<int, EvaluatedObjectReference> PassedMethodParameters { get; } =
            new Dictionary<int, EvaluatedObjectReference>();

        /// <summary>
        ///     Gets the returning method parameters.
        /// </summary>
        /// <value>
        ///     The returning method parameters.
        /// </value>
        public EvaluatedObjectReference ReturningMethodParameters
        {
            get { return _returningMethodParameters; }
        }

        /// <summary>
        ///     Gets or sets the this reference.
        /// </summary>
        /// <value>
        ///     The this reference.
        /// </value>
        public EvaluatedObjectReference ThisReference { get; set; }

        #endregion
    }
}