//  Project              : GLP
//  Module               : Sysmex.GLP.Debug.dll
//  File                 : WorkflowScope.cs
//  Author               : Alecsandru
//  Last Updated         : 05/11/2015 at 17:27
//  
// 
//  Contains             : Implementation of the WorkflowScope.cs class.
//  Classes              : WorkflowScope.cs
// 
//  
//  ----------------------------------------------------------------------- 
//   <copyright file="WorkflowScope.cs" company="Sysmex"> 
//       Copyright (c) Sysmex. All rights reserved. 
//   </copyright> 
//  -----------------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace CodeAnalyzer.Workflow
{
    #region Using

    

    #endregion

    [Serializable]
    public class Workflow
    {
        #region Fields

        private readonly WorkflowStep _parentStep;

        private readonly List<WorkflowStep> _steps = new List<WorkflowStep>();

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="Workflow" /> class.
        /// </summary>
        /// <param name="parentStep">The parent step.</param>
        public Workflow(WorkflowStep parentStep)
        {
            _parentStep = parentStep;
        }

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
        public WorkflowStep ParentStep
        {
            get
            {
                return _parentStep;
            }
        }

        /// <summary>
        ///     Gets all steps.
        /// </summary>
        /// <value>
        ///     All steps.
        /// </value>
        public List<WorkflowStep> Steps
        {
            get
            {
                return _steps;
            }
        }

        #endregion
    }
}