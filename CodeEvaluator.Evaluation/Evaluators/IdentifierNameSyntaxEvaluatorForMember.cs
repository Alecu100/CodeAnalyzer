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
    public class IdentifierNameSyntaxEvaluatorForMember : BaseMethodDeclarationSyntaxEvaluator
    {
        #region Protected Methods and Operators

        protected override void EvaluateSyntaxNodeInternal(
            SyntaxNode syntaxNode,
            CodeEvaluatorExecutionState workflowEvaluatorExecutionState)
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

        #endregion

        #region Private Methods and Operators

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
                foreach (var field in evaluatedObject.Fields)
                {
                    if (field.IdentifierText == identifierNameSyntax.Identifier.ValueText)
                    {
                        reference.AssignEvaluatedObject(field);
                        foundReference = true;
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
                if (localReference.IdentifierText == identifierNameSyntax.Identifier.ValueText)
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
                foreach (var field in thisEvaluatedObject.Fields)
                {
                    if (field.IdentifierText == identifierNameSyntax.Identifier.ValueText)
                    {
                        reference.AssignEvaluatedObject(field);
                        foundReference = true;
                    }
                }
            }

            if (foundReference)
            {
                workflowEvaluatorExecutionState.CurrentExecutionFrame.MemberAccessReference = reference;
            }
        }

        #endregion
    }
}
