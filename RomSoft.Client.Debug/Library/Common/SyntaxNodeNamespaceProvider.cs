//  Project              : GLP
//  Module               : RomSoft.Client.Debug.dll
//  File                 : SyntaxNodeNamespaceProvider.cs
//  Author               : Alecsandru
//  Last Updated         : 17/11/2015 at 16:47
//  
// 
//  Contains             : Implementation of the SyntaxNodeNamespaceProvider.cs class.
//  Classes              : SyntaxNodeNamespaceProvider.cs
// 
//  
//  ----------------------------------------------------------------------- 
//   <copyright file="SyntaxNodeNamespaceProvider.cs" company="Sysmex"> 
//       Copyright (c) Sysmex. All rights reserved. 
//   </copyright> 
//  -----------------------------------------------------------------------

namespace RomSoft.Client.Debug.Library.Common
{
    #region Using

    using System.Collections.Generic;

    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp.Syntax;

    using RomSoft.Client.Debug.Library.Interfaces;

    #endregion

    public class SyntaxNodeNamespaceProvider : ISyntaxNodeNamespaceProvider
    {
        #region Fields

        private List<NamespaceDeclarationSyntax> _foundNamespaceDeclarations;

        private List<UsingDirectiveSyntax> _foundUsingDirectives;

        #endregion

        #region Public Methods and Operators

        public List<NamespaceDeclarationSyntax> GetNamespaceDeclarations(SyntaxNode syntaxNode)
        {
            _foundNamespaceDeclarations = new List<NamespaceDeclarationSyntax>();

            GetNamespaceDeclarationsFromParent(syntaxNode.Parent);

            _foundNamespaceDeclarations.Reverse();

            return _foundNamespaceDeclarations;
        }

        public List<UsingDirectiveSyntax> GetUsingDirectives(SyntaxNode syntaxNode)
        {
            _foundUsingDirectives = new List<UsingDirectiveSyntax>();

            GetUsingDirectivesFromParent(syntaxNode.Parent);

            return _foundUsingDirectives;
        }

        #endregion

        #region Private Methods and Operators

        private void GetNamespaceDeclarationsFromParent(SyntaxNode parent)
        {
            while (parent != null)
            {
                var syntaxNodes = parent.ChildNodes();

                foreach (var syntaxNode in syntaxNodes)
                {
                    if (syntaxNode is NamespaceDeclarationSyntax)
                    {
                        var namespaceDeclarationSyntax = syntaxNode as NamespaceDeclarationSyntax;
                        _foundNamespaceDeclarations.Add(namespaceDeclarationSyntax);
                    }
                }

                parent = parent.Parent;
            }
        }

        private void GetUsingDirectivesFromParent(SyntaxNode parent)
        {
            while (parent != null)
            {
                var syntaxNodes = parent.ChildNodes();

                foreach (var syntaxNode in syntaxNodes)
                {
                    if (syntaxNode is UsingDirectiveSyntax)
                    {
                        var usingDirectiveSyntax = syntaxNode as UsingDirectiveSyntax;
                        _foundUsingDirectives.Add(usingDirectiveSyntax);
                    }
                }

                parent = parent.Parent;
            }
        }

        #endregion
    }
}