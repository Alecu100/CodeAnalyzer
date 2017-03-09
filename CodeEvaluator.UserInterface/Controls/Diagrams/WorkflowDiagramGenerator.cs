//  Project              : GLP
//  Module               : RomSoft.Client.Debug.dll
//  File                 : WorkflowDiagramGenerator.cs
//  Author               : Alecsandru
//  Last Updated         : 11/02/2016 at 14:28
//  
// 
//  Contains             : Implementation of the WorkflowDiagramGenerator.cs class.
//  Classes              : WorkflowDiagramGenerator.cs
// 
//  
//  ----------------------------------------------------------------------- 
//   <copyright file="WorkflowDiagramGenerator.cs" company="Sysmex"> 
//       Copyright (c) Sysmex. All rights reserved. 
//   </copyright> 
//  -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using CodeAnalyzer.UserInterface.Controls.Base;
using CodeAnalyzer.UserInterface.Controls.Base.Enums;
using CodeAnalyzer.UserInterface.Interfaces;
using CodeEvaluator.Workflows;
using CodeEvaluator.Workflows.Enums;
using StructureMap;

namespace CodeAnalyzer.UserInterface.Controls.Diagrams
{

    #region Using

    #endregion

    public class WorkflowDiagramGenerator : IWorkflowDiagramGenerator
    {
        #region Constructors and Destructors

