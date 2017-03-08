//  Project              : GLP
//  Module               : RomSoft.Client.Debug.dll
//  File                 : GenerateWorkflowDiagramControl.xaml.cs
//  Author               : Alecsandru
//  Last Updated         : 11/02/2016 at 16:33
//  
// 
//  Contains             : Implementation of the GenerateWorkflowDiagramControl.xaml.cs class.
//  Classes              : GenerateWorkflowDiagramControl.xaml.cs
// 
//  
//  ----------------------------------------------------------------------- 
//   <copyright file="GenerateWorkflowDiagramControl.xaml.cs" company="Sysmex"> 
//       Copyright (c) Sysmex. All rights reserved. 
//   </copyright> 
//  -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using CodeAnalysis.Core.Common;
using CodeAnalysis.Core.Constants;
using CodeAnalysis.Core.Interfaces;
using CodeAnalysis.Core.Listeners;
using CodeAnalyzer.UserInterface.Interfaces;
using StructureMap;

namespace CodeAnalyzer.UserInterface.Controls.Views
{
    #region Using

    

    #endregion

    /// <summary>
    ///     Interaction logic for MyControl.xaml
    /// </summary>
    public partial class GenerateWorkflowDiagramControl : IVsSolutionEvents, IXamlResourcesRepository
    {
        #region Fields

        private Solution _currentSolution;

        private ClassDeclarationSyntax _selectedClassDeclarationSyntax;

        private ProjectItem _selectedFile;

        private MethodDeclarationSyntax _selectedMethodDeclarationSyntax;

        private Project _selectedProject;

        private readonly ObservableCollection<Project> _availableProjects = new ObservableCollection<Project>();

        private readonly ObservableCollection<ClassDeclarationSyntax> _loadedClasses =
            new ObservableCollection<ClassDeclarationSyntax>();

        private readonly ObservableCollection<MethodDeclarationSyntax> _loadedMethods =
            new ObservableCollection<MethodDeclarationSyntax>();

        private readonly ObservableCollection<ProjectItem> _loadedProjectItems = new ObservableCollection<ProjectItem>();

        private readonly ObservableCollection<Project> _loadedProjects = new ObservableCollection<Project>();

        private readonly ObservableCollection<Project> _selectedProjects = new ObservableCollection<Project>();

        private readonly uint _solutionCookie;

        #endregion

        #region Constructors and Destructors

        public GenerateWorkflowDiagramControl()
        {
            InitializeComponent();

            var solution = ObjectFactory.GetInstance<IVsSolution>();

            solution.AdviseSolutionEvents(this, out _solutionCookie);

            LayoutUpdated += OnLayoutUpdated;

            ObjectFactory.Configure(config => config.For<IXamlResourcesRepository>().Use(this));
        }

        #endregion

        #region Public Properties

        public ObservableCollection<Project> AvailableProjects
        {
            get
            {
                return _availableProjects;
            }
        }

        public ObservableCollection<ClassDeclarationSyntax> LoadedClasses
        {
            get
            {
                return _loadedClasses;
            }
        }

        public ObservableCollection<MethodDeclarationSyntax> LoadedMethods
        {
            get
            {
                return _loadedMethods;
            }
        }

        public ObservableCollection<ProjectItem> LoadedProjectItems
        {
            get
            {
                return _loadedProjectItems;
            }
        }

        public ObservableCollection<Project> LoadedProjects
        {
            get
            {
                return _loadedProjects;
            }
        }

        public ObservableCollection<Project> SelectedProjects
        {
            get
            {
                return _selectedProjects;
            }
        }

