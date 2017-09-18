namespace CodeEvaluator.Evaluation.Interfaces
{
    using System.Collections.Generic;

    using CodeEvaluator.Dto;
    using CodeEvaluator.Evaluation.Members;

    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp.Syntax;

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
        IReadOnlyList<EvaluatedTypeInfo> EvaluatedTypeInfos { get; }

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

        EvaluatedTypeInfo GetTypeInfo(string typeName, EvaluatedTypeInfo evaluatedTypeInfo);

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

        void ClearTypeInfos();

        void RebuildExternalTypeInfos(IList<EvaluatedTypeInfoDto> externalTypeInfos);

        #endregion
    }
}