//  Project              : GLP
//  Module               : RomSoft.Client.Debug.dll
//  File                 : StaticEvaluator.cs
//  Author               : Alecsandru
//  Last Updated         : 19/12/2015 at 1:20
//  
// 
//  Contains             : Implementation of the StaticEvaluator.cs class.
//  Classes              : StaticEvaluator.cs
// 
//  
//  ----------------------------------------------------------------------- 
//   <copyright file="StaticEvaluator.cs" company="Sysmex"> 
//       Copyright (c) Sysmex. All rights reserved. 
//   </copyright> 
//  -----------------------------------------------------------------------

using System.Collections.Generic;
using CodeAnalysis.Core.Interfaces;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using StructureMap;

namespace CodeAnalysis.Core.Common
{

    #region Using

    #endregion

    public class StaticEvaluator : IStaticEvaluator
    {
        #region Fields

        #endregion

        #region Public Properties

        public StaticWorkflowEvaluatorContext Context { get; private set; }

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
            Context = new StaticWorkflowEvaluatorContext();
            Context.Parameters = new StaticWorkflowEvaluatorParameters();
            Context.Parameters.SelectedProjects.AddRange(selectedProjets);
            Context.Parameters.Listeners.AddRange(listeners);

            var projectFilesProvider = ObjectFactory.GetInstance<IProjectFilesProvider>();
            var allReferencesFromProjects = projectFilesProvider.GetAllReferencesFromProjects(selectedProjets);

            Context.Parameters.ProjectsReferences.AddRange(allReferencesFromProjects);
        }

        private void InitializeExecutionFrame(ClassDeclarationSyntax targetClass)
        {
            var wellKnownTypesCache = ObjectFactory.GetInstance<IEvaluatedTypesInfoTable>();
            var trackedTypeInfo = wellKnownTypesCache.GetTypeInfo(targetClass);
            var staticWorkflowEvaluatorExecutionFrameFactory =
                ObjectFactory.GetInstance<IEvaluatorExecutionFrameFactory>();
            var initialExecutionFrame =
                staticWorkflowEvaluatorExecutionFrameFactory.BuildInitialExecutionFrame(trackedTypeInfo);

            Context.PushFramePassingParametersFromPreviousFrame(initialExecutionFrame);
        }

        private void ParseSourceFilesFromSelectedProjects(IList<Project> selectedProjets)
        {
            var parsedSourceFilesCache = ObjectFactory.GetInstance<IParsedSourceFilesCache>();
            parsedSourceFilesCache.RebuildFromProjects(selectedProjets);

            var wellKnownTypesCache = ObjectFactory.GetInstance<IEvaluatedTypesInfoTable>();
            wellKnownTypesCache.RebuildWellKnownTypesWithMethods(parsedSourceFilesCache);
        }

        private void ResetSharedResources()
        {
            var trackedVariablesHeap = ObjectFactory.GetInstance<IEvaluatedObjectsHeap>();
            trackedVariablesHeap.Clear();
        }

        private void StartEvaluation(MethodDeclarationSyntax startMethod)
        {
            var syntaxNodeEvaluatorFactory = ObjectFactory.GetInstance<ISyntaxNodeEvaluatorFactory>();
            var syntaxNodeEvaluator = syntaxNodeEvaluatorFactory.GetSyntaxNodeEvaluator(startMethod);

            if (syntaxNodeEvaluator != null)
            {
                syntaxNodeEvaluator.EvaluateSyntaxNode(startMethod, Context);
            }
        }

        #endregion
    }
}