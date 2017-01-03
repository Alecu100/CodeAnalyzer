//  Project              : GLP
//  Module               : RomSoft.Client.Debug.dll
//  File                 : TrackedVariablesHeap.cs
//  Author               : Alecsandru
//  Last Updated         : 07/12/2015 at 12:50
//  
// 
//  Contains             : Implementation of the TrackedVariablesHeap.cs class.
//  Classes              : TrackedVariablesHeap.cs
// 
//  
//  ----------------------------------------------------------------------- 
//   <copyright file="TrackedVariablesHeap.cs" company="Sysmex"> 
//       Copyright (c) Sysmex. All rights reserved. 
//   </copyright> 
//  -----------------------------------------------------------------------

namespace RomSoft.Client.Debug.Library.Members
{
    #region Using

    using System.Collections.Generic;
    using System.Linq;

    using RomSoft.Client.Debug.Library.Interfaces;

    #endregion

    public class TrackedVariablesHeap : List<TrackedVariable>, ITrackedVariablesHeap
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Adds an item to the <see cref="T:System.Collections.Generic.ICollection`1" />.
        /// </summary>
        /// <param name="item">The object to add to the <see cref="T:System.Collections.Generic.ICollection`1" />.</param>
        /// <exception cref="T:System.NotSupportedException">
        ///     The <see cref="T:System.Collections.Generic.ICollection`1" /> is
        ///     read-only.
        /// </exception>
        public void Add(TrackedVariable item)
        {
            base.Add(item);

            item.ParentHeap = this;
        }

        /// <summary>
        ///     Removes the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        public void Remove(TrackedVariable item)
        {
            var allReferences = item.References.ToArray();

            foreach (var trackedVariableReference in allReferences)
            {
                item.RemoveReference(trackedVariableReference);
            }
        }

        #endregion
    }
}