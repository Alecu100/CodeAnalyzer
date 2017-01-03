//  Project              : GLP
//  Module               : RomSoft.Client.Debug.dll
//  File                 : TrackedMethodParameter.cs
//  Author               : Alecsandru
//  Last Updated         : 18/12/2015 at 15:36
//  
// 
//  Contains             : Implementation of the TrackedMethodParameter.cs class.
//  Classes              : TrackedMethodParameter.cs
// 
//  
//  ----------------------------------------------------------------------- 
//   <copyright file="TrackedMethodParameter.cs" company="Sysmex"> 
//       Copyright (c) Sysmex. All rights reserved. 
//   </copyright> 
//  -----------------------------------------------------------------------

namespace RomSoft.Client.Debug.Library.Members
{
    public class TrackedMethodParameter : TrackedMember
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
        public TrackedTypeInfo TypeInfo { get; set; }

        #endregion
    }
}