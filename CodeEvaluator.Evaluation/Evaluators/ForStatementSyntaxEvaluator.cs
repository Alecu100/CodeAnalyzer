﻿namespace CodeEvaluator.Evaluation.Evaluators
{
    using CodeEvaluator.Evaluation.Common;

    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp.Syntax;

    #region Using

    

    #endregion

    public class ForStatementSyntaxEvaluator : BaseSyntaxNodeEvaluator
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
            var forStatementSyntax = (ForStatementSyntax)syntaxNode;

            if (forStatementSyntax.Initializers.Count > 0)
            {
                foreach (var initializer in forStatementSyntax.Initializers)
                {
                    var syntaxNodeEvaluator = SyntaxNodeEvaluatorFactory.GetSyntaxNodeEvaluator(initializer, EEvaluatorActions.None);

                    if (syntaxNodeEvaluator != null)
                    {
                        syntaxNodeEvaluator.EvaluateSyntaxNode(syntaxNode, workflowEvaluatorExecutionState);
                    }
                }
            }

            if (forStatementSyntax.Incrementors.Count > 0)
            {
                foreach (var incrementor in forStatementSyntax.Incrementors)
                {
                    var syntaxNodeEvaluator = SyntaxNodeEvaluatorFactory.GetSyntaxNodeEvaluator(incrementor, EEvaluatorActions.None);

                    if (syntaxNodeEvaluator != null)
                    {
                        syntaxNodeEvaluator.EvaluateSyntaxNode(syntaxNode, workflowEvaluatorExecutionState);
                    }
                }
            }

            if (forStatementSyntax.Condition != null)
            {
                var syntaxNodeEvaluator = SyntaxNodeEvaluatorFactory.GetSyntaxNodeEvaluator(
                    forStatementSyntax.Condition, EEvaluatorActions.None);

                if (syntaxNodeEvaluator != null)
                {
                    syntaxNodeEvaluator.EvaluateSyntaxNode(forStatementSyntax.Condition, workflowEvaluatorExecutionState);
                }
            }

            if (forStatementSyntax.Statement != null)
            {
                var syntaxNodeEvaluator = SyntaxNodeEvaluatorFactory.GetSyntaxNodeEvaluator(
                    forStatementSyntax.Statement, EEvaluatorActions.None);

                if (syntaxNodeEvaluator != null)
                {
                    syntaxNodeEvaluator.EvaluateSyntaxNode(forStatementSyntax.Statement, workflowEvaluatorExecutionState);
                }
            }
        }

        #endregion
    }
}