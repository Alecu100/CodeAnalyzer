//  Project              : GLP
//  Module               : RomSoft.Client.Debug.dll
//  File                 : ISyntaxNodeNamespaceProvider.cs
//  Author               : Alecsandru
//  Last Updated         : 17/11/2015 at 16:33
//  
// 
//  Contains             : Implementation of the ISyntaxNodeNamespaceProvider.cs class.
//  Classes              : ISyntaxNodeNamespaceProvider.cs
// 
//  
//  ----------------------------------------------------------------------- 
//   <copyright file="ISyntaxNodeNamespaceProvider.cs" company="Sysmex"> 
//       Copyright (c) Sysmex. All rights reserved. 
//   </copyright> 
//  -----------------------------------------------------------------------

namespace RomSoft.Client.Debug.Library.Interfaces
{
    #region Using

    using System.Collections.Generic;

    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp.Syntax;

    #endregion

    public interface ISyntaxNodeNamespaceProvider
    {
        #region Public Methods and Operators

        List<NamespaceDeclarationSyntax> GetNamespaceDeclarations(SyntaxNode syntaxNode);

        List<UsingDirectiveSyntax> GetUsingDirectives(SyntaxNode syntaxNode);

        #endregion
    }
}