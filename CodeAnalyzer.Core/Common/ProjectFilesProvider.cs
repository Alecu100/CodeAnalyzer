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
using CodeAnalysis.Core.Constants;
using CodeAnalysis.Core.Interfaces;

namespace CodeAnalysis.Core.Common
{
    #region Using

    

    #endregion

    public class ProjectFilesProvider : IProjectFilesProvider
    {
        #region Fields

        private List<ProjectItem> _loadedProjectItems;

        #endregion

        #region Public Methods and Operators

        public List<Reference> GetAllReferencesFromProjects(IList<Project> searchLocations)
        {
            var references = new List<Reference>();

            foreach (var searchLocation in searchLocations)
            {
                var vsProject = searchLocation.Object as VSProject;
                if (vsProject != null)
                {
                    foreach (var referenceObj in vsProject.References)
                    {
                        var reference = referenceObj as Reference;
                        if (reference != null)
                        {
                            references.Add(reference);
                        }
                    }
                }
            }

            return references;
        }

        public IList<ProjectItem> GetAllSourceFileNamesFromProjects(IList<Project> searchLocations)
        {
            _loadedProjectItems = new List<ProjectItem>();

            foreach (var project in searchLocations)
            {
                foreach (var item in project.ProjectItems)
                {
                    var projectItem = item as ProjectItem;

                    if (projectItem != null)
                    {
                        AddProjectItem(projectItem);
                    }
                }
            }

            return _loadedProjectItems;
        }

        #endregion

        #region Private Methods and Operators

        private void AddProjectItem(ProjectItem projectItem)
        {
            if (projectItem.Kind == VsConstants.FolderFileKind)
            {
                foreach (var item in projectItem.ProjectItems)
                {
                    var childProjectItem = item as ProjectItem;

                    if (childProjectItem != null)
                    {
                        AddProjectItem(childProjectItem);
                    }
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