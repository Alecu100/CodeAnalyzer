//  Project              : GLP
//  Module               : RomSoft.Client.Debug.dll
//  File                 : EqualsValueClauseSyntaxEvaluator.cs
//  Author               : Alecsandru
//  Last Updated         : 14/12/2015 at 17:02
//  
// 
//  Contains             : Implementation of the EqualsValueClauseSyntaxEvaluator.cs class.
//  Classes              : EqualsValueClauseSyntaxEvaluator.cs
// 
//  
//  ----------------------------------------------------------------------- 
//   <copyright file="EqualsValueClauseSyntaxEvaluator.cs" company="Sysmex"> 
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

    public class EqualsValueClauseSyntaxEvaluator : BaseSyntaxNodeEvaluator
    {
        #region Protected Methods and Operators

        protected override void EvaluateSyntaxNodeInternal(
            SyntaxNode syntaxNode,
            StaticWorkflowEvaluatorContext workflowEvaluatorContext)
        {
            var equalsValueClauseSyntax = (EqualsValueClauseSyntax)syntaxNode;

            var syntaxNodeEvaluator = SyntaxNodeEvaluatorFactory.GetSyntaxNodeEvaluator(equalsValueClauseSyntax.Value);

            if (syntaxNodeEvaluator != null)
            {
                syntaxNodeEvaluator.EvaluateSyntaxNode(equalsValueClauseSyntax.Value, workflowEvaluatorContext);
            }
        }

        #endregion
    }
}