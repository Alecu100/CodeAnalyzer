namespace CodeEvaluator.Evaluation.Evaluators
{
    using CodeEvaluator.Evaluation.Common;
    using CodeEvaluator.Evaluation.Extensions;
    using CodeEvaluator.Evaluation.Members;

    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp.Syntax;

    public class IdentifierNameSyntaxEvaluatorForMember : BaseMethodDeclarationSyntaxEvaluator
    {
        #region Protected Methods and Operators

        protected override void EvaluateSyntaxNodeInternal(
            SyntaxNode syntaxNode,
            CodeEvaluatorExecutionStack workflowEvaluatorExecutionStack)
        {
            var identifierNameSyntax = (IdentifierNameSyntax)syntaxNode;
            var foundReference = false;

            if (workflowEvaluatorExecutionStack.CurrentExecutionFrame.MemberAccessReference != null)
            {
                TryToFindReferenceInAccessedReference(
                    workflowEvaluatorExecutionStack,
                    identifierNameSyntax,
                    ref foundReference);
            }

            if (foundReference == false)
            {
                TryToFindReferenceInLocalReferences(
                    workflowEvaluatorExecutionStack,
                    identifierNameSyntax,
                    ref foundReference);
            }

            if (foundReference == false)
            {
                TryToFindReferenceInThisReference(
                    workflowEvaluatorExecutionStack,
                    identifierNameSyntax,
                    ref foundReference);
            }
        }

        #endregion

        #region Private Methods and Operators

        private void TryToFindReferenceInAccessedReference(
            CodeEvaluatorExecutionStack workflowEvaluatorExecutionStack,
            IdentifierNameSyntax identifierNameSyntax,
            ref bool foundReference)
        {
            EvaluatedObjectReferenceBase reference = null;

            foreach (var evaluatedObject in
                workflowEvaluatorExecutionStack.CurrentExecutionFrame.MemberAccessReference.EvaluatedObjects)
            {
                foreach (var field in evaluatedObject.Fields)
                {
                    if (field.IdentifierText == identifierNameSyntax.Identifier.ValueText)
                    {
                        reference = field;
                        field.EvaluatedObjects.ForEach(
                            currentObject => currentObject.PushHistory(workflowEvaluatorExecutionStack));
                        foundReference = true;
                    }
                }
            }

            if (foundReference)
            {
                workflowEvaluatorExecutionStack.CurrentExecutionFrame.MemberAccessReference = reference;
            }
        }

        private void TryToFindReferenceInLocalReferences(
            CodeEvaluatorExecutionStack workflowEvaluatorExecutionStack,
            IdentifierNameSyntax identifierNameSyntax,
            ref bool foundReference)
        {
            EvaluatedObjectReferenceBase reference = null;

            foreach (var localReference in workflowEvaluatorExecutionStack.CurrentExecutionFrame.LocalReferences)
            {
                if (localReference.IdentifierText == identifierNameSyntax.Identifier.ValueText)
                {
                    reference = localReference;
                    localReference.EvaluatedObjects.ForEach(
                        currentObject => currentObject.PushHistory(workflowEvaluatorExecutionStack));
                    foundReference = true;
                }
            }

            if (foundReference)
            {
                workflowEvaluatorExecutionStack.CurrentExecutionFrame.MemberAccessReference = reference;
            }
        }

        private void TryToFindReferenceInThisReference(
            CodeEvaluatorExecutionStack workflowEvaluatorExecutionStack,
            IdentifierNameSyntax identifierNameSyntax,
            ref bool foundReference)
        {
            EvaluatedObjectReferenceBase reference = null;

            foreach (var thisEvaluatedObject in
                workflowEvaluatorExecutionStack.CurrentExecutionFrame.ThisReference.EvaluatedObjects)
            {
                foreach (var field in thisEvaluatedObject.Fields)
                {
                    if (field.IdentifierText == identifierNameSyntax.Identifier.ValueText)
                    {
                        reference = field;
                        field.EvaluatedObjects.ForEach(
                            currentObject => currentObject.PushHistory(workflowEvaluatorExecutionStack));
                        foundReference = true;
                    }
                }
            }

            if (foundReference)
            {
                workflowEvaluatorExecutionStack.CurrentExecutionFrame.MemberAccessReference = reference;
            }
        }

        #endregion
    }
}