using CodeEvaluator.Evaluation.Common;
using CodeEvaluator.Evaluation.Extensions;
using CodeEvaluator.Evaluation.Members;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CodeEvaluator.Evaluation.Evaluators
{
    public class IdentifierNameSyntaxEvaluatorForMember : SyntaxNodeEvaluator
    {
        #region Protected Methods and Operators

        protected override void EvaluateSyntaxNodeInternal(
            SyntaxNode syntaxNode,
            CodeEvaluatorExecutionStack workflowEvaluatorExecutionStack)
        {
            var identifierNameSyntax = (IdentifierNameSyntax) syntaxNode;
            var foundReference = false;

            if (workflowEvaluatorExecutionStack.CurrentExecutionFrame.MemberAccessReference != null)
                TryToFindReferenceInAccessedReference(
                    workflowEvaluatorExecutionStack,
                    identifierNameSyntax,
                    ref foundReference);

            if (workflowEvaluatorExecutionStack.CurrentExecutionFrame.MemberAccessReference != null)
                TryToFindPropertyInAccessedReference(
                    workflowEvaluatorExecutionStack,
                    identifierNameSyntax,
                    ref foundReference);

            if (foundReference == false)
                TryToFindReferenceInLocalReferences(
                    workflowEvaluatorExecutionStack,
                    identifierNameSyntax,
                    ref foundReference);

            if (foundReference == false)
                TryToFindReferenceInThisReference(
                    workflowEvaluatorExecutionStack,
                    identifierNameSyntax,
                    ref foundReference);

            if (foundReference == false)
                TryToFindPropertyInThisReference(
                    workflowEvaluatorExecutionStack,
                    identifierNameSyntax,
                    ref foundReference);

            if (foundReference == false)
                TryToFindStaticReference(workflowEvaluatorExecutionStack, identifierNameSyntax, ref foundReference);

            if (foundReference == false)
            {
                var identifierNameSyntaxEvaluatorForMethod = new IdentifierNameSyntaxEvaluatorForMethod();

                identifierNameSyntaxEvaluatorForMethod.EvaluateSyntaxNode(syntaxNode, workflowEvaluatorExecutionStack);
            }
        }

        private void TryToFindPropertyInThisReference(CodeEvaluatorExecutionStack workflowEvaluatorExecutionStack,
            IdentifierNameSyntax identifierNameSyntax, ref bool foundReference)
        {
            var reference = new EvaluatedPropertyObjectReference();


            foreach (var evaluatedProperty in workflowEvaluatorExecutionStack.CurrentExecutionFrame.ThisReference
                .TypeInfo.AccesibleProperties)
                if (evaluatedProperty.IdentifierText == identifierNameSyntax.Identifier.ValueText)
                {
                    reference.AssignEvaluatedProperty(new EvaluatedPropertyObject(
                        workflowEvaluatorExecutionStack.CurrentExecutionFrame.ThisReference.TypeInfo,
                        workflowEvaluatorExecutionStack.CurrentExecutionFrame.ThisReference.EvaluatedObjects[0],
                        evaluatedProperty, workflowEvaluatorExecutionStack));
                    workflowEvaluatorExecutionStack.CurrentExecutionFrame.ThisReference.EvaluatedObjects[0]
                        .PushHistory(workflowEvaluatorExecutionStack);
                    foundReference = true;
                }


            if (foundReference)
                workflowEvaluatorExecutionStack.CurrentExecutionFrame.MemberAccessReference = reference;
        }

        private void TryToFindPropertyInAccessedReference(CodeEvaluatorExecutionStack workflowEvaluatorExecutionStack,
            IdentifierNameSyntax identifierNameSyntax, ref bool foundReference)
        {
            var reference = new EvaluatedPropertyObjectReference();

            foreach (var evaluatedObject in
                workflowEvaluatorExecutionStack.CurrentExecutionFrame.MemberAccessReference.EvaluatedObjects)
            foreach (var evaluatedProperty in evaluatedObject.TypeInfo.AccesibleProperties)
                if (evaluatedProperty.IdentifierText == identifierNameSyntax.Identifier.ValueText)
                {
                    reference.AssignEvaluatedProperty(new EvaluatedPropertyObject(
                        workflowEvaluatorExecutionStack.CurrentExecutionFrame.MemberAccessReference.TypeInfo,
                        evaluatedObject, evaluatedProperty, workflowEvaluatorExecutionStack));
                    evaluatedObject.PushHistory(workflowEvaluatorExecutionStack);
                    foundReference = true;
                }

            if (foundReference)
                workflowEvaluatorExecutionStack.CurrentExecutionFrame.MemberAccessReference = reference;
        }

        private void TryToFindStaticReference(
            CodeEvaluatorExecutionStack workflowEvaluatorExecutionStack,
            IdentifierNameSyntax identifierNameSyntax,
            ref bool foundReference)
        {
            var evaluatedTypeInfo = EvaluatedTypesInfoTable.GetTypeInfo(
                identifierNameSyntax.Identifier.ValueText,
                workflowEvaluatorExecutionStack.CurrentExecutionFrame.ThisReference.TypeInfo);

            if (evaluatedTypeInfo != null)
            {
                workflowEvaluatorExecutionStack.CurrentExecutionFrame.MemberAccessReference =
                    new EvaluatedObjectDirectReference(evaluatedTypeInfo.SharedStaticObject);

                foundReference = true;
            }
        }

        #endregion

        #region Private Methods and Operators

        private void TryToFindReferenceInAccessedReference(
            CodeEvaluatorExecutionStack workflowEvaluatorExecutionStack,
            IdentifierNameSyntax identifierNameSyntax,
            ref bool foundReference)
        {
            var reference = new EvaluatedObjectIndirectReference();

            foreach (var evaluatedObject in
                workflowEvaluatorExecutionStack.CurrentExecutionFrame.MemberAccessReference.EvaluatedObjects)
            foreach (var field in evaluatedObject.Fields)
                if (field.IdentifierText == identifierNameSyntax.Identifier.ValueText)
                {
                    reference.AssignEvaluatedObjectReference(field);
                    field.EvaluatedObjects.ForEach(
                        currentObject => currentObject.PushHistory(workflowEvaluatorExecutionStack));
                    foundReference = true;
                }

            if (foundReference)
                workflowEvaluatorExecutionStack.CurrentExecutionFrame.MemberAccessReference = reference;
        }

        private void TryToFindReferenceInLocalReferences(
            CodeEvaluatorExecutionStack workflowEvaluatorExecutionStack,
            IdentifierNameSyntax identifierNameSyntax,
            ref bool foundReference)
        {
            EvaluatedObjectReference reference = null;

            foreach (var localReference in workflowEvaluatorExecutionStack.CurrentExecutionFrame.LocalReferences)
                if (localReference.IdentifierText == identifierNameSyntax.Identifier.ValueText)
                {
                    reference = localReference;
                    localReference.EvaluatedObjects.ForEach(
                        currentObject => currentObject.PushHistory(workflowEvaluatorExecutionStack));
                    foundReference = true;
                }

            if (foundReference)
                workflowEvaluatorExecutionStack.CurrentExecutionFrame.MemberAccessReference = reference;
        }

        private void TryToFindReferenceInThisReference(
            CodeEvaluatorExecutionStack workflowEvaluatorExecutionStack,
            IdentifierNameSyntax identifierNameSyntax,
            ref bool foundReference)
        {
            var reference = new EvaluatedObjectIndirectReference();

            foreach (var thisEvaluatedObject in
                workflowEvaluatorExecutionStack.CurrentExecutionFrame.ThisReference.EvaluatedObjects)
            foreach (var field in thisEvaluatedObject.Fields)
                if (field.IdentifierText == identifierNameSyntax.Identifier.ValueText)
                {
                    reference.AssignEvaluatedObjectReference(field);
                    field.EvaluatedObjects.ForEach(
                        currentObject => currentObject.PushHistory(workflowEvaluatorExecutionStack));
                    foundReference = true;
                }

            if (foundReference)
                workflowEvaluatorExecutionStack.CurrentExecutionFrame.MemberAccessReference = reference;
        }

        #endregion
    }
}