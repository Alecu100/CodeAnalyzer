//  Project              : GLP
//  Module               : RomSoft.Client.Debug.dll
//  File                 : IParsedSourceFilesCache.cs
//  Author               : Alecsandru
//  Last Updated         : 30/11/2015 at 18:31
//  
// 
//  Contains             : Implementation of the IParsedSourceFilesCache.cs class.
//  Classes              : IParsedSourceFilesCache.cs
// 
//  
//  ----------------------------------------------------------------------- 
//   <copyright file="IParsedSourceFilesCache.cs" company="Sysmex"> 
//       Copyright (c) Sysmex. All rights reserved. 
//   </copyright> 
//  -----------------------------------------------------------------------

namespace RomSoft.Client.Debug.Library.Interfaces
{
    #region Using

    using System.Collections.Generic;

    using EnvDTE;

    using Microsoft.CodeAnalysis;

    #endregion

    public interface IParsedSourceFilesCache : IList<SyntaxTree>
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Rebuilds from projects.
        /// </summary>
        /// <param name="selectedProjects">The selected projects.</param>
        void RebuildFromProjects(IList<Project> selectedProjects);

        #endregion
    }
}