using System.Collections.Generic;
using CodeEvaluator.Packages.Core.Interfaces;
using EnvDTE;

namespace CodeEvaluator.ProjectVs2015.Wrappers
{
    public class ProjectItemWrapper : IProjectItemWrapper
    {
        private readonly ProjectItem _projectItem;

        public ProjectItemWrapper(ProjectItem projectItem)
        {
            _projectItem = projectItem;
        }

        public string Name
        {
            get { return _projectItem.Name; }
        }

        public string Kind
        {
            get { return _projectItem.Kind; }
        }

        public IEnumerable<IProjectItemWrapper> ProjectItems
        {
            get
            {
                var projectItemWrappers = new List<IProjectItemWrapper>();

                foreach (var projectItem in _projectItem.ProjectItems)
                {
                    if (projectItem is ProjectItem)
                    {
                        projectItemWrappers.Add(new ProjectItemWrapper((ProjectItem) projectItem));
                    }
                }

                return projectItemWrappers;
            }
        }

        public IEnumerable<string> FileNames
        {
            get
            {
                var fileNames = new List<string>();

                for (short i = 0; i < _projectItem.FileCount; i++)
                {
                    fileNames.Add(_projectItem.FileNames[i]);
                }

                return fileNames;
            }
        }
    }
}