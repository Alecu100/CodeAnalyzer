//  Project              : GLP
//  Module               : RomSoft.Client.Debug.dll
//  File                 : ITrackedVariablesHeap.cs
//  Author               : Alecsandru
//  Last Updated         : 07/12/2015 at 12:48
//  
// 
//  Contains             : Implementation of the ITrackedVariablesHeap.cs class.
//  Classes              : ITrackedVariablesHeap.cs
// 
//  
//  ----------------------------------------------------------------------- 
//   <copyright file="ITrackedVariablesHeap.cs" company="Sysmex"> 
//       Copyright (c) Sysmex. All rights reserved. 
//   </copyright> 
//  -----------------------------------------------------------------------

namespace RomSoft.Client.Debug.Library.Interfaces
{
    #region Using

    using System.Collections.Generic;

    using RomSoft.Client.Debug.Library.Common;
    using RomSoft.Client.Debug.Library.Members;

    #endregion

    public interface ITrackedVariablesHeap : IList<TrackedVariable>
    {
        #region Public Methods and Operators

        /// <summary>
        /// Removes the specified tracked variable.
        /// </summary>
        /// <param name="trackedVariable">The tracked variable.</param>
        void Remove(TrackedVariable trackedVariable);

        #endregion
    }
}