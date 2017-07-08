using System;
using System.Collections.Generic;

namespace CodeEvaluator.Workflows
{

    #region Using

    #endregion

    [Serializable]
    public class Workflow
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="Workflow" /> class.
        /// </summary>
        /// <param name="parentStep">The parent step.</param>
        public Workflow(WorkflowStep parentStep)
        {
            ParentStep = parentStep;
        }

        #endregion

        #region Fields

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets or sets the last step.
        /// </summary>
        /// <value>
        ///     The last step.
        /// </value>
        public WorkflowStep LastStep { get; set; }

        /// <summary>
        ///     Gets or sets the parent step.
        /// </summary>
        /// <value>
        ///     The parent step.
        /// </value>
        public WorkflowStep ParentStep { get; }

        /// <summary>
        ///     Gets all steps.
        /// </summary>
        /// <value>
        ///     All steps.
        /// </value>
        public List<WorkflowStep> Steps { get; } = new List<WorkflowStep>();

        #endregion
    }
}