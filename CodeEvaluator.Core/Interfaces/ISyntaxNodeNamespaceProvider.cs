using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CodeAnalysis.Core.Interfaces
{

    #region Using

    #endregion

    public interface ISyntaxNodeNamespaceProvider
    {
        #region Public Methods and Operators

        List<NamespaceDeclarationSyntax> GetNamespaceDeclarations(SyntaxNode syntaxNode);

        List<UsingDirectiveSyntax> GetUsingDirectives(SyntaxNode syntaxNode);

        #endregion
    }
}