﻿//  Project              : GLP
//  Module               : RomSoft.Client.Debug.dll
//  File                 : IEvaluatedTypesInfoTable.cs
//  Author               : Alecsandru
//  Last Updated         : 19/12/2015 at 1:18
//  
// 
//  Contains             : Implementation of the IEvaluatedTypesInfoTable.cs class.
//  Classes              : IEvaluatedTypesInfoTable.cs
// 
//  
//  ----------------------------------------------------------------------- 
//   <copyright file="IEvaluatedTypesInfoTable.cs" company="Sysmex"> 
//       Copyright (c) Sysmex. All rights reserved. 
//   </copyright> 
//  -----------------------------------------------------------------------

using System.Collections.Generic;
using CodeAnalysis.Core.Members;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CodeAnalysis.Core.Interfaces
{

    #region Using

    #endregion

    public interface IEvaluatedTypesInfoTable
    {
        #region Public Properties

        /// <summary>
        ///     Gets the well known types.
        /// </summary>
        /// <value>
        ///     The well known types.
        /// </value>
        IReadOnlyList<EvaluatedTypeInfo> InternalTypeInfos { get; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Gets the well known type information.
        /// </summary>
        /// <param name="typeName">Name of the type.</param>
        /// <param name="usingDirectives">The using directives.</param>
        /// <param name="namespaceDeclarations">The namespace declarations.</param>
        /// <returns></returns>
        EvaluatedTypeInfo GetTypeInfo(
            string typeName,
            List<UsingDirectiveSyntax> usingDirectives,
            List<MemberDeclarationSyntax> namespaceDeclarations);

        /// <summary>
        ///     Gets the well known type information.
        /// </summary>
        /// <param name="constructor">The constructor.</param>
        /// <returns></returns>
        EvaluatedTypeInfo GetTypeInfo(ConstructorDeclarationSyntax constructor);

        /// <summary>
        ///     Gets the type information.
        /// </summary>
        /// <param name="typeDeclaration">The type declaration.</param>
        /// <returns></returns>
        EvaluatedTypeInfo GetTypeInfo(BaseTypeDeclarationSyntax typeDeclaration);

        /// <summary>
        ///     Rebuilds the well known methods.
        /// </summary>
        /// <param name="syntaxTrees">The syntax trees.</param>
        void RebuildWellKnownTypesWithMethods(IList<SyntaxTree> syntaxTrees);

        #endregion
    }
}