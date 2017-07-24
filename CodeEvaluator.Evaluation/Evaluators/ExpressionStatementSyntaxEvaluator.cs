namespace CodeEvaluator.Evaluation.Evaluators
{
    using CodeEvaluator.Evaluation.Common;

    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp.Syntax;

    #region Using

    #endregion

    public class ExpressionStatementSyntaxEvaluator : BaseSyntaxNodeEvaluator
    {
        #region Protected Methods and Operators

        /// <summary>
        ///     Evaluates the syntax node.
        /// </summary>
        /// <param name="syntaxNode">The syntax node.</param>
        /// <param name="workflowEvaluatorExecutionState">The workflow evaluator stack.</param>
        protected override void EvaluateSyntaxNodeInternal(
            SyntaxNode syntaxNode,
            CodeEvaluatorExecutionState workflowEvaluatorExecutionState)
        {
            var expressionStatementSyntax = (ExpressionStatementSyntax) syntaxNode;
            var syntaxNodeEvaluator =
                SyntaxNodeEvaluatorFactory.GetSyntaxNodeEvaluator(expressionStatementSyntax.Expression, EEvaluatorActions.None);

            if (syntaxNodeEvaluator != null)
            {
                syntaxNodeEvaluator.EvaluateSyntaxNode(expressionStatementSyntax.Expression, workflowEvaluatorExecutionState);
            }
        }

        #endregion
    }
}