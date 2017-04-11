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

using System.Linq;
using CodeAnalysis.Core.Common;
using CodeAnalysis.Core.Members;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CodeAnalysis.Core.SyntaxNodeEvaluators
{

    #region Using

    #endregion

    public class InvocationExpressionSyntaxEvaluator : BaseSyntaxNodeEvaluator
    {
        #region Protected Methods and Operators

        /// <summary>
        ///     Evaluates the syntax node.
        /// </summary>
        /// <param name="syntaxNode">The syntax node.</param>
        /// <param name="workflowEvaluatorExecutionState">The workflow evaluator stack.</param>
        protected override void EvaluateSyntaxNodeInternal(
            SyntaxNode syntaxNode,
            CodeEvaluatorExecutionState workflowEvaluatorExecutionState)
        {
            var invocationExpressionSyntax = (InvocationExpressionSyntax) syntaxNode;

            foreach (var staticWorkflowListener in workflowEvaluatorExecutionState.Parameters.Listeners)
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
                        workflowEvaluatorExecutionState);


                    if (workflowEvaluatorExecutionState.CurrentExecutionFrame.MemberAccessResult != null)
                    {
                        var accessedReferenceMember =
                            workflowEvaluatorExecutionState.CurrentExecutionFrame.MemberAccessResult;

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

                            workflowEvaluatorExecutionState.CurrentExecutionFrame.PassedMethodParameters[-1] =
                                evaluatedDelegate.Fields.First();

                            for (var i = 0; i < invocationExpressionSyntax.ArgumentList.Arguments.Count; i++)
                            {
                                var argumentSyntax = invocationExpressionSyntax.ArgumentList.Arguments[i];

                                var nodeEvaluator = SyntaxNodeEvaluatorFactory.GetSyntaxNodeEvaluator(argumentSyntax);

                                if (nodeEvaluator != null)
                                {
                                    workflowEvaluatorExecutionState.CurrentExecutionFrame.MemberAccessResult = null;

                                    nodeEvaluator.EvaluateSyntaxNode(
                                        argumentSyntax.Expression,
                                        workflowEvaluatorExecutionState);
                                }

                                if (workflowEvaluatorExecutionState.CurrentExecutionFrame.MemberAccessResult != null)
                                {
                                    workflowEvaluatorExecutionState.CurrentExecutionFrame.PassedMethodParameters[i] =
                                        workflowEvaluatorExecutionState.CurrentExecutionFrame.MemberAccessResult;
                                }
                            }

                            workflowEvaluatorExecutionState.CurrentExecutionFrame.MemberAccessResult = null;

                            var methodEvaluator =
                                SyntaxNodeEvaluatorFactory.GetSyntaxNodeEvaluator(currentMethod.Declaration);

                            if (methodEvaluator != null)
                            {
                                methodEvaluator.EvaluateSyntaxNode(currentMethod.Declaration, workflowEvaluatorExecutionState);
                            }
                        }
                    }
                }
            }
        }

        #endregion
    }
}