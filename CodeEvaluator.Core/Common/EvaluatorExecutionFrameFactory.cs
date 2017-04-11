//  Project              : GLP
//  Module               : RomSoft.Client.Debug.dll
//  File                 : EvaluatorExecutionFrameFactory.cs
//  Author               : Alecsandru
//  Last Updated         : 19/12/2015 at 1:20
//  
// 
//  Contains             : Implementation of the EvaluatorExecutionFrameFactory.cs class.
//  Classes              : EvaluatorExecutionFrameFactory.cs
// 
//  
//  ----------------------------------------------------------------------- 
//   <copyright file="EvaluatorExecutionFrameFactory.cs" company="Sysmex"> 
//       Copyright (c) Sysmex. All rights reserved. 
//   </copyright> 
//  -----------------------------------------------------------------------

using CodeAnalysis.Core.Interfaces;
using CodeAnalysis.Core.Members;
using StructureMap;

namespace CodeAnalysis.Core.Common
{

    #region Using

    #endregion

    public class EvaluatorExecutionFrameFactory : IEvaluatorExecutionFrameFactory
    {
        #region Public Methods and Operators

        public CodeEvaluatorExecutionFrame BuildInitialExecutionFrame(EvaluatedTypeInfo evaluatedType)
        {
            var staticWorkflowEvaluatorExecutionFrame = new CodeEvaluatorExecutionFrame();

            var trackedVariableAllocator = ObjectFactory.GetInstance<IEvaluatedObjectAllocator>();
            var allocateVariable = trackedVariableAllocator.AllocateVariable(evaluatedType);
            var thisEvaluatedObjectReference = new EvaluatedObjectReference();

            thisEvaluatedObjectReference.AssignEvaluatedObject(allocateVariable);
            staticWorkflowEvaluatorExecutionFrame.ThisReference = thisEvaluatedObjectReference;
            staticWorkflowEvaluatorExecutionFrame.PassedMethodParameters[-1] = thisEvaluatedObjectReference;

            return staticWorkflowEvaluatorExecutionFrame;
        }

        public CodeEvaluatorExecutionFrame BuildNewExecutionFrameForMethodCall(
            EvaluatedMethodBase targetMethod,
            EvaluatedObjectReference thisReference)
        {
            var newExecutionFrameForMethodCall = new CodeEvaluatorExecutionFrame();

            newExecutionFrameForMethodCall.CurrentMethod = targetMethod;
            newExecutionFrameForMethodCall.ThisReference = thisReference;
            newExecutionFrameForMethodCall.CurrentSyntaxNode = targetMethod.Declaration;

            return newExecutionFrameForMethodCall;
        }

        #endregion
    }
}