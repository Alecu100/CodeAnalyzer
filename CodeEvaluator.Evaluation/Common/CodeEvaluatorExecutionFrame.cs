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
        public EvaluatedObjectReference MemberAccessReference { get; set; }

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
        public Dictionary<int, EvaluatedObjectReference> PassedMethodParametersDeprecated { get; } =
            new Dictionary<int, EvaluatedObjectReference>();

        public EvaluatedMethodPassedParameters PassedMethodParameters { get; set; }

        /// <summary>
        ///     Gets the returning method parameters.
        /// </summary>
        /// <value>
        ///     The returning method parameters.
        /// </value>
        public EvaluatedObjectReference ReturningMethodParameters { get; } = new EvaluatedObjectReference();

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