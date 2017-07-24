﻿namespace CodeEvaluator.Evaluation.Evaluators
{
    using CodeEvaluator.Evaluation.Common;

    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp.Syntax;

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
                    SyntaxNodeEvaluatorFactory.GetSyntaxNodeEvaluator(returnStatementSyntax.Expression, EEvaluatorActions.None);

                if (syntaxNodeEvaluator != null)
                {
                    syntaxNodeEvaluator.EvaluateSyntaxNode(returnStatementSyntax.Expression, workflowEvaluatorExecutionState);
                }

                if (workflowEvaluatorExecutionState.CurrentExecutionFrame.MemberAccessReference != null)
                {
                    workflowEvaluatorExecutionState.CurrentExecutionFrame.ReturningMethodParameters.AssignEvaluatedObject(
                        workflowEvaluatorExecutionState.CurrentExecutionFrame.MemberAccessReference);

                    workflowEvaluatorExecutionState.CurrentExecutionFrame.MemberAccessReference = null;
                }
            }
        }

        #endregion
    }
}