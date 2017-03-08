//  Project              : GLP
//  Module               : RomSoft.Client.Debug.dll
//  File                 : ConstructorDeclarationSyntaxEvaluator.cs
//  Author               : Alecsandru
//  Last Updated         : 19/12/2015 at 0:24
//  
// 
//  Contains             : Implementation of the ConstructorDeclarationSyntaxEvaluator.cs class.
//  Classes              : ConstructorDeclarationSyntaxEvaluator.cs
// 
//  
//  ----------------------------------------------------------------------- 
//   <copyright file="ConstructorDeclarationSyntaxEvaluator.cs" company="Sysmex"> 
//       Copyright (c) Sysmex. All rights reserved. 
//   </copyright> 
//  -----------------------------------------------------------------------

using System.Linq;
using CodeAnalysis.Core.Common;
using CodeAnalysis.Core.Members;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CodeAnalysis.Core.SyntaxNodeEvaluators
{

    #region Using

    #endregion

    public class ConstructorDeclarationSyntaxEvaluator : BaseMethodDeclarationSyntaxEvaluator
    {
        #region Protected Methods and Operators

        protected override void EvaluateSyntaxNodeInternal(
            SyntaxNode syntaxNode,
            StaticWorkflowEvaluatorContext workflowEvaluatorContext)
        {
            _baseMethodDeclarationSyntax = (ConstructorDeclarationSyntax) syntaxNode;
            _workflowEvaluatorContext = workflowEvaluatorContext;

            InitializeThisVariable();
            InitializeExecutionFrame();
            InitializeParameters();

            var syntaxNodeEvaluator =
                SyntaxNodeEvaluatorFactory.GetSyntaxNodeEvaluator(_baseMethodDeclarationSyntax.Body);

            if (syntaxNodeEvaluator != null)
            {
                syntaxNodeEvaluator.EvaluateSyntaxNode(syntaxNode, workflowEvaluatorContext);
            }

            ReturnThisReference();

            ResetExecutionFrame();
        }

        #endregion

        #region Private Methods and Operators

        private void InitializeThisVariable()
        {
            var trackedVariableTypeInfo =
                EvaluatedTypesInfoTable.GetTypeInfo(_baseMethodDeclarationSyntax as ConstructorDeclarationSyntax);

            if (trackedVariableTypeInfo != null)
            {
                _thisReference = new EvaluatedObjectReference();
                _thisReference.AssignEvaluatedObject(VariableAllocator.AllocateVariable(trackedVariableTypeInfo));
                _thisReference.TypeInfo = trackedVariableTypeInfo;
                _evaluatedMethod =
                    trackedVariableTypeInfo.Constructors.First(
                        constructor =>
                            ((ConstructorDeclarationSyntax) constructor.Declaration).ParameterList.ToString()
                            == _baseMethodDeclarationSyntax.ParameterList.ToString());
            }
        }

        private void ReturnThisReference()
        {
            _workflowEvaluatorContext.CurrentExecutionFrame.ReturningMethodParameters.AssignEvaluatedObject(
                _thisReference);
        }

        #endregion
    }
}