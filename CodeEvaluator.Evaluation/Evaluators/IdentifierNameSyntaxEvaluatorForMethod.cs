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
        protected override void EvaluateSyntaxNodeInternal(SyntaxNode syntaxNode, CodeEvaluatorExecutionState workflowEvaluatorExecutionState)
        {
            var identifierNameSyntax = (IdentifierNameSyntax)syntaxNode;
            var foundReference = false;

            if (workflowEvaluatorExecutionState.CurrentExecutionFrame.MemberAccessReference != null)
            {
                TryToFindReferenceInAccessedReference(
                    workflowEvaluatorExecutionState,
                    identifierNameSyntax,
                    ref foundReference);
            }

            if (foundReference == false)
            {
                TryToFindReferenceInLocalReferences(workflowEvaluatorExecutionState, identifierNameSyntax,
                    ref foundReference);
            }

            if (foundReference == false)
            {
                TryToFindReferenceInThisReference(workflowEvaluatorExecutionState, identifierNameSyntax,
                    ref foundReference);
            }
        }

        private void TryToFindReferenceInAccessedReference(
            CodeEvaluatorExecutionState workflowEvaluatorExecutionState,
            IdentifierNameSyntax identifierNameSyntax,
            ref bool foundReference)
        {
            var reference = new EvaluatedObjectReference();

            foreach (
                var evaluatedObject in
                    workflowEvaluatorExecutionState.CurrentExecutionFrame.MemberAccessReference.EvaluatedObjects)
            {

                    foreach (var evaluatedMethod in evaluatedObject.TypeInfo.AccesibleMethods)
                    {
                        if (evaluatedMethod.IdentifierText == identifierNameSyntax.Identifier.ValueText)
                        {
                            var evaluatedDelegate =
                                new EvaluatedDelegate(
                                    workflowEvaluatorExecutionState.CurrentExecutionFrame.MemberAccessReference.TypeInfo,
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
                        workflowEvaluatorExecutionState.CurrentExecutionFrame.MemberAccessReference.EvaluatedObjects)
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
                workflowEvaluatorExecutionState.CurrentExecutionFrame.MemberAccessReference = reference;
            }
        }

        private void TryToFindReferenceInLocalReferences(
            CodeEvaluatorExecutionState workflowEvaluatorExecutionState,
            IdentifierNameSyntax identifierNameSyntax,
            ref bool foundReference)
        {
            var reference = new EvaluatedObjectReference();

            foreach (var localReference in workflowEvaluatorExecutionState.CurrentExecutionFrame.LocalReferences)
            {
                if (localReference.IdentifierText == identifierNameSyntax.Identifier.ValueText && localReference.EvaluatedObjects.All(evaluatedObject => evaluatedObject is EvaluatedDelegate))
                {
                    reference.AssignEvaluatedObject(localReference);
                    foundReference = true;
                }
            }

            if (foundReference)
            {
                workflowEvaluatorExecutionState.CurrentExecutionFrame.MemberAccessReference = reference;
            }
        }

        private void TryToFindReferenceInThisReference(
            CodeEvaluatorExecutionState workflowEvaluatorExecutionState,
            IdentifierNameSyntax identifierNameSyntax,
            ref bool foundReference)
        {
            var reference = new EvaluatedObjectReference();

            foreach (
                var thisEvaluatedObject in
                    workflowEvaluatorExecutionState.CurrentExecutionFrame.ThisReference.EvaluatedObjects)
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
                    workflowEvaluatorExecutionState.CurrentExecutionFrame.ThisReference.EvaluatedObjects)
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
                workflowEvaluatorExecutionState.CurrentExecutionFrame.MemberAccessReference = reference;
            }
        }
    }
}
