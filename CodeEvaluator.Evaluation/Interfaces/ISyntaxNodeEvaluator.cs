namespace CodeEvaluator.Evaluation.Interfaces
{
    using CodeEvaluator.Evaluation.Common;

    using Microsoft.CodeAnalysis;

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