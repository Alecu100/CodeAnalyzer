//  Project              : GLP
//  Module               : RomSoft.Client.Debug.dll
//  File                 : InvocationExpressionSyntaxEvaluator.cs
//  Author               : Alecsandru
//  Last Updated         : 11/02/2016 at 18:11
//  
// 
//  Contains             : Implementation of the InvocationExpressionSyntaxEvaluator.cs class.
//  Classes              : InvocationExpressionSyntaxEvaluator.cs
// 
//  
//  ----------------------------------------------------------------------- 
//   <copyright file="InvocationExpressionSyntaxEvaluator.cs" company="Sysmex"> 
//       Copyright (c) Sysmex. All rights reserved. 
//   </copyright> 
//  -----------------------------------------------------------------------

namespace RomSoft.Client.Debug.Library.SyntaxNodeEvaluators
{
    #region Using

    using System.Linq;

    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp.Syntax;

    using RomSoft.Client.Debug.Library.Common;
    using RomSoft.Client.Debug.Library.Members;

    #endregion

    public class InvocationExpressionSyntaxEvaluator : BaseSyntaxNodeEvaluator
    {
        #region Protected Methods and Operators

        /// <summary>
        ///     Evaluates the syntax node.
        /// </summary>
        /// <param name="syntaxNode">The syntax node.</param>
        /// <param name="workflowEvaluatorContext">The workflow evaluator stack.</param>
        protected override void EvaluateSyntaxNodeInternal(
            SyntaxNode syntaxNode,
            StaticWorkflowEvaluatorContext workflowEvaluatorContext)
        {
            var invocationExpressionSyntax = (InvocationExpressionSyntax)syntaxNode;

            foreach (var staticWorkflowListener in workflowEvaluatorContext.Parameters.Listeners)
            {
                staticWorkflowListener.BeforeMethodCalled(invocationExpressionSyntax);
            }

            if (invocationExpressionSyntax.Expression != null)
            {
                var syntaxNodeEvaluator =
                    SyntaxNodeEvaluatorFactory.GetSyntaxNodeEvaluator(invocationExpressionSyntax.Expression);

                if (syntaxNodeEvaluator != null)
                {
                    syntaxNodeEvaluator.EvaluateSyntaxNode(
                        invocationExpressionSyntax.Expression,
                        workflowEvaluatorContext);

                    TrackedVariableReference accessedReference = null;

                    if (workflowEvaluatorContext.CurrentExecutionFrame.AccessedReference != null)
                    {
                        accessedReference = workflowEvaluatorContext.CurrentExecutionFrame.AccessedReference.Move();
                    }
                    else if (workflowEvaluatorContext.CurrentExecutionFrame.ReturnedMethodParameters != null)
                    {
                        accessedReference = new TrackedVariableReference();

                        foreach (
                            var trackedVariableReference in
                                workflowEvaluatorContext.CurrentExecutionFrame.ReturnedMethodParameters)
                        {
                            accessedReference = accessedReference.Merge(trackedVariableReference);
                        }

                        workflowEvaluatorContext.CurrentExecutionFrame.ReturnedMethodParameters.Clear();
                    }
                    else
                    {
                        accessedReference = workflowEvaluatorContext.CurrentExecutionFrame.ThisReference;
                    }

                    if (accessedReference != null
                        && workflowEvaluatorContext.CurrentExecutionFrame.AccessedReferenceMember != null)
                    {
                        var accessedReferenceMember =
                            workflowEvaluatorContext.CurrentExecutionFrame.AccessedReferenceMember;

                        foreach (var trackedVariable in accessedReference.Variables)
                        {
                            var currentMethod =
                                trackedVariable.TypeInfo.AllMethods.FirstOrDefault(
                                    method => method.IdentifierText == accessedReferenceMember.Identifier.ValueText);

                            if (currentMethod == null)
                            {
                                continue;
                            }

                            workflowEvaluatorContext.CurrentExecutionFrame.PassedMethodParameters[-1] =
                                accessedReference.SelectVariable(trackedVariable);

                            for (int i = 0; i < invocationExpressionSyntax.ArgumentList.Arguments.Count; i++)
                            {
                                var argumentSyntax = invocationExpressionSyntax.ArgumentList.Arguments[i];

                                var nodeEvaluator = SyntaxNodeEvaluatorFactory.GetSyntaxNodeEvaluator(argumentSyntax);

                                workflowEvaluatorContext.CurrentExecutionFrame.AccessedReference = null;
                                workflowEvaluatorContext.CurrentExecutionFrame.AccessedReferenceMember = null;

                                if (nodeEvaluator != null)
                                {
                                    nodeEvaluator.EvaluateSyntaxNode(
                                        argumentSyntax.Expression,
                                        workflowEvaluatorContext);
                                }

                                if (workflowEvaluatorContext.CurrentExecutionFrame.AccessedReference != null)
                                {
                                    workflowEvaluatorContext.CurrentExecutionFrame.PassedMethodParameters[i] =
                                        workflowEvaluatorContext.CurrentExecutionFrame.AccessedReference.Move();
                                }
                            }

                            workflowEvaluatorContext.CurrentExecutionFrame.AccessedReference = null;
                            workflowEvaluatorContext.CurrentExecutionFrame.AccessedReferenceMember = null;

                            var methodEvaluator =
                                SyntaxNodeEvaluatorFactory.GetSyntaxNodeEvaluator(currentMethod.Declaration);

                            if (methodEvaluator != null)
                            {
                                methodEvaluator.EvaluateSyntaxNode(currentMethod.Declaration, workflowEvaluatorContext);
                            }
                        }
                    }
                }
            }
        }

        #endregion
    }
}