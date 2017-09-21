namespace CodeEvaluator.Evaluation.Evaluators
{
    using CodeEvaluator.Evaluation.Common;
    using CodeEvaluator.Evaluation.Members;

    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp.Syntax;

    public class ObjectCreationExpressionSyntaxEvaluator : SyntaxNodeEvaluator
    {
        #region Protected Methods and Operators

        protected override void EvaluateSyntaxNodeInternal(
            SyntaxNode syntaxNode,
            CodeEvaluatorExecutionStack workflowEvaluatorExecutionStack)
        {

            var objectCreationExpressionSyntax = (ObjectCreationExpressionSyntax) syntaxNode;

            var syntaxNodeEvaluator =
                SyntaxNodeEvaluatorFactory.GetSyntaxNodeEvaluator(objectCreationExpressionSyntax.Type, EEvaluatorActions.GetConstructor);

            if (syntaxNodeEvaluator != null)
            {
                syntaxNodeEvaluator.EvaluateSyntaxNode(objectCreationExpressionSyntax.Type,
                    workflowEvaluatorExecutionStack);

                if (workflowEvaluatorExecutionStack.CurrentExecutionFrame.MemberAccessReference != null)
                {
                    var accessedReferenceMember =
                        workflowEvaluatorExecutionStack.CurrentExecutionFrame.MemberAccessReference;

                    foreach (var evaluatedObject in accessedReferenceMember.EvaluatedObjects)
                    {
                        if (!(evaluatedObject is EvaluatedInvokableObject))
                        {
                            continue;
                        }

                        var evaluatedDelegate = (EvaluatedInvokableObject) evaluatedObject;

                        var currentMethod =
                            evaluatedDelegate.Method;

                        if (currentMethod == null)
                        {
                            continue;
                        }

                        for (var i = 0; i < objectCreationExpressionSyntax.ArgumentList.Arguments.Count; i++)
                        {
                            var argumentSyntax = objectCreationExpressionSyntax.ArgumentList.Arguments[i];

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

                        var constructorEvaluator =
                            SyntaxNodeEvaluatorFactory.GetSyntaxNodeEvaluator(currentMethod.Declaration, EEvaluatorActions.None);

                        if (constructorEvaluator != null)
                        {
                            constructorEvaluator.EvaluateSyntaxNode(currentMethod.Declaration,
                                workflowEvaluatorExecutionStack);
                        }
                    }
                }
            }
        }

        #endregion
    }
}