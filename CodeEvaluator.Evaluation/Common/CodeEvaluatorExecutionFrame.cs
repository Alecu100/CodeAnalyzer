using System.Collections.Generic;
using CodeEvaluator.Evaluation.Members;
using Microsoft.CodeAnalysis;

namespace CodeEvaluator.Evaluation.Common
{
    #region Using

    #endregion

    public class CodeEvaluatorExecutionFrame
    {
        #region Public Properties

        /// <summary>
        ///     Gets or sets the accessed reference.
        /// </summary>
        /// <value>
        ///     The accessed reference.
        /// </value>
        public EvaluatedObjectReferenceBase MemberAccessReference { get; set; }

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
        public List<EvaluatedObjectReferenceBase> LocalReferences { get; } = new List<EvaluatedObjectReferenceBase>();

        /// <summary>
        ///     Gets or sets the stack variables.
        /// </summary>
        /// <value>
        ///     The stack variables.
        /// </value>
        public Dictionary<int, EvaluatedObjectReferenceBase> PassedMethodParameters { get; } =
            new Dictionary<int, EvaluatedObjectReferenceBase>();

        /// <summary>
        ///     Gets the returning method parameters.
        /// </summary>
        /// <value>
        ///     The returning method parameters.
        /// </value>
        public EvaluatedObjectReferenceBase ReturningMethodParameters { get; set; }

        /// <summary>
        ///     Gets or sets the this reference.
        /// </summary>
        /// <value>
        ///     The this reference.
        /// </value>
        public EvaluatedObjectReferenceBase ThisReference { get; set; }

        #endregion
    }
}