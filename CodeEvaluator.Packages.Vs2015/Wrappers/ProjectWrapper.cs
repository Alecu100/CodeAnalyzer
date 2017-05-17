using System.Collections.Generic;
using CodeEvaluator.Packages.Core.Interfaces;
using EnvDTE;
using VSLangProj;

namespace CodeEvaluator.Packages.Vs2015.Wrappers
{
    public class ProjectWrapper : IProjectWrapper
    {
        private readonly Project _project;

        public ProjectWrapper(Project project)
        {
            _project = project;
        }

        public VSProject VsProject
        {
            get { return _project.Object as VSProject; }
        }

        public string Kind
        {
            get { return _project.Kind; }
        }

        public string UniqueName
        {
            get { return _project.UniqueName; }
        }

        public string Name
        {
            get { return _project.Name; }
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

        public IEnumerable<IReference> References
        {
            get
            {
                var referencesWrappers = new List<IReference>();

                var vsProject = _project.Object as VSProject;

                if (vsProject != null)
                {
                    foreach (var reference in vsProject.References)
                    {
                        if (reference is Reference)
                        {
                            referencesWrappers.Add(new ReferenceWrapper((Reference) reference));
                        }
                    }
                }

                return referencesWrappers;
            }
        }
    }
}