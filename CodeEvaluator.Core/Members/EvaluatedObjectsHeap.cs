//  Project              : GLP
//  Module               : RomSoft.Client.Debug.dll
//  File                 : EvaluatedObjectsHeap.cs
//  Author               : Alecsandru
//  Last Updated         : 07/12/2015 at 12:50
//  
// 
//  Contains             : Implementation of the EvaluatedObjectsHeap.cs class.
//  Classes              : EvaluatedObjectsHeap.cs
// 
//  
//  ----------------------------------------------------------------------- 
//   <copyright file="EvaluatedObjectsHeap.cs" company="Sysmex"> 
//       Copyright (c) Sysmex. All rights reserved. 
//   </copyright> 
//  -----------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using CodeAnalysis.Core.Interfaces;

namespace CodeAnalysis.Core.Members
{
    #region Using

    

    #endregion

    public class EvaluatedObjectsHeap : List<EvaluatedObject>, IEvaluatedObjectsHeap
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
        public void Add(EvaluatedObject item)
        {
            base.Add(item);

            item.ParentHeap = this;
        }

        /// <summary>
        ///     Removes the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        public void Remove(EvaluatedObject item)
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