using System.Collections.Generic;
using Microsoft.CodeAnalysis;

namespace CodeAnalysis.Core.Members
{

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