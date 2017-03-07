//  Project              : GLP
//  Module               : RomSoft.Client.Debug.dll
//  File                 : IEvaluatorExecutionFrameFactory.cs
//  Author               : Alecsandru
//  Last Updated         : 14/12/2015 at 12:45
//  
// 
//  Contains             : Implementation of the IEvaluatorExecutionFrameFactory.cs class.
//  Classes              : IEvaluatorExecutionFrameFactory.cs
// 
//  
//  ----------------------------------------------------------------------- 
//   <copyright file="IEvaluatorExecutionFrameFactory.cs" company="Sysmex"> 
//       Copyright (c) Sysmex. All rights reserved. 
//   </copyright> 
//  -----------------------------------------------------------------------

using CodeAnalysis.Core.Common;
using CodeAnalysis.Core.Members;

namespace CodeAnalysis.Core.Interfaces
{
    #region Using

    

    #endregion

    public interface IEvaluatorExecutionFrameFactory
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Builds the child execution frame.
        /// </summary>
        /// <param name="parentExecutionFrame">The execution frame.</param>
        /// <returns></returns>
        EvaluatorExecutionFrame BuildChildExecutionFrameForNestedBlock(
            EvaluatorExecutionFrame parentExecutionFrame);

        /// <summary>
        ///     Builds the new type of the execution frame for method call in different.
        /// </summary>
        /// <param name="sourceExecutionFrame">The source execution frame.</param>
        /// <returns></returns>
        EvaluatorExecutionFrame BuildNewExecutionFrameForMethodCall(
            EvaluatedMethodBase targetMethod,
            EvaluatedObjectReference thisReference);

        #endregion

        /// <summary>
        ///     Builds the initial execution frame.
        /// </summary>
        /// <param name="evaluatedType">Type of the tracked.</param>
        /// <param name="startMethod">The start method.</param>
        /// <returns></returns>
        EvaluatorExecutionFrame BuildInitialExecutionFrame(EvaluatedTypeInfo evaluatedType);
    }
}