//  Project              : GLP
//  Module               : RomSoft.Client.Debug.dll
//  File                 : StaticWorkflowEvaluator.cs
//  Author               : Alecsandru
//  Last Updated         : 19/12/2015 at 1:20
//  
// 
//  Contains             : Implementation of the StaticWorkflowEvaluator.cs class.
//  Classes              : StaticWorkflowEvaluator.cs
// 
//  
//  ----------------------------------------------------------------------- 
//   <copyright file="StaticWorkflowEvaluator.cs" company="Sysmex"> 
//       Copyright (c) Sysmex. All rights reserved. 
//   </copyright> 
//  -----------------------------------------------------------------------

namespace RomSoft.Client.Debug.Library.Common
{
    #region Using

    using System.Collections.Generic;

    using EnvDTE;

    using Microsoft.CodeAnalysis.CSharp.Syntax;

    using RomSoft.Client.Debug.Library.Interfaces;

    using StructureMap;

    #endregion

    public class StaticWorkflowEvaluator : IStaticWorkflowEvaluator
    {
        #region Fields

        private StaticWorkflowEvaluatorContext _context;

        #endregion

        #region Public Properties

        public StaticWorkflowEvaluatorContext Context
        {
            get
            {
                return _context;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Evaluates the workflow.
        /// </summary>
        /// <param name="listeners">The listeners.</param>
        /// <param name="selectedProjets">The selected projets.</param>
        /// <param name="targetClass">The target class.</param>
        /// <param name="startMethod">The start method.</param>
        public void EvaluateWorkflow(
            IList<IStaticWorkflowEvaluatorListener> listeners,
            IList<Project> selectedProjets,
            ClassDeclarationSyntax targetClass,
            MethodDeclarationSyntax startMethod)
        {
            InitializeContext(listeners, selectedProjets);

            ResetSharedResources();

            ParseSourceFilesFromSelectedProjects(selectedProjets);

            InitializeExecutionFrame(targetClass);

            StartEvaluation(startMethod);
        }

        #endregion

        #region Private Methods and Operators

        private void InitializeContext(
            IList<IStaticWorkflowEvaluatorListener> listeners,
            IList<Project> selectedProjets)
        {
            _context = new StaticWorkflowEvaluatorContext();
            _context.Parameters = new StaticWorkflowEvaluatorParameters();
            _context.Parameters.SelectedProjects.AddRange(selectedProjets);
            _context.Parameters.Listeners.AddRange(listeners);

            var projectFilesProvider = ObjectFactory.GetInstance<IProjectFilesProvider>();
            var allReferencesFromProjects = projectFilesProvider.GetAllReferencesFromProjects(selectedProjets);

            _context.Parameters.ProjectsReferences.AddRange(allReferencesFromProjects);
        }

        private void InitializeExecutionFrame(ClassDeclarationSyntax targetClass)
        {
            var wellKnownTypesCache = ObjectFactory.GetInstance<ITrackedVariableTypeInfosCache>();
            var trackedTypeInfo = wellKnownTypesCache.GetTypeInfo(targetClass);
            var staticWorkflowEvaluatorExecutionFrameFactory =
                ObjectFactory.GetInstance<IStaticWorkflowEvaluatorExecutionFrameFactory>();
            var initialExecutionFrame =
                staticWorkflowEvaluatorExecutionFrameFactory.BuildInitialExecutionFrame(trackedTypeInfo);

            _context.PushExecutionFramePassingInputs(initialExecutionFrame);
        }

        private void ParseSourceFilesFromSelectedProjects(IList<Project> selectedProjets)
        {
            var parsedSourceFilesCache = ObjectFactory.GetInstance<IParsedSourceFilesCache>();
            parsedSourceFilesCache.RebuildFromProjects(selectedProjets);

            var wellKnownTypesCache = ObjectFactory.GetInstance<ITrackedVariableTypeInfosCache>();
            wellKnownTypesCache.RebuildWellKnownTypesWithMethods(parsedSourceFilesCache);
        }

        private void ResetSharedResources()
        {
            var trackedVariablesHeap = ObjectFactory.GetInstance<ITrackedVariablesHeap>();
            trackedVariablesHeap.Clear();
        }

        private void StartEvaluation(MethodDeclarationSyntax startMethod)
        {
            var syntaxNodeEvaluatorFactory = ObjectFactory.GetInstance<ISyntaxNodeEvaluatorFactory>();
            var syntaxNodeEvaluator = syntaxNodeEvaluatorFactory.GetSyntaxNodeEvaluator(startMethod);

            if (syntaxNodeEvaluator != null)
            {
                syntaxNodeEvaluator.EvaluateSyntaxNode(startMethod, _context);
            }
        }

        #endregion
    }
}