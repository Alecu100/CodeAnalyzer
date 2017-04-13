﻿//  Project              : GLP
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
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CodeAnalysis.Core.Interfaces
{

    #region Using

    #endregion

    public interface IEvaluatorExecutionFrameFactory
    {
        CodeEvaluatorExecutionFrame BuildNewExecutionFrameForMethodCall(
            EvaluatedMethodBase targetMethod,
            EvaluatedObjectReference thisReference);


        CodeEvaluatorExecutionFrame BuildInitialExecutionFrame(EvaluatedTypeInfo evaluatedType, EvaluatedMethod startMethod);
    }
}