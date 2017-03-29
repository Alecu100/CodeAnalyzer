using System.Collections.Generic;
using CodeAnalyzer.UserInterface.Interfaces;
using CodeEvaluator.Packages.Core.Interfaces;
using EnvDTE;

namespace CodeEvaluator.Packages.Vs2015.Wrappers
{
    public class ProjectWrapper : IProjectWrapper
    {
        private readonly Project _project;

        public ProjectWrapper(Project project)
        {
            _project = project;
        }

        public string Kind
        {
            get { return _project.Kind; }
        }

        public string UniqueName
        {
            get { return _project.UniqueName; }
        }

        public IEnumerable<IProjectItemWrapper> ProjectItems
        {
            get
            {
                var projectItemWrappers = new List<IProjectItemWrapper>();

                foreach (var projectItem in _project.ProjectItems)
                {
                    if (projectItem is ProjectItem)
                    {
                        projectItemWrappers.Add(new ProjectItemWrapper((ProjectItem) projectItem));
                    }
                }

                return projectItemWrappers;
            }
        }
    }
}