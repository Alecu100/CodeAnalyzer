//  Project              : GLP
//  Module               : RomSoft.Debug.dll
//  File                 : WorkflowEvaluator.cs
//  Author               : Alecsandru
//  Last Updated         : 11/02/2016 at 16:14
//  
// 
//  Contains             : Implementation of the WorkflowEvaluator.cs class.
//  Classes              : WorkflowEvaluator.cs
// 
//  
//  ----------------------------------------------------------------------- 
//   <copyright file="WorkflowEvaluator.cs" company="Sysmex"> 
//       Copyright (c) Sysmex. All rights reserved. 
//   </copyright> 
//  -----------------------------------------------------------------------


using CodeEvaluator.Workflows.Enums;

namespace CodeEvaluator.Workflows
{

    #region Using

    #endregion

    public static class WorkflowEvaluator
    {
        #region Public Properties

        public static WorkflowExecutionSnapshot CurrentExecutionSnapshot { get; private set; }

        #endregion

        #region Static Fields

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Adds the decision.
        /// </summary>
        /// <param name="name">The name.</param>
        public static void AddDecision(string name, string description)
        {
            var workflowStep = new WorkflowStep(EWorkflowStepType.Decision, CurrentExecutionSnapshot.ActiveWorkflow);
            workflowStep.Label = name;
            workflowStep.Description = description;
            CurrentExecutionSnapshot.ActiveWorkflow.Steps.Add(workflowStep);
            if (CurrentExecutionSnapshot.ActiveWorkflow.LastStep != null)
            {
                CurrentExecutionSnapshot.ActiveWorkflow.LastStep.NextStep = workflowStep;
            }
            CurrentExecutionSnapshot.ActiveWorkflow.LastStep = workflowStep;
        }

        /// <summary>
        ///     Adds the process.
        /// </summary>
        /// <param name="name">The name.</param>
        public static void AddProcess(string name, string description)
        {
            var workflowStep = new WorkflowStep(EWorkflowStepType.Process, CurrentExecutionSnapshot.ActiveWorkflow);
            workflowStep.Label = name;
            workflowStep.Description = description;
            CurrentExecutionSnapshot.ActiveWorkflow.Steps.Add(workflowStep);
            if (CurrentExecutionSnapshot.ActiveWorkflow.LastStep != null)
            {
                CurrentExecutionSnapshot.ActiveWorkflow.LastStep.NextStep = workflowStep;
            }
            CurrentExecutionSnapshot.ActiveWorkflow.LastStep = workflowStep;
        }

        /// <summary>
        ///     Starts the scope.
        /// </summary>
        public static void BeginWorkflow()
        {
            if (CurrentExecutionSnapshot.ActiveWorkflow.LastStep != null)
            {
                var workflowScope = new Workflow(CurrentExecutionSnapshot.ActiveWorkflow.LastStep);
                CurrentExecutionSnapshot.ActiveWorkflow.LastStep.ActiveChildWorkflows.Add(workflowScope);
                CurrentExecutionSnapshot.ActiveWorkflow.LastStep.AllChildWorkflows.Add(workflowScope);
                CurrentExecutionSnapshot.ActiveWorkflow = workflowScope;
            }
            else
            {
                var workflowScope = new Workflow(CurrentExecutionSnapshot.StartStep);
                CurrentExecutionSnapshot.StartStep.ActiveChildWorkflows.Add(workflowScope);
                CurrentExecutionSnapshot.StartStep.AllChildWorkflows.Add(workflowScope);
                CurrentExecutionSnapshot.ActiveWorkflow = workflowScope;
            }
        }

        /// <summary>
        ///     Ends the scope.
        /// </summary>
        public static void EndWorkflow()
        {
            CurrentExecutionSnapshot.ActiveWorkflow = CurrentExecutionSnapshot.ActiveWorkflow.ParentStep.ParentWorkflow;
        }

        /// <summary>
        ///     Initializes this instance.
        /// </summary>
        public static void Initialize()
        {
            CurrentExecutionSnapshot = new WorkflowExecutionSnapshot();
        }

        /// <summary>
        ///     Stops the scope connecting it to the stop step
        /// </summary>
        public static void StopWorkflow()
        {
            var stopStep = new WorkflowStep(EWorkflowStepType.Stop, "Stop");
            CurrentExecutionSnapshot.ActiveWorkflow.ParentStep.ActiveChildWorkflows.Remove(
                CurrentExecutionSnapshot.ActiveWorkflow);
            CurrentExecutionSnapshot.ActiveWorkflow.LastStep.NextStep = stopStep;
            CurrentExecutionSnapshot.ActiveWorkflow.Steps.Add(stopStep);
            CurrentExecutionSnapshot.ActiveWorkflow.LastStep = stopStep;
            CurrentExecutionSnapshot.ActiveWorkflow = CurrentExecutionSnapshot.ActiveWorkflow.ParentStep.ParentWorkflow;
        }

        #endregion
    }
}