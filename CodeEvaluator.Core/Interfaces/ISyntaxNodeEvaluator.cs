using CodeAnalysis.Core.Common;
using Microsoft.CodeAnalysis;

namespace CodeAnalysis.Core.Interfaces
{

    #region Using

    #endregion

    public interface ISyntaxNodeEvaluator
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Evaluates the syntax node.
        /// </summary>
        /// <param name="syntaxNode">The syntax node.</param>
        /// <param name="workflowEvaluatorExecutionState">The workflow evaluator stack.</param>
        void EvaluateSyntaxNode(SyntaxNode syntaxNode, CodeEvaluatorExecutionState workflowEvaluatorExecutionState);

        #endregion
    }
}