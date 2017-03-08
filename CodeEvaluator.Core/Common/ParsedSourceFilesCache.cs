//  Project              : GLP
//  Module               : RomSoft.Client.Debug.dll
//  File                 : ParsedSourceFilesCache.cs
//  Author               : Alecsandru
//  Last Updated         : 30/11/2015 at 18:41
//  
// 
//  Contains             : Implementation of the ParsedSourceFilesCache.cs class.
//  Classes              : ParsedSourceFilesCache.cs
// 
//  
//  ----------------------------------------------------------------------- 
//   <copyright file="ParsedSourceFilesCache.cs" company="Sysmex"> 
//       Copyright (c) Sysmex. All rights reserved. 
//   </copyright> 
//  -----------------------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using CodeAnalysis.Core.Interfaces;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace CodeAnalysis.Core.Common
{

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