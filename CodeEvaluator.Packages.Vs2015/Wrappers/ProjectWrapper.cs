namespace CodeEvaluator.Packages.Vs2015.Wrappers
{
    using System;
    using System.Collections.Generic;

    using CodeEvaluator.Packages.Core.Interfaces;

    using EnvDTE;

    using VSLangProj;

    public class ProjectWrapper : IProjectWrapper
    {
        private readonly Project _project;

        public ProjectWrapper(Project project)
        {
            _project = project;
        }

        public string Kind
        {
            get
            {
                try
                {
                    return _project.Kind;
                }
                catch (Exception)
                {
                    return string.Empty;
                }
            }
        }

        public string UniqueName
        {
            get
            {
                try
                {
                    return _project.UniqueName;
                }
                catch (Exception)
                {
                    return string.Empty;
                }
            }
        }

        public string Name
        {
            get
            {
                try
                {
                    return _project.Name;
                    ;
                }
                catch (Exception)
                {
                    return string.Empty;
                }
            }
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
                        projectItemWrappers.Add(new ProjectItemWrapper((ProjectItem)projectItem));
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
                            referencesWrappers.Add(new ReferenceWrapper((Reference)reference));
                        }
                    }
                }

                return referencesWrappers;
            }
        }
    }
}