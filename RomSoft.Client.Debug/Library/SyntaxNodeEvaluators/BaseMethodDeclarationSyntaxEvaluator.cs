//  Project              : GLP
//  Module               : RomSoft.Client.Debug.dll
//  File                 : BaseMethodDeclarationSyntaxEvaluator.cs
//  Author               : Alecsandru
//  Last Updated         : 19/12/2015 at 1:07
//  
// 
//  Contains             : Implementation of the BaseMethodDeclarationSyntaxEvaluator.cs class.
//  Classes              : BaseMethodDeclarationSyntaxEvaluator.cs
// 
//  
//  ----------------------------------------------------------------------- 
//   <copyright file="BaseMethodDeclarationSyntaxEvaluator.cs" company="Sysmex"> 
//       Copyright (c) Sysmex. All rights reserved. 
//   </copyright> 
//  -----------------------------------------------------------------------

namespace RomSoft.Client.Debug.Library.SyntaxNodeEvaluators
{
    #region Using

    using Microsoft.CodeAnalysis.CSharp.Syntax;

    using RomSoft.Client.Debug.Library.Common;
    using RomSoft.Client.Debug.Library.Interfaces;
    using RomSoft.Client.Debug.Library.Members;

    using StructureMap;

    #endregion

    public class BaseMethodDeclarationSyntaxEvaluator : BaseSyntaxNodeEvaluator
    {
        #region Fields

        protected BaseMethodDeclarationSyntax _baseMethodDeclarationSyntax;

        protected TrackedVariableReference _thisReference;

        protected TrackedMethodBase _trackedMethod;

        protected StaticWorkflowEvaluatorContext _workflowEvaluatorContext;

        #endregion

        #region Public Properties

        public ITrackedVariableAllocator VariableAllocator { get; set; }

        #endregion

        #region Protected Methods and Operators

        protected void InitializeExecutionFrame()
        {
            var staticWorkflowEvaluatorExecutionFrameFactory =
                ObjectFactory.GetInstance<IStaticWorkflowEvaluatorExecutionFrameFactory>();
            var buildNewExecutionFrameForMethodCall =
                staticWorkflowEvaluatorExecutionFrameFactory.BuildNewExecutionFrameForMethodCall(
                    _trackedMethod,
                    _thisReference);
            _workflowEvaluatorContext.PushExecutionFramePassingInputs(buildNewExecutionFrameForMethodCall);
        }

        protected void InitializeParameters()
        {
            for (var i = 0; i < _trackedMethod.Parameters.Count; i++)
            {
                if (_workflowEvaluatorContext.CurrentExecutionFrame.PassedMethodParameters.ContainsKey(i))
                {
                    var trackedMethodParameter = _trackedMethod.Parameters[i];
                    var passedParameters = _workflowEvaluatorContext.CurrentExecutionFrame.PassedMethodParameters[i];

                    if (trackedMethodParameter.TypeInfo == null || passedParameters == null)
                    {
                        continue;
                    }

                    var trackedVariableReference = new TrackedVariableReference();
                    trackedVariableReference.Declaration = trackedMethodParameter.Declaration;
                    trackedVariableReference.TypeInfo = trackedMethodParameter.TypeInfo;
                    trackedVariableReference.Identifier = trackedMethodParameter.Identifier;
                    trackedVariableReference.IdentifierText = trackedMethodParameter.IdentifierText;
                    trackedVariableReference = trackedVariableReference.AddVariables(passedParameters.Variables);
                    _workflowEvaluatorContext.CurrentExecutionFrame.LocalReferences.Add(trackedVariableReference);
                    passedParameters.Dispose();
                    _workflowEvaluatorContext.CurrentExecutionFrame.PassedMethodParameters.Remove(i);
                }
                else
                {
                    var trackedMethodParameter = _trackedMethod.Parameters[i];

                    if (trackedMethodParameter.TypeInfo == null)
                    {
                        continue;
                    }

                    var trackedVariableReference = new TrackedVariableReference();
                    trackedVariableReference.Declaration = trackedMethodParameter.Declaration;
                    trackedVariableReference.TypeInfo = trackedMethodParameter.TypeInfo;
                    trackedVariableReference.Identifier = trackedMethodParameter.Identifier;
                    trackedVariableReference.IdentifierText = trackedMethodParameter.IdentifierText;
                    trackedVariableReference =
                        trackedVariableReference.AddVariable(
                            VariableAllocator.AllocateVariable(trackedMethodParameter.TypeInfo));
                    _workflowEvaluatorContext.CurrentExecutionFrame.LocalReferences.Add(trackedVariableReference);
                }
            }
        }

        protected void ResetExecutionFrame()
        {
            _workflowEvaluatorContext.PopExecutionFramePassingReturningParameters();
        }

        #endregion
    }
}