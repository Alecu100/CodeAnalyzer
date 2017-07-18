namespace CodeAnalysis.Core.Evaluators
{
    using CodeAnalysis.Core.Common;

    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp.Syntax;

    #region Using

    #endregion

    public class EqualsValueClauseSyntaxEvaluator : BaseSyntaxNodeEvaluator
    {
        #region Protected Methods and Operators

        protected override void EvaluateSyntaxNodeInternal(
            SyntaxNode syntaxNode,
            CodeEvaluatorExecutionState workflowEvaluatorExecutionState)
        {
            var equalsValueClauseSyntax = (EqualsValueClauseSyntax) syntaxNode;

            var syntaxNodeEvaluator = SyntaxNodeEvaluatorFactory.GetSyntaxNodeEvaluator(equalsValueClauseSyntax.Value);

            if (syntaxNodeEvaluator != null)
            {
                syntaxNodeEvaluator.EvaluateSyntaxNode(equalsValueClauseSyntax.Value, workflowEvaluatorExecutionState);
            }
        }

        #endregion
    }
}