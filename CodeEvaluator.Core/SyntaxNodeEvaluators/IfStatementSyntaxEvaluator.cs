using CodeAnalysis.Core.Common;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CodeAnalysis.Core.SyntaxNodeEvaluators
{
    #region Using

    

    #endregion

    public class IfStatementSyntaxEvaluator : BaseSyntaxNodeEvaluator
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
            var ifStatementSyntax = (IfStatementSyntax)syntaxNode;

            if (ifStatementSyntax.Condition != null)
            {
                var syntaxNodeEvaluator = SyntaxNodeEvaluatorFactory.GetSyntaxNodeEvaluator(ifStatementSyntax.Condition);

                if (syntaxNodeEvaluator != null)
                {
                    syntaxNodeEvaluator.EvaluateSyntaxNode(ifStatementSyntax.Condition, workflowEvaluatorExecutionState);
                }
            }

            if (ifStatementSyntax.Statement != null)
            {
                var syntaxNodeEvaluator = SyntaxNodeEvaluatorFactory.GetSyntaxNodeEvaluator(ifStatementSyntax.Statement);

                if (syntaxNodeEvaluator != null)
                {
                    syntaxNodeEvaluator.EvaluateSyntaxNode(ifStatementSyntax.Statement, workflowEvaluatorExecutionState);
                }
            }

            if (ifStatementSyntax.Else != null && ifStatementSyntax.Else.Statement != null)
            {
                var syntaxNodeEvaluator =
                    SyntaxNodeEvaluatorFactory.GetSyntaxNodeEvaluator(ifStatementSyntax.Else.Statement);

                if (syntaxNodeEvaluator != null)
                {
                    syntaxNodeEvaluator.EvaluateSyntaxNode(ifStatementSyntax.Else.Statement, workflowEvaluatorExecutionState);
                }
            }
        }

        #endregion
    }
}