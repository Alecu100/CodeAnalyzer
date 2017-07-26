namespace CodeEvaluator.Evaluation.Listeners
{
    using System.Linq;

    using CodeEvaluator.Evaluation.Common;
    using CodeEvaluator.Evaluation.Interfaces;
    using CodeEvaluator.Workflows;

    using Microsoft.CodeAnalysis.CSharp.Syntax;

    #region Using

    #endregion

    public class WorkflowEvaluatorEvaluatorListener : ISyntaxNodeEvaluatorListener
    {
        #region Constructors and Destructors

        public WorkflowEvaluatorEvaluatorListener()
        {
            WorkflowEvaluator.Initialize();
        }

        #endregion

        public void OnBeforeSyntaxNodeEvaluated(
            ISyntaxNodeEvaluator syntaxNodeEvaluator,
            SyntaxNodeEvaluatorListenerArgs args)
        {
            var methodCallInvocationExpression = args.EvaluatedSyntaxNode as InvocationExpressionSyntax;

            if (methodCallInvocationExpression == null)
            {
                return;
            }

            var syntaxNodes = methodCallInvocationExpression.ChildNodes().ToList();

            if (syntaxNodes.Count == 2)
            {
                if (syntaxNodes[0] is MemberAccessExpressionSyntax)
                {
                    var memberAccessExpression = (MemberAccessExpressionSyntax)syntaxNodes[0];
                    var methodAndClass = memberAccessExpression.ChildNodes().ToList();

                    if (methodAndClass.Count == 2)
                    {
                        var classIdentifier = methodAndClass[0] as IdentifierNameSyntax;
                        var methodIdentifier = methodAndClass[1] as IdentifierNameSyntax;

                        if (classIdentifier != null && methodIdentifier != null
                            && classIdentifier.Identifier.ValueText == "WorkflowEvaluator")
                        {
                            switch (methodIdentifier.Identifier.ValueText)
                            {
                                case "AddProcess":
                                    ExecuteAddProcess(methodCallInvocationExpression);
                                    break;
                                case "AddDecision":
                                    ExecuteAddDecision(methodCallInvocationExpression);
                                    break;
                                case "BeginWorkflow":
                                    ExecuteBeginScope();
                                    break;
                                case "EndWorkflow":
                                    ExecuteEndScope();
                                    break;
                                case "StopWorkflow":
                                    ExecuteStopScope();
                                    break;
                            }
                        }
                    }
                }
            }
        }

        public void OnAfterSyntaxNodeEvaluated(
            ISyntaxNodeEvaluator syntaxNodeEvaluator,
            SyntaxNodeEvaluatorListenerArgs args)
        {
        }

        #region Private Methods and Operators

        private void ExecuteAddDecision(InvocationExpressionSyntax methodCallInvocationExpression)
        {
            var arguments = methodCallInvocationExpression.ArgumentList.Arguments;
            string name = null;
            string description = null;

            if (arguments.Count == 2)
            {
                var nameArgument = arguments[0].ChildNodes().FirstOrDefault();

                if (nameArgument is LiteralExpressionSyntax)
                {
                    name = nameArgument.GetText().ToString().Trim('\"');
                }

                var descriptionArgument = arguments[1].ChildNodes().FirstOrDefault();

                if (descriptionArgument is LiteralExpressionSyntax)
                {
                    description = descriptionArgument.GetText().ToString().Trim('\"');
                }
            }

            if (name != null)
            {
                WorkflowEvaluator.AddDecision(name, description);
            }
        }

        private void ExecuteAddProcess(InvocationExpressionSyntax methodCallInvocationExpression)
        {
            var arguments = methodCallInvocationExpression.ArgumentList.Arguments;
            string name = null;
            string description = null;

            if (arguments.Count == 2)
            {
                var nameArgument = arguments[0].ChildNodes().FirstOrDefault();

                if (nameArgument is LiteralExpressionSyntax)
                {
                    name = nameArgument.GetText().ToString().Trim('\"');
                }

                var descriptionArgument = arguments[1].ChildNodes().FirstOrDefault();

                if (descriptionArgument is LiteralExpressionSyntax)
                {
                    description = descriptionArgument.GetText().ToString().Trim('\"');
                }
            }

            if (name != null)
            {
                WorkflowEvaluator.AddProcess(name, description);
            }
        }

        private void ExecuteBeginScope()
        {
            WorkflowEvaluator.BeginWorkflow();
        }

        private void ExecuteEndScope()
        {
            WorkflowEvaluator.EndWorkflow();
        }

        private void ExecuteStopScope()
        {
            WorkflowEvaluator.StopWorkflow();
        }

        #endregion
    }
}