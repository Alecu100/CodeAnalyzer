namespace CodeEvaluator.Evaluation.Evaluators
{
    using CodeEvaluator.Evaluation.Common;

    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp.Syntax;

    public class ArgumentSyntaxEvaluator : SyntaxNodeEvaluator
    {
        protected override void EvaluateSyntaxNodeInternal(
            SyntaxNode syntaxNode,
            CodeEvaluatorExecutionStack workflowEvaluatorExecutionStack)
        {
            var argumentSyntax = (ArgumentSyntax)syntaxNode;

            if (argumentSyntax.Expression != null)
            {
                var syntaxNodeEvaluator = SyntaxNodeEvaluatorFactory.GetSyntaxNodeEvaluator(
                    argumentSyntax,
                    EEvaluatorActions.GetMember);

                if (syntaxNodeEvaluator != null)
                {
                    syntaxNodeEvaluator.EvaluateSyntaxNode(argumentSyntax.Expression, workflowEvaluatorExecutionStack);
                }
            }
        }
    }
}