//  Project              : GLP
//  Module               : RomSoft.Client.Debug.dll
//  File                 : IfStatementSyntaxEvaluator.cs
//  Author               : Alecsandru
//  Last Updated         : 11/12/2015 at 18:19
//  
// 
//  Contains             : Implementation of the IfStatementSyntaxEvaluator.cs class.
//  Classes              : IfStatementSyntaxEvaluator.cs
// 
//  
//  ----------------------------------------------------------------------- 
//   <copyright file="IfStatementSyntaxEvaluator.cs" company="Sysmex"> 
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

    public class IfStatementSyntaxEvaluator : BaseSyntaxNodeEvaluator
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
            var ifStatementSyntax = (IfStatementSyntax)syntaxNode;

            if (ifStatementSyntax.Condition != null)
            {
                var syntaxNodeEvaluator = SyntaxNodeEvaluatorFactory.GetSyntaxNodeEvaluator(ifStatementSyntax.Condition);

                if (syntaxNodeEvaluator != null)
                {
                    syntaxNodeEvaluator.EvaluateSyntaxNode(ifStatementSyntax.Condition, workflowEvaluatorContext);
                }
            }

            if (ifStatementSyntax.Statement != null)
            {
                var syntaxNodeEvaluator = SyntaxNodeEvaluatorFactory.GetSyntaxNodeEvaluator(ifStatementSyntax.Statement);

                if (syntaxNodeEvaluator != null)
                {
                    syntaxNodeEvaluator.EvaluateSyntaxNode(ifStatementSyntax.Statement, workflowEvaluatorContext);
                }
            }

            if (ifStatementSyntax.Else != null && ifStatementSyntax.Else.Statement != null)
            {
                var syntaxNodeEvaluator =
                    SyntaxNodeEvaluatorFactory.GetSyntaxNodeEvaluator(ifStatementSyntax.Else.Statement);

                if (syntaxNodeEvaluator != null)
                {
                    syntaxNodeEvaluator.EvaluateSyntaxNode(ifStatementSyntax.Else.Statement, workflowEvaluatorContext);
                }
            }
        }

        #endregion
    }
}