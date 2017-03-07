//  Project              : GLP
//  Module               : RomSoft.Client.Debug.dll
//  File                 : StaticWorkflowEvaluatorParameters.cs
//  Author               : Alecsandru
//  Last Updated         : 19/12/2015 at 1:11
//  
// 
//  Contains             : Implementation of the StaticWorkflowEvaluatorParameters.cs class.
//  Classes              : StaticWorkflowEvaluatorParameters.cs
// 
//  
//  ----------------------------------------------------------------------- 
//   <copyright file="StaticWorkflowEvaluatorParameters.cs" company="Sysmex"> 
//       Copyright (c) Sysmex. All rights reserved. 
//   </copyright> 
//  -----------------------------------------------------------------------

namespace RomSoft.Client.Debug.Library.Common
{
    #region Using

    using System.Collections.Generic;

    using EnvDTE;

    using RomSoft.Client.Debug.Library.Interfaces;

    using VSLangProj;

    #endregion

    public class StaticWorkflowEvaluatorParameters
    {
        #region Fields

        private readonly List<IStaticWorkflowEvaluatorListener> _listeners =
            new List<IStaticWorkflowEvaluatorListener>();

        private readonly List<Reference> _projectsReferences = new List<Reference>();

        private readonly List<Project> _selectedProjects = new List<Project>();

        private readonly List<Project> _selectedProjets = new List<Project>();

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets the listeners.
        /// </summary>
        /// <value>
        ///     The listeners.
        /// </value>
        public List<IStaticWorkflowEvaluatorListener> Listeners
        {
            get
            {
                return _listeners;
            }
        }

        /// <summary>
        ///     Gets or sets the projects references.
        /// </summary>
        /// <value>
        ///     The projects references.
        /// </value>
        public List<Reference> ProjectsReferences
        {
            get
            {
                return _projectsReferences;
            }
        }

        /// <summary>
        ///     Gets or sets the selected projects.
        /// </summary>
        /// <value>
        ///     The selected projects.
        /// </value>
        public List<Project> SelectedProjects
        {
            get
            {
                return _selectedProjects;
            }
        }

        /// <summary>
        ///     Gets the selected projets.
        /// </summary>
        /// <value>
        ///     The selected projets.
        /// </value>
        public List<Project> SelectedProjets
        {
            get
            {
                return _selectedProjets;
            }
        }

        #endregion
    }
}