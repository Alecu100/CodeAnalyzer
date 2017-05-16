//  Project              : GLP
//  Module               : RomSoft.Client.Debug.dll
//  File                 : EvaluatedField.cs
//  Author               : Alecsandru
//  Last Updated         : 14/12/2015 at 11:44
//  
// 
//  Contains             : Implementation of the EvaluatedField.cs class.
//  Classes              : EvaluatedField.cs
// 
//  
//  ----------------------------------------------------------------------- 
//   <copyright file="EvaluatedField.cs" company="Sysmex"> 
//       Copyright (c) Sysmex. All rights reserved. 
//   </copyright> 
//  -----------------------------------------------------------------------

using System;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CodeAnalysis.Core.Members
{

    #region Using

    #endregion

    [Serializable]
    public class EvaluatedField : EvaluatedMember
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
        public EvaluatedTypeInfo TypeInfo { get; set; }

        #endregion
    }
}