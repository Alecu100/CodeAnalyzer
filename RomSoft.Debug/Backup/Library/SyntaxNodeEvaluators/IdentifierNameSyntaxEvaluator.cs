//  Project              : GLP
//  Module               : RomSoft.Client.Debug.dll
//  File                 : IdentifierNameSyntaxEvaluator.cs
//  Author               : Alecsandru
//  Last Updated         : 18/12/2015 at 18:13
//  
// 
//  Contains             : Implementation of the IdentifierNameSyntaxEvaluator.cs class.
//  Classes              : IdentifierNameSyntaxEvaluator.cs
// 
//  
//  ----------------------------------------------------------------------- 
//   <copyright file="IdentifierNameSyntaxEvaluator.cs" company="Sysmex"> 
//       Copyright (c) Sysmex. All rights reserved. 
//   </copyright> 
//  -----------------------------------------------------------------------

namespace RomSoft.Client.Debug.Library.SyntaxNodeEvaluators
{
    #region Using

    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp.Syntax;

    using RomSoft.Client.Debug.Library.Common;
    using RomSoft.Client.Debug.Library.Members;

    #endregion

    public class IdentifierNameSyntaxEvaluator : BaseSyntaxNodeEvaluator
    {
        #region Protected Methods and Operators

        protected override void EvaluateSyntaxNodeInternal(
            SyntaxNode syntaxNode,
            StaticWorkflowEvaluatorContext workflowEvaluatorContext)
        {
            var identifierNameSyntax = (IdentifierNameSyntax)syntaxNode;
            var foundReference = false;

            if (workflowEvaluatorContext.CurrentExecutionFrame.AccessedReference != null)
            {
                TryToFindReferenceInAccessedReference(
                    workflowEvaluatorContext,
                    identifierNameSyntax,
                    ref foundReference);
            }

            if (foundReference == false)
            {
                TryToFindReferenceInLocalReferences(workflowEvaluatorContext, identifierNameSyntax, ref foundReference);
            }

            if (foundReference == false)
            {
                TryToFindReferenceInThisReference(workflowEvaluatorContext, identifierNameSyntax, ref foundReference);
            }
        }

        #endregion

        #region Private Methods and Operators

        private void TryToFindReferenceInAccessedReference(
            StaticWorkflowEvaluatorContext workflowEvaluatorContext,
            IdentifierNameSyntax identifierNameSyntax,
            ref bool foundReference)
        {
            var reference = new TrackedVariableReference();

            foreach (var trackedVariable in workflowEvaluatorContext.CurrentExecutionFrame.AccessedReference.Variables)
            {
                foreach (var field in trackedVariable.Fields)
                {
                    if (field.IdentifierText == identifierNameSyntax.Identifier.ValueText)
                    {
                        reference = reference.Merge(field);
                        foundReference = true;
                    }
                }
            }

            if (workflowEvaluatorContext.CurrentExecutionFrame.AccessedReference != null)
            {
                workflowEvaluatorContext.CurrentExecutionFrame.AccessedReference.Dispose();
            }
            workflowEvaluatorContext.CurrentExecutionFrame.AccessedReference = reference;
        }

        private void TryToFindReferenceInLocalReferences(
            StaticWorkflowEvaluatorContext workflowEvaluatorContext,
            IdentifierNameSyntax identifierNameSyntax,
            ref bool foundReference)
        {
            var reference = new TrackedVariableReference();

            foreach (var localReference in workflowEvaluatorContext.CurrentExecutionFrame.LocalReferences)
            {
                if (localReference.IdentifierText == identifierNameSyntax.Identifier.ValueText)
                {
                    reference = reference.Merge(localReference);
                    foundReference = true;
                }
            }

            if (foundReference)
            {
                if (workflowEvaluatorContext.CurrentExecutionFrame.AccessedReference != null)
                {
                    workflowEvaluatorContext.CurrentExecutionFrame.AccessedReference.Dispose();
                }
                workflowEvaluatorContext.CurrentExecutionFrame.AccessedReference = reference;
            }
        }

        private void TryToFindReferenceInThisReference(
            StaticWorkflowEvaluatorContext workflowEvaluatorContext,
            IdentifierNameSyntax identifierNameSyntax,
            ref bool foundReference)
        {
            var reference = new TrackedVariableReference();

            foreach (var thisVariable in workflowEvaluatorContext.CurrentExecutionFrame.ThisReference.Variables)
            {
                foreach (var field in thisVariable.Fields)
                {
                    if (field.IdentifierText == identifierNameSyntax.Identifier.ValueText)
                    {
                        reference = reference.Merge(field);
                        foundReference = true;
                    }
                }
            }

            if (foundReference)
            {
                if (workflowEvaluatorContext.CurrentExecutionFrame.AccessedReference != null)
                {
                    workflowEvaluatorContext.CurrentExecutionFrame.AccessedReference.Dispose();
                }
                workflowEvaluatorContext.CurrentExecutionFrame.AccessedReference = reference;
            }
        }

        #endregion
    }
}