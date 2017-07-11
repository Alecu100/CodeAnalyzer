using CodeEvaluator.UserInterface.Controls.Base;
using CodeEvaluator.Workflows;

namespace CodeEvaluator.UserInterface.Interfaces
{

    #region Using

    #endregion

    public interface IWorkflowDiagramGenerator
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Generates the workflow diagram.
        /// </summary>
        /// <param name="snapshot">The snapshot.</param>
        /// <param name="canvas">The canvas.</param>
        void GenerateWorkflowDiagram(WorkflowExecutionSnapshot snapshot, WorkflowCanvas canvas);

        #endregion
    }
}