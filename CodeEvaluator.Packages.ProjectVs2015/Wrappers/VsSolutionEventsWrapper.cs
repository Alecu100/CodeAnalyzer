using CodeAnalyzer.UserInterface.Controls.Views;
using EnvDTE;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using StructureMap;

namespace CodeEvaluator.ProjectVs2015.Wrappers
{
    public class VsSolutionEventsWrapper : IVsSolutionEvents
    {
        private GenerateWorkflowDiagramControl _workflowDiagramControl;

        public GenerateWorkflowDiagramControl WorkflowDiagramControl
        {
            get
            {
                if (_workflowDiagramControl == null)
                {
                    var vsUiShell = (IVsUIShell) Package.GetGlobalService(typeof(SVsUIShell));
                    var guid = typeof(GenerateWorkflowDiagramWindow).GUID;
                    IVsWindowFrame windowFrame;
                    var result = vsUiShell.FindToolWindow((uint) __VSFINDTOOLWIN.FTW_fFindFirst, ref guid,
                        out windowFrame);


                    if (result != VSConstants.S_OK)
                        vsUiShell.FindToolWindow((uint) __VSFINDTOOLWIN.FTW_fForceCreate, ref guid, out windowFrame);
                            // Crate MyToolWindow if not found

                    var generateWorkflowDiagramWindow = (GenerateWorkflowDiagramWindow) windowFrame;

                    _workflowDiagramControl = (GenerateWorkflowDiagramControl) generateWorkflowDiagramWindow.Content;
                }

                return _workflowDiagramControl;
            }
        }

        public int OnAfterOpenProject(IVsHierarchy pHierarchy, int fAdded)
        {
            object objProj;

            pHierarchy.GetProperty(VSConstants.VSITEMID_ROOT, (int) __VSHPROPID.VSHPROPID_ExtObject, out objProj);

            var projectItem = objProj as Project;

            WorkflowDiagramControl.OnAfterOpenProject(new ProjectWrapper(projectItem));

            return VSConstants.S_OK;
        }

        public int OnQueryCloseProject(IVsHierarchy pHierarchy, int fRemoving, ref int pfCancel)
        {
            return VSConstants.S_OK;
        }

        public int OnBeforeCloseProject(IVsHierarchy pHierarchy, int fRemoved)
        {
            return VSConstants.S_OK;
        }

        public int OnAfterLoadProject(IVsHierarchy pStubHierarchy, IVsHierarchy pRealHierarchy)
        {
            return VSConstants.S_OK;
        }

        public int OnQueryUnloadProject(IVsHierarchy pRealHierarchy, ref int pfCancel)
        {
            return VSConstants.S_OK;
        }

        public int OnBeforeUnloadProject(IVsHierarchy pRealHierarchy, IVsHierarchy pStubHierarchy)
        {
            return VSConstants.S_OK;
        }

        public int OnAfterOpenSolution(object pUnkReserved, int fNewSolution)
        {
            var dte = ObjectFactory.GetInstance<DTE>();

            WorkflowDiagramControl.OnAfterOpenSolution(new SolutionWrapper(dte.Solution));

            return VSConstants.S_OK;
        }

        public int OnQueryCloseSolution(object pUnkReserved, ref int pfCancel)
        {
            return VSConstants.S_OK;
        }

        public int OnBeforeCloseSolution(object pUnkReserved)
        {
            WorkflowDiagramControl.OnBeforeCloseSolution();

            return VSConstants.S_OK;
        }

        public int OnAfterCloseSolution(object pUnkReserved)
        {
            WorkflowDiagramControl.OnAfterCloseSolution();

            return VSConstants.S_OK;
        }
    }
}