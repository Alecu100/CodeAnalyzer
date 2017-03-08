﻿//  Project              : GLP
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

using CodeAnalysis.Core.Common;
using CodeAnalysis.Core.Members;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CodeAnalysis.Core.SyntaxNodeEvaluators
{

    #region Using

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
            var variableDeclarationSyntax = (VariableDeclarationSyntax) syntaxNode;
            var thisTypeInfo = workflowEvaluatorContext.CurrentExecutionFrame.ThisReference.EvaluatedObjects[0].TypeInfo;

            foreach (var variableDeclarator in variableDeclarationSyntax.Variables)
            {
                var reference = new EvaluatedObjectReference
                {
                    Declaration = variableDeclarationSyntax,
                    Declarator = variableDeclarator,
                    Identifier = variableDeclarator.Identifier,
                    IdentifierText = variableDeclarator.Identifier.ValueText
                };

                reference.TypeInfo = EvaluatedTypesInfoTable.GetTypeInfo(
                    variableDeclarationSyntax.Type.GetText().ToString(),
                    thisTypeInfo.UsingDirectives,
                    thisTypeInfo.NamespaceDeclarations);

                workflowEvaluatorContext.CurrentExecutionFrame.LocalReferences.Add(reference);

                if (variableDeclarator.Initializer != null && variableDeclarator.Initializer.Value != null)
                {
                    var syntaxNodeEvaluator =
                        SyntaxNodeEvaluatorFactory.GetSyntaxNodeEvaluator(variableDeclarator.Initializer);

                    if (syntaxNodeEvaluator != null)
                    {
                        syntaxNodeEvaluator.EvaluateSyntaxNode(variableDeclarator.Initializer, workflowEvaluatorContext);

                        if (workflowEvaluatorContext.CurrentExecutionFrame.MemberAccessResult != null)
                        {
                            reference.AssignEvaluatedObject(
                                workflowEvaluatorContext.CurrentExecutionFrame.MemberAccessResult);

                            workflowEvaluatorContext.CurrentExecutionFrame.MemberAccessResult = null;
                        }
                    }
                }

                workflowEvaluatorContext.CurrentExecutionFrame.LocalReferences.Add(reference);
            }

            workflowEvaluatorContext.PopSyntaxNodeEvaluator();
        }

        #endregion
    }
}