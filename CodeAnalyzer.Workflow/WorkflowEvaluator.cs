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

using CodeAnalyzer.Workflow.Enums;

namespace CodeAnalyzer.Workflow
{

    #region Using

    #endregion

    public static class WorkflowEvaluator
    {
        #region Static Fields

        private static WorkflowExecutionSnapshot currentExecutionSnapshot;

        #endregion

        #region Public Properties

        public static WorkflowExecutionSnapshot CurrentExecutionSnapshot
        {
            get
            {
                return currentExecutionSnapshot;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Adds the decision.
        /// </summary>
        /// <param name="name">The name.</param>
        public static void AddDecision(string name, string description)
        {
            var workflowStep = new WorkflowStep(EWorkflowStepType.Decision, currentExecutionSnapshot.ActiveWorkflow);
            workflowStep.Label = name;
            workflowStep.Description = description;
            currentExecutionSnapshot.ActiveWorkflow.Steps.Add(workflowStep);
            if (currentExecutionSnapshot.ActiveWorkflow.LastStep != null)
            {
                currentExecutionSnapshot.ActiveWorkflow.LastStep.NextStep = workflowStep;
            }
            currentExecutionSnapshot.ActiveWorkflow.LastStep = workflowStep;
        }

        /// <summary>
        ///     Adds the process.
        /// </summary>
        /// <param name="name">The name.</param>
        public static void AddProcess(string name, string description)
        {
            var workflowStep = new WorkflowStep(EWorkflowStepType.Process, currentExecutionSnapshot.ActiveWorkflow);
            workflowStep.Label = name;
            workflowStep.Description = description;
            currentExecutionSnapshot.ActiveWorkflow.Steps.Add(workflowStep);
            if (currentExecutionSnapshot.ActiveWorkflow.LastStep != null)
            {
                currentExecutionSnapshot.ActiveWorkflow.LastStep.NextStep = workflowStep;
            }
            currentExecutionSnapshot.ActiveWorkflow.LastStep = workflowStep;
        }

        /// <summary>
        ///     Starts the scope.
        /// </summary>
        public static void BeginWorkflow()
        {
            var workflowScope = new Workflow(currentExecutionSnapshot.ActiveWorkflow.LastStep);
            currentExecutionSnapshot.ActiveWorkflow.LastStep.ActiveChildWorkflows.Add(workflowScope);
            currentExecutionSnapshot.ActiveWorkflow.LastStep.AllChildWorkflows.Add(workflowScope);
            currentExecutionSnapshot.ActiveWorkflow = workflowScope;
        }

        /// <summary>
        ///     Ends the scope.
        /// </summary>
        public static void EndWorkflow()
        {
            currentExecutionSnapshot.ActiveWorkflow = currentExecutionSnapshot.ActiveWorkflow.ParentStep.ParentWorkflow;
        }

        /// <summary>
        ///     Initializes this instance.
        /// </summary>
        public static void Initialize()
        {
            currentExecutionSnapshot = new WorkflowExecutionSnapshot();
        }

        /// <summary>
        ///     Stops the scope connecting it to the stop step
        /// </summary>
        public static void StopWorkflow()
        {
            var stopStep = new WorkflowStep(EWorkflowStepType.Stop, "Stop");
            currentExecutionSnapshot.ActiveWorkflow.ParentStep.ActiveChildWorkflows.Remove(
                currentExecutionSnapshot.ActiveWorkflow);
            currentExecutionSnapshot.ActiveWorkflow.LastStep.NextStep = stopStep;
            currentExecutionSnapshot.ActiveWorkflow.Steps.Add(stopStep);
            currentExecutionSnapshot.ActiveWorkflow.LastStep = stopStep;
            currentExecutionSnapshot.ActiveWorkflow = currentExecutionSnapshot.ActiveWorkflow.ParentStep.ParentWorkflow;
        }

        #endregion
    }
}