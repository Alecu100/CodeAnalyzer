//  Project              : GLP
//  Module               : Sysmex.GLP.Debug.dll
//  File                 : EWorkflowStepType.cs
//  Author               : Alecsandru
//  Last Updated         : 05/11/2015 at 16:48
//  
// 
//  Contains             : Implementation of the EWorkflowStepType.cs class.
//  Classes              : EWorkflowStepType.cs
// 
//  
//  ----------------------------------------------------------------------- 
//   <copyright file="EWorkflowStepType.cs" company="Sysmex"> 
//       Copyright (c) Sysmex. All rights reserved. 
//   </copyright> 
//  -----------------------------------------------------------------------

using System;

namespace CodeEvaluator.Workflows.Enums
{
    [Serializable]
    public enum EWorkflowStepType
    {
        None,

        Decision,

        Process,

        Start,

        Stop
    }
}