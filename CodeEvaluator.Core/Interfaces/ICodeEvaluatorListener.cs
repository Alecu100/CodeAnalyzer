using CodeAnalysis.Core.Members;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CodeAnalysis.Core.Interfaces
{

    #region Using

    #endregion

    public interface ICodeEvaluatorListener
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