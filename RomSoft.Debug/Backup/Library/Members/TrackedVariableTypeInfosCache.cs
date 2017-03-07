//  Project              : GLP
//  Module               : RomSoft.Client.Debug.dll
//  File                 : TrackedVariableTypeInfosCache.cs
//  Author               : Alecsandru
//  Last Updated         : 19/12/2015 at 1:36
//  
// 
//  Contains             : Implementation of the TrackedVariableTypeInfosCache.cs class.
//  Classes              : TrackedVariableTypeInfosCache.cs
// 
//  
//  ----------------------------------------------------------------------- 
//   <copyright file="TrackedVariableTypeInfosCache.cs" company="Sysmex"> 
//       Copyright (c) Sysmex. All rights reserved. 
//   </copyright> 
//  -----------------------------------------------------------------------

namespace RomSoft.Client.Debug.Library.Members
{
    #region Using

    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp.Syntax;

    using RomSoft.Client.Debug.Library.Interfaces;

    #endregion

    public class TrackedVariableTypeInfosCache : ITrackedVariableTypeInfosCache
    {
        #region Fields

        private readonly List<TrackedTypeInfo> _externalTypeInfos = new List<TrackedTypeInfo>();

        private readonly List<TrackedTypeInfo> _internalTypeInfos = new List<TrackedTypeInfo>();

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets the external type infos.
        /// </summary>
        /// <value>
        ///     The external type infos.
        /// </value>
        public IReadOnlyList<TrackedTypeInfo> ExternalTypeInfos
        {
            get
            {
                return _externalTypeInfos;
            }
        }

