//  Project              : GLP
//  Module               : RomSoft.Client.Debug.dll
//  File                 : IProjectFilesProvider.cs
//  Author               : Alecsandru
//  Last Updated         : 02/12/2015 at 15:36
//  
// 
//  Contains             : Implementation of the IProjectFilesProvider.cs class.
//  Classes              : IProjectFilesProvider.cs
// 
//  
//  ----------------------------------------------------------------------- 
//   <copyright file="IProjectFilesProvider.cs" company="Sysmex"> 
//       Copyright (c) Sysmex. All rights reserved. 
//   </copyright> 
//  -----------------------------------------------------------------------

using System.Collections.Generic;

namespace CodeAnalysis.Core.Interfaces
{
    #region Using

    

    #endregion

    public interface IProjectFilesProvider
    {
        #region Public Methods and Operators

        List<Reference> GetAllReferencesFromProjects(IList<Project> searchLocations);

        IList<ProjectItem> GetAllSourceFileNamesFromProjects(IList<Project> searchLocations);

        #endregion
    }
}