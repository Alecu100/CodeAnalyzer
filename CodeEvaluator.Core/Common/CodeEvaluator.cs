//  Project              : GLP
//  Module               : RomSoft.Client.Debug.dll
//  File                 : CodeEvaluator.cs
//  Author               : Alecsandru
//  Last Updated         : 19/12/2015 at 1:20
//  
// 
//  Contains             : Implementation of the CodeEvaluator.cs class.
//  Classes              : CodeEvaluator.cs
// 
//  
//  ----------------------------------------------------------------------- 
//   <copyright file="CodeEvaluator.cs" company="Sysmex"> 
//       Copyright (c) Sysmex. All rights reserved. 
//   </copyright> 
//  -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using CodeAnalysis.Core.Interfaces;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using StructureMap;

namespace CodeAnalysis.Core.Common
{

    #region Using

    #endregion

    public class CodeEvaluator : ICodeEvaluator
    {
        #region Public Properties

        public CodeEvaluatorExecutionState ExecutionState { get; private set; }

        #endregion

        #region Public Methods and Operators

        public void Evaluate(
            List<ICodeEvaluatorListener> listeners,
            List<string> codeFileNames,
            ClassDeclarationSyntax targetClass,
            MethodDeclarationSyntax startMethod,
            List<string> assemblyFileNames = null)
        {
            InitializeContext(listeners);

            ResetSharedResources();

            ParseSourceFilesFromSelectedProjects(codeFileNames, assemblyFileNames);

            InitializeExecutionFrame(targetClass, startMethod);

            StartEvaluation(startMethod);
        }

        #endregion

        #region SpecificFields

        #endregion

        #region Private Methods and Operators

        private void InitializeContext(
            IList<ICodeEvaluatorListener> listeners)
        {
            ExecutionState = new CodeEvaluatorExecutionState();
            ExecutionState.Parameters = new CodeEvaluatorParameters();
            ExecutionState.Parameters.Listeners.AddRange(listeners);
        }

        private void InitializeExecutionFrame(ClassDeclarationSyntax targetClass, MethodDeclarationSyntax startMethod)
        {
            var wellKnownTypesCache = ObjectFactory.GetInstance<IEvaluatedTypesInfoTable>();
            var trackedTypeInfo = wellKnownTypesCache.GetTypeInfo(targetClass);
            var startMethodInfo =
                trackedTypeInfo.AccesibleMethods.First(
                    method =>
                        method.IdentifierText == startMethod.Identifier.ValueText &&
                        method.Parameters.Count == startMethod.ParameterList.Parameters.Count);
            var staticWorkflowEvaluatorExecutionFrameFactory =
                ObjectFactory.GetInstance<IEvaluatorExecutionFrameFactory>();
            var initialExecutionFrame =
                staticWorkflowEvaluatorExecutionFrameFactory.BuildInitialExecutionFrame(trackedTypeInfo, startMethodInfo);

            ExecutionState.PushFramePassingParametersFromPreviousFrame(initialExecutionFrame);
        }

        private void ParseSourceFilesFromSelectedProjects(IList<string> codeFileNames, List<string> assemblyNames)
        {
            var parsedSourceFilesCache = ObjectFactory.GetInstance<IParsedSourceFilesCache>();
            parsedSourceFilesCache.RebuildFromCodeFiles(codeFileNames);

            var evaluatedTypesInfoTable = ObjectFactory.GetInstance<IEvaluatedTypesInfoTable>();
            evaluatedTypesInfoTable.ClearTypeInfos();

            try
            {
                var assemblyTypesReader = ObjectFactory.GetInstance<IAssemblyTypesReader>();
                var evaluatedTypeInfos = assemblyTypesReader.ReadTypeInfos(assemblyNames);

                evaluatedTypesInfoTable.RebuildExternalTypeInfos(evaluatedTypeInfos);
            }
            catch (Exception)
            {
            }
            finally
            {
                evaluatedTypesInfoTable.RebuildWellKnownTypesWithMethods(parsedSourceFilesCache);
            }
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
                syntaxNodeEvaluator.EvaluateSyntaxNode(startMethod, ExecutionState);
            }
        }

        #endregion
    }
}