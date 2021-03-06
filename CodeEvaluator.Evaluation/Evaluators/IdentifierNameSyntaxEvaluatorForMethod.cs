﻿using System.Linq;
using CodeEvaluator.Evaluation.Common;
using CodeEvaluator.Evaluation.Members;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CodeEvaluator.Evaluation.Evaluators
{
    public class IdentifierNameSyntaxEvaluatorForMethod : SyntaxNodeEvaluator
    {
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
        }

        private void TryToFindReferenceInAccessedReference(
            CodeEvaluatorExecutionStack workflowEvaluatorExecutionStack,
            IdentifierNameSyntax identifierNameSyntax,
            ref bool foundReference)
        {
            var reference = new EvaluatedObjectDirectReference();

            foreach (var evaluatedObject in
                workflowEvaluatorExecutionStack.CurrentExecutionFrame.MemberAccessReference.EvaluatedObjects)
            {
                var foundMethods =
                    evaluatedObject.TypeInfo.AccesibleMethods.Where(
                        evaluatedMethod => evaluatedMethod.IdentifierText == identifierNameSyntax.Identifier.ValueText).ToArray();

                if (foundMethods.Any())
                {
                    var evaluatedDelegate =
                        new EvaluatedInvokableObject(
                            workflowEvaluatorExecutionStack.CurrentExecutionFrame.MemberAccessReference.TypeInfo,
                            evaluatedObject,
                            foundMethods);
                    reference.AssignEvaluatedObject(evaluatedDelegate);
                    foundReference = true;
                }
            }

            if (foundReference == false)
                foreach (var evaluatedObject in
                    workflowEvaluatorExecutionStack.CurrentExecutionFrame.MemberAccessReference.EvaluatedObjects)
                foreach (var field in evaluatedObject.Fields)
                    if (field.IdentifierText == identifierNameSyntax.Identifier.ValueText
                        && field.EvaluatedObjects.All(evaluatedObject2 => evaluatedObject2 is EvaluatedInvokableObject))
                    {
                        reference.AssignEvaluatedObject(field);
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
            var reference = new EvaluatedObjectDirectReference();

            foreach (var localReference in workflowEvaluatorExecutionStack.CurrentExecutionFrame.LocalReferences)
                if (localReference.IdentifierText == identifierNameSyntax.Identifier.ValueText
                    &&
                    localReference.EvaluatedObjects.All(evaluatedObject => evaluatedObject is EvaluatedInvokableObject))
                {
                    reference.AssignEvaluatedObject(localReference);
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
            var reference = new EvaluatedObjectDirectReference();

            foreach (var thisEvaluatedObject in
                workflowEvaluatorExecutionStack.CurrentExecutionFrame.ThisReference.EvaluatedObjects)
            {
                var foundMethods =
                    thisEvaluatedObject.TypeInfo.AccesibleMethods.Where(
                            evaluatedMethod => evaluatedMethod.IdentifierText == identifierNameSyntax.Identifier.ValueText)
                        .ToArray();

                if (foundMethods.Any())
                {
                    var evaluatedDelegate = new EvaluatedInvokableObject(
                        thisEvaluatedObject.TypeInfo,
                        thisEvaluatedObject,
                        foundMethods);
                    reference.AssignEvaluatedObject(evaluatedDelegate);
                    foundReference = true;
                }
            }

            if (foundReference == false)
                foreach (var thisEvaluatedObject in
                    workflowEvaluatorExecutionStack.CurrentExecutionFrame.ThisReference.EvaluatedObjects)
                foreach (var field in thisEvaluatedObject.Fields)
                    if (field.IdentifierText == identifierNameSyntax.Identifier.ValueText
                        && field.EvaluatedObjects.All(evaluatedObject => evaluatedObject is EvaluatedInvokableObject))
                    {
                        reference.AssignEvaluatedObject(field);
                        foundReference = true;
                    }

            if (foundReference)
                workflowEvaluatorExecutionStack.CurrentExecutionFrame.MemberAccessReference = reference;
        }
    }
}