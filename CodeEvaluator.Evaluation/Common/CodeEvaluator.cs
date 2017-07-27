namespace CodeEvaluator.Evaluation.Common
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using global::CodeEvaluator.Dto;
    using global::CodeEvaluator.Evaluation.Interfaces;

    using Microsoft.CodeAnalysis.CSharp.Syntax;

    using StructureMap;

    #region Using

    #endregion

    public class CodeEvaluator : ICodeEvaluator
    {
        #region Public Properties

        public CodeEvaluatorExecutionStack ExecutionStack { get; private set; }

        #endregion

        #region Public Methods and Operators

        public void Evaluate(
            List<ISyntaxNodeEvaluatorListener> listeners,
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
            IList<ISyntaxNodeEvaluatorListener> listeners)
        {
            ExecutionStack = new CodeEvaluatorExecutionStack();
            ExecutionStack.Parameters = new CodeEvaluatorParameters();
            ExecutionStack.Parameters.EvaluatorListeners.AddRange(listeners);
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

            ExecutionStack.PushFramePassingParametersFromPreviousFrame(initialExecutionFrame);
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
            var syntaxNodeEvaluator = syntaxNodeEvaluatorFactory.GetSyntaxNodeEvaluator(startMethod, EEvaluatorActions.None);

            if (syntaxNodeEvaluator != null)
            {
                syntaxNodeEvaluator.EvaluateSyntaxNode(startMethod, ExecutionStack);
            }
        }

        #endregion
    }
}