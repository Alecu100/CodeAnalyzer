namespace CodeEvaluator.UserInterface.Controls.Views
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.IO;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;

    using CodeEvaluator.Evaluation.Interfaces;
    using CodeEvaluator.Evaluation.Listeners;
    using CodeEvaluator.Packages.Core;
    using CodeEvaluator.Packages.Core.Interfaces;
    using CodeEvaluator.UserInterface.Interfaces;
    using CodeEvaluator.Workflows;

    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;

    using StructureMap;

    #region Using

    #endregion

    /// <summary>
    ///     Interaction logic for MyControl.xaml
    /// </summary>
    public partial class GenerateWorkflowDiagramControl : IXamlResourcesRepository
    {
        private void btnExportDiagram_Click(object sender, RoutedEventArgs e)
        {
        }

        private void LstAvailableProjects_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var item = ((FrameworkElement)e.OriginalSource).DataContext as IProjectWrapper;
            if (item != null)
            {
                AvailableProjects.Remove(item);
                SelectedProjects.Add(item);
            }
        }

        private void LstSelectedProjects_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var item = ((FrameworkElement)e.OriginalSource).DataContext as IProjectWrapper;
            if (item != null)
            {
                SelectedProjects.Remove(item);
                AvailableProjects.Add(item);
            }
        }

        #region Constructors and Destructors

        public GenerateWorkflowDiagramControl()
        {
            InitializeComponent();

            //var solution = ObjectFactory.GetInstance<IVsSolution>();

            //solution.AdviseSolutionEvents(this, out _solutionCookie);

            LayoutUpdated += OnLayoutUpdated;

            ObjectFactory.Configure(config => config.For<IXamlResourcesRepository>().Use(this));

            try
            {
                var solution = ObjectFactory.GetInstance<ISolutionWrapper>();

                var currentProjectSettings =
                    SystemSettings.GetProjectSettingsByFullSolutionName(
                        _currentSolution != null ? _currentSolution.FullName : string.Empty);

                if (solution != null)
                {
                    foreach (var project in solution.Projects)
                    {
                        AddLoadedProject(project, currentProjectSettings);
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        private void AddLoadedProject(IProjectWrapper project, ProjectSettings currentProjectSettings)
        {
            LoadedProjects.Add(project);

            if (currentProjectSettings != null && currentProjectSettings.SelectedProjects.Contains(project.UniqueName))
            {
                SelectedProjects.Add(project);
            }
            else
            {
                AvailableProjects.Add(project);
            }

            if (currentProjectSettings != null && currentProjectSettings.SelectedProjectName == project.UniqueName)
            {
                cmbTargetProject.SelectedItem = project;
                var projectItems = cmbTargetFile.Items.Cast<IProjectItemWrapper>();
                var selectedFile = projectItems.FirstOrDefault(p => p.Name == currentProjectSettings.SelectedFileName);

                if (selectedFile != null)
                {
                    cmbTargetFile.SelectedItem = selectedFile;

                    var selectedClass =
                        cmbTargetClass.Items.Cast<ClassDeclarationSyntax>()
                            .FirstOrDefault(cd => cd.Identifier.ValueText == currentProjectSettings.SelectedClassName);

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

        #endregion

        #region SpecificFields

        private ISolutionWrapper _currentSolution;

        private ClassDeclarationSyntax _selectedClassDeclarationSyntax;

        private IProjectItemWrapper _selectedFile;

        private MethodDeclarationSyntax _selectedMethodDeclarationSyntax;

        private IProjectWrapper _selectedProject;

        //private readonly uint _solutionCookie;

        #endregion

        #region Public Properties

        public ObservableCollection<IProjectWrapper> AvailableProjects { get; } =
            new ObservableCollection<IProjectWrapper>();

        public ObservableCollection<ClassDeclarationSyntax> LoadedClasses { get; } =
            new ObservableCollection<ClassDeclarationSyntax>();

        public ObservableCollection<MethodDeclarationSyntax> LoadedMethods { get; } =
            new ObservableCollection<MethodDeclarationSyntax>();

        public ObservableCollection<IProjectItemWrapper> LoadedProjectItems { get; } =
            new ObservableCollection<IProjectItemWrapper>();

        public ObservableCollection<IProjectWrapper> LoadedProjects { get; } =
            new ObservableCollection<IProjectWrapper>();

        public ObservableCollection<IProjectWrapper> SelectedProjects { get; } =
            new ObservableCollection<IProjectWrapper>();

        public ISystemSettings SystemSettings
        {
            get
            {
                return ObjectFactory.GetInstance<ISystemSettings>();
            }
        }

        #endregion

        #region Public Methods and Operators

        public void OnAfterCloseSolution( /*object pUnkReserved*/)
        {
            ClearWorkspace();
            //return VSConstants.S_OK;
        }

        private void ClearWorkspace()
        {
            LoadedProjects.Clear();
            AvailableProjects.Clear();
            LoadedClasses.Clear();
            LoadedMethods.Clear();
            LoadedProjectItems.Clear();
            SelectedProjects.Clear();
            cnvWorkflow.Children.Clear();
        }

        public void OnAfterOpenProject(IProjectWrapper loadedProject)
        {
            if (loadedProject != null && loadedProject.Kind.ToUpperInvariant() == VsConstants.CsProjectKind)
            {
                var currentProjectSettings =
                    SystemSettings.GetProjectSettingsByFullSolutionName(
                        _currentSolution != null ? _currentSolution.FullName : string.Empty);

                AddLoadedProject(loadedProject, currentProjectSettings);
            }
        }

        public void OnAfterOpenSolution(ISolutionWrapper newSolution /*object pUnkReserved, int fNewSolution*/)
        {
            ClearWorkspace();

            _currentSolution = newSolution;
        }

        public void OnBeforeCloseSolution()
        {
            if (_currentSolution == null)
            {
                return;
            }

            var selectedProjects = new List<string>();

            foreach (var selectedProject in SelectedProjects)
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

            //return VSConstants.S_OK;
        }

        #endregion

        #region Private Methods and Operators

        private void AddAllClassesFromSyntaxNode(SyntaxNode syntaxNode)
        {
            if (syntaxNode is ClassDeclarationSyntax)
            {
                var classDeclarationSyntax = syntaxNode as ClassDeclarationSyntax;

                LoadedClasses.Add(classDeclarationSyntax);
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

            LoadedClasses.Clear();
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
                var itemsToAdd = new List<IProjectWrapper>();

                foreach (var selectedItem in lstAvailableProjects.SelectedItems)
                {
                    var project = selectedItem as IProjectWrapper;

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
                var itemsToRemove = new List<IProjectWrapper>();

                foreach (var selectedItem in lstSelectedProjects.SelectedItems)
                {
                    var project = selectedItem as IProjectWrapper;

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
            var projectFilesProvider = ObjectFactory.GetInstance<IProjectFilesProvider>();
            var allSourceFileNamesFromProjects = projectFilesProvider.GetAllSourceFileNamesFromProjects(
                SelectedProjects);
            var allReferencesFromProjects = projectFilesProvider.GetAllReferencesFromProjects(SelectedProjects);

            var codeFileNames = new List<string>();

            foreach (var allSourceFileNamesFromProject in allSourceFileNamesFromProjects)
            {
                codeFileNames.AddRange(allSourceFileNamesFromProject.FileNames);
            }

            var assemblyFileNames = new List<string>();

            foreach (var allReferencesFromProject in allReferencesFromProjects)
            {
                assemblyFileNames.Add(allReferencesFromProject.Path);
            }

            staticWorkflowEvaluator.Evaluate(
                new List<ISyntaxNodeEvaluatorListener> { new WorkflowEvaluatorEvaluatorListener() },
                codeFileNames,
                _selectedClassDeclarationSyntax,
                _selectedMethodDeclarationSyntax,
                assemblyFileNames);

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
            if (e.AddedItems.Count > 0 && e.AddedItems[0] is IProjectItemWrapper)
            {
                var projectItem = e.AddedItems[0] as IProjectItemWrapper;
                _selectedFile = projectItem;

                foreach (var fileName in projectItem.FileNames)
                {
                    AddClassesFromFile(fileName);
                }
            }
        }

        private void CmbTargetProject_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0 && e.AddedItems[0] is IProjectWrapper)
            {
                var project = (IProjectWrapper)e.AddedItems[0];
                LoadedProjectItems.Clear();
                var sourceFilesProvider = ObjectFactory.GetInstance<IProjectFilesProvider>();
                var allSourceFileNamesFromProjects =
                    sourceFilesProvider.GetAllSourceFileNamesFromProjects(new List<IProjectWrapper> { project })
                        .ToList();

                _selectedProject = project;

                allSourceFileNamesFromProjects.Sort(CompareSourceFiles);

                foreach (var sourceFile in allSourceFileNamesFromProjects)
                {
                    LoadedProjectItems.Add(sourceFile);
                }

                if (SelectedProjects.All(p => p != project))
                {
                    SelectedProjects.Add(project);
                    AvailableProjects.Remove(project);
                }
            }
        }

        private int CompareSourceFiles(IProjectItemWrapper item1, IProjectItemWrapper item2)
        {
            return string.CompareOrdinal(item1.Name, item2.Name);
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