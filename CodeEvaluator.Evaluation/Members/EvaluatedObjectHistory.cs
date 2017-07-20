namespace CodeEvaluator.Evaluation.Members
{
    using System.Collections.Generic;

    using Microsoft.CodeAnalysis;

    #region Using

    #endregion

    public class EvaluatedObjectHistory
    {
        #region Public Properties

        /// <summary>
        ///     Gets or sets the syntax node.
        /// </summary>
        /// <value>
        ///     The syntax node.
        /// </value>
        public SyntaxNode SyntaxNode { get; set; }

        public List<SyntaxNode> SyntaxNodeStack { get; set; }

        #endregion
    }
}