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

        /// <summary>
        ///     Builds the child execution frame.
        /// </summary>
        /// <param name="parentExecutionFrame">The execution frame.</param>
        /// <returns></returns>
        public EvaluatorExecutionFrame BuildChildExecutionFrameForNestedBlock(
            EvaluatorExecutionFrame parentExecutionFrame)
        {
            var staticWorkflowEvaluatorExecutionFrame = new EvaluatorExecutionFrame();

            staticWorkflowEvaluatorExecutionFrame.CurrentMethod = parentExecutionFrame.CurrentMethod;

            foreach (var localReference in parentExecutionFrame.LocalReferences)
            {
                staticWorkflowEvaluatorExecutionFrame.LocalReferences.Add(localReference.Copy());
            }

            staticWorkflowEvaluatorExecutionFrame.ThisReference = parentExecutionFrame.ThisReference.Copy();
            staticWorkflowEvaluatorExecutionFrame.CurrentSyntaxNode = parentExecutionFrame.CurrentSyntaxNode;

            return staticWorkflowEvaluatorExecutionFrame;
        }

        /// <summary>
        ///     Builds the initial execution frame.
        /// </summary>
        /// <param name="evaluatedType">Type of the tracked.</param>
        /// <param name="startMethod">The start method.</param>
        /// <returns></returns>
        public EvaluatorExecutionFrame BuildInitialExecutionFrame(EvaluatedTypeInfo evaluatedType)
        {
            var staticWorkflowEvaluatorExecutionFrame = new EvaluatorExecutionFrame();

            var trackedVariableAllocator = ObjectFactory.GetInstance<IEvaluatedObjectAllocator>();
            var allocateVariable = trackedVariableAllocator.AllocateVariable(evaluatedType);
            var thisEvaluatedObjectReference = new EvaluatedObjectReference();

            thisEvaluatedObjectReference.AssignEvaluatedObject(allocateVariable);
            thisEvaluatedObjectReference.TypeInfo = evaluatedType;
            staticWorkflowEvaluatorExecutionFrame.ThisReference = thisEvaluatedObjectReference;
            staticWorkflowEvaluatorExecutionFrame.PassedMethodParameters[-1] = thisEvaluatedObjectReference.Move();

            return staticWorkflowEvaluatorExecutionFrame;
        }

        /// <summary>
        ///     Builds the new type of the execution frame for method call in different.
        /// </summary>
        /// <param name="sourceExecutionFrame">The source execution frame.</param>
        /// <returns></returns>
        public EvaluatorExecutionFrame BuildNewExecutionFrameForMethodCall(
            EvaluatedMethodBase targetMethod,
            EvaluatedObjectReference thisReference)
        {
            var staticWorkflowEvaluatorExecutionFrame = new EvaluatorExecutionFrame();

            staticWorkflowEvaluatorExecutionFrame.CurrentMethod = targetMethod;
            staticWorkflowEvaluatorExecutionFrame.ThisReference = thisReference;
            staticWorkflowEvaluatorExecutionFrame.CurrentSyntaxNode = targetMethod.Declaration;

            return staticWorkflowEvaluatorExecutionFrame;
        }

        #endregion
    }
}