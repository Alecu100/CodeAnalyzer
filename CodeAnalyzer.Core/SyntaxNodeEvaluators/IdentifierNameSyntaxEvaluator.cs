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

using CodeAnalysis.Core.Common;
using CodeAnalysis.Core.Members;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CodeAnalysis.Core.SyntaxNodeEvaluators
{

    #region Using

    #endregion

    public class IdentifierNameSyntaxEvaluator : BaseSyntaxNodeEvaluator
    {
        #region Protected Methods and Operators

        protected override void EvaluateSyntaxNodeInternal(
            SyntaxNode syntaxNode,
            StaticWorkflowEvaluatorContext workflowEvaluatorContext)
        {
            var identifierNameSyntax = (IdentifierNameSyntax) syntaxNode;
            var foundReference = false;

            if (workflowEvaluatorContext.CurrentExecutionFrame.AccessedMember != null)
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
            var reference = new EvaluatedObjectReference();

            foreach (var trackedVariable in workflowEvaluatorContext.CurrentExecutionFrame.AccessedMember.EvaluatedObjects)
            {
                foreach (var field in trackedVariable.Fields)
                {
                    if (field.IdentifierText == identifierNameSyntax.Identifier.ValueText)
                    {
                        reference = reference.AssignEvaluatedObject(field);
                        foundReference = true;
                    }
                }
            }

            if (workflowEvaluatorContext.CurrentExecutionFrame.AccessedMember != null)
            {
                workflowEvaluatorContext.CurrentExecutionFrame.AccessedMember.Dispose();
            }
            workflowEvaluatorContext.CurrentExecutionFrame.AccessedMember = reference;
        }

        private void TryToFindReferenceInLocalReferences(
            StaticWorkflowEvaluatorContext workflowEvaluatorContext,
            IdentifierNameSyntax identifierNameSyntax,
            ref bool foundReference)
        {
            var reference = new EvaluatedObjectReference();

            foreach (var localReference in workflowEvaluatorContext.CurrentExecutionFrame.LocalReferences)
            {
                if (localReference.IdentifierText == identifierNameSyntax.Identifier.ValueText)
                {
                    reference.AssignEvaluatedObject(localReference);
                    foundReference = true;
                }
            }

            if (foundReference)
            {
                if (workflowEvaluatorContext.CurrentExecutionFrame.AccessedMember != null)
                {
                    workflowEvaluatorContext.CurrentExecutionFrame.AccessedMember.Dispose();
                }
                workflowEvaluatorContext.CurrentExecutionFrame.AccessedMember = reference;
            }
        }

        private void TryToFindReferenceInThisReference(
            StaticWorkflowEvaluatorContext workflowEvaluatorContext,
            IdentifierNameSyntax identifierNameSyntax,
            ref bool foundReference)
        {
            var reference = new EvaluatedObjectReference();

            foreach (var thisVariable in workflowEvaluatorContext.CurrentExecutionFrame.ThisReference.EvaluatedObjects)
            {
                foreach (var field in thisVariable.Fields)
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
                if (workflowEvaluatorContext.CurrentExecutionFrame.AccessedMember != null)
                {
                    workflowEvaluatorContext.CurrentExecutionFrame.AccessedMember.Dispose();
                }
                workflowEvaluatorContext.CurrentExecutionFrame.AccessedMember = reference;
            }
        }

        #endregion
    }
}