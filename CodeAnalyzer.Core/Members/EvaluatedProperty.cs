//  Project              : GLP
//  Module               : RomSoft.Client.Debug.dll
//  File                 : EvaluatedProperty.cs
//  Author               : Alecsandru
//  Last Updated         : 18/12/2015 at 21:59
//  
// 
//  Contains             : Implementation of the EvaluatedProperty.cs class.
//  Classes              : EvaluatedProperty.cs
// 
//  
//  ----------------------------------------------------------------------- 
//   <copyright file="EvaluatedProperty.cs" company="Sysmex"> 
//       Copyright (c) Sysmex. All rights reserved. 
//   </copyright> 
//  -----------------------------------------------------------------------

namespace CodeAnalysis.Core.Members
{
    public class EvaluatedProperty : EvaluatedMember
    {
        #region Public Properties

        /// <summary>
        ///     Gets or sets a value indicating whether this instance is automatic property.
        /// </summary>
        /// <value>
        ///     <c>true</c> if this instance is automatic property; otherwise, <c>false</c>.
        /// </value>
        public bool IsAutoProperty { get; set; }

        /// <summary>
        ///     Gets or sets the property get accessor.
        /// </summary>
        /// <value>
        ///     The property get accessor.
        /// </value>
        public EvaluatedPropertyGetAccessor PropertyGetAccessor { get; set; }

        /// <summary>
        ///     Gets or sets the property set accessor.
        /// </summary>
        /// <value>
        ///     The property set accessor.
        /// </value>
        public EvaluatedPropertySetAccessor PropertySetAccessor { get; set; }

        /// <summary>
        ///     Gets or sets the variable type information.
        /// </summary>
        /// <value>
        ///     The variable type information.
        /// </value>
        public EvaluatedTypeInfo TypeInfo { get; set; }

        #endregion
    }
}