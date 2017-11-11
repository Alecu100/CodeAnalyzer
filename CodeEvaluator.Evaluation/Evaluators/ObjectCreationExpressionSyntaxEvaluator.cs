using System.Collections.Generic;
using System.Linq;
using CodeEvaluator.Evaluation.Common;
using CodeEvaluator.Evaluation.Extensions;
using CodeEvaluator.Evaluation.Interfaces;
using CodeEvaluator.Evaluation.Members;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using StructureMap;

namespace CodeEvaluator.Evaluation.Evaluators
{
    public class ObjectCreationExpressionSyntaxEvaluator : SyntaxNodeEvaluator
    {
        public ObjectCreationExpressionSyntaxEvaluator()
        {
            ObjectFactory.BuildUp(this);
        }

        public IMethodInvocationResolver MethodInvocationResolver { get; set; }

        #region Protected Methods and Operators

        protected override void EvaluateSyntaxNodeInternal(
            SyntaxNode syntaxNode,
            CodeEvaluatorExecutionStack workflowEvaluatorExecutionStack)
        {
            var objectCreationExpressionSyntax = (ObjectCreationExpressionSyntax) syntaxNode;

            var syntaxNodeEvaluator =
                SyntaxNodeEvaluatorFactory.GetSyntaxNodeEvaluator(objectCreationExpressionSyntax.Type,
                    EEvaluatorActions.GetConstructor);

            if (syntaxNodeEvaluator != null)
            {
                syntaxNodeEvaluator.EvaluateSyntaxNode(objectCreationExpressionSyntax.Type,
                    workflowEvaluatorExecutionStack);

                if (CanInvokeMethod(workflowEvaluatorExecutionStack))
                {
                    var accessedReferenceMember =
                        workflowEvaluatorExecutionStack.CurrentExecutionFrame.MemberAccessReference;

                    var mandatoryParamenters = new List<EvaluatedObjectReference>();
                    var optionalParameters = new Dictionary<string, EvaluatedObjectReference>();

                    for (var i = 0; i < objectCreationExpressionSyntax.ArgumentList.Arguments.Count; i++)
                    {
                        var argumentSyntax = objectCreationExpressionSyntax.ArgumentList.Arguments[i];

                        var nodeEvaluator = SyntaxNodeEvaluatorFactory.GetSyntaxNodeEvaluator(
                            argumentSyntax.Expression,
                            EEvaluatorActions.GetMember);

                        if (nodeEvaluator != null)
                        {
                            workflowEvaluatorExecutionStack.CurrentExecutionFrame.MemberAccessReference = null;

                            nodeEvaluator.EvaluateSyntaxNode(
                                argumentSyntax.Expression,
                                workflowEvaluatorExecutionStack);
                        }

                        if (argumentSyntax.NameColon != null)
                            optionalParameters.Add(
                                argumentSyntax.NameColon.Name.Identifier.ValueText,
                                workflowEvaluatorExecutionStack.CurrentExecutionFrame.MemberAccessReference);
                        else
                            mandatoryParamenters.Add(
                                workflowEvaluatorExecutionStack.CurrentExecutionFrame.MemberAccessReference);
                    }

                    workflowEvaluatorExecutionStack.CurrentExecutionFrame.MemberAccessReference = null;

                    foreach (var evaluatedObject in accessedReferenceMember.EvaluatedObjects)
                    {
                        var evaluatedDelegate = (EvaluatedInvokableObject) evaluatedObject;

                        var methodInvocationResolverResult =
                            MethodInvocationResolver.ResolveMethodInvocation(
                                evaluatedDelegate.TargetMethodGroup, null, evaluatedDelegate.TypeInfo,
                                mandatoryParamenters,
                                optionalParameters);

                        if (methodInvocationResolverResult.CanInvokeMethod)
                        {
                            workflowEvaluatorExecutionStack.CurrentExecutionFrame.PassedMethodParameters.Clear();

                            foreach (var evaluatedObjectReferenceBase in methodInvocationResolverResult
                                .ResolvedPassedParameters)
                                workflowEvaluatorExecutionStack.CurrentExecutionFrame.PassedMethodParameters[
                                    evaluatedObjectReferenceBase.Key] = evaluatedObjectReferenceBase.Value;

                            var methodEvaluator =
                                SyntaxNodeEvaluatorFactory.GetSyntaxNodeEvaluator(
                                    methodInvocationResolverResult.ResolvedMethod.Declaration,
                                    EEvaluatorActions.None);

                            if (methodEvaluator != null)
                                methodEvaluator.EvaluateSyntaxNode(
                                    methodInvocationResolverResult.ResolvedMethod.Declaration,
                                    workflowEvaluatorExecutionStack);
                        }
                    }
                }
            }
        }

        private static bool CanInvokeMethod(CodeEvaluatorExecutionStack workflowEvaluatorExecutionStack)
        {
            return workflowEvaluatorExecutionStack.CurrentExecutionFrame.MemberAccessReference.IsNotNull()
                   && workflowEvaluatorExecutionStack.CurrentExecutionFrame.MemberAccessReference.EvaluatedObjects.All(
                       o => o is EvaluatedInvokableObject);
        }

        #endregion
    }
}