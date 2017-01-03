//  Project              : GLP
//  Module               : RomSoft.Client.Debug.dll
//  File                 : ForStatementSyntaxEvaluator.cs
//  Author               : Alecsandru
//  Last Updated         : 11/12/2015 at 18:17
//  
// 
//  Contains             : Implementation of the ForStatementSyntaxEvaluator.cs class.
//  Classes              : ForStatementSyntaxEvaluator.cs
// 
//  
//  ----------------------------------------------------------------------- 
//   <copyright file="ForStatementSyntaxEvaluator.cs" company="Sysmex"> 
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

    public class ForStatementSyntaxEvaluator : BaseSyntaxNodeEvaluator
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
            var forStatementSyntax = (ForStatementSyntax)syntaxNode;

            if (forStatementSyntax.Initializers.Count > 0)
            {
                foreach (var initializer in forStatementSyntax.Initializers)
                {
                    var syntaxNodeEvaluator = SyntaxNodeEvaluatorFactory.GetSyntaxNodeEvaluator(initializer);

                    if (syntaxNodeEvaluator != null)
                    {
                        syntaxNodeEvaluator.EvaluateSyntaxNode(syntaxNode, workflowEvaluatorContext);
                    }
                }
            }

            if (forStatementSyntax.Incrementors.Count > 0)
            {
                foreach (var incrementor in forStatementSyntax.Incrementors)
                {
                    var syntaxNodeEvaluator = SyntaxNodeEvaluatorFactory.GetSyntaxNodeEvaluator(incrementor);

                    if (syntaxNodeEvaluator != null)
                    {
                        syntaxNodeEvaluator.EvaluateSyntaxNode(syntaxNode, workflowEvaluatorContext);
                    }
                }
            }

            if (forStatementSyntax.Condition != null)
            {
                var syntaxNodeEvaluator = SyntaxNodeEvaluatorFactory.GetSyntaxNodeEvaluator(
                    forStatementSyntax.Condition);

                if (syntaxNodeEvaluator != null)
                {
                    syntaxNodeEvaluator.EvaluateSyntaxNode(forStatementSyntax.Condition, workflowEvaluatorContext);
                }
            }

            if (forStatementSyntax.Statement != null)
            {
                var syntaxNodeEvaluator = SyntaxNodeEvaluatorFactory.GetSyntaxNodeEvaluator(
                    forStatementSyntax.Statement);

                if (syntaxNodeEvaluator != null)
                {
                    syntaxNodeEvaluator.EvaluateSyntaxNode(forStatementSyntax.Statement, workflowEvaluatorContext);
                }
            }
        }

        #endregion
    }
}