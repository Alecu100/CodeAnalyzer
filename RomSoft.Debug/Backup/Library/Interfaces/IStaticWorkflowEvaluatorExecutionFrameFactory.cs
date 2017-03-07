//  Project              : GLP
//  Module               : RomSoft.Client.Debug.dll
//  File                 : IStaticWorkflowEvaluatorExecutionFrameFactory.cs
//  Author               : Alecsandru
//  Last Updated         : 14/12/2015 at 12:45
//  
// 
//  Contains             : Implementation of the IStaticWorkflowEvaluatorExecutionFrameFactory.cs class.
//  Classes              : IStaticWorkflowEvaluatorExecutionFrameFactory.cs
// 
//  
//  ----------------------------------------------------------------------- 
//   <copyright file="IStaticWorkflowEvaluatorExecutionFrameFactory.cs" company="Sysmex"> 
//       Copyright (c) Sysmex. All rights reserved. 
//   </copyright> 
//  -----------------------------------------------------------------------

namespace RomSoft.Client.Debug.Library.Interfaces
{
    #region Using

    using RomSoft.Client.Debug.Library.Common;
    using RomSoft.Client.Debug.Library.Members;

    #endregion

    public interface IStaticWorkflowEvaluatorExecutionFrameFactory
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Builds the child execution frame.
        /// </summary>
        /// <param name="parentExecutionFrame">The execution frame.</param>
        /// <returns></returns>
        StaticWorkflowEvaluatorExecutionFrame BuildChildExecutionFrameForNestedBlock(
            StaticWorkflowEvaluatorExecutionFrame parentExecutionFrame);

        /// <summary>
        ///     Builds the new type of the execution frame for method call in different.
        /// </summary>
        /// <param name="sourceExecutionFrame">The source execution frame.</param>
        /// <returns></returns>
        StaticWorkflowEvaluatorExecutionFrame BuildNewExecutionFrameForMethodCall(
            TrackedMethodBase targetMethod,
            TrackedVariableReference thisReference);

        #endregion

        /// <summary>
        ///     Builds the initial execution frame.
        /// </summary>
        /// <param name="trackedType">Type of the tracked.</param>
        /// <param name="startMethod">The start method.</param>
        /// <returns></returns>
        StaticWorkflowEvaluatorExecutionFrame BuildInitialExecutionFrame(TrackedTypeInfo trackedType);
    }
}