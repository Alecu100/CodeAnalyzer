//  Project              : GLP
//  Module               : RomSoft.Client.Debug.dll
//  File                 : EvaluatedMember.cs
//  Author               : Alecsandru
//  Last Updated         : 18/12/2015 at 15:38
//  
// 
//  Contains             : Implementation of the EvaluatedMember.cs class.
//  Classes              : EvaluatedMember.cs
// 
//  
//  ----------------------------------------------------------------------- 
//   <copyright file="EvaluatedMember.cs" company="Sysmex"> 
//       Copyright (c) Sysmex. All rights reserved. 
//   </copyright> 
//  -----------------------------------------------------------------------

using System;
using CodeAnalysis.Core.Enums;
using Microsoft.CodeAnalysis;

namespace CodeAnalysis.Core.Members
{

    #region Using

    #endregion

    [Serializable]
    public class EvaluatedMember
    {
        #region Public Properties

        /// <summary>
        ///     Gets or sets the declaration.
        /// </summary>
        /// <value>
        ///     The declaration.
        /// </value>
        public SyntaxNode Declaration { get; set; }

        /// <summary>
        ///     Gets or sets the full name.
        /// </summary>
        /// <value>
        ///     The full name.
        /// </value>
        public string FullIdentifierText { get; set; }

        /// <summary>
        ///     Gets or sets the name of the identifier.
        /// </summary>
        /// <value>
        ///     The name of the identifier.
        /// </value>
        public SyntaxToken Identifier { get; set; }

        /// <summary>
        ///     Gets or sets the identifier text.
        /// </summary>
        /// <value>
        ///     The identifier text.
        /// </value>
        public string IdentifierText { get; set; }

        public EMemberFlags MemberFlags { get; set; }

        #endregion
    }
}