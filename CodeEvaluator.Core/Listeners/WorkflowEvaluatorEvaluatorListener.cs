//  Project              : GLP
//  Module               : RomSoft.Client.Debug.dll
//  File                 : WorkflowEvaluatorEvaluatorListener.cs
//  Author               : Alecsandru
//  Last Updated         : 14/12/2015 at 13:45
//  
// 
//  Contains             : Implementation of the WorkflowEvaluatorEvaluatorListener.cs class.
//  Classes              : WorkflowEvaluatorEvaluatorListener.cs
// 
//  
//  ----------------------------------------------------------------------- 
//   <copyright file="WorkflowEvaluatorEvaluatorListener.cs" company="Sysmex"> 
//       Copyright (c) Sysmex. All rights reserved. 
//   </copyright> 
//  -----------------------------------------------------------------------

using System.Linq;
using CodeAnalysis.Core.Interfaces;
using CodeAnalysis.Core.Members;
using CodeEvaluator.Workflows;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CodeAnalysis.Core.Listeners
{

    #region Using

    #endregion

    public class WorkflowEvaluatorEvaluatorListener : ICodeEvaluatorListener
    {
        #region Constructors and Destructors

        public WorkflowEvaluatorEvaluatorListener()
        {
            WorkflowEvaluator.Initialize();
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Called when [method called].
        /// </summary>
        /// <param name="classDeclaration">Name of the class.</param>
        /// <param name="methodDeclaration">Name of the method.</param>
        public void AfterMethodCalled(
            ClassDeclarationSyntax classDeclaration,
            MethodDeclarationSyntax methodDeclaration)
        {
        }

        /// <summary>
        ///     Afters the variable declared.
        /// </summary>
        /// <param name="variable">The variable.</param>
        public void AfterVariableDeclared(EvaluatedObject variable)
        {
        }

        /// <summary>
        ///     Befores the method called.
        /// </summary>
        /// <param name="methodCallInvocationExpression">The method call invocation expression.</param>
        public void BeforeMethodCalled(InvocationExpressionSyntax methodCallInvocationExpression)
        {
            var syntaxNodes = methodCallInvocationExpression.ChildNodes().ToList();

            if (syntaxNodes.Count == 2)
            {
                if (syntaxNodes[0] is MemberAccessExpressionSyntax)
                {
                    var memberAccessExpression = (MemberAccessExpressionSyntax) syntaxNodes[0];
                    var methodAndClass = memberAccessExpression.ChildNodes().ToList();

                    if (methodAndClass.Count == 2)
                    {
                        var classIdentifier = methodAndClass[0] as IdentifierNameSyntax;
                        var methodIdentifier = methodAndClass[1] as IdentifierNameSyntax;

                        if (classIdentifier != null && methodIdentifier != null
                            && classIdentifier.Identifier.ValueText == "WorkflowEvaluator")
                        {
                            switch (methodIdentifier.Identifier.ValueText)
                            {
                                case "AddProcess":
                                    ExecuteAddProcess(methodCallInvocationExpression);
                                    break;
                                case "AddDecision":
                                    ExecuteAddDecision(methodCallInvocationExpression);
                                    break;
                                case "BeginWorkflow":
                                    ExecuteBeginScope();
                                    break;
                                case "EndWorkflow":
                                    ExecuteEndScope();
                                    break;
                                case "StopWorkflow":
                                    ExecuteStopScope();
                                    break;
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        ///     Befores the variable declared.
        /// </summary>
        /// <param name="variableDeclaration">The variable declaration.</param>
        public void BeforeVariableDeclared(
            VariableDeclarationSyntax variableDeclaration,
            VariableDeclaratorSyntax variableDeclarator)
        {
        }

        #endregion

        #region Private Methods and Operators

        private void ExecuteAddDecision(InvocationExpressionSyntax methodCallInvocationExpression)
        {
            var arguments = methodCallInvocationExpression.ArgumentList.Arguments;
            string name = null;
            string description = null;

            if (arguments.Count == 2)
            {
                var nameArgument = arguments[0].ChildNodes().FirstOrDefault();

                if (nameArgument is LiteralExpressionSyntax)
                {
                    name = nameArgument.GetText().ToString().Trim('\"');
                }

                var descriptionArgument = arguments[1].ChildNodes().FirstOrDefault();

                if (descriptionArgument is LiteralExpressionSyntax)
                {
                    description = descriptionArgument.GetText().ToString().Trim('\"');
                }
            }

            if (name != null)
            {
                WorkflowEvaluator.AddDecision(name, description);
            }
        }

        private void ExecuteAddProcess(InvocationExpressionSyntax methodCallInvocationExpression)
        {
            var arguments = methodCallInvocationExpression.ArgumentList.Arguments;
            string name = null;
            string description = null;

            if (arguments.Count == 2)
            {
                var nameArgument = arguments[0].ChildNodes().FirstOrDefault();

                if (nameArgument is LiteralExpressionSyntax)
                {
                    name = nameArgument.GetText().ToString().Trim('\"');
                }

                var descriptionArgument = arguments[1].ChildNodes().FirstOrDefault();

                if (descriptionArgument is LiteralExpressionSyntax)
                {
                    description = descriptionArgument.GetText().ToString().Trim('\"');
                }
            }

            if (name != null)
            {
                WorkflowEvaluator.AddProcess(name, description);
            }
        }

        private void ExecuteBeginScope()
        {
            WorkflowEvaluator.BeginWorkflow();
        }

        private void ExecuteEndScope()
        {
            WorkflowEvaluator.EndWorkflow();
        }

        private void ExecuteStopScope()
        {
            WorkflowEvaluator.StopWorkflow();
        }

        #endregion
    }
}