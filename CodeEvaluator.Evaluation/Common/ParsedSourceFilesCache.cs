namespace CodeEvaluator.Evaluation.Common
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
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

            var csCodeFiles = codeFileNames.Where(codefile => codefile.ToLower().EndsWith(".cs"));

            Parallel.ForEach(
                csCodeFiles,
                (codeFile, state, arg3) =>
                    {
                        try
                        {
                            var sourceText = File.ReadAllText(codeFile);
                            var syntaxTree = CSharpSyntaxTree.ParseText(sourceText);
                            lock (this)
                            {
                                Add(syntaxTree);
                            }
                        }
                        catch (Exception)
                        {
                        }
                    });
        }

        #endregion
    }
}