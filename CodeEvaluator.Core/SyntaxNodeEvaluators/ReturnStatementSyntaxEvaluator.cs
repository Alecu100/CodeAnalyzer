//  Project              : GLP
//  Module               : RomSoft.Client.Debug.dll
//  File                 : ReturnStatementSyntaxEvaluator.cs
//  Author               : Alecsandru
//  Last Updated         : 18/12/2015 at 22:40
//  
// 
//  Contains             : Implementation of the ReturnStatementSyntaxEvaluator.cs class.
//  Classes              : ReturnStatementSyntaxEvaluator.cs
// 
//  
//  ----------------------------------------------------------------------- 
//   <copyright file="ReturnStatementSyntaxEvaluator.cs" company="Sysmex"> 
//       Copyright (c) Sysmex. All rights reserved. 
//   </copyright> 
//  -----------------------------------------------------------------------

using CodeAnalysis.Core.Common;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CodeAnalysis.Core.SyntaxNodeEvaluators
{

    #region Using

    #endregion

    public class ReturnStatementSyntaxEvaluator : BaseSyntaxNodeEvaluator
    {
        #region Protected Methods and Operators

        protected override void EvaluateSyntaxNodeInternal(
            SyntaxNode syntaxNode,
            StaticWorkflowEvaluatorContext workflowEvaluatorContext)
        {
            var returnStatementSyntax = (ReturnStatementSyntax) syntaxNode;

            if (returnStatementSyntax.Expression != null)
            {
                var syntaxNodeEvaluator =
                    SyntaxNodeEvaluatorFactory.GetSyntaxNodeEvaluator(returnStatementSyntax.Expression);

                if (syntaxNodeEvaluator != null)
                {
                    syntaxNodeEvaluator.EvaluateSyntaxNode(returnStatementSyntax.Expression, workflowEvaluatorContext);
                }

                if (workflowEvaluatorContext.CurrentExecutionFrame.MemberAccessResult != null)
                {
                    workflowEvaluatorContext.CurrentExecutionFrame.ReturningMethodParameters.Add(
                        workflowEvaluatorContext.CurrentExecutionFrame.MemberAccessResult.Move());

                    workflowEvaluatorContext.CurrentExecutionFrame.MemberAccessResult = null;
                }

                if (workflowEvaluatorContext.CurrentExecutionFrame.ReturnedMethodParameters.Count > 0)
                {
                    foreach (var trackedVariableReference in
                        workflowEvaluatorContext.CurrentExecutionFrame.ReturnedMethodParameters)
                    {
                        workflowEvaluatorContext.CurrentExecutionFrame.ReturningMethodParameters.Add(
                            trackedVariableReference.Move());
                    }

                    workflowEvaluatorContext.CurrentExecutionFrame.ReturnedMethodParameters.Clear();
                }
            }
        }

        #endregion
    }
}