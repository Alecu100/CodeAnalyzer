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
using StructureMap;

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
        public void RebuildFromProjects(IList<Project> selectedProjects)
        {
            Clear();
            var sourceFilesProvider = ObjectFactory.GetInstance<IProjectFilesProvider>();
            var allSourceFileNamesFromProjects = sourceFilesProvider.GetAllSourceFileNamesFromProjects(selectedProjects);

            Parallel.ForEach(
                allSourceFileNamesFromProjects,
                (allSourceFileNamesFromProject, state, arg3) =>
                {
                    for (short i = 0; i < allSourceFileNamesFromProject.FileCount; i++)
                    {
                        var sourceText = File.ReadAllText(allSourceFileNamesFromProject.FileNames[i]);
                        var syntaxTree = CSharpSyntaxTree.ParseText(sourceText);
                        lock (this)
                        {
                            Add(syntaxTree);
                        }
                    }
                });
        }

        #endregion
    }
}