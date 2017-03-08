//  Project              : GLP
//  Module               : RomSoft.Client.Debug.dll
//  File                 : IEvaluatedObjectAllocator.cs
//  Author               : Alecsandru
//  Last Updated         : 18/12/2015 at 23:47
//  
// 
//  Contains             : Implementation of the IEvaluatedObjectAllocator.cs class.
//  Classes              : IEvaluatedObjectAllocator.cs
// 
//  
//  ----------------------------------------------------------------------- 
//   <copyright file="IEvaluatedObjectAllocator.cs" company="Sysmex"> 
//       Copyright (c) Sysmex. All rights reserved. 
//   </copyright> 
//  -----------------------------------------------------------------------

using CodeAnalysis.Core.Members;

namespace CodeAnalysis.Core.Interfaces
{
    #region Using

    

    #endregion

    public interface IEvaluatedObjectAllocator
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Allocates the variable.
        /// </summary>
        /// <param name="typeInfo">The type information.</param>
        /// <returns></returns>
        EvaluatedObject AllocateVariable(EvaluatedTypeInfo typeInfo);

        #endregion
    }
}