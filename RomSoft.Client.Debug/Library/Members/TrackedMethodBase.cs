//  Project              : GLP
//  Module               : RomSoft.Client.Debug.dll
//  File                 : TrackedMethodBase.cs
//  Author               : Alecsandru
//  Last Updated         : 18/12/2015 at 15:37
//  
// 
//  Contains             : Implementation of the TrackedMethodBase.cs class.
//  Classes              : TrackedMethodBase.cs
// 
//  
//  ----------------------------------------------------------------------- 
//   <copyright file="TrackedMethodBase.cs" company="Sysmex"> 
//       Copyright (c) Sysmex. All rights reserved. 
//   </copyright> 
//  -----------------------------------------------------------------------

namespace RomSoft.Client.Debug.Library.Members
{
    #region Using

    using System.Collections.Generic;

    #endregion

    public class TrackedMethodBase : TrackedMember
    {
        #region Fields

        private readonly List<TrackedMethodParameter> _parameters = new List<TrackedMethodParameter>();

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets or sets a value indicating whether this instance is override.
        /// </summary>
        /// <value>
        ///     <c>true</c> if this instance is override; otherwise, <c>false</c>.
        /// </value>
        public bool IsOverride { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether this instance is virtual.
        /// </summary>
        /// <value>
        ///     <c>true</c> if this instance is virtual; otherwise, <c>false</c>.
        /// </value>
        public bool IsVirtual { get; set; }

        /// <summary>
        ///     Gets the parameters.
        /// </summary>
        /// <value>
        ///     The parameters.
        /// </value>
        public List<TrackedMethodParameter> Parameters
        {
            get
            {
                return _parameters;
            }
        }

        #endregion
    }
}