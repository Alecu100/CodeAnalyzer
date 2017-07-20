namespace CodeEvaluator.Evaluation.Evaluators
{
    using CodeEvaluator.Evaluation.Common;
    using CodeEvaluator.Evaluation.Members;

    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp.Syntax;

    #region Using

    #endregion

    public class VariableDeclarationSyntaxEvaluator : BaseSyntaxNodeEvaluator
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
            var variableDeclarationSyntax = (VariableDeclarationSyntax) syntaxNode;
            var thisTypeInfo = workflowEvaluatorExecutionState.CurrentExecutionFrame.ThisReference.EvaluatedObjects[0].TypeInfo;

            foreach (var variableDeclarator in variableDeclarationSyntax.Variables)
            {
                var reference = new EvaluatedObjectReference
                {
                    Declaration = variableDeclarationSyntax,
                    Declarator = variableDeclarator,
                    Identifier = variableDeclarator.Identifier,
                    IdentifierText = variableDeclarator.Identifier.ValueText
                };

                reference.TypeInfo = EvaluatedTypesInfoTable.GetTypeInfo(
                    variableDeclarationSyntax.Type.GetText().ToString(),
                    thisTypeInfo.UsingDirectives,
                    thisTypeInfo.NamespaceDeclarations);

                workflowEvaluatorExecutionState.CurrentExecutionFrame.LocalReferences.Add(reference);

                if (variableDeclarator.Initializer != null && variableDeclarator.Initializer.Value != null)
                {
                    var syntaxNodeEvaluator =
                        SyntaxNodeEvaluatorFactory.GetSyntaxNodeEvaluator(variableDeclarator.Initializer);

                    if (syntaxNodeEvaluator != null)
                    {
                        syntaxNodeEvaluator.EvaluateSyntaxNode(variableDeclarator.Initializer, workflowEvaluatorExecutionState);

                        if (workflowEvaluatorExecutionState.CurrentExecutionFrame.MemberAccessReference != null)
                        {
                            reference.AssignEvaluatedObject(
                                workflowEvaluatorExecutionState.CurrentExecutionFrame.MemberAccessReference);

                            workflowEvaluatorExecutionState.CurrentExecutionFrame.MemberAccessReference = null;
                        }
                    }
                }

                workflowEvaluatorExecutionState.CurrentExecutionFrame.LocalReferences.Add(reference);
            }
        }

        #endregion
    }
}