﻿using System;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.InteropServices;
using CodeAnalysis.Core.Configuration;
using CodeAnalyzer.UserInterface.Controls.Diagrams;
using CodeAnalyzer.UserInterface.Interfaces;
using CodeEvaluator.Packages.Core;
using CodeEvaluator.Packages.Core.Interfaces;
using CodeEvaluator.Packages.Vs2015.Wrappers;
using EnvDTE;
using EnvDTE80;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using StructureMap;

namespace CodeEvaluator.Packages.Vs2015
{

    #region Using

    #endregion

    /// <summary>
    ///     This is the class that implements the package exposed by this assembly.
    ///     The minimum requirement for a class to be considered a valid package for Visual Studio
    ///     is to implement the IVsPackage interface and register itself with the shell.
    ///     This package uses the helper classes defined inside the Managed Package Framework (MPF)
    ///     to do it: it derives from the Package class that provides the implementation of the
    ///     IVsPackage interface and uses the registration attributes defined in the framework to
    ///     register itself and its components with the shell.
    /// </summary>
    // This attribute tells the PkgDef creation utility (CreatePkgDef.exe) that this class is
    // a package.
    [PackageRegistration(UseManagedResourcesOnly = true)]
    // This attribute is used to register the information needed to show this package
    // in the Help/About dialog of Visual Studio.
    [InstalledProductRegistration("#110", "#112", "1.0", IconResourceID = 400)]
    // This attribute is needed to let the shell know that this package exposes some menus.
    [ProvideMenuResource("Menus.ctmenu", 1)]
    // This attribute registers a tool window exposed by this package.
    [ProvideToolWindow(typeof(GenerateWorkflowDiagramWindow))]
    [Guid(GuidList.guidRomSoft_Client_DebugPkgString)]
    public sealed class CodeEvaluatorVs2015Package : Package
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Default constructor of the package.
        ///     Inside this method you can place any initialization code that does not require
        ///     any Visual Studio service because at this point the package object is created but
        ///     not sited yet inside Visual Studio environment. The place to do all the other
        ///     initialization is the Initialize method.
        /// </summary>
        public CodeEvaluatorVs2015Package()
        {
            Debug.WriteLine(CultureInfo.CurrentCulture.ToString(), "Entering constructor for: {0}",
                ToString());
        }

        #endregion

        #region Protected Methods and Operators

        /// <summary>
        ///     Initialization of the package; this method is called right after the package is sited, so this is the place
        ///     where you can put all the initialization code that rely on services provided by VisualStudio.
        /// </summary>
        protected override void Initialize()
        {
            Debug.WriteLine(CultureInfo.CurrentCulture.ToString(), "Entering Initialize() of: {0}",
                ToString());
            base.Initialize();

            RegisterVisualStudioServices();
            RegisterInternalServices();
            RegisterCodeAnalyzerServices();
            RegisterVisualStudioIntegration();
        }

        private void RegisterCodeAnalyzerServices()
        {
            StandardSetupBootstrapper.RegisterStandardComponents();
        }

        #endregion

        #region Private Methods and Operators

        private void RegisterInternalServices()
        {
            ObjectFactory.Configure(
                config => config.For<IWorkflowDiagramGenerator>().Use(() => new WorkflowDiagramGenerator()));
            ObjectFactory.Configure(config => config.For<IWorkflowDiagramSizes>().Use(() => new WorkflowDiagramSizes()));
            ObjectFactory.Configure(config => config.SetAllProperties(x => x.OfType<IWorkflowDiagramSizes>()));
            ObjectFactory.Configure(config => config.For<IProjectFilesProvider>().Use(() => new ProjectFilesProvider()));
            ObjectFactory.Configure(config => config.SetAllProperties(x => x.OfType<IProjectFilesProvider>()));
            ObjectFactory.Configure(config => config.For<ISystemSettings>().Use(new SystemSettings()));
            ObjectFactory.Configure(config => config.SetAllProperties(x => x.OfType<ISystemSettings>()));
        }

        private void RegisterVisualStudioIntegration()
        {
            // Add our command handlers for menu (commands must exist in the .vsct file)
            var mcs = GetService(typeof(IMenuCommandService)) as OleMenuCommandService;
            if (null != mcs)
            {
                // Create the command for the tool window
                var toolwndCommandID = new CommandID(
                    GuidList.guidRomSoft_Client_DebugCmdSet,
                    (int) PkgCmdIDList.CmdGenerateWorkflowDiagram);
                var menuToolWin = new MenuCommand(ShowToolWindow, toolwndCommandID);
                mcs.AddCommand(menuToolWin);
            }

            var solution = ObjectFactory.GetInstance<IVsSolution>();
            uint cookie;
            solution.AdviseSolutionEvents(new VsSolutionEventsWrapper(), out cookie);
        }

        private void RegisterVisualStudioServices()
        {
            ObjectFactory.Configure(config => config.For<DTE>().Use(() => GetService(typeof(DTE)) as DTE));
            ObjectFactory.Configure(config => config.For<DTE2>().Use(() => GetService(typeof(DTE2)) as DTE2));
            ObjectFactory.Configure(
                config => config.For<IVsSolution>().Use(() => GetService(typeof(SVsSolution)) as IVsSolution));
            ObjectFactory.Configure(config => config.For<Package>().Use(this));
        }

        /// <summary>
        ///     This function is called when the user clicks the menu item that shows the
        ///     tool window. See the Initialize method to see how the menu item is associated to
        ///     this function using the OleMenuCommandService service and the MenuCommand class.
        /// </summary>
        private void ShowToolWindow(object sender, EventArgs e)
        {
            // Get the instance number 0 of this tool window. This window is single instance so this instance
            // is actually the only one.
            // The last flag is set to true so that if the tool window does not exists it will be created.
            var window = FindToolWindow(typeof(GenerateWorkflowDiagramWindow), 0, true);
            if ((null == window) || (null == window.Frame))
            {
                throw new NotSupportedException(Resources._112);
            }
            var windowFrame = (IVsWindowFrame) window.Frame;
            ErrorHandler.ThrowOnFailure(windowFrame.Show());
        }

        #endregion
    }
}