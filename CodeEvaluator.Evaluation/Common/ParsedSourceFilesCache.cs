﻿namespace CodeEvaluator.Evaluation.Common
{
    using System.Collections.Generic;
    using System.IO;
    using System.Threading.Tasks;

    using global::CodeEvaluator.Evaluation.Interfaces;

    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;

    #region Using

    #endregion

    public class ParsedSourceFilesCache : List<SyntaxTree>, IParsedSourceFilesCache
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Rebuilds from projects.
        /// </summary>
        /// <param name="selectedProjects">The selected projects.</param>
        public void RebuildFromCodeFiles(IList<string> codeFileNames)
        {
            Clear();

            Parallel.ForEach(
                codeFileNames,
                (codeFile, state, arg3) =>
                {
                    var sourceText = File.ReadAllText(codeFile);
                    var syntaxTree = CSharpSyntaxTree.ParseText(sourceText);
                    lock (this)
                    {
                        Add(syntaxTree);
                    }
                });
        }

        #endregion
    }
}