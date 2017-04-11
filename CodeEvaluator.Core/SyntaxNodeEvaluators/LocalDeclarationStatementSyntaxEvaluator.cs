//  Project              : GLP
//  Module               : RomSoft.Client.Debug.dll
//  File                 : LocalDeclarationStatementSyntaxEvaluator.cs
//  Author               : Alecsandru
//  Last Updated         : 11/12/2015 at 18:21
//  
// 
//  Contains             : Implementation of the LocalDeclarationStatementSyntaxEvaluator.cs class.
//  Classes              : LocalDeclarationStatementSyntaxEvaluator.cs
// 
//  
//  ----------------------------------------------------------------------- 
//   <copyright file="LocalDeclarationStatementSyntaxEvaluator.cs" company="Sysmex"> 
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

    public class LocalDeclarationStatementSyntaxEvaluator : BaseSyntaxNodeEvaluator
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
            var localDeclarationStatementSyntax = (LocalDeclarationStatementSyntax) syntaxNode;

            var syntaxNodeEvaluator =
                SyntaxNodeEvaluatorFactory.GetSyntaxNodeEvaluator(localDeclarationStatementSyntax.Declaration);

            if (syntaxNodeEvaluator != null)
            {
                syntaxNodeEvaluator.EvaluateSyntaxNode(
                    localDeclarationStatementSyntax.Declaration,
                    workflowEvaluatorExecutionState);
            }
        }

        #endregion
    }
}