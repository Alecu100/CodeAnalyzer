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
            CodeEvaluatorExecutionState workflowEvaluatorExecutionState)
        {
            var returnStatementSyntax = (ReturnStatementSyntax) syntaxNode;

            if (returnStatementSyntax.Expression != null)
            {
                var syntaxNodeEvaluator =
                    SyntaxNodeEvaluatorFactory.GetSyntaxNodeEvaluator(returnStatementSyntax.Expression);

                if (syntaxNodeEvaluator != null)
                {
                    syntaxNodeEvaluator.EvaluateSyntaxNode(returnStatementSyntax.Expression, workflowEvaluatorExecutionState);
                }

                if (workflowEvaluatorExecutionState.CurrentExecutionFrame.MemberAccessResult != null)
                {
                    workflowEvaluatorExecutionState.CurrentExecutionFrame.ReturningMethodParameters.AssignEvaluatedObject(
                        workflowEvaluatorExecutionState.CurrentExecutionFrame.MemberAccessResult);

                    workflowEvaluatorExecutionState.CurrentExecutionFrame.MemberAccessResult = null;
                }
            }
        }

        #endregion
    }
}