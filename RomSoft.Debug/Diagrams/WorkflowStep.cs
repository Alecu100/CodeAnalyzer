//  Project              : GLP
//  Module               : RomSoft.Debug.dll
//  File                 : WorkflowStep.cs
//  Author               : Alecsandru
//  Last Updated         : 10/02/2016 at 15:53
//  
// 
//  Contains             : Implementation of the WorkflowStep.cs class.
//  Classes              : WorkflowStep.cs
// 
//  
//  ----------------------------------------------------------------------- 
//   <copyright file="WorkflowStep.cs" company="Sysmex"> 
//       Copyright (c) Sysmex. All rights reserved. 
//   </copyright> 
//  -----------------------------------------------------------------------

namespace RomSoft.Debug.Diagrams
{
    #region Using

    using System;
    using System.Collections.Generic;

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

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="WorkflowStep" /> class.
        /// </summary>
        /// <param name="type">The type.</param>
        public WorkflowStep(EWorkflowStepType type)
        {
            _type = type;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="WorkflowStep" /> class.
        /// </summary>
        /// <param name="type">The type.</param>
        public WorkflowStep(EWorkflowStepType type, string label)
        {
            _type = type;
            _label = label;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="WorkflowStep" /> class.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="parentWorkflow">The parent scope.</param>
        public WorkflowStep(EWorkflowStepType type, Workflow parentWorkflow)
        {
            _type = type;
            _parentWorkflow = parentWorkflow;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="WorkflowStep" /> class.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="parentWorkflow">The parent scope.</param>
        public WorkflowStep(EWorkflowStepType type, Workflow parentWorkflow, string label)
        {
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

        #endregion
    }
}