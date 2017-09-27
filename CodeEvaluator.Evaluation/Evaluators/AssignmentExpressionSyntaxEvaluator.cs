using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeEvaluator.Evaluation.Evaluators
{
    using CodeEvaluator.Evaluation.Common;
    using CodeEvaluator.Evaluation.Extensions;

    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp.Syntax;

    public class AssignmentExpressionSyntaxEvaluator : SyntaxNodeEvaluator
    {
        protected override void EvaluateSyntaxNodeInternal(SyntaxNode syntaxNode, CodeEvaluatorExecutionStack workflowEvaluatorExecutionStack)
        {
            workflowEvaluatorExecutionStack.CurrentExecutionFrame.MemberAccessReference = null;

            var assignmentExpressionSyntax = (AssignmentExpressionSyntax)syntaxNode;

            var syntaxNodeEvaluator = SyntaxNodeEvaluatorFactory.GetSyntaxNodeEvaluator(
                assignmentExpressionSyntax.Right,
                EEvaluatorActions.GetMember);

            if (syntaxNodeEvaluator != null)
            {
                syntaxNodeEvaluator.EvaluateSyntaxNode(assignmentExpressionSyntax.Right, workflowEvaluatorExecutionStack);
            }

            var valueToAssign = workflowEvaluatorExecutionStack.CurrentExecutionFrame.MemberAccessReference;
            workflowEvaluatorExecutionStack.CurrentExecutionFrame.MemberAccessReference = null;


            syntaxNodeEvaluator = SyntaxNodeEvaluatorFactory.GetSyntaxNodeEvaluator(
                assignmentExpressionSyntax.Left,
                EEvaluatorActions.GetMember);

            if (syntaxNodeEvaluator != null)
            {
                syntaxNodeEvaluator.EvaluateSyntaxNode(assignmentExpressionSyntax.Left, workflowEvaluatorExecutionStack);
            }

            if (valueToAssign.IsNotNull()
                && workflowEvaluatorExecutionStack.CurrentExecutionFrame.MemberAccessReference.IsNotNull())
            {
                workflowEvaluatorExecutionStack.CurrentExecutionFrame.MemberAccessReference.AssignEvaluatedObject(valueToAssign);
            }

            workflowEvaluatorExecutionStack.CurrentExecutionFrame.MemberAccessReference = null;
        }
    }
}
