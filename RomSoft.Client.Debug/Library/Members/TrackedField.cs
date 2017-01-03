//  Project              : GLP
//  Module               : RomSoft.Client.Debug.dll
//  File                 : TrackedField.cs
//  Author               : Alecsandru
//  Last Updated         : 14/12/2015 at 11:44
//  
// 
//  Contains             : Implementation of the TrackedField.cs class.
//  Classes              : TrackedField.cs
// 
//  
//  ----------------------------------------------------------------------- 
//   <copyright file="TrackedField.cs" company="Sysmex"> 
//       Copyright (c) Sysmex. All rights reserved. 
//   </copyright> 
//  -----------------------------------------------------------------------

namespace RomSoft.Client.Debug.Library.Members
{
    #region Using

    using Microsoft.CodeAnalysis.CSharp.Syntax;

    #endregion

    public class TrackedField : TrackedMember
    {
        #region Public Properties

        /// <summary>
        ///     Gets or sets the initializer expression.
        /// </summary>
        /// <value>
        ///     The initializer expression.
        /// </value>
        public EqualsValueClauseSyntax InitializerExpression { get; set; }

        /// <summary>
        ///     Gets or sets the variable type information.
        /// </summary>
        /// <value>
        ///     The variable type information.
        /// </value>
        public TrackedTypeInfo TypeInfo { get; set; }

        #endregion
    }
}