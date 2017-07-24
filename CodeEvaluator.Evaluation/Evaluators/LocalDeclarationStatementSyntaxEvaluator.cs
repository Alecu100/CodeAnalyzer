namespace CodeEvaluator.Evaluation.Evaluators
{
    using CodeEvaluator.Evaluation.Common;

    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp.Syntax;

    #region Using

    #endregion

    public class LocalDeclarationStatementSyntaxEvaluator : BaseSyntaxNodeEvaluator
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
            var localDeclarationStatementSyntax = (LocalDeclarationStatementSyntax) syntaxNode;

            var syntaxNodeEvaluator =
                SyntaxNodeEvaluatorFactory.GetSyntaxNodeEvaluator(localDeclarationStatementSyntax.Declaration, EEvaluatorActions.None);

            if (syntaxNodeEvaluator != null)
            {
                syntaxNodeEvaluator.EvaluateSyntaxNode(
                    localDeclarationStatementSyntax.Declaration,
                    workflowEvaluatorExecutionState);
            }
        }

        #endregion
    }
}