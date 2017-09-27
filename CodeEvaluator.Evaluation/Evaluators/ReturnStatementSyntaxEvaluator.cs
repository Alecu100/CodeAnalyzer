namespace CodeEvaluator.Evaluation.Evaluators
{
    using CodeEvaluator.Evaluation.Common;
    using CodeEvaluator.Evaluation.Extensions;

    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp.Syntax;

    #region Using

    #endregion

    public class ReturnStatementSyntaxEvaluator : SyntaxNodeEvaluator
    {
        #region Protected Methods and Operators

        protected override void EvaluateSyntaxNodeInternal(
            SyntaxNode syntaxNode,
            CodeEvaluatorExecutionStack workflowEvaluatorExecutionStack)
        {
            var returnStatementSyntax = (ReturnStatementSyntax) syntaxNode;

            if (returnStatementSyntax.Expression != null)
            {
                var syntaxNodeEvaluator =
                    SyntaxNodeEvaluatorFactory.GetSyntaxNodeEvaluator(returnStatementSyntax.Expression, EEvaluatorActions.GetMember);

                workflowEvaluatorExecutionStack.CurrentExecutionFrame.MemberAccessReference = null;

                if (syntaxNodeEvaluator != null)
                {
                    syntaxNodeEvaluator.EvaluateSyntaxNode(returnStatementSyntax.Expression, workflowEvaluatorExecutionStack);
                }

                if (workflowEvaluatorExecutionStack.CurrentExecutionFrame.MemberAccessReference.IsNotNull())
                {
                    workflowEvaluatorExecutionStack.CurrentExecutionFrame.ReturningMethodParameters.AssignEvaluatedObject(
                        workflowEvaluatorExecutionStack.CurrentExecutionFrame.MemberAccessReference);

                    workflowEvaluatorExecutionStack.CurrentExecutionFrame.MemberAccessReference = null;
                }
            }
        }

        #endregion
    }
}