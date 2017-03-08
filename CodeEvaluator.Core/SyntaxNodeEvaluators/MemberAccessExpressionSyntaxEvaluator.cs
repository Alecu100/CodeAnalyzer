//  Project              : GLP
//  Module               : RomSoft.Client.Debug.dll
//  File                 : MemberAccessExpressionSyntaxEvaluator.cs
//  Author               : Alecsandru
//  Last Updated         : 16/12/2015 at 11:35
//  
// 
//  Contains             : Implementation of the MemberAccessExpressionSyntaxEvaluator.cs class.
//  Classes              : MemberAccessExpressionSyntaxEvaluator.cs
// 
//  
//  ----------------------------------------------------------------------- 
//   <copyright file="MemberAccessExpressionSyntaxEvaluator.cs" company="Sysmex"> 
//       Copyright (c) Sysmex. All rights reserved. 
//   </copyright> 
//  -----------------------------------------------------------------------

using CodeAnalysis.Core.Common;

namespace CodeAnalysis.Core.SyntaxNodeEvaluators
{
    #region Using

    

    #endregion

    public class MemberAccessExpressionSyntaxEvaluator : BaseSyntaxNodeEvaluator
    {
        #region Protected Methods and Operators

        protected override void EvaluateSyntaxNodeInternal(
            SyntaxNode syntaxNode,
            StaticWorkflowEvaluatorContext workflowEvaluatorContext)
        {
            var memberAccessExpressionSyntax = (MemberAccessExpressionSyntax)syntaxNode;

            workflowEvaluatorContext.CurrentExecutionFrame.AccessedReferenceMember = memberAccessExpressionSyntax.Name;

            if (memberAccessExpressionSyntax.Expression == null)
            {
                workflowEvaluatorContext.CurrentExecutionFrame.AccessedMember =
                    workflowEvaluatorContext.CurrentExecutionFrame.ThisReference.Copy();
            }
            else
            {
                var syntaxNodeEvaluator =
                    SyntaxNodeEvaluatorFactory.GetSyntaxNodeEvaluator(memberAccessExpressionSyntax.Expression);

                if (syntaxNodeEvaluator != null)
                {
                    syntaxNodeEvaluator.EvaluateSyntaxNode(
                        memberAccessExpressionSyntax.Expression,
                        workflowEvaluatorContext);
                }
            }
        }

        #endregion
    }
}