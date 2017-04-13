//  Project              : GLP
//  Module               : RomSoft.Client.Debug.dll
//  File                 : EvaluatedMethodBase.cs
//  Author               : Alecsandru
//  Last Updated         : 18/12/2015 at 15:37
//  
// 
//  Contains             : Implementation of the EvaluatedMethodBase.cs class.
//  Classes              : EvaluatedMethodBase.cs
// 
//  
//  ----------------------------------------------------------------------- 
//   <copyright file="EvaluatedMethodBase.cs" company="Sysmex"> 
//       Copyright (c) Sysmex. All rights reserved. 
//   </copyright> 
//  -----------------------------------------------------------------------

using System.Collections.Generic;

namespace CodeAnalysis.Core.Members
{
    #region Using

    

    #endregion

    public class EvaluatedMethodBase : EvaluatedMember
    {
        #region SpecificFields

        private readonly List<EvaluatedMethodParameter> _parameters = new List<EvaluatedMethodParameter>();

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
        public List<EvaluatedMethodParameter> Parameters
        {
            get
            {
                return _parameters;
            }
        }

        #endregion
    }
}