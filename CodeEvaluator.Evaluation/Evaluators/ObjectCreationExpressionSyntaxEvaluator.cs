namespace CodeEvaluator.Evaluation.Evaluators
{
    using CodeEvaluator.Evaluation.Common;
    using CodeEvaluator.Evaluation.Members;

    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp.Syntax;

    public class ObjectCreationExpressionSyntaxEvaluator : BaseSyntaxNodeEvaluator
    {
        #region Protected Methods and Operators

        protected override void EvaluateSyntaxNodeInternal(
            SyntaxNode syntaxNode,
            CodeEvaluatorExecutionState workflowEvaluatorExecutionState)
        {

            var objectCreationExpressionSyntax = (ObjectCreationExpressionSyntax) syntaxNode;

            var syntaxNodeEvaluator =
                SyntaxNodeEvaluatorFactory.GetSyntaxNodeEvaluator(objectCreationExpressionSyntax.Type, EEvaluatorActions.GetConstructor);

            if (syntaxNodeEvaluator != null)
            {
                syntaxNodeEvaluator.EvaluateSyntaxNode(objectCreationExpressionSyntax.Type,
                    workflowEvaluatorExecutionState);

                if (workflowEvaluatorExecutionState.CurrentExecutionFrame.MemberAccessReference != null)
                {
                    var accessedReferenceMember =
                        workflowEvaluatorExecutionState.CurrentExecutionFrame.MemberAccessReference;

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

                        for (var i = 0; i < objectCreationExpressionSyntax.ArgumentList.Arguments.Count; i++)
                        {
                            var argumentSyntax = objectCreationExpressionSyntax.ArgumentList.Arguments[i];

                            var nodeEvaluator = SyntaxNodeEvaluatorFactory.GetSyntaxNodeEvaluator(argumentSyntax, EEvaluatorActions.GetMember);

                            if (nodeEvaluator != null)
                            {
                                workflowEvaluatorExecutionState.CurrentExecutionFrame.MemberAccessReference = null;

                                nodeEvaluator.EvaluateSyntaxNode(
                                    argumentSyntax.Expression,
                                    workflowEvaluatorExecutionState);
                            }

                            if (workflowEvaluatorExecutionState.CurrentExecutionFrame.MemberAccessReference != null)
                            {
                                workflowEvaluatorExecutionState.CurrentExecutionFrame.PassedMethodParameters[i] =
                                    workflowEvaluatorExecutionState.CurrentExecutionFrame.MemberAccessReference;
                            }
                        }


                        workflowEvaluatorExecutionState.CurrentExecutionFrame.MemberAccessReference = null;

                        var constructorEvaluator =
                            SyntaxNodeEvaluatorFactory.GetSyntaxNodeEvaluator(currentMethod.Declaration, EEvaluatorActions.None);

                        if (constructorEvaluator != null)
                        {
                            constructorEvaluator.EvaluateSyntaxNode(currentMethod.Declaration,
                                workflowEvaluatorExecutionState);
                        }
                    }
                }
            }
        }

        #endregion
    }
}