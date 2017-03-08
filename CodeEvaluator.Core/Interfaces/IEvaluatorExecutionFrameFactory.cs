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
        EvaluatorExecutionFrame BuildNewExecutionFrameForMethodCall(
            EvaluatedMethodBase targetMethod,
            EvaluatedObjectReference thisReference);


        EvaluatorExecutionFrame BuildInitialExecutionFrame(EvaluatedTypeInfo evaluatedType);
    }
}