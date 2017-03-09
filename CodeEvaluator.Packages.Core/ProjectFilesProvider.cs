//  Project              : GLP
//  Module               : RomSoft.Client.Debug.dll
//  File                 : ProjectFilesProvider.cs
//  Author               : Alecsandru
//  Last Updated         : 02/12/2015 at 15:36
//  
// 
//  Contains             : Implementation of the ProjectFilesProvider.cs class.
//  Classes              : ProjectFilesProvider.cs
// 
//  
//  ----------------------------------------------------------------------- 
//   <copyright file="ProjectFilesProvider.cs" company="Sysmex"> 
//       Copyright (c) Sysmex. All rights reserved. 
//   </copyright> 
//  -----------------------------------------------------------------------

using System.Collections.Generic;
using CodeAnalyzer.UserInterface.Interfaces;
using CodeEvaluator.Packages.Core.Interfaces;

namespace CodeEvaluator.Packages.Core
{

    #region Using

    #endregion

    public class ProjectFilesProvider : IProjectFilesProvider
    {
        #region Fields

        private List<IProjectItemWrapper> _loadedProjectItems;

        #endregion

        #region Public Methods and Operators

        public IList<IProjectItemWrapper> GetAllSourceFileNamesFromProjects(IList<IProjectWrapper> searchLocations)
        {
            _loadedProjectItems = new List<IProjectItemWrapper>();

            foreach (var project in searchLocations)
            {
                foreach (var projectItem in project.ProjectItems)
                {
                    AddProjectItem(projectItem);
                }
            }

            return _loadedProjectItems;
        }

        #endregion

        #region Private Methods and Operators

        private void AddProjectItem(IProjectItemWrapper projectItem)
        {
            if (projectItem.Kind == VsConstants.FolderFileKind)
            {
                foreach (var item in projectItem.ProjectItems)
                {
                    AddProjectItem(item);
                }

                return;
            }

            if (projectItem.Kind == VsConstants.CsFileKind)
            {
                _loadedProjectItems.Add(projectItem);
            }
        }

        #endregion
    }
}