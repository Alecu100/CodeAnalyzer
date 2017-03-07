//  Project              : GLP
//  Module               : RomSoft.Client.Debug.dll
//  File                 : ITrackedVariableTypeInfosCache.cs
//  Author               : Alecsandru
//  Last Updated         : 19/12/2015 at 1:18
//  
// 
//  Contains             : Implementation of the ITrackedVariableTypeInfosCache.cs class.
//  Classes              : ITrackedVariableTypeInfosCache.cs
// 
//  
//  ----------------------------------------------------------------------- 
//   <copyright file="ITrackedVariableTypeInfosCache.cs" company="Sysmex"> 
//       Copyright (c) Sysmex. All rights reserved. 
//   </copyright> 
//  -----------------------------------------------------------------------

namespace RomSoft.Client.Debug.Library.Interfaces
{
    #region Using

    using System.Collections.Generic;

    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp.Syntax;

    using RomSoft.Client.Debug.Library.Members;

    #endregion

    public interface ITrackedVariableTypeInfosCache
    {
        #region Public Properties

        /// <summary>
        ///     Gets the well known types.
        /// </summary>
        /// <value>
        ///     The well known types.
        /// </value>
        IReadOnlyList<TrackedTypeInfo> InternalTypeInfos { get; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Gets the well known type information.
        /// </summary>
        /// <param name="typeName">Name of the type.</param>
        /// <param name="usingDirectives">The using directives.</param>
        /// <param name="namespaceDeclarations">The namespace declarations.</param>
        /// <returns></returns>
        TrackedTypeInfo GetTypeInfo(
            string typeName,
            List<UsingDirectiveSyntax> usingDirectives,
            List<MemberDeclarationSyntax> namespaceDeclarations);

        /// <summary>
        ///     Gets the well known type information.
        /// </summary>
        /// <param name="constructor">The constructor.</param>
        /// <returns></returns>
        TrackedTypeInfo GetTypeInfo(ConstructorDeclarationSyntax constructor);

        /// <summary>
        ///     Gets the type information.
        /// </summary>
        /// <param name="typeDeclaration">The type declaration.</param>
        /// <returns></returns>
        TrackedTypeInfo GetTypeInfo(BaseTypeDeclarationSyntax typeDeclaration);

        /// <summary>
        ///     Rebuilds the well known methods.
        /// </summary>
        /// <param name="syntaxTrees">The syntax trees.</param>
        void RebuildWellKnownTypesWithMethods(IList<SyntaxTree> syntaxTrees);

        #endregion
    }
}