//  Project              : GLP
//  Module               : RomSoft.Client.Debug.dll
//  File                 : StaticWorkflowEvaluatorExecutionFrameFactory.cs
//  Author               : Alecsandru
//  Last Updated         : 19/12/2015 at 1:20
//  
// 
//  Contains             : Implementation of the StaticWorkflowEvaluatorExecutionFrameFactory.cs class.
//  Classes              : StaticWorkflowEvaluatorExecutionFrameFactory.cs
// 
//  
//  ----------------------------------------------------------------------- 
//   <copyright file="StaticWorkflowEvaluatorExecutionFrameFactory.cs" company="Sysmex"> 
//       Copyright (c) Sysmex. All rights reserved. 
//   </copyright> 
//  -----------------------------------------------------------------------

namespace RomSoft.Client.Debug.Library.Common
{
    #region Using

    using RomSoft.Client.Debug.Library.Interfaces;
    using RomSoft.Client.Debug.Library.Members;

    using StructureMap;

    #endregion

    public class StaticWorkflowEvaluatorExecutionFrameFactory : IStaticWorkflowEvaluatorExecutionFrameFactory
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Builds the child execution frame.
        /// </summary>
        /// <param name="parentExecutionFrame">The execution frame.</param>
        /// <returns></returns>
        public StaticWorkflowEvaluatorExecutionFrame BuildChildExecutionFrameForNestedBlock(
            StaticWorkflowEvaluatorExecutionFrame parentExecutionFrame)
        {
            var staticWorkflowEvaluatorExecutionFrame = new StaticWorkflowEvaluatorExecutionFrame();

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
        /// <param name="trackedType">Type of the tracked.</param>
        /// <param name="startMethod">The start method.</param>
        /// <returns></returns>
        public StaticWorkflowEvaluatorExecutionFrame BuildInitialExecutionFrame(TrackedTypeInfo trackedType)
        {
            var staticWorkflowEvaluatorExecutionFrame = new StaticWorkflowEvaluatorExecutionFrame();

            var trackedVariableAllocator = ObjectFactory.GetInstance<ITrackedVariableAllocator>();
            var allocateVariable = trackedVariableAllocator.AllocateVariable(trackedType);
            var trackedVariableReference = new TrackedVariableReference();

            trackedVariableReference = trackedVariableReference.AddVariable(allocateVariable);
            trackedVariableReference.TypeInfo = trackedType;
            staticWorkflowEvaluatorExecutionFrame.ThisReference = trackedVariableReference;
            staticWorkflowEvaluatorExecutionFrame.PassedMethodParameters[-1] = trackedVariableReference.Move();

            return staticWorkflowEvaluatorExecutionFrame;
        }

        /// <summary>
        ///     Builds the new type of the execution frame for method call in different.
        /// </summary>
        /// <param name="sourceExecutionFrame">The source execution frame.</param>
        /// <returns></returns>
        public StaticWorkflowEvaluatorExecutionFrame BuildNewExecutionFrameForMethodCall(
            TrackedMethodBase targetMethod,
            TrackedVariableReference thisReference)
        {
            var staticWorkflowEvaluatorExecutionFrame = new StaticWorkflowEvaluatorExecutionFrame();

            staticWorkflowEvaluatorExecutionFrame.CurrentMethod = targetMethod;
            staticWorkflowEvaluatorExecutionFrame.ThisReference = thisReference;
            staticWorkflowEvaluatorExecutionFrame.CurrentSyntaxNode = targetMethod.Declaration;

            return staticWorkflowEvaluatorExecutionFrame;
        }

        #endregion
    }
}