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
        /// <param name="workflowEvaluatorExecutionStack">The workflow evaluator stack.</param>
        protected override void EvaluateSyntaxNodeInternal(
            SyntaxNode syntaxNode,
            CodeEvaluatorExecutionStack workflowEvaluatorExecutionStack)
        {
            var variableDeclarationSyntax = (VariableDeclarationSyntax) syntaxNode;
            var thisTypeInfo = workflowEvaluatorExecutionStack.CurrentExecutionFrame.ThisReference.EvaluatedObjects[0].TypeInfo;

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

                if (variableDeclarator.Initializer != null && variableDeclarator.Initializer.Value != null)
                {
                    var syntaxNodeEvaluator =
                        SyntaxNodeEvaluatorFactory.GetSyntaxNodeEvaluator(variableDeclarator.Initializer, EEvaluatorActions.None);

                    if (syntaxNodeEvaluator != null)
                    {
                        syntaxNodeEvaluator.EvaluateSyntaxNode(variableDeclarator.Initializer, workflowEvaluatorExecutionStack);

                        if (workflowEvaluatorExecutionStack.CurrentExecutionFrame.MemberAccessReference != null)
                        {
                            reference.AssignEvaluatedObject(
                                workflowEvaluatorExecutionStack.CurrentExecutionFrame.MemberAccessReference);

                            workflowEvaluatorExecutionStack.CurrentExecutionFrame.MemberAccessReference = null;
                        }
                    }
                }

                workflowEvaluatorExecutionStack.CurrentExecutionFrame.LocalReferences.Add(reference);
            }
        }

        #endregion
    }
}