        public WorkflowDiagramGenerator()
        {
            ObjectFactory.BuildUp(this);
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets or sets the workflow diagram sizes.
        /// </summary>
        /// <value>
        ///     The workflow diagram sizes.
        /// </value>
        public IWorkflowDiagramSizes WorkflowDiagramSizes { get; set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Generates the workflow diagram.
        /// </summary>
        /// <param name="snapshot">The snapshot.</param>
        /// <param name="canvas">The canvas.</param>
        public void GenerateWorkflowDiagram(WorkflowExecutionSnapshot snapshot, WorkflowCanvas canvas)
        {
            TraverseWorkflowGraph(snapshot);

            PopulateWorkflowItems(canvas);

            PopulateWorkflowItemConnections(canvas);
        }

        #endregion

        #region Fields

        private Stack<Tuple<WorkflowStep, WorkflowStep, Workflow>> _itemsToBeProcessed;

        private readonly Dictionary<WorkflowStep, List<WorkflowStep>> _children =
            new Dictionary<WorkflowStep, List<WorkflowStep>>();

        private readonly Dictionary<WorkflowItem, WorkflowStep> _createdItemsForSteps =
            new Dictionary<WorkflowItem, WorkflowStep>();

        private readonly Dictionary<int, List<WorkflowStep>> _levels = new Dictionary<int, List<WorkflowStep>>();

        private readonly Dictionary<WorkflowStep, int> _levelsForCreatedItems = new Dictionary<WorkflowStep, int>();

        private readonly Dictionary<WorkflowStep, List<WorkflowStep>> _parents =
            new Dictionary<WorkflowStep, List<WorkflowStep>>();

        private readonly Dictionary<WorkflowStep, WorkflowItem> _stepsForCreatedItems =
            new Dictionary<WorkflowStep, WorkflowItem>();

        #endregion

        #region Private Methods and Operators

        private void AddCreatedItem(WorkflowStep step, WorkflowItem createdItem)
        {
            _createdItemsForSteps[createdItem] = step;
            _stepsForCreatedItems[step] = createdItem;
        }

        private void AddItemOnLevel(int level, WorkflowStep step)
        {
            if (!_levels.ContainsKey(level))
            {
                _levels[level] = new List<WorkflowStep>();
            }

            _levelsForCreatedItems[step] = level;
            _levels[level].Add(step);
        }

        private void AddParentChild(WorkflowStep child, WorkflowStep parent)
        {
            if (!_parents.ContainsKey(child))
            {
                _parents[child] = new List<WorkflowStep>();
            }

            _parents[child].Add(parent);

            if (!_children.ContainsKey(parent))
            {
                _children[parent] = new List<WorkflowStep>();
            }

            _children[parent].Add(child);
        }

        private int ConnectCurrentStep(
            WorkflowStep currentStep,
            WorkflowStep previousStep,
            bool connectWithChildWorkflowsFromParent)
        {
            var level = 1;

            if (previousStep != null)
            {
                if (previousStep.ActiveChildWorkflows.Any() && connectWithChildWorkflowsFromParent
                    && previousStep.AllChildWorkflows.All(workflow => workflow != currentStep.ParentWorkflow))
                {
                    foreach (var activeChildWorkflow in previousStep.ActiveChildWorkflows)
                    {
                        AddParentChild(currentStep, activeChildWorkflow.LastStep);

                        if (_levelsForCreatedItems[activeChildWorkflow.LastStep] + 1 > level)
                        {
                            level = _levelsForCreatedItems[activeChildWorkflow.LastStep] + 1;
                        }
                    }
                }
                else
                {
                    AddParentChild(currentStep, previousStep);

                    level = _levelsForCreatedItems[previousStep] + 1;
                }
            }
            return level;
        }

        private void InitializeItemsToBeProcessed(WorkflowExecutionSnapshot snapshot)
        {
            _itemsToBeProcessed = new Stack<Tuple<WorkflowStep, WorkflowStep, Workflow>>();

            if (snapshot.StartStep != null)
            {
                _itemsToBeProcessed.Push(
                    new Tuple<WorkflowStep, WorkflowStep, Workflow>(null, snapshot.StartStep, null));
            }
        }

        private bool ItemsAreAvailableToProcess()
        {
            return _itemsToBeProcessed.Any();
        }

        private void PopulateWorkflowItemConnections(WorkflowCanvas canvas)
        {
            foreach (var row in _levels)
            {
                foreach (var workflowStep in row.Value)
                {
                    if (!_children.ContainsKey(workflowStep))
                    {
                        continue;
                    }

                    foreach (var workflowStepChild in _children[workflowStep])
                    {
                        var workflowItem = _stepsForCreatedItems[workflowStep];
                        var workflowItemChild = _stepsForCreatedItems[workflowStepChild];
                        var connection = new WorkflowConnection(
                            workflowItem.WorkflowConnectorBottom,
                            workflowItemChild.WorkflowConnectorTop);
                        canvas.Children.Insert(0, connection);
                    }
                }
            }
        }

        private void PopulateWorkflowItems(WorkflowCanvas canvas)
        {
            canvas.Children.Clear();

            var maxItemsPerLevel = 0;

            foreach (var level in _levels)
            {
                if (level.Value.Count > maxItemsPerLevel)
                {
                    maxItemsPerLevel = level.Value.Count;
                }
            }

            var maxLevelWidth = (maxItemsPerLevel - 1)*WorkflowDiagramSizes.MinimumColumnSpacing
                                + maxItemsPerLevel*WorkflowDiagramSizes.ColumnWidth
                                + 2*WorkflowDiagramSizes.MinimumMargin;

            foreach (var row in _levels)
            {
                var topDistance = (row.Key - 1)*(WorkflowDiagramSizes.RowSpacing + WorkflowDiagramSizes.RowHeight)
                                  + WorkflowDiagramSizes.MinimumMargin;

                for (var column = 0; column < row.Value.Count; column++)
                {
                    var currentStep = row.Value[column];
                    var currentDiagramItem = _stepsForCreatedItems[currentStep];

                    var leftDistance = maxLevelWidth/(row.Value.Count + 1)*(column + 1);

                    Canvas.SetTop(currentDiagramItem, topDistance);
                    Canvas.SetLeft(currentDiagramItem, leftDistance);

                    canvas.Children.Add(currentDiagramItem);
                }
            }
        }

        private void QueueNextStepsToBeProcessed(WorkflowStep currentStep)
        {
            if (currentStep.AllChildWorkflows.Any())
            {
                foreach (var childWorkflow in currentStep.AllChildWorkflows)
                {
                    _itemsToBeProcessed.Push(
                        new Tuple<WorkflowStep, WorkflowStep, Workflow>(currentStep, null, childWorkflow));
                }
            }

            if (currentStep.NextStep != null)
            {
                _itemsToBeProcessed.Push(
                    new Tuple<WorkflowStep, WorkflowStep, Workflow>(currentStep, currentStep.NextStep, null));
            }
        }

        private void TraverseWorkflowGraph(WorkflowExecutionSnapshot snapshot)
        {
            InitializeItemsToBeProcessed(snapshot);

            while (ItemsAreAvailableToProcess())
            {
                var currentQueuedItem = _itemsToBeProcessed.Pop();

                if (currentQueuedItem.Item2 != null)
                {
                    var workflowItemType = GetWorkflowItemType(currentQueuedItem.Item2.Type);

                    var workflowItem = new WorkflowItem(workflowItemType);
                    workflowItem.Label = currentQueuedItem.Item2.Label;

                    AddCreatedItem(currentQueuedItem.Item2, workflowItem);

                    var level = ConnectCurrentStep(currentQueuedItem.Item2, currentQueuedItem.Item1, true);

                    AddItemOnLevel(level, currentQueuedItem.Item2);

                    QueueNextStepsToBeProcessed(currentQueuedItem.Item2);
                }
                else if (currentQueuedItem.Item3 != null && currentQueuedItem.Item3.Steps.Any())
                {
                    var firstStepWorkflow = currentQueuedItem.Item3.Steps[0];
                    var workflowItemType = GetWorkflowItemType(firstStepWorkflow.Type);

                    var workflowItem = new WorkflowItem(workflowItemType);
                    workflowItem.Label = firstStepWorkflow.Label;

                    AddCreatedItem(firstStepWorkflow, workflowItem);

                    var level = ConnectCurrentStep(firstStepWorkflow, currentQueuedItem.Item1, false);

                    AddItemOnLevel(level, firstStepWorkflow);

                    QueueNextStepsToBeProcessed(firstStepWorkflow);
                }
            }
        }

        private static EWorkflowItemType GetWorkflowItemType(EWorkflowStepType stepType)
        {
            EWorkflowItemType workflowItemType;

            switch (stepType)
            {
                case EWorkflowStepType.Decision:
                    workflowItemType = EWorkflowItemType.Decission;
                    break;
                case EWorkflowStepType.Process:
                    workflowItemType = EWorkflowItemType.Process;
                    break;
                case EWorkflowStepType.Start:
                    workflowItemType = EWorkflowItemType.Start;
                    break;
                case EWorkflowStepType.Stop:
                    workflowItemType = EWorkflowItemType.Stop;
                    break;
                default:
                    workflowItemType = EWorkflowItemType.None;
                    break;
            }
            return workflowItemType;
        }

        #endregion
    }
}