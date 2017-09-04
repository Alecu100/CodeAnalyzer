using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeEvaluator.Evaluation.Common;
using CodeEvaluator.Evaluation.Interfaces;
using CodeEvaluator.Evaluation.Members;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using StructureMap;

namespace CodeEvaluator.Evaluation.Evaluators
{
    public class IdentifierNameSyntaxEvaluatorForMethod : BaseMethodDeclarationSyntaxEvaluator
    {
        protected override void EvaluateSyntaxNodeInternal(SyntaxNode syntaxNode, CodeEvaluatorExecutionStack workflowEvaluatorExecutionStack)
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
                TryToFindReferenceInLocalReferences(workflowEvaluatorExecutionStack, identifierNameSyntax,
                    ref foundReference);
            }

            if (foundReference == false)
            {
                TryToFindReferenceInThisReference(workflowEvaluatorExecutionStack, identifierNameSyntax,
                    ref foundReference);
            }
        }

        private void TryToFindReferenceInAccessedReference(
            CodeEvaluatorExecutionStack workflowEvaluatorExecutionStack,
            IdentifierNameSyntax identifierNameSyntax,
            ref bool foundReference)
        {
            var reference = new EvaluatedObjectReference();

            foreach (
                var evaluatedObject in
                    workflowEvaluatorExecutionStack.CurrentExecutionFrame.MemberAccessReference.EvaluatedObjects)
            {

                    foreach (var evaluatedMethod in evaluatedObject.TypeInfo.AccesibleMethods)
                    {
                        if (evaluatedMethod.IdentifierText == identifierNameSyntax.Identifier.ValueText)
                        {
                            var evaluatedDelegate =
                                new EvaluatedDelegate(
                                    workflowEvaluatorExecutionStack.CurrentExecutionFrame.MemberAccessReference.TypeInfo,
                                    evaluatedObject, evaluatedMethod);
                            reference.AssignEvaluatedObject(evaluatedDelegate);
                            foundReference = true;
                        }
                    }

            }

            if (foundReference == false)
            {
                foreach (
                    var evaluatedObject in
                        workflowEvaluatorExecutionStack.CurrentExecutionFrame.MemberAccessReference.EvaluatedObjects)
                {
                    foreach (var field in evaluatedObject.Fields)
                    {
                        if (field.IdentifierText == identifierNameSyntax.Identifier.ValueText &&
                            field.EvaluatedObjects.All(evaluatedObject2 => evaluatedObject2 is EvaluatedDelegate))
                        {
                            reference.AssignEvaluatedObject(field);
                            foundReference = true;
                        }
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
            var reference = new EvaluatedObjectReference();

            foreach (var localReference in workflowEvaluatorExecutionStack.CurrentExecutionFrame.LocalReferences)
            {
                if (localReference.IdentifierText == identifierNameSyntax.Identifier.ValueText && localReference.EvaluatedObjects.All(evaluatedObject => evaluatedObject is EvaluatedDelegate))
                {
                    reference.AssignEvaluatedObject(localReference);
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
            var reference = new EvaluatedObjectReference();

            foreach (
                var thisEvaluatedObject in
                    workflowEvaluatorExecutionStack.CurrentExecutionFrame.ThisReference.EvaluatedObjects)
            {

                foreach (var evaluatedMethod in thisEvaluatedObject.TypeInfo.AccesibleMethods)
                {
                    if (evaluatedMethod.IdentifierText == identifierNameSyntax.Identifier.ValueText)
                    {
                        var evaluatedDelegate = new EvaluatedDelegate(thisEvaluatedObject.TypeInfo,
                            thisEvaluatedObject,
                            evaluatedMethod);
                        reference.AssignEvaluatedObject(evaluatedDelegate);
                        foundReference = true;
                    }
                }
            }


            foreach (
                var thisEvaluatedObject in
                    workflowEvaluatorExecutionStack.CurrentExecutionFrame.ThisReference.EvaluatedObjects)
            {
                if (foundReference == false)
                {
                    foreach (var field in thisEvaluatedObject.Fields)
                    {
                        if (field.IdentifierText == identifierNameSyntax.Identifier.ValueText &&
                            field.EvaluatedObjects.All(evaluatedObject => evaluatedObject is EvaluatedDelegate))
                        {
                            reference.AssignEvaluatedObject(field);
                            foundReference = true;
                        }
                    }
                }
            }

            if (foundReference)
            {
                workflowEvaluatorExecutionStack.CurrentExecutionFrame.MemberAccessReference = reference;
            }
        }
    }
}
