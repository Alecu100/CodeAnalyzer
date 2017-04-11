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

using CodeAnalysis.Core.Common;
using CodeAnalysis.Core.Interfaces;
using CodeAnalysis.Core.Members;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using StructureMap;

namespace CodeAnalysis.Core.SyntaxNodeEvaluators
{

    #region Using

    #endregion

    public class BaseMethodDeclarationSyntaxEvaluator : BaseSyntaxNodeEvaluator
    {
        #region Public Properties

        public IEvaluatedObjectAllocator VariableAllocator { get; set; }

        #endregion

        #region Fields

        protected BaseMethodDeclarationSyntax _baseMethodDeclarationSyntax;

        protected EvaluatedObjectReference _thisReference;

        protected EvaluatedMethodBase _evaluatedMethod;

        protected CodeEvaluatorExecutionState _workflowEvaluatorExecutionState;

        #endregion

        #region Protected Methods and Operators

        protected void InitializeExecutionFrame()
        {
            var staticWorkflowEvaluatorExecutionFrameFactory =
                ObjectFactory.GetInstance<IEvaluatorExecutionFrameFactory>();
            var buildNewExecutionFrameForMethodCall =
                staticWorkflowEvaluatorExecutionFrameFactory.BuildNewExecutionFrameForMethodCall(
                    _evaluatedMethod,
                    _thisReference);
            _workflowEvaluatorExecutionState.PushFramePassingParametersFromPreviousFrame(buildNewExecutionFrameForMethodCall);
        }

        protected void InitializeParameters()
        {
            for (var i = 0; i < _evaluatedMethod.Parameters.Count; i++)
            {
                if (_workflowEvaluatorExecutionState.CurrentExecutionFrame.PassedMethodParameters.ContainsKey(i))
                {
                    var trackedMethodParameter = _evaluatedMethod.Parameters[i];
                    var passedParameters = _workflowEvaluatorExecutionState.CurrentExecutionFrame.PassedMethodParameters[i];

                    if (trackedMethodParameter.TypeInfo == null || passedParameters == null)
                    {
                        continue;
                    }

                    var trackedVariableReference = new EvaluatedObjectReference();
                    trackedVariableReference.Declaration = trackedMethodParameter.Declaration;
                    trackedVariableReference.TypeInfo = trackedMethodParameter.TypeInfo;
                    trackedVariableReference.Identifier = trackedMethodParameter.Identifier;
                    trackedVariableReference.IdentifierText = trackedMethodParameter.IdentifierText;
                    trackedVariableReference.AssignEvaluatedObject(passedParameters);
                    _workflowEvaluatorExecutionState.CurrentExecutionFrame.LocalReferences.Add(trackedVariableReference);
                    _workflowEvaluatorExecutionState.CurrentExecutionFrame.PassedMethodParameters.Remove(i);
                }
                else
                {
                    var trackedMethodParameter = _evaluatedMethod.Parameters[i];

                    if (trackedMethodParameter.TypeInfo == null)
                    {
                        continue;
                    }

                    var trackedVariableReference = new EvaluatedObjectReference();
                    trackedVariableReference.Declaration = trackedMethodParameter.Declaration;
                    trackedVariableReference.TypeInfo = trackedMethodParameter.TypeInfo;
                    trackedVariableReference.Identifier = trackedMethodParameter.Identifier;
                    trackedVariableReference.IdentifierText = trackedMethodParameter.IdentifierText;
                    trackedVariableReference.AssignEvaluatedObject(
                        VariableAllocator.AllocateVariable(trackedMethodParameter.TypeInfo));
                    _workflowEvaluatorExecutionState.CurrentExecutionFrame.LocalReferences.Add(trackedVariableReference);
                }
            }
        }

        protected void ResetExecutionFrame()
        {
            _workflowEvaluatorExecutionState.PopFramePassingReturnedObjectsToPreviousFrame();
        }

        #endregion
    }
}