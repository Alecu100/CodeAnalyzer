//  Project              : GLP
//  Module               : RomSoft.Client.Debug.dll
//  File                 : BlockSyntaxEvaluator.cs
//  Author               : Alecsandru
//  Last Updated         : 11/12/2015 at 18:16
//  
// 
//  Contains             : Implementation of the BlockSyntaxEvaluator.cs class.
//  Classes              : BlockSyntaxEvaluator.cs
// 
//  
//  ----------------------------------------------------------------------- 
//   <copyright file="BlockSyntaxEvaluator.cs" company="Sysmex"> 
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

    public class BlockSyntaxEvaluator : BaseSyntaxNodeEvaluator
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
            var blockSyntax = (BlockSyntax)syntaxNode;

            foreach (var statementSyntax in blockSyntax.Statements)
            {
                var syntaxNodeEvaluator = SyntaxNodeEvaluatorFactory.GetSyntaxNodeEvaluator(statementSyntax);

                if (syntaxNodeEvaluator != null)
                {
                    syntaxNodeEvaluator.EvaluateSyntaxNode(statementSyntax, workflowEvaluatorExecutionState);
                }
            }
        }

        #endregion
    }
}