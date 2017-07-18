namespace CodeAnalysis.Core.Evaluators
{
    using CodeAnalysis.Core.Common;
    using CodeAnalysis.Core.Enums;
    using CodeAnalysis.Core.Interfaces;
    using CodeAnalysis.Core.Members;

    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp.Syntax;

    using StructureMap;

    #region Using

    #endregion

    public class IdentifierNameSyntaxEvaluator : BaseSyntaxNodeEvaluator
    {
        #region Protected Methods and Operators

        protected override void EvaluateSyntaxNodeInternal(
            SyntaxNode syntaxNode,
            CodeEvaluatorExecutionState workflowEvaluatorExecutionState)
        {
            var identifierNameSyntax = (IdentifierNameSyntax) syntaxNode;

            if (workflowEvaluatorExecutionState.CurrentExecutionFrame.CurrrentAction ==
                EEvaluatorActions.InvokeConstructor)
            {
                TryToFindConstructorReference(workflowEvaluatorExecutionState, identifierNameSyntax);
            }
            else
            {
                TryToFindMembnerReference(workflowEvaluatorExecutionState, identifierNameSyntax);
            }
        }

        private void TryToFindMembnerReference(CodeEvaluatorExecutionState workflowEvaluatorExecutionState,
            IdentifierNameSyntax identifierNameSyntax)
        {
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

        private void TryToFindConstructorReference(CodeEvaluatorExecutionState workflowEvaluatorExecutionState,
            IdentifierNameSyntax identifierNameSyntax)
        {
            var evaluatedTypesInfoTable = ObjectFactory.GetInstance<IEvaluatedTypesInfoTable>();

            var evaluatedTypeInfo = evaluatedTypesInfoTable.GetTypeInfo(identifierNameSyntax.Identifier.ValueText,
                workflowEvaluatorExecutionState.CurrentExecutionFrame.ThisReference.TypeInfo);

            var reference = new EvaluatedObjectReference();

            foreach (var evaluatedConstructor in evaluatedTypeInfo.Constructors)
            {
                var evaluatedDelegate =
                    new EvaluatedDelegate(
                        evaluatedTypeInfo, evaluatedConstructor);
                reference.AssignEvaluatedObject(evaluatedDelegate);
            }

            workflowEvaluatorExecutionState.CurrentExecutionFrame.MemberAccessReference = reference;
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
                if (workflowEvaluatorExecutionState.CurrentExecutionFrame.CurrrentAction ==
                    EEvaluatorActions.InvokeMethod)
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
                else
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
                    workflowEvaluatorExecutionState.CurrentExecutionFrame.ThisReference.EvaluatedObjects
                )
            {
                if (workflowEvaluatorExecutionState.CurrentExecutionFrame.CurrrentAction ==
                    EEvaluatorActions.InvokeMethod)
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
                else
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
            }

            if (foundReference)
            {
                workflowEvaluatorExecutionState.CurrentExecutionFrame.MemberAccessReference = reference;
            }
        }

        #endregion
    }
}