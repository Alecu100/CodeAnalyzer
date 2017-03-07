//  Project              : GLP
//  Module               : RomSoft.Client.Debug.dll
//  File                 : EvaluatedObjectHistory.cs
//  Author               : Alecsandru
//  Last Updated         : 07/12/2015 at 15:07
//  
// 
//  Contains             : Implementation of the EvaluatedObjectHistory.cs class.
//  Classes              : EvaluatedObjectHistory.cs
// 
//  
//  ----------------------------------------------------------------------- 
//   <copyright file="EvaluatedObjectHistory.cs" company="Sysmex"> 
//       Copyright (c) Sysmex. All rights reserved. 
//   </copyright> 
//  -----------------------------------------------------------------------

using System.Collections.Generic;
using Microsoft.CodeAnalysis;

namespace CodeAnalysis.Core.Members
{

    #region Using

    #endregion

    public class EvaluatedObjectHistory
    {
        #region Public Properties

        /// <summary>
        ///     Gets or sets the syntax node.
        /// </summary>
        /// <value>
        ///     The syntax node.
        /// </value>
        public SyntaxNode SyntaxNode { get; set; }

        public List<SyntaxNode> SyntaxNodeStack { get; set; }

        #endregion
    }
}