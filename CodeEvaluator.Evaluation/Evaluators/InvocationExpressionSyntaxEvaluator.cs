namespace CodeEvaluator.Evaluation.Evaluators
{
    using System.Linq;

    using CodeEvaluator.Evaluation.Common;
    using CodeEvaluator.Evaluation.Members;

    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp.Syntax;

    #region Using

    #endregion

    public class InvocationExpressionSyntaxEvaluator : BaseSyntaxNodeEvaluator
    {
        #region Protected Methods and Operators

        /// <summary>
        ///     Evaluates the syntax node.
        /// </summary>
        /// <param name="syntaxNode">The syntax node.</param>
        /// <param name="workflowEvaluatorExecutionStack">The workflow evaluator stack.</param>
        protected override void EvaluateSyntaxNodeInternal(
            SyntaxNode syntaxNode,
            CodeEvaluatorExecutionStack workflowEvaluatorExecutionStack)
        {
            var invocationExpressionSyntax = (InvocationExpressionSyntax) syntaxNode;

            if (invocationExpressionSyntax.Expression != null)
            {
                var syntaxNodeEvaluator =
                    SyntaxNodeEvaluatorFactory.GetSyntaxNodeEvaluator(invocationExpressionSyntax.Expression, EEvaluatorActions.GetMethod);

                if (syntaxNodeEvaluator != null)
                {
                    syntaxNodeEvaluator.EvaluateSyntaxNode(
                        invocationExpressionSyntax.Expression,
                        workflowEvaluatorExecutionStack);


                    if (workflowEvaluatorExecutionStack.CurrentExecutionFrame.MemberAccessReference != null)
                    {
                        var accessedReferenceMember =
                            workflowEvaluatorExecutionStack.CurrentExecutionFrame.MemberAccessReference;

                        foreach (var evaluatedObject in accessedReferenceMember.EvaluatedObjects)
                        {
                            if (!(evaluatedObject is EvaluatedDelegate))
                            {
                                continue;
                            }

                            var evaluatedDelegate = (EvaluatedDelegate) evaluatedObject;

                            var currentMethod =
                                evaluatedDelegate.Method;

                            if (currentMethod == null)
                            {
                                continue;
                            }

                            workflowEvaluatorExecutionStack.CurrentExecutionFrame.PassedMethodParameters[-1] =
                                evaluatedDelegate.Fields.First();

                            for (var i = 0; i < invocationExpressionSyntax.ArgumentList.Arguments.Count; i++)
                            {
                                var argumentSyntax = invocationExpressionSyntax.ArgumentList.Arguments[i];

                                var nodeEvaluator = SyntaxNodeEvaluatorFactory.GetSyntaxNodeEvaluator(argumentSyntax, EEvaluatorActions.GetMember);

                                if (nodeEvaluator != null)
                                {
                                    workflowEvaluatorExecutionStack.CurrentExecutionFrame.MemberAccessReference = null;

                                    nodeEvaluator.EvaluateSyntaxNode(
                                        argumentSyntax.Expression,
                                        workflowEvaluatorExecutionStack);
                                }

                                if (workflowEvaluatorExecutionStack.CurrentExecutionFrame.MemberAccessReference != null)
                                {
                                    workflowEvaluatorExecutionStack.CurrentExecutionFrame.PassedMethodParameters[i] =
                                        workflowEvaluatorExecutionStack.CurrentExecutionFrame.MemberAccessReference;
                                }
                            }

                            workflowEvaluatorExecutionStack.CurrentExecutionFrame.MemberAccessReference = null;

                            var methodEvaluator =
                                SyntaxNodeEvaluatorFactory.GetSyntaxNodeEvaluator(currentMethod.Declaration, EEvaluatorActions.None);

                            if (methodEvaluator != null)
                            {
                                methodEvaluator.EvaluateSyntaxNode(currentMethod.Declaration, workflowEvaluatorExecutionStack);
                            }
                        }
                    }
                }
            }
        }

        #endregion
    }
}