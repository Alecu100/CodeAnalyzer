﻿//  Project              : GLP
//  Module               : RomSoft.Client.Debug.dll
//  File                 : IStaticWorkflowEvaluatorListener.cs
//  Author               : Alecsandru
//  Last Updated         : 04/12/2015 at 22:24
//  
// 
//  Contains             : Implementation of the IStaticWorkflowEvaluatorListener.cs class.
//  Classes              : IStaticWorkflowEvaluatorListener.cs
// 
//  
//  ----------------------------------------------------------------------- 
//   <copyright file="IStaticWorkflowEvaluatorListener.cs" company="Sysmex"> 
//       Copyright (c) Sysmex. All rights reserved. 
//   </copyright> 
//  -----------------------------------------------------------------------

using CodeAnalysis.Core.Members;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CodeAnalysis.Core.Interfaces
{

    #region Using

    #endregion

    public interface IStaticWorkflowEvaluatorListener
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Called when [method called].
        /// </summary>
        /// <param name="classDeclaration">Name of the class.</param>
        /// <param name="methodDeclaration">Name of the method.</param>
        void AfterMethodCalled(ClassDeclarationSyntax classDeclaration, MethodDeclarationSyntax methodDeclaration);

        /// <summary>
        ///     Afters the variable declared.
        /// </summary>
        /// <param name="variable">The variable.</param>
        void AfterVariableDeclared(EvaluatedObject variable);

        /// <summary>
        ///     Befores the method called.
        /// </summary>
        /// <param name="methodCallInvocationExpression">The method call invocation expression.</param>
        void BeforeMethodCalled(InvocationExpressionSyntax methodCallInvocationExpression);

        /// <summary>
        ///     Befores the variable declared.
        /// </summary>
        /// <param name="variableDeclaration">The variable declaration.</param>
        void BeforeVariableDeclared(
            VariableDeclarationSyntax variableDeclaration,
            VariableDeclaratorSyntax variableDeclarator);

        #endregion
    }
}