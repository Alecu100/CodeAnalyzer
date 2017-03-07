//  Project              : GLP
//  Module               : RomSoft.Client.Debug.dll
//  File                 : SyntaxNodeResolverFoundNode.cs
//  Author               : Alecsandru
//  Last Updated         : 16/11/2015 at 17:35
//  
// 
//  Contains             : Implementation of the SyntaxNodeResolverFoundNode.cs class.
//  Classes              : SyntaxNodeResolverFoundNode.cs
// 
//  
//  ----------------------------------------------------------------------- 
//   <copyright file="SyntaxNodeResolverFoundNode.cs" company="Sysmex"> 
//       Copyright (c) Sysmex. All rights reserved. 
//   </copyright> 
//  -----------------------------------------------------------------------

namespace CodeAnalysis.Core.Common
{
    #region Using

    

    #endregion

    public class SyntaxNodeResolverFoundNode
    {
        #region Public Properties

        /// <summary>
        ///     Gets or sets the filename.
        /// </summary>
        /// <value>
        ///     The filename.
        /// </value>
        public string Filename { get; set; }

        /// <summary>
        ///     Gets or sets the found syntax node.
        /// </summary>
        /// <value>
        ///     The found syntax node.
        /// </value>
        public SyntaxNode FoundSyntaxNode { get; set; }

        /// <summary>
        ///     Gets or sets the parent class syntax node.
        /// </summary>
        /// <value>
        ///     The parent class syntax node.
        /// </value>
        public SyntaxNode ParentClassSyntaxNode { get; set; }

        #endregion
    }
}