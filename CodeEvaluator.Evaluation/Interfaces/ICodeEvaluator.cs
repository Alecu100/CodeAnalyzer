﻿namespace CodeEvaluator.Evaluation.Interfaces
{
    using System.Collections.Generic;

    using Microsoft.CodeAnalysis.CSharp.Syntax;

    #region Using

    #endregion

    public interface ICodeEvaluator
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Evaluates the workflow.
        /// </summary>
        /// <param name="listeners">The listeners.</param>
        /// <param name="codeFileNames">The selected projets.</param>
        /// <param name="targetClass">The target class.</param>
        /// <param name="startMethod">The start method.</param>
        void Evaluate(
            List<ISyntaxNodeEvaluatorListener> listeners,
            List<string> codeFileNames,
            ClassDeclarationSyntax targetClass,
            MethodDeclarationSyntax startMethod,
            List<string> assemblyFileNames = null);

        #endregion
    }
}