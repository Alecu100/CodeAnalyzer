//  Project              : GLP
//  Module               : RomSoft.Client.Debug.dll
//  File                 : IEvaluatedObjectsHeap.cs
//  Author               : Alecsandru
//  Last Updated         : 07/12/2015 at 12:48
//  
// 
//  Contains             : Implementation of the IEvaluatedObjectsHeap.cs class.
//  Classes              : IEvaluatedObjectsHeap.cs
// 
//  
//  ----------------------------------------------------------------------- 
//   <copyright file="IEvaluatedObjectsHeap.cs" company="Sysmex"> 
//       Copyright (c) Sysmex. All rights reserved. 
//   </copyright> 
//  -----------------------------------------------------------------------

using System.Collections.Generic;
using CodeAnalysis.Core.Members;

namespace CodeAnalysis.Core.Interfaces
{

    #region Using

    #endregion

    public interface IEvaluatedObjectsHeap : IList<EvaluatedObject>
    {
    }
}