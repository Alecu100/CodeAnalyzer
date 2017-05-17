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
using System.Linq;
using CodeEvaluator.Packages.Core.Interfaces;

namespace CodeEvaluator.Packages.Core
{

    #region Using

    #endregion

    public class ProjectFilesProvider : IProjectFilesProvider
    {
        private List<IProjectItemWrapper> _loadedProjectItems;

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

        public IList<IReference> GetAllReferencesFromProjects(IList<IProjectWrapper> searchLocations)
        {
            var references = new List<IReference>();

            foreach (var projectWrapper in searchLocations)
            {
                foreach (var reference in projectWrapper.References)
                {
                    if (searchLocations.All(location => !location.Name.Contains(reference.Name)))
                    {
                        references.Add(reference);
                    }
                }
            }


            return references;
        }

        #endregion
    }
}