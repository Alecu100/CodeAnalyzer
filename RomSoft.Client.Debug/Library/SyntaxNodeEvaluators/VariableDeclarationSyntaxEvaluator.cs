//  Project              : GLP
//  Module               : RomSoft.Client.Debug.dll
//  File                 : VariableDeclarationSyntaxEvaluator.cs
//  Author               : Alecsandru
//  Last Updated         : 18/12/2015 at 12:54
//  
// 
//  Contains             : Implementation of the VariableDeclarationSyntaxEvaluator.cs class.
//  Classes              : VariableDeclarationSyntaxEvaluator.cs
// 
//  
//  ----------------------------------------------------------------------- 
//   <copyright file="VariableDeclarationSyntaxEvaluator.cs" company="Sysmex"> 
//       Copyright (c) Sysmex. All rights reserved. 
//   </copyright> 
//  -----------------------------------------------------------------------

namespace RomSoft.Client.Debug.Library.SyntaxNodeEvaluators
{
    #region Using

    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp.Syntax;

    using RomSoft.Client.Debug.Library.Common;
    using RomSoft.Client.Debug.Library.Interfaces;
    using RomSoft.Client.Debug.Library.Members;

    using StructureMap;

    #endregion

    public class VariableDeclarationSyntaxEvaluator : BaseSyntaxNodeEvaluator
    {
        #region Protected Methods and Operators

        /// <summary>
        ///     Evaluates the syntax node.
        /// </summary>
        /// <param name="syntaxNode">The syntax node.</param>
        /// <param name="workflowEvaluatorContext">The workflow evaluator stack.</param>
        protected override void EvaluateSyntaxNodeInternal(
            SyntaxNode syntaxNode,
            StaticWorkflowEvaluatorContext workflowEvaluatorContext)
        {
            var variableDeclarationSyntax = (VariableDeclarationSyntax)syntaxNode;
            var trackedVariableExpressionEvaluatorFactory =
                ObjectFactory.GetInstance<ITrackedVariableEvaluatorFactory>();
            var thisTypeInfo = workflowEvaluatorContext.CurrentExecutionFrame.ThisReference.Variables[0].TypeInfo;

            foreach (var variableDeclarator in variableDeclarationSyntax.Variables)
            {
                var reference = new TrackedVariableReference
                                    {
                                        Declaration = variableDeclarationSyntax,
                                        Declarator = variableDeclarator,
                                        Identifier = variableDeclarator.Identifier,
                                        IdentifierText = variableDeclarator.Identifier.ValueText
                                    };

                var trackedVariableTypeInfo =
                    TrackedVariableTypeInfosCache.GetTypeInfo(
                        variableDeclarationSyntax.Type.GetText().ToString(),
                        thisTypeInfo.UsingDirectives,
                        thisTypeInfo.NamespaceDeclarations);

                if (trackedVariableTypeInfo != null)
                {
                    reference.TypeInfo = trackedVariableTypeInfo;
                }

                workflowEvaluatorContext.CurrentExecutionFrame.LocalReferences.Add(reference);

                if (variableDeclarator.Initializer != null && variableDeclarator.Initializer.Value != null)
                {
                    var syntaxNodeEvaluator =
                        SyntaxNodeEvaluatorFactory.GetSyntaxNodeEvaluator(variableDeclarator.Initializer);

                    if (syntaxNodeEvaluator != null)
                    {
                        syntaxNodeEvaluator.EvaluateSyntaxNode(variableDeclarator.Initializer, workflowEvaluatorContext);

                        if (workflowEvaluatorContext.CurrentExecutionFrame.ReturnedMethodParameters.Count > 0)
                        {
                            foreach (
                                var storedOutput in
                                    workflowEvaluatorContext.CurrentExecutionFrame.ReturnedMethodParameters)
                            {
                                var trackedVariableReference = storedOutput;

                                if (trackedVariableReference != null)
                                {
                                    reference.AddVariables(trackedVariableReference.Variables);

                                    if (reference.TypeInfo == null)
                                    {
                                        reference.TypeInfo = trackedVariableReference.TypeInfo;
                                    }
                                }
                            }

                            workflowEvaluatorContext.CurrentExecutionFrame.ReturnedMethodParameters.Clear();
                        }
                    }
                }

                var trackedVariableExpressionEvaluator =
                    trackedVariableExpressionEvaluatorFactory.GetVariableExpressionEvaluator(
                        reference,
                        variableDeclarator.Initializer);

                if (trackedVariableExpressionEvaluator != null)
                {
                    trackedVariableExpressionEvaluator.EvaluateVariable(reference, variableDeclarator.Initializer);
                }
            }

            workflowEvaluatorContext.PopSyntaxNodeEvaluator();
        }

        #endregion
    }
}