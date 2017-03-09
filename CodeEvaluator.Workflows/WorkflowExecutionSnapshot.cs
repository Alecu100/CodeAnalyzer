//  Project              : GLP
//  Module               : RomSoft.Debug.dll
//  File                 : WorkflowExecutionSnapshot.cs
//  Author               : Alecsandru
//  Last Updated         : 11/02/2016 at 16:11
//  
// 
//  Contains             : Implementation of the WorkflowExecutionSnapshot.cs class.
//  Classes              : WorkflowExecutionSnapshot.cs
// 
//  
//  ----------------------------------------------------------------------- 
//   <copyright file="WorkflowExecutionSnapshot.cs" company="Sysmex"> 
//       Copyright (c) Sysmex. All rights reserved. 
//   </copyright> 
//  -----------------------------------------------------------------------

using System;
using CodeEvaluator.Workflows.Enums;

namespace CodeEvaluator.Workflows
{

    #region Using

    #endregion

    [Serializable]
    public class WorkflowExecutionSnapshot
    {
        #region Constructors and Destructors

        public WorkflowExecutionSnapshot()
        {
            _startStep = new WorkflowStep(EWorkflowStepType.Start, "Start");
            _activeWorkflow = new Workflow(StartStep);

            _startStep.ActiveChildWorkflows.Add(_activeWorkflow);
            _startStep.AllChildWorkflows.Add(_activeWorkflow);
        }

        #endregion

        #region Fields

        private Workflow _activeWorkflow;

        private WorkflowStep _startStep;

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets or sets the activ scope.
        /// </summary>
        /// <value>
        ///     The activ scope.
        /// </value>
        public Workflow ActiveWorkflow
        {
            get { return _activeWorkflow; }
            set { _activeWorkflow = value; }
        }

        /// <summary>
        ///     Gets the start step.
        /// </summary>
        /// <value>
        ///     The start step.
        /// </value>
        public WorkflowStep StartStep
        {
            get { return _startStep; }
            set { _startStep = value; }
        }

        #endregion
    }
}