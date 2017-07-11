using CodeAnalysis.Core.Common;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CodeAnalysis.Core.SyntaxNodeEvaluators
{

    #region Using

    #endregion

    public class ReturnStatementSyntaxEvaluator : BaseSyntaxNodeEvaluator
    {
        #region Protected Methods and Operators

        protected override void EvaluateSyntaxNodeInternal(
            SyntaxNode syntaxNode,
            CodeEvaluatorExecutionState workflowEvaluatorExecutionState)
        {
            var returnStatementSyntax = (ReturnStatementSyntax) syntaxNode;

            if (returnStatementSyntax.Expression != null)
            {
                var syntaxNodeEvaluator =
                    SyntaxNodeEvaluatorFactory.GetSyntaxNodeEvaluator(returnStatementSyntax.Expression);

                if (syntaxNodeEvaluator != null)
                {
                    syntaxNodeEvaluator.EvaluateSyntaxNode(returnStatementSyntax.Expression, workflowEvaluatorExecutionState);
                }

                if (workflowEvaluatorExecutionState.CurrentExecutionFrame.MemberAccessResult != null)
                {
                    workflowEvaluatorExecutionState.CurrentExecutionFrame.ReturningMethodParameters.AssignEvaluatedObject(
                        workflowEvaluatorExecutionState.CurrentExecutionFrame.MemberAccessResult);

                    workflowEvaluatorExecutionState.CurrentExecutionFrame.MemberAccessResult = null;
                }
            }
        }

        #endregion
    }
}