        /// <summary>
        ///     Gets the well known types.
        /// </summary>
        /// <value>
        ///     The well known types.
        /// </value>
        public IReadOnlyList<TrackedTypeInfo> InternalTypeInfos
        {
            get
            {
                return _internalTypeInfos;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Gets the well known type information.
        /// </summary>
        /// <param name="typeName">Name of the type.</param>
        /// <param name="usingDirectives">The using directives.</param>
        /// <param name="namespaceDeclarations">The namespace declarations.</param>
        /// <returns></returns>
        public TrackedTypeInfo GetTypeInfo(
            string typeName,
            List<UsingDirectiveSyntax> usingDirectives,
            List<MemberDeclarationSyntax> namespaceDeclarations)
        {
            var fullNamespace = GetFullTypeNamespace(namespaceDeclarations, typeName);
            var foundType = _internalTypeInfos.FirstOrDefault(typeInfo => typeInfo.FullIdentifierText == fullNamespace);

            if (foundType != null)
            {
                return foundType;
            }

            foreach (var usingDirectiveSyntax in usingDirectives)
            {
                fullNamespace = GetFullTypeNamespace(usingDirectiveSyntax, typeName);
                foundType = _internalTypeInfos.FirstOrDefault(typeInfo => typeInfo.FullIdentifierText == fullNamespace);
                if (foundType != null)
                {
                    return foundType;
                }
            }

            return null;
        }

        /// <summary>
        ///     Gets the well known type information.
        /// </summary>
        /// <param name="constructor">The constructor.</param>
        /// <returns></returns>
        public TrackedTypeInfo GetTypeInfo(ConstructorDeclarationSyntax constructor)
        {
            return GetTypeInfo(constructor.Parent as ClassDeclarationSyntax);
        }

        /// <summary>
        ///     Gets the type information.
        /// </summary>
        /// <param name="typeDeclaration">The type declaration.</param>
        /// <returns></returns>
        public TrackedTypeInfo GetTypeInfo(BaseTypeDeclarationSyntax typeDeclaration)
        {
            var parent = typeDeclaration.Parent;
            List<MemberDeclarationSyntax> namespaces = new List<MemberDeclarationSyntax>();

            while (parent != null)
            {
                if (parent is MemberDeclarationSyntax)
                {
                    var baseTypeDeclarationSyntax = (MemberDeclarationSyntax)parent;
                    namespaces.Add(baseTypeDeclarationSyntax);
                }

                parent = parent.Parent;
            }
            namespaces.Reverse();

            return GetTypeInfo(typeDeclaration.Identifier.ValueText, new List<UsingDirectiveSyntax>(), namespaces);
        }

        /// <summary>
        ///     Rebuilds the well known methods.
        /// </summary>
        /// <param name="syntaxTrees">The syntax trees.</param>
        public void RebuildWellKnownTypesWithMethods(IList<SyntaxTree> syntaxTrees)
        {
            _internalTypeInfos.Clear();

            BuildVariableTypeInfos(syntaxTrees);

            BuildVariableTypeInfosMissingTypeInfos();

            BuildVariableTypeInfosCompleteMembers();
        }

        #endregion

        #region Private Methods and Operators

        private void AddCompilationUnit(
            SyntaxNode syntaxNode,
            TrackedTypeInfo currentTypeInfo,
            List<MemberDeclarationSyntax> namespaceDeclarations,
            List<UsingDirectiveSyntax> usingDirectives)
        {
            var compilationUnitSyntax = (CompilationUnitSyntax)syntaxNode;
            var namespaceDeclarationSyntaxesClone = namespaceDeclarations.ToArray().ToList();
            var usingDirectiveSyntaxesClone = usingDirectives.ToArray().ToList();

            usingDirectiveSyntaxesClone.AddRange(compilationUnitSyntax.Usings);

            foreach (var childNode in compilationUnitSyntax.ChildNodes())
            {
                BuildVariableTypeInfos(
                    childNode,
                    currentTypeInfo,
                    namespaceDeclarationSyntaxesClone,
                    usingDirectiveSyntaxesClone);
            }
        }

        private void AddNamespace(
            SyntaxNode syntaxNode,
            TrackedTypeInfo currentTypeInfo,
            List<MemberDeclarationSyntax> namespaceDeclarations,
            List<UsingDirectiveSyntax> usingDirectives)
        {
            var namespaceDeclarationSyntax = (NamespaceDeclarationSyntax)syntaxNode;
            var namespaceDeclarationSyntaxesClone = namespaceDeclarations.ToArray().ToList();
            var usingDirectiveSyntaxesClone = usingDirectives.ToArray().ToList();

            namespaceDeclarationSyntaxesClone.Add(namespaceDeclarationSyntax);
            usingDirectiveSyntaxesClone.AddRange(namespaceDeclarationSyntax.Usings);

            foreach (var childNode in namespaceDeclarationSyntax.ChildNodes())
            {
                BuildVariableTypeInfos(
                    childNode,
                    currentTypeInfo,
                    namespaceDeclarationSyntaxesClone,
                    usingDirectiveSyntaxesClone);
            }
        }

        private void AddNameToFullNamespace(MemberDeclarationSyntax namespaceDeclaration, StringBuilder fullNamespace)
        {
            if (namespaceDeclaration is ClassDeclarationSyntax)
            {
                var cl = (ClassDeclarationSyntax)namespaceDeclaration;
                fullNamespace.Append(NormalizeName(cl.Identifier.ValueText));
            }
            else if (namespaceDeclaration is NamespaceDeclarationSyntax)
            {
                var nm = (NamespaceDeclarationSyntax)namespaceDeclaration;
                fullNamespace.Append(NormalizeName(nm.Name.GetText().ToString()));
            }
            else if (namespaceDeclaration is StructDeclarationSyntax)
            {
                var st = (StructDeclarationSyntax)namespaceDeclaration;
                fullNamespace.Append(NormalizeName(st.Identifier.ValueText));
            }
            else if (namespaceDeclaration is InterfaceDeclarationSyntax)
            {
                var it = (InterfaceDeclarationSyntax)namespaceDeclaration;
                fullNamespace.Append(NormalizeName(it.Identifier.ValueText));
            }
        }

        private void AddTypeInfo(
            SyntaxNode syntaxNode,
            List<MemberDeclarationSyntax> namespaceDeclarations,
            List<UsingDirectiveSyntax> usingDirectives)
        {
            var typeDeclarationSyntax = (BaseTypeDeclarationSyntax)syntaxNode;
            var fullNamespace = GetFullTypeNamespace(namespaceDeclarations, typeDeclarationSyntax.Identifier.ValueText);

            var trackedVariableTypeInfo = GetTypeInfo(
                typeDeclarationSyntax.Identifier.ValueText,
                usingDirectives,
                namespaceDeclarations);

            if (trackedVariableTypeInfo == null)
            {
                trackedVariableTypeInfo = new TrackedTypeInfo();
                trackedVariableTypeInfo.Declaration = typeDeclarationSyntax;
                trackedVariableTypeInfo.IsValueType = typeDeclarationSyntax is StructDeclarationSyntax;
                trackedVariableTypeInfo.IsReferenceType = typeDeclarationSyntax is ClassDeclarationSyntax;
                trackedVariableTypeInfo.IsInterfaceType = typeDeclarationSyntax is InterfaceDeclarationSyntax;
                trackedVariableTypeInfo.FullIdentifierText = fullNamespace;
                trackedVariableTypeInfo.Identifier = typeDeclarationSyntax.Identifier;
                trackedVariableTypeInfo.IdentifierText = typeDeclarationSyntax.Identifier.ValueText;
            }

            trackedVariableTypeInfo.NamespaceDeclarations.AddRange(namespaceDeclarations);
            trackedVariableTypeInfo.UsingDirectives.AddRange(usingDirectives);

            var namespaceDeclarationSyntaxesClone = namespaceDeclarations.ToArray().ToList();
            var usingDirectiveSyntaxesClone = usingDirectives.ToArray().ToList();

            namespaceDeclarationSyntaxesClone.Add(typeDeclarationSyntax);

            _internalTypeInfos.Add(trackedVariableTypeInfo);

            foreach (var childNode in typeDeclarationSyntax.ChildNodes())
            {
                BuildVariableTypeInfos(
                    childNode,
                    trackedVariableTypeInfo,
                    namespaceDeclarationSyntaxesClone,
                    usingDirectiveSyntaxesClone);
            }
        }

        private void AddTypeInfoConstructor(
            SyntaxNode syntaxNode,
            TrackedTypeInfo currentTypeInfo,
            List<MemberDeclarationSyntax> namespaceDeclarations)
        {
            var constructorDeclarationSyntax = (ConstructorDeclarationSyntax)syntaxNode;
            var fullNamespace = GetFullTypeNamespace(
                namespaceDeclarations,
                constructorDeclarationSyntax.Identifier.ValueText);

            var trackedConstructor = new TrackedConstructor();
            trackedConstructor.Declaration = constructorDeclarationSyntax;
            trackedConstructor.FullIdentifierText = fullNamespace;
            trackedConstructor.Identifier = constructorDeclarationSyntax.Identifier;
            trackedConstructor.IdentifierText = constructorDeclarationSyntax.Identifier.ValueText;
            currentTypeInfo.Constructors.Add(trackedConstructor);

            for (var i = 0; i < constructorDeclarationSyntax.ParameterList.Parameters.Count; i++)
            {
                var trackedMethodParameter = new TrackedMethodParameter();
                trackedMethodParameter.Declaration = constructorDeclarationSyntax.ParameterList.Parameters[i];
                trackedMethodParameter.Index = i;
                trackedMethodParameter.Identifier = constructorDeclarationSyntax.ParameterList.Parameters[i].Identifier;
                trackedMethodParameter.IdentifierText =
                    constructorDeclarationSyntax.ParameterList.Parameters[i].Identifier.ValueText;
                trackedConstructor.Parameters.Add(trackedMethodParameter);
            }
        }

        private void AddTypeInfoField(
            SyntaxNode syntaxNode,
            TrackedTypeInfo currentTypeInfo,
            List<MemberDeclarationSyntax> namespaceDeclarations)
        {
            var fieldDeclarationSyntax = (FieldDeclarationSyntax)syntaxNode;

            foreach (var variableDeclaratorSyntax in fieldDeclarationSyntax.Declaration.Variables)
            {
                var fullNamespace = GetFullTypeNamespace(
                    namespaceDeclarations,
                    variableDeclaratorSyntax.Identifier.ValueText);
                var trackedField = new TrackedField();
                trackedField.Declaration = fieldDeclarationSyntax;
                trackedField.FullIdentifierText = fullNamespace;
                trackedField.Identifier = variableDeclaratorSyntax.Identifier;
                trackedField.IdentifierText = variableDeclaratorSyntax.Identifier.ValueText;
                trackedField.InitializerExpression = variableDeclaratorSyntax.Initializer;
                currentTypeInfo.Fields.Add(trackedField);
            }
        }

        private void AddTypeInfoMethod(
            SyntaxNode syntaxNode,
            TrackedTypeInfo currentTypeInfo,
            List<MemberDeclarationSyntax> namespaceDeclarations)
        {
            var methodDeclarationSyntax = (MethodDeclarationSyntax)syntaxNode;
            var fullNamespace = GetFullTypeNamespace(
                namespaceDeclarations,
                methodDeclarationSyntax.Identifier.ValueText);

            var trackedMethod = new TrackedMethod();
            trackedMethod.Declaration = methodDeclarationSyntax;
            trackedMethod.FullIdentifierText = fullNamespace;
            trackedMethod.Identifier = methodDeclarationSyntax.Identifier;
            trackedMethod.IdentifierText = methodDeclarationSyntax.Identifier.ValueText;
            currentTypeInfo.Methods.Add(trackedMethod);

            for (int i = 0; i < methodDeclarationSyntax.ParameterList.Parameters.Count; i++)
            {
                var trackedMethodParameter = new TrackedMethodParameter();
                trackedMethodParameter.Declaration = methodDeclarationSyntax.ParameterList.Parameters[i];
                trackedMethodParameter.Index = i;
                trackedMethodParameter.Identifier = methodDeclarationSyntax.ParameterList.Parameters[i].Identifier;
                trackedMethodParameter.IdentifierText =
                    methodDeclarationSyntax.ParameterList.Parameters[i].Identifier.ValueText;
                trackedMethod.Parameters.Add(trackedMethodParameter);
            }
        }

        private void AddTypeInfoProperty(
            SyntaxNode syntaxNode,
            TrackedTypeInfo currentTypeInfo,
            List<MemberDeclarationSyntax> namespaceDeclarations)
        {
            var propertyDeclarationSyntax = (PropertyDeclarationSyntax)syntaxNode;
            var fullNamespace = GetFullTypeNamespace(
                namespaceDeclarations,
                propertyDeclarationSyntax.Identifier.ValueText);

            var trackedProperty = new TrackedProperty();

            trackedProperty.Declaration = propertyDeclarationSyntax;
            trackedProperty.FullIdentifierText = fullNamespace;
            trackedProperty.Identifier = propertyDeclarationSyntax.Identifier;
            trackedProperty.IdentifierText = propertyDeclarationSyntax.Identifier.ValueText;
            trackedProperty.IsAutoProperty = true;
            currentTypeInfo.Properties.Add(trackedProperty);

            foreach (var accessorDeclarationSyntax in propertyDeclarationSyntax.AccessorList.Accessors)
            {
                if (accessorDeclarationSyntax.Keyword.ValueText == "get")
                {
                    var trackedPropertyGetAccessor = new TrackedPropertyGetAccessor();
                    trackedPropertyGetAccessor.Declaration = accessorDeclarationSyntax;
                    trackedPropertyGetAccessor.Identifier = propertyDeclarationSyntax.Identifier;
                    trackedPropertyGetAccessor.IdentifierText = propertyDeclarationSyntax.Identifier.ValueText;
                    trackedPropertyGetAccessor.FullIdentifierText = fullNamespace
                                                                    + NormalizeName(
                                                                        "Get"
                                                                        + propertyDeclarationSyntax.Identifier.ValueText);
                    trackedProperty.PropertyGetAccessor = trackedPropertyGetAccessor;

                    if (accessorDeclarationSyntax.Body != null)
                    {
                        trackedProperty.IsAutoProperty = false;
                    }
                    continue;
                }

                if (accessorDeclarationSyntax.Keyword.ValueText == "set")
                {
                    var propertySetAccessor = new TrackedPropertySetAccessor();
                    propertySetAccessor.Declaration = accessorDeclarationSyntax;
                    propertySetAccessor.Identifier = propertyDeclarationSyntax.Identifier;
                    propertySetAccessor.IdentifierText = propertyDeclarationSyntax.Identifier.ValueText;
                    propertySetAccessor.FullIdentifierText = fullNamespace
                                                             + NormalizeName(
                                                                 "Set" + propertyDeclarationSyntax.Identifier.ValueText);
                    trackedProperty.PropertySetAccessor = propertySetAccessor;

                    var trackedMethodParameter = new TrackedMethodParameter();
                    trackedMethodParameter.Index = 0;
                    trackedMethodParameter.IdentifierText = "value";
                    trackedMethodParameter.Declaration = accessorDeclarationSyntax;

                    if (accessorDeclarationSyntax.Body != null)
                    {
                        trackedProperty.IsAutoProperty = false;
                    }
                }
            }
        }

        private void BuildVariableTypeInfos(IList<SyntaxTree> syntaxTrees)
        {
            foreach (var syntaxTree in syntaxTrees)
            {
                BuildVariableTypeInfos(
                    syntaxTree.GetRoot(),
                    null,
                    new List<MemberDeclarationSyntax>(),
                    new List<UsingDirectiveSyntax>());
            }
        }

        private void BuildVariableTypeInfos(
            SyntaxNode syntaxNode,
            TrackedTypeInfo currentTypeInfo,
            List<MemberDeclarationSyntax> namespaceDeclarations,
            List<UsingDirectiveSyntax> usingDirectives)
        {
            if (syntaxNode is BaseTypeDeclarationSyntax)
            {
                AddTypeInfo(syntaxNode, namespaceDeclarations, usingDirectives);

                return;
            }

            if (syntaxNode is MethodDeclarationSyntax && currentTypeInfo != null)
            {
                AddTypeInfoMethod(syntaxNode, currentTypeInfo, namespaceDeclarations);

                return;
            }

            if (syntaxNode is ConstructorDeclarationSyntax && currentTypeInfo != null)
            {
                AddTypeInfoConstructor(syntaxNode, currentTypeInfo, namespaceDeclarations);

                return;
            }

            if (syntaxNode is PropertyDeclarationSyntax && currentTypeInfo != null)
            {
                AddTypeInfoProperty(syntaxNode, currentTypeInfo, namespaceDeclarations);

                return;
            }

            if (syntaxNode is FieldDeclarationSyntax && currentTypeInfo != null)
            {
                AddTypeInfoField(syntaxNode, currentTypeInfo, namespaceDeclarations);

                return;
            }

            if (syntaxNode is NamespaceDeclarationSyntax)
            {
                AddNamespace(syntaxNode, currentTypeInfo, namespaceDeclarations, usingDirectives);

                return;
            }

            if (syntaxNode is CompilationUnitSyntax)
            {
                AddCompilationUnit(syntaxNode, currentTypeInfo, namespaceDeclarations, usingDirectives);
            }
        }

        private void BuildVariableTypeInfosCompleteMembers()
        {
            foreach (var trackedVariableTypeInfo in _internalTypeInfos)
            {
                if (trackedVariableTypeInfo.IsReferenceType)
                {
                    var allTypeInfos = new List<TrackedTypeInfo>();

                    allTypeInfos.Add(trackedVariableTypeInfo);

                    bool foundNewTypes;

                    do
                    {
                        foundNewTypes = false;

                        var tempTypeInfo = new List<TrackedTypeInfo>();

                        foreach (var trackedTypeInfo in allTypeInfos)
                        {
                            foreach (var baseVariableTypeInfo in trackedTypeInfo.BaseTypeInfos)
                            {
                                if (!allTypeInfos.Contains(baseVariableTypeInfo) && baseVariableTypeInfo.IsReferenceType)
                                {
                                    tempTypeInfo.Add(baseVariableTypeInfo);
                                    foundNewTypes = true;
                                }
                            }
                        }

                        allTypeInfos.AddRange(tempTypeInfo);
                    }
                    while (foundNewTypes);

                    foreach (var trackedTypeInfo in allTypeInfos)
                    {
                        trackedVariableTypeInfo.AllMethods.AddRange(trackedTypeInfo.Methods);
                        trackedVariableTypeInfo.AllFields.AddRange(trackedTypeInfo.Fields);
                        trackedVariableTypeInfo.AllProperties.AddRange(trackedTypeInfo.Properties);
                    }
                }
            }
        }

        private void BuildVariableTypeInfosMissingTypeInfos()
        {
            foreach (var trackedVariableTypeInfo in _internalTypeInfos)
            {
                var baseTypeDeclarationSyntax = trackedVariableTypeInfo.Declaration as BaseTypeDeclarationSyntax;

                if (baseTypeDeclarationSyntax != null && baseTypeDeclarationSyntax.BaseList != null)
                {
                    foreach (var baseTypeSyntax in baseTypeDeclarationSyntax.BaseList.Types)
                    {
                        var typeName = baseTypeSyntax.Type.GetText().ToString();

                        var baseTypeInfo = GetTypeInfo(
                            typeName,
                            trackedVariableTypeInfo.UsingDirectives,
                            trackedVariableTypeInfo.NamespaceDeclarations);

                        if (baseTypeInfo != null)
                        {
                            trackedVariableTypeInfo.BaseTypeInfos.Add(baseTypeInfo);
                        }
                    }

                    foreach (var trackedField in trackedVariableTypeInfo.Fields)
                    {
                        var fieldDeclarationSyntax = trackedField.Declaration as FieldDeclarationSyntax;

                        if (fieldDeclarationSyntax != null)
                        {
                            var typeName = fieldDeclarationSyntax.Declaration.Type.GetText().ToString();
                            var wellKnownTypeInfo = GetTypeInfo(
                                typeName,
                                trackedVariableTypeInfo.UsingDirectives,
                                trackedVariableTypeInfo.NamespaceDeclarations);

                            if (wellKnownTypeInfo != null)
                            {
                                trackedField.TypeInfo = wellKnownTypeInfo;
                            }
                        }
                    }

                    foreach (var trackedProperty in trackedVariableTypeInfo.Properties)
                    {
                        var propertyDeclarationSyntax = trackedProperty.Declaration as PropertyDeclarationSyntax;

                        if (propertyDeclarationSyntax != null)
                        {
                            var typeName = propertyDeclarationSyntax.Type.GetText().ToString();
                            var wellKnownTypeInfo = GetTypeInfo(
                                typeName,
                                trackedVariableTypeInfo.UsingDirectives,
                                trackedVariableTypeInfo.NamespaceDeclarations);

                            if (wellKnownTypeInfo != null)
                            {
                                trackedProperty.TypeInfo = wellKnownTypeInfo;

                                if (trackedProperty.PropertySetAccessor != null
                                    && trackedProperty.PropertySetAccessor.Parameters.Count > 0)
                                {
                                    trackedProperty.PropertySetAccessor.Parameters[0].TypeInfo = wellKnownTypeInfo;
                                }
                            }
                        }
                    }

                    foreach (var trackedMethod in trackedVariableTypeInfo.Methods)
                    {
                        foreach (var trackedMethodParameter in trackedMethod.Parameters)
                        {
                            var parameterSyntax = trackedMethodParameter.Declaration as ParameterSyntax;

                            if (parameterSyntax != null)
                            {
                                var typeName = parameterSyntax.Type.GetText().ToString();
                                var wellKnownTypeInfo = GetTypeInfo(
                                    typeName,
                                    trackedVariableTypeInfo.UsingDirectives,
                                    trackedVariableTypeInfo.NamespaceDeclarations);

                                if (wellKnownTypeInfo != null)
                                {
                                    trackedMethodParameter.TypeInfo = wellKnownTypeInfo;
                                }
                            }
                        }
                    }
                }
            }
        }

        private string GetFullTypeNamespace(
            List<MemberDeclarationSyntax> namespaceDeclarations,
            string typeDeclarationName)
        {
            var fullNamespace = new StringBuilder();

            foreach (var namespaceDeclaration in namespaceDeclarations)
            {
                AddNameToFullNamespace(namespaceDeclaration, fullNamespace);
            }

            fullNamespace.Append(NormalizeName(typeDeclarationName));

            if (fullNamespace.Length > 0)
            {
                fullNamespace.Remove(0, 1);
            }
            return fullNamespace.ToString();
        }

        private string GetFullTypeNamespace(UsingDirectiveSyntax usingDirective, string typeDeclarationName)
        {
            var fullNamespace = new StringBuilder();

            fullNamespace.Append(usingDirective.Name);
            fullNamespace.Append(NormalizeName(typeDeclarationName));

            return fullNamespace.ToString();
        }

        private string NormalizeName(string name)
        {
            return name.Replace(" ", "").Replace("\r", "").Replace("\n", "");
        }

        #endregion
    }
}