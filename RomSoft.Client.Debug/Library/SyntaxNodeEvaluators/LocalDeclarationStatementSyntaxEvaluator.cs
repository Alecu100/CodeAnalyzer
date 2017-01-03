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

namespace RomSoft.Client.Debug.Library.SyntaxNodeEvaluators
{
    #region Using

    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp.Syntax;

    using RomSoft.Client.Debug.Library.Common;

    #endregion

    public class LocalDeclarationStatementSyntaxEvaluator : BaseSyntaxNodeEvaluator
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
            var localDeclarationStatementSyntax = (LocalDeclarationStatementSyntax)syntaxNode;

            var syntaxNodeEvaluator =
                SyntaxNodeEvaluatorFactory.GetSyntaxNodeEvaluator(localDeclarationStatementSyntax.Declaration);

            if (syntaxNodeEvaluator != null)
            {
                syntaxNodeEvaluator.EvaluateSyntaxNode(
                    localDeclarationStatementSyntax.Declaration,
                    workflowEvaluatorContext);
            }
        }

        #endregion
    }
}