//  Project              : GLP
//  Module               : RomSoft.Client.Debug.dll
//  File                 : ExpressionStatementSyntaxEvaluator.cs
//  Author               : Alecsandru
//  Last Updated         : 11/12/2015 at 18:16
//  
// 
//  Contains             : Implementation of the ExpressionStatementSyntaxEvaluator.cs class.
//  Classes              : ExpressionStatementSyntaxEvaluator.cs
// 
//  
//  ----------------------------------------------------------------------- 
//   <copyright file="ExpressionStatementSyntaxEvaluator.cs" company="Sysmex"> 
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

    public class ExpressionStatementSyntaxEvaluator : BaseSyntaxNodeEvaluator
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
            var expressionStatementSyntax = (ExpressionStatementSyntax) syntaxNode;
            var syntaxNodeEvaluator =
                SyntaxNodeEvaluatorFactory.GetSyntaxNodeEvaluator(expressionStatementSyntax.Expression);

            if (syntaxNodeEvaluator != null)
            {
                syntaxNodeEvaluator.EvaluateSyntaxNode(expressionStatementSyntax.Expression, workflowEvaluatorExecutionState);
            }
        }

        #endregion
    }
}