        public ISystemSettings SystemSettings
        {
            get
            {
                return ObjectFactory.GetInstance<ISystemSettings>();
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Notifies listening clients that a solution has been closed.
        /// </summary>
        /// <returns>
        ///     If the method succeeds, it returns <see cref="F:Microsoft.VisualStudio.VSConstants.S_OK" />. If it fails, it
        ///     returns an error code.
        /// </returns>
        /// <param name="pUnkReserved">[in] Reserved for future use.</param>
        public int OnAfterCloseSolution(object pUnkReserved)
        {
            LoadedProjects.Clear();
            AvailableProjects.Clear();
            LoadedClasses.Clear();
            LoadedMethods.Clear();
            LoadedProjectItems.Clear();
            SelectedProjects.Clear();
            return VSConstants.S_OK;
        }

        /// <summary>
        ///     Notifies listening clients that the project has been loaded.
        /// </summary>
        /// <returns>
        ///     If the method succeeds, it returns <see cref="F:Microsoft.VisualStudio.VSConstants.S_OK" />. If it fails, it
        ///     returns an error code.
        /// </returns>
        /// <param name="pStubHierarchy">
        ///     [in] Pointer to the <see cref="T:Microsoft.VisualStudio.Shell.Interop.IVsHierarchy" />
        ///     interface of the placeholder hierarchy for the unloaded project.
        /// </param>
        /// <param name="pRealHierarchy">
        ///     [in] Pointer to the <see cref="T:Microsoft.VisualStudio.Shell.Interop.IVsHierarchy" />
        ///     interface of the project that was loaded.
        /// </param>
        public int OnAfterLoadProject(IVsHierarchy pStubHierarchy, IVsHierarchy pRealHierarchy)
        {
            return VSConstants.S_OK;
        }

        /// <summary>
        ///     Notifies listening clients that the project has been opened.
        /// </summary>
        /// <returns>
        ///     If the method succeeds, it returns <see cref="F:Microsoft.VisualStudio.VSConstants.S_OK" />. If it fails, it
        ///     returns an error code.
        /// </returns>
        /// <param name="pHierarchy">
        ///     [in] Pointer to the <see cref="T:Microsoft.VisualStudio.Shell.Interop.IVsHierarchy" />
        ///     interface of the project being loaded.
        /// </param>
        /// <param name="fAdded">
        ///     [in] true if the project is added to the solution after the solution is opened. false if the
        ///     project is added to the solution while the solution is being opened.
        /// </param>
        public int OnAfterOpenProject(IVsHierarchy pHierarchy, int fAdded)
        {
            object objProj;
            pHierarchy.GetProperty(VSConstants.VSITEMID_ROOT, (int)__VSHPROPID.VSHPROPID_ExtObject, out objProj);

            var projectItem = objProj as Project;

            if (projectItem != null && projectItem.Kind.ToUpperInvariant() == VsConstants.CsProjectKind)
            {
                _loadedProjects.Add(projectItem);

                var currentProjectSettings =
                    SystemSettings.GetProjectSettingsByFullSolutionName(
                        _currentSolution != null ? _currentSolution.FullName : string.Empty);

                if (currentProjectSettings != null
                    && currentProjectSettings.SelectedProjects.Contains(projectItem.UniqueName))
                {
                    _selectedProjects.Add(projectItem);
                }
                else
                {
                    _availableProjects.Add(projectItem);
                }

                if (currentProjectSettings != null
                    && currentProjectSettings.SelectedProjectName == projectItem.UniqueName)
                {
                    cmbTargetProject.SelectedItem = projectItem;
                    var projectItems = cmbTargetFile.Items.Cast<ProjectItem>();
                    var selectedFile =
                        projectItems.FirstOrDefault(p => p.Name == currentProjectSettings.SelectedFileName);

                    if (selectedFile != null)
                    {
                        cmbTargetFile.SelectedItem = selectedFile;

                        var selectedClass =
                            cmbTargetClass.Items.Cast<ClassDeclarationSyntax>()
                                .FirstOrDefault(
                                    cd => cd.Identifier.ValueText == currentProjectSettings.SelectedClassName);

                        if (selectedClass != null)
                        {
                            cmbTargetClass.SelectedItem = selectedClass;

                            var selectedMethod =
                                cmbStartMethod.Items.Cast<MethodDeclarationSyntax>()
                                    .FirstOrDefault(
                                        md => md.Identifier.ValueText == currentProjectSettings.SelectedMethodName);
                            {
                                cmbStartMethod.SelectedItem = selectedMethod;
                            }
                        }
                    }
                }
            }

            return VSConstants.S_OK;
        }

        /// <summary>
        ///     Notifies listening clients that the solution has been opened.
        /// </summary>
        /// <returns>
        ///     If the method succeeds, it returns <see cref="F:Microsoft.VisualStudio.VSConstants.S_OK" />. If it fails, it
        ///     returns an error code.
        /// </returns>
        /// <param name="pUnkReserved">[in] Reserved for future use.</param>
        /// <param name="fNewSolution">
        ///     [in] true if the solution is being created. false if the solution was created previously or
        ///     is being loaded.
        /// </param>
        public int OnAfterOpenSolution(object pUnkReserved, int fNewSolution)
        {
            var dte = ObjectFactory.GetInstance<DTE>();
            _currentSolution = dte.Solution;
            return VSConstants.S_OK;
        }

        /// <summary>
        ///     Notifies listening clients that the project is about to be closed.
        /// </summary>
        /// <returns>
        ///     If the method succeeds, it returns <see cref="F:Microsoft.VisualStudio.VSConstants.S_OK" />. If it fails, it
        ///     returns an error code.
        /// </returns>
        /// <param name="pHierarchy">
        ///     [in] Pointer to the <see cref="T:Microsoft.VisualStudio.Shell.Interop.IVsHierarchy" />
        ///     interface of the project being closed.
        /// </param>
        /// <param name="fRemoved">
        ///     [in] true if the project was removed from the solution before the solution was closed. false if
        ///     the project was removed from the solution while the solution was being closed.
        /// </param>
        public int OnBeforeCloseProject(IVsHierarchy pHierarchy, int fRemoved)
        {
            return VSConstants.S_OK;
        }

        /// <summary>
        ///     Notifies listening clients that the solution is about to be closed.
        /// </summary>
        /// <returns>
        ///     If the method succeeds, it returns <see cref="F:Microsoft.VisualStudio.VSConstants.S_OK" />. If it fails, it
        ///     returns an error code.
        /// </returns>
        /// <param name="pUnkReserved">[in] Reserved for future use.</param>
        public int OnBeforeCloseSolution(object pUnkReserved)
        {
            if (_currentSolution == null)
            {
                return VSConstants.S_OK;
            }

            var selectedProjects = new List<string>();

            foreach (var selectedProject in _selectedProjects)
            {
                selectedProjects.Add(selectedProject.UniqueName);
            }

            SystemSettings.AddOrReplaceProjectSettings(
                new ProjectSettings
                    {
                        SelectedProjects = selectedProjects,
                        SelectedClassName =
                            _selectedClassDeclarationSyntax != null
                                ? _selectedClassDeclarationSyntax.Identifier.ValueText
                                : null,
                        SelectedMethodName =
                            _selectedMethodDeclarationSyntax != null
                                ? _selectedMethodDeclarationSyntax.Identifier.ValueText
                                : null,
                        SelectedProjectName =
                            _selectedProject != null ? _selectedProject.UniqueName : null,
                        SelectedFileName = _selectedFile != null ? _selectedFile.Name : null,
                        FullSolutionName = _currentSolution.FullName
                    });
            SystemSettings.Save();

            return VSConstants.S_OK;
        }

        /// <summary>
        ///     Notifies listening clients that the project is about to be unloaded.
        /// </summary>
        /// <returns>
        ///     If the method succeeds, it returns <see cref="F:Microsoft.VisualStudio.VSConstants.S_OK" />. If it fails, it
        ///     returns an error code.
        /// </returns>
        /// <param name="pRealHierarchy">
        ///     [in] Pointer to the <see cref="T:Microsoft.VisualStudio.Shell.Interop.IVsHierarchy" />
        ///     interface of the project that will be unloaded.
        /// </param>
        /// <param name="pStubHierarchy">
        ///     [in] Pointer to the <see cref="T:Microsoft.VisualStudio.Shell.Interop.IVsHierarchy" />
        ///     interface of the placeholder hierarchy for the project being unloaded.
        /// </param>
        public int OnBeforeUnloadProject(IVsHierarchy pRealHierarchy, IVsHierarchy pStubHierarchy)
        {
            return VSConstants.S_OK;
        }

        /// <summary>
        ///     Queries listening clients as to whether the project can be closed.
        /// </summary>
        /// <returns>
        ///     If the method succeeds, it returns <see cref="F:Microsoft.VisualStudio.VSConstants.S_OK" />. If it fails, it
        ///     returns an error code.
        /// </returns>
        /// <param name="pHierarchy">
        ///     [in] Pointer to the <see cref="T:Microsoft.VisualStudio.Shell.Interop.IVsHierarchy" />
        ///     interface of the project to be closed.
        /// </param>
        /// <param name="fRemoving">
        ///     [in] true if the project is being removed from the solution before the solution is closed.
        ///     false if the project is being removed from the solution while the solution is being closed.
        /// </param>
        /// <param name="pfCancel">
        ///     [out] true if the client vetoed the closing of the project. false if the client approved the
        ///     closing of the project.
        /// </param>
        public int OnQueryCloseProject(IVsHierarchy pHierarchy, int fRemoving, ref int pfCancel)
        {
            return VSConstants.S_OK;
        }

        /// <summary>
        ///     Queries listening clients as to whether the solution can be closed.
        /// </summary>
        /// <returns>
        ///     If the method succeeds, it returns <see cref="F:Microsoft.VisualStudio.VSConstants.S_OK" />. If it fails, it
        ///     returns an error code.
        /// </returns>
        /// <param name="pUnkReserved">[in] Reserved for future use.</param>
        /// <param name="pfCancel">
        ///     [out] true if the client vetoed closing the solution. false if the client approved closing the
        ///     solution.
        /// </param>
        public int OnQueryCloseSolution(object pUnkReserved, ref int pfCancel)
        {
            return VSConstants.S_OK;
        }

        /// <summary>
        ///     Queries listening clients as to whether the project can be unloaded.
        /// </summary>
        /// <returns>
        ///     If the method succeeds, it returns <see cref="F:Microsoft.VisualStudio.VSConstants.S_OK" />. If it fails, it
        ///     returns an error code.
        /// </returns>
        /// <param name="pRealHierarchy">
        ///     [in] Pointer to the <see cref="T:Microsoft.VisualStudio.Shell.Interop.IVsHierarchy" />
        ///     interface of the project to be unloaded.
        /// </param>
        /// <param name="pfCancel">
        ///     [out] true if the client vetoed unloading the project. false if the client approved unloading
        ///     the project.
        /// </param>
        public int OnQueryUnloadProject(IVsHierarchy pRealHierarchy, ref int pfCancel)
        {
            return VSConstants.S_OK;
        }

        #endregion

        #region Private Methods and Operators

        private void AddAllClassesFromSyntaxNode(SyntaxNode syntaxNode)
        {
            if (syntaxNode is ClassDeclarationSyntax)
            {
                var classDeclarationSyntax = syntaxNode as ClassDeclarationSyntax;

                _loadedClasses.Add(classDeclarationSyntax);
                return;
            }

            foreach (var childNode in syntaxNode.ChildNodes())
            {
                AddAllClassesFromSyntaxNode(childNode);
            }
        }

        private void AddClassesFromFile(string fileName)
        {
            var allCode = File.ReadAllText(fileName);
            var syntaxTree = CSharpSyntaxTree.ParseText(allCode);
            var syntaxNode = syntaxTree.GetRoot();

            _loadedClasses.Clear();
            AddAllClassesFromSyntaxNode(syntaxNode);
        }

        private void AddMethodsFromClassBody(SyntaxNode syntaxNode)
        {
            if (syntaxNode is MethodDeclarationSyntax)
            {
                LoadedMethods.Add(syntaxNode as MethodDeclarationSyntax);
                return;
            }

            foreach (var childNode in syntaxNode.ChildNodes())
            {
                if (!(childNode is ClassDeclarationSyntax))
                {
                    AddMethodsFromClassBody(childNode);
                }
            }
        }

        private void BtnAddProject_OnClick(object sender, RoutedEventArgs e)
        {
            if (lstAvailableProjects.SelectedItems.Count > 0)
            {
                var itemsToAdd = new List<Project>();

                foreach (var selectedItem in lstAvailableProjects.SelectedItems)
                {
                    var project = selectedItem as Project;

                    if (project != null)
                    {
                        itemsToAdd.Add(project);
                    }
                }

                foreach (var project in itemsToAdd)
                {
                    AvailableProjects.Remove(project);
                    SelectedProjects.Add(project);
                }
            }
        }

        private void BtnRemoveProject_OnClick(object sender, RoutedEventArgs e)
        {
            if (lstSelectedProjects.SelectedItems.Count > 0)
            {
                var itemsToRemove = new List<Project>();

                foreach (var selectedItem in lstSelectedProjects.SelectedItems)
                {
                    var project = selectedItem as Project;

                    if (project != null)
                    {
                        itemsToRemove.Add(project);
                    }
                }

                foreach (var project in itemsToRemove)
                {
                    AvailableProjects.Add(project);
                    SelectedProjects.Remove(project);
                }
            }
        }

        private void BtnStartSearch_OnClick(object sender, RoutedEventArgs e)
        {
            var staticWorkflowEvaluator = ObjectFactory.GetInstance<ICodeEvaluator>();

            staticWorkflowEvaluator.Evaluate(
                new List<ICodeEvaluatorListener> { new WorkflowEvaluatorEvaluatorListener() },
                SelectedProjects,
                _selectedClassDeclarationSyntax,
                _selectedMethodDeclarationSyntax);

            var workflowExecutionSnapshot = WorkflowEvaluator.CurrentExecutionSnapshot;

            var workflowDiagramGenerator = ObjectFactory.GetInstance<IWorkflowDiagramGenerator>();
            workflowDiagramGenerator.GenerateWorkflowDiagram(workflowExecutionSnapshot, cnvWorkflow);
        }

        private void CmbStartMethod_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0 && e.AddedItems[0] is MethodDeclarationSyntax)
            {
                _selectedMethodDeclarationSyntax = e.AddedItems[0] as MethodDeclarationSyntax;
            }
        }

