//  Project              : GLP
//  Module               : RomSoft.Client.Debug.dll
//  File                 : MethodDeclarationSyntaxEvaluator.cs
//  Author               : Alecsandru
//  Last Updated         : 19/12/2015 at 1:52
//  
// 
//  Contains             : Implementation of the MethodDeclarationSyntaxEvaluator.cs class.
//  Classes              : MethodDeclarationSyntaxEvaluator.cs
// 
//  
//  ----------------------------------------------------------------------- 
//   <copyright file="MethodDeclarationSyntaxEvaluator.cs" company="Sysmex"> 
//       Copyright (c) Sysmex. All rights reserved. 
//   </copyright> 
//  -----------------------------------------------------------------------

namespace RomSoft.Client.Debug.Library.SyntaxNodeEvaluators
{
    #region Using

    using System.Linq;

    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp.Syntax;

    using RomSoft.Client.Debug.Library.Common;

    #endregion

    public class MethodDeclarationSyntaxEvaluator : BaseMethodDeclarationSyntaxEvaluator
    {
        #region Fields

        private MethodDeclarationSyntax _methodDeclarationSyntax;

        #endregion

        #region Protected Methods and Operators

        protected override void EvaluateSyntaxNodeInternal(
            SyntaxNode syntaxNode,
            StaticWorkflowEvaluatorContext workflowEvaluatorContext)
        {
            _baseMethodDeclarationSyntax = (MethodDeclarationSyntax)syntaxNode;
            _methodDeclarationSyntax = (MethodDeclarationSyntax)syntaxNode;
            _workflowEvaluatorContext = workflowEvaluatorContext;

            InitializeThisVariable();
            InitializeExecutionFrame();
            InitializeParameters();

            var syntaxNodeEvaluator =
                SyntaxNodeEvaluatorFactory.GetSyntaxNodeEvaluator(_baseMethodDeclarationSyntax.Body);

            if (syntaxNodeEvaluator != null)
            {
                syntaxNodeEvaluator.EvaluateSyntaxNode(_methodDeclarationSyntax.Body, workflowEvaluatorContext);
            }

            ResetExecutionFrame();
        }

        #endregion

        #region Private Methods and Operators

        private void InitializeThisVariable()
        {
            _thisReference = _workflowEvaluatorContext.CurrentExecutionFrame.PassedMethodParameters[-1].Move();
            _workflowEvaluatorContext.CurrentExecutionFrame.PassedMethodParameters.Remove(-1);
            _trackedMethod =
                _thisReference.TypeInfo.AllMethods.First(
                    method => method.IdentifierText == _methodDeclarationSyntax.Identifier.ValueText);
        }

        #endregion
    }
}