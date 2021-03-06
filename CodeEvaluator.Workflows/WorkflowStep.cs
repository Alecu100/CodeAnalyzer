﻿using System;
using System.Collections.Generic;
using CodeEvaluator.Workflows.Enums;

namespace CodeEvaluator.Workflows
{
    #region Using

    

    #endregion

    [Serializable]
    public class WorkflowStep
    {
        #region Fields

        private string _description;

        private string _label;

        private WorkflowStep _nextStep;

        private readonly List<Workflow> _activeChildWorkflows = new List<Workflow>();

        private readonly List<Workflow> _allChildWorkflows = new List<Workflow>();

        private readonly Workflow _parentWorkflow;

        private readonly EWorkflowStepType _type;

        public readonly string _id;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="WorkflowStep" /> class.
        /// </summary>
        /// <param name="type">The type.</param>
        public WorkflowStep(EWorkflowStepType type, string id)
        {
            _type = type;
            _id = id;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="WorkflowStep" /> class.
        /// </summary>
        /// <param name="type">The type.</param>
        public WorkflowStep(EWorkflowStepType type, string id, string label)
        {
            _type = type;
            _label = label;
            _id = id;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="WorkflowStep" /> class.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="parentWorkflow">The parent scope.</param>
        public WorkflowStep(EWorkflowStepType type, string id, Workflow parentWorkflow)
        {
            _id = id;
            _type = type;
            _parentWorkflow = parentWorkflow;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="WorkflowStep" /> class.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="parentWorkflow">The parent scope.</param>
        public WorkflowStep(EWorkflowStepType type, string id, Workflow parentWorkflow, string label)
        {
            _id = id;
            _type = type;
            _parentWorkflow = parentWorkflow;
            _label = label;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets the active scopes.
        /// </summary>
        /// <value>
        ///     The active scopes.
        /// </value>
        public List<Workflow> ActiveChildWorkflows
        {
            get
            {
                return _activeChildWorkflows;
            }
        }

        /// <summary>
        ///     Gets all scopes.
        /// </summary>
        /// <value>
        ///     All scopes.
        /// </value>
        public List<Workflow> AllChildWorkflows
        {
            get
            {
                return _allChildWorkflows;
            }
        }

        /// <summary>
        ///     Gets or sets the description.
        /// </summary>
        /// <value>
        ///     The description.
        /// </value>
        public string Description
        {
            get
            {
                return _description;
            }
            set
            {
                _description = value;
            }
        }

        /// <summary>
        ///     Gets or sets the name.
        /// </summary>
        /// <value>
        ///     The name.
        /// </value>
        public string Label
        {
            get
            {
                return _label;
            }
            set
            {
                _label = value;
            }
        }

        public WorkflowStep NextStep
        {
            get
            {
                return _nextStep;
            }
            set
            {
                _nextStep = value;
            }
        }

        /// <summary>
        ///     Gets the parent scope.
        /// </summary>
        /// <value>
        ///     The parent scope.
        /// </value>
        public Workflow ParentWorkflow
        {
            get
            {
                return _parentWorkflow;
            }
        }

        /// <summary>
        ///     Gets or sets the type.
        /// </summary>
        /// <value>
        ///     The type.
        /// </value>
        public EWorkflowStepType Type
        {
            get
            {
                return _type;
            }
        }

        public string Id
        {
            get { return _id; }
        }

        #endregion
    }
}