        private void CmbTargetClass_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0 && e.AddedItems[0] is ClassDeclarationSyntax)
            {
                _selectedClassDeclarationSyntax = e.AddedItems[0] as ClassDeclarationSyntax;

                LoadedMethods.Clear();

                AddMethodsFromClassBody(_selectedClassDeclarationSyntax);
            }
        }

        private void CmbTargetFile_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0 && e.AddedItems[0] is ProjectItem)
            {
                var projectItem = e.AddedItems[0] as ProjectItem;
                _selectedFile = projectItem;

                for (short i = 0; i < projectItem.FileCount; i++)
                {
                    AddClassesFromFile(projectItem.FileNames[i]);
                }
            }
        }

        private void CmbTargetProject_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0 && e.AddedItems[0] is Project)
            {
                var project = (Project)e.AddedItems[0];
                LoadedProjectItems.Clear();
                var sourceFilesProvider = ObjectFactory.GetInstance<IProjectFilesProvider>();
                var allSourceFileNamesFromProjects =
                    sourceFilesProvider.GetAllSourceFileNamesFromProjects(new List<Project> { project }).ToList();

                _selectedProject = project;

                allSourceFileNamesFromProjects.Sort(CompareSourceFiles);

                foreach (var sourceFile in allSourceFileNamesFromProjects)
                {
                    _loadedProjectItems.Add(sourceFile);
                }

                if (_selectedProjects.All(p => p != project))
                {
                    _selectedProjects.Add(project);
                    _availableProjects.Remove(project);
                }
            }
        }

        private int CompareSourceFiles(ProjectItem item1, ProjectItem item2)
        {
            return String.CompareOrdinal(item1.Name, item2.Name);
        }

        private void OnLayoutUpdated(object sender, EventArgs eventArgs)
        {
            var grdContentHeight = ActualHeight > 800 ? ActualHeight : 800;
            var grdContentWitdh = ActualWidth > 800 ? ActualWidth : 800;

            if (Math.Abs(grdContentHeight - grdContent.Height) > 0.1d
                || Math.Abs(grdContentWitdh - grdContent.Width) > 0.1d)
            {
                grdContent.Height = grdContentHeight;
                grdContent.Width = grdContentWitdh;

                lstAvailableProjects.Height = 0;
                lstAvailableProjects.Width = 0;

                lstSelectedProjects.Height = 0;
                lstSelectedProjects.Width = 0;

                cnvWorkflow.Height = 0;
                cnvWorkflow.Width = 0;
            }

            else
            {
                var lblAvailableProjectsTransform =
                    lblAvailableProjects.TransformToAncestor(grpSearchOptions).Transform(new Point(0, 0));
                var btnAddProjectTransform =
                    btnAddProject.TransformToAncestor(grpSearchOptions).Transform(new Point(0, 0));

                var lstAvailableProjectsHeight = btnAddProjectTransform.Y - lblAvailableProjectsTransform.Y
                                                 - lblAvailableProjects.ActualHeight - 28;

                var lstAvailableProjectsWidth = grpSearchOptions.ActualWidth - 32;

                if (Math.Abs(lstAvailableProjects.Height - lstAvailableProjectsHeight) > 0.1d
                    || Math.Abs(lstAvailableProjects.Width - lstAvailableProjectsWidth) > 0.1d)
                {
                    lstAvailableProjects.Height = lstAvailableProjectsHeight;

                    lstAvailableProjects.Width = lstAvailableProjectsWidth;
                }

                var lblSelectedProjectsTransform =
                    lblSelectedProjects.TransformToAncestor(grpSearchOptions).Transform(new Point(0, 0));
                var btnRemoveProjectTransform =
                    btnRemoveProject.TransformToAncestor(grpSearchOptions).Transform(new Point(0, 0));

                var lstSelectedProjectsHeight = btnRemoveProjectTransform.Y - lblSelectedProjectsTransform.Y
                                                - lblSelectedProjects.ActualHeight - 28;

                var lstSelectedProjectsWidth = grpSearchOptions.ActualWidth - 32;

                if (Math.Abs(lstSelectedProjects.Height - lstSelectedProjectsHeight) > 0.1d
                    || Math.Abs(lstSelectedProjects.Width - lstSelectedProjectsWidth) > 0.1d)
                {
                    lstSelectedProjects.Height = lstSelectedProjectsHeight;

                    lstSelectedProjects.Width = lstSelectedProjectsWidth;
                }

                var scrlWorkflowHeight = grdContent.ActualHeight - 90;
                var scrlWorkflowWidth = grpWorkflow.ActualWidth - 40;

                if (Math.Abs(scrlWorkflow.Height - scrlWorkflowHeight) > 0.1d
                    || Math.Abs(scrlWorkflow.Width - scrlWorkflowWidth) > 0.1d)
                {
                    scrlWorkflow.Height = scrlWorkflowHeight;
                    scrlWorkflow.Width = scrlWorkflowWidth;
                }
            }
        }

        #endregion
    }
}