//  Project              : GLP
//  Module               : RomSoft.Client.Debug.dll
//  File                 : SyntaxNodeEvaluatorFactory.cs
//  Author               : Alecsandru
//  Last Updated         : 19/12/2015 at 2:04
//  
// 
//  Contains             : Implementation of the SyntaxNodeEvaluatorFactory.cs class.
//  Classes              : SyntaxNodeEvaluatorFactory.cs
// 
//  
//  ----------------------------------------------------------------------- 
//   <copyright file="SyntaxNodeEvaluatorFactory.cs" company="Sysmex"> 
//       Copyright (c) Sysmex. All rights reserved. 
//   </copyright> 
//  -----------------------------------------------------------------------

namespace RomSoft.Client.Debug.Library.Common
{
    #region Using

    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp.Syntax;

    using RomSoft.Client.Debug.Library.Interfaces;
    using RomSoft.Client.Debug.Library.SyntaxNodeEvaluators;

    #endregion

    public class SyntaxNodeEvaluatorFactory : ISyntaxNodeEvaluatorFactory
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Builds the syntax node evaluator.
        /// </summary>
        /// <param name="syntaxNode">The syntax node.</param>
        /// <returns></returns>
        public ISyntaxNodeEvaluator GetSyntaxNodeEvaluator(SyntaxNode syntaxNode)
        {
            if (syntaxNode is MethodDeclarationSyntax)
            {
                return new MethodDeclarationSyntaxEvaluator();
            }

            if (syntaxNode is BlockSyntax)
            {
                return new BlockSyntaxEvaluator();
            }

            if (syntaxNode is ExpressionStatementSyntax)
            {
                return new ExpressionStatementSyntaxEvaluator();
            }

            if (syntaxNode is InvocationExpressionSyntax)
            {
                return new InvocationExpressionSyntaxEvaluator();
            }

            if (syntaxNode is IfStatementSyntax)
            {
                return new IfStatementSyntaxEvaluator();
            }

            if (syntaxNode is LocalDeclarationStatementSyntax)
            {
                return new LocalDeclarationStatementSyntaxEvaluator();
            }

            if (syntaxNode is ForStatementSyntax)
            {
                return new ForStatementSyntaxEvaluator();
            }

            if (syntaxNode is ConstructorDeclarationSyntax)
            {
                return new ConstructorDeclarationSyntaxEvaluator();
            }

            if (syntaxNode is EqualsValueClauseSyntax)
            {
                return new EqualsValueClauseSyntaxEvaluator();
            }

            if (syntaxNode is ReturnStatementSyntax)
            {
                return new ReturnStatementSyntaxEvaluator();
            }

            if (syntaxNode is VariableDeclarationSyntax)
            {
                return new VariableDeclarationSyntaxEvaluator();
            }

            if (syntaxNode is MemberAccessExpressionSyntax)
            {
                return new MemberAccessExpressionSyntaxEvaluator();
            }

            if (syntaxNode is IdentifierNameSyntax)
            {
                return new IdentifierNameSyntaxEvaluator();
            }

            return null;
        }

        #endregion
    }
}