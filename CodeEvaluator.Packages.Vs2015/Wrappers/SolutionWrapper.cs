namespace CodeEvaluator.Packages.Vs2015.Wrappers
{
    using System.Collections.Generic;

    using CodeEvaluator.Packages.Core;
    using CodeEvaluator.Packages.Core.Interfaces;

    using EnvDTE;

    public class SolutionWrapper : ISolutionWrapper
    {
        private readonly Solution _solution;

        public SolutionWrapper(Solution solution)
        {
            _solution = solution;
        }

        public IEnumerable<IProjectWrapper> Projects
        {
            get
            {
                var projectWrappers = new List<IProjectWrapper>();

                foreach (var pr in _solution.Projects)
                {
                    var project = (Project)pr;

                    if (project.Kind.ToUpperInvariant() == VsConstants.CsProjectKind)
                    {
                        projectWrappers.Add(new ProjectWrapper(project));
                    }
                    else if (project.Kind.ToUpperInvariant() == VsConstants.VirtualFolderFileKind)
                    {
                        GetProjects(project, projectWrappers);
                    }
                }

                return projectWrappers;
            }
        }

        public string FullName
        {
            get
            {
                return _solution.FullName;
            }
        }

        private void GetProjects(Project project, List<IProjectWrapper> projects)
        {
            if (project.Kind.ToUpperInvariant() == VsConstants.VirtualFolderFileKind)
            {
                foreach (var item in project.ProjectItems)
                {
                    var projectItem = (ProjectItem)item;

                    if (projectItem.SubProject != null)
                    {
                        var subProject = projectItem.SubProject;

                        if (subProject.Kind.ToUpperInvariant() == VsConstants.CsProjectKind)
                        {
                            projects.Add(new ProjectWrapper(subProject));
                        }
                        else if (subProject.Kind.ToUpperInvariant() == VsConstants.VirtualFolderFileKind)
                        {
                            GetProjects(subProject, projects);
                        }
                    }
                }
            }
        }
    }
}