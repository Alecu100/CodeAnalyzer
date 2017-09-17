using System.Collections.Generic;
using System.Linq;
using CodeEvaluator.Evaluation.Common;
using CodeEvaluator.Evaluation.Interfaces;
using CodeEvaluator.Evaluation.Members;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using StructureMap;

namespace CodeEvaluator.Evaluation.Evaluators
{
    #region Using

    #endregion

    public class InvocationExpressionSyntaxEvaluator : BaseSyntaxNodeEvaluator
    {
        public InvocationExpressionSyntaxEvaluator()
        {
            ObjectFactory.BuildUp(this);
        }

        public IMethodInvocationResolver MethodInvocationResolver { get; set; }

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
                    SyntaxNodeEvaluatorFactory.GetSyntaxNodeEvaluator(
                        invocationExpressionSyntax.Expression,
                        EEvaluatorActions.GetMethod);

                if (syntaxNodeEvaluator != null)
                {
                    syntaxNodeEvaluator.EvaluateSyntaxNode(
                        invocationExpressionSyntax.Expression,
                        workflowEvaluatorExecutionStack);

                    if (CanInvokeMethod(workflowEvaluatorExecutionStack))
                    {
                        var accessedReferenceMember =
                            workflowEvaluatorExecutionStack.CurrentExecutionFrame.MemberAccessReference;

                        var mandatoryParamenters = new List<EvaluatedObjectReferenceBase>();
                        var optionalParameters = new Dictionary<string, EvaluatedObjectReferenceBase>();

                        for (var i = 0; i < invocationExpressionSyntax.ArgumentList.Arguments.Count; i++)
                        {
                            var argumentSyntax = invocationExpressionSyntax.ArgumentList.Arguments[i];

                            var nodeEvaluator = SyntaxNodeEvaluatorFactory.GetSyntaxNodeEvaluator(
                                argumentSyntax,
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
                            var evaluatedDelegate = (EvaluatedDelegate) evaluatedObject;

                            var methodInvocationResolverResult =
                                MethodInvocationResolver.ResolveMethodInvocation(
                                    evaluatedDelegate,
                                    mandatoryParamenters,
                                    optionalParameters);

                            if (methodInvocationResolverResult.CanInvokeMethod)
                            {
                                workflowEvaluatorExecutionStack.CurrentExecutionFrame.PassedMethodParameters.Clear();

                                foreach (var evaluatedObjectReferenceBase in methodInvocationResolverResult
                                    .ResolvedPassedParameters)
                                {
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
            }
        }

        private static bool CanInvokeMethod(CodeEvaluatorExecutionStack workflowEvaluatorExecutionStack)
        {
            return workflowEvaluatorExecutionStack.CurrentExecutionFrame.MemberAccessReference != null
                   && workflowEvaluatorExecutionStack.CurrentExecutionFrame.MemberAccessReference.EvaluatedObjects.All(
                       o => o is EvaluatedDelegate);
        }

        #endregion
    }
}