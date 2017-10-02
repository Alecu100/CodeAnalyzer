namespace CodeEvaluator.Evaluation.Evaluators
{
    using System.Collections.Generic;

    using CodeEvaluator.Evaluation.Common;
    using CodeEvaluator.Evaluation.Members;

    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp.Syntax;

    public class LiteralExpressionSyntaxEvaluator : SyntaxNodeEvaluator
    {
        protected override void EvaluateSyntaxNodeInternal(
            SyntaxNode syntaxNode,
            CodeEvaluatorExecutionStack workflowEvaluatorExecutionStack)
        {
            var literalExpressionSyntax = (LiteralExpressionSyntax)syntaxNode;

            var literalValue = literalExpressionSyntax.Token.Value;

            var evaluatedTypeInfo =
                EvaluatedTypesInfoTable.GetTypeInfo(
                    literalExpressionSyntax.Token.Value.GetType().ToString(),
                    new List<UsingDirectiveSyntax>(),
                    new List<MemberDeclarationSyntax>());

            var evaluatedLiteralObject = new EvaluatedLiteralObject(evaluatedTypeInfo);

            evaluatedLiteralObject.PushHistory(workflowEvaluatorExecutionStack);
            evaluatedLiteralObject.LiteralValue = literalValue;

            var evaluatedObjectReference = new EvaluatedObjectDirectReference(evaluatedLiteralObject);

            workflowEvaluatorExecutionStack.CurrentExecutionFrame.MemberAccessReference = evaluatedObjectReference;
        }
    }
}