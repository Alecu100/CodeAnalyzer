//  Project              : GLP
//  Module               : RomSoft.Client.Debug.dll
//  File                 : ITrackedVariableAllocator.cs
//  Author               : Alecsandru
//  Last Updated         : 18/12/2015 at 23:47
//  
// 
//  Contains             : Implementation of the ITrackedVariableAllocator.cs class.
//  Classes              : ITrackedVariableAllocator.cs
// 
//  
//  ----------------------------------------------------------------------- 
//   <copyright file="ITrackedVariableAllocator.cs" company="Sysmex"> 
//       Copyright (c) Sysmex. All rights reserved. 
//   </copyright> 
//  -----------------------------------------------------------------------

namespace RomSoft.Client.Debug.Library.Interfaces
{
    #region Using

    using RomSoft.Client.Debug.Library.Members;

    #endregion

    public interface ITrackedVariableAllocator
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Allocates the variable.
        /// </summary>
        /// <param name="typeInfo">The type information.</param>
        /// <returns></returns>
        TrackedVariable AllocateVariable(TrackedTypeInfo typeInfo);

        #endregion
    }
}