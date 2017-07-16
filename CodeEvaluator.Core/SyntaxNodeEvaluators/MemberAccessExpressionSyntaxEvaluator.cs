using CodeAnalysis.Core.Common;
using CodeAnalysis.Core.Enums;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CodeAnalysis.Core.SyntaxNodeEvaluators
{

    #region Using

    #endregion

    public class MemberAccessExpressionSyntaxEvaluator : BaseSyntaxNodeEvaluator
    {
        #region Protected Methods and Operators

        protected override void EvaluateSyntaxNodeInternal(
            SyntaxNode syntaxNode,
            CodeEvaluatorExecutionState workflowEvaluatorExecutionState)
        {
            var memberAccessExpressionSyntax = (MemberAccessExpressionSyntax) syntaxNode;

            var syntaxNodeEvaluator =
                SyntaxNodeEvaluatorFactory.GetSyntaxNodeEvaluator(memberAccessExpressionSyntax.Expression);

            if (syntaxNodeEvaluator != null)
            {
                workflowEvaluatorExecutionState.CurrentExecutionFrame.PushAction(EEvaluatorActions.GetMember);

                syntaxNodeEvaluator.EvaluateSyntaxNode(
                    memberAccessExpressionSyntax.Expression,
                    workflowEvaluatorExecutionState);

                workflowEvaluatorExecutionState.CurrentExecutionFrame.PopAction();
            }

            syntaxNodeEvaluator =
                SyntaxNodeEvaluatorFactory.GetSyntaxNodeEvaluator(memberAccessExpressionSyntax.Name);

            if (syntaxNodeEvaluator != null)
            {
                syntaxNodeEvaluator.EvaluateSyntaxNode(
                    memberAccessExpressionSyntax.Name,
                    workflowEvaluatorExecutionState);
            }
        }

        #endregion
    }
}