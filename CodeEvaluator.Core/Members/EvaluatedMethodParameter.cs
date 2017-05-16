//  Project              : GLP
//  Module               : RomSoft.Client.Debug.dll
//  File                 : EvaluatedMethodParameter.cs
//  Author               : Alecsandru
//  Last Updated         : 18/12/2015 at 15:36
//  
// 
//  Contains             : Implementation of the EvaluatedMethodParameter.cs class.
//  Classes              : EvaluatedMethodParameter.cs
// 
//  
//  ----------------------------------------------------------------------- 
//   <copyright file="EvaluatedMethodParameter.cs" company="Sysmex"> 
//       Copyright (c) Sysmex. All rights reserved. 
//   </copyright> 
//  -----------------------------------------------------------------------

using System;

namespace CodeAnalysis.Core.Members
{
    [Serializable]
    public class EvaluatedMethodParameter : EvaluatedMember
    {
        #region Public Properties

        /// <summary>
        ///     Gets or sets the index.
        /// </summary>
        /// <value>
        ///     The index.
        /// </value>
        public int Index { get; set; }

        /// <summary>
        ///     Gets or sets the type information.
        /// </summary>
        /// <value>
        ///     The type information.
        /// </value>
        public EvaluatedTypeInfo TypeInfo { get; set; }

        #endregion
    }
}