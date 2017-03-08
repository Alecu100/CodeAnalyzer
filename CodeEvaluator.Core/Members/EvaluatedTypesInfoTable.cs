//  Project              : GLP
//  Module               : RomSoft.Client.Debug.dll
//  File                 : EvaluatedTypesInfoTable.cs
//  Author               : Alecsandru
//  Last Updated         : 19/12/2015 at 1:36
//  
// 
//  Contains             : Implementation of the EvaluatedTypesInfoTable.cs class.
//  Classes              : EvaluatedTypesInfoTable.cs
// 
//  
//  ----------------------------------------------------------------------- 
//   <copyright file="EvaluatedTypesInfoTable.cs" company="Sysmex"> 
//       Copyright (c) Sysmex. All rights reserved. 
//   </copyright> 
//  -----------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using System.Text;
using CodeAnalysis.Core.Interfaces;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CodeAnalysis.Core.Members
{

    #region Using

    #endregion

    public class EvaluatedTypesInfoTable : IEvaluatedTypesInfoTable
    {
        #region Fields

        private readonly List<EvaluatedTypeInfo> _internalTypeInfos = new List<EvaluatedTypeInfo>();

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets the well known types.
        /// </summary>
        /// <value>
        ///     The well known types.
        /// </value>
        public IReadOnlyList<EvaluatedTypeInfo> InternalTypeInfos
        {
            get { return _internalTypeInfos; }
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
        public EvaluatedTypeInfo GetTypeInfo(
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
        public EvaluatedTypeInfo GetTypeInfo(ConstructorDeclarationSyntax constructor)
        {
            return GetTypeInfo(constructor.Parent as ClassDeclarationSyntax);
        }

        /// <summary>
        ///     Gets the type information.
        /// </summary>
        /// <param name="typeDeclaration">The type declaration.</param>
        /// <returns></returns>
        public EvaluatedTypeInfo GetTypeInfo(BaseTypeDeclarationSyntax typeDeclaration)
        {
            var parent = typeDeclaration.Parent;
            var namespaces = new List<MemberDeclarationSyntax>();

            while (parent != null)
            {
                if (parent is MemberDeclarationSyntax)
                {
                    var baseTypeDeclarationSyntax = (MemberDeclarationSyntax) parent;
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
            EvaluatedTypeInfo currentTypeInfo,
            List<MemberDeclarationSyntax> namespaceDeclarations,
            List<UsingDirectiveSyntax> usingDirectives)
        {
            var compilationUnitSyntax = (CompilationUnitSyntax) syntaxNode;
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
            EvaluatedTypeInfo currentTypeInfo,
            List<MemberDeclarationSyntax> namespaceDeclarations,
            List<UsingDirectiveSyntax> usingDirectives)
        {
            var namespaceDeclarationSyntax = (NamespaceDeclarationSyntax) syntaxNode;
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
                var cl = (ClassDeclarationSyntax) namespaceDeclaration;
                fullNamespace.Append(NormalizeName(cl.Identifier.ValueText));
            }
            else if (namespaceDeclaration is NamespaceDeclarationSyntax)
            {
                var nm = (NamespaceDeclarationSyntax) namespaceDeclaration;
                fullNamespace.Append(NormalizeName(nm.Name.GetText().ToString()));
            }
            else if (namespaceDeclaration is StructDeclarationSyntax)
            {
                var st = (StructDeclarationSyntax) namespaceDeclaration;
                fullNamespace.Append(NormalizeName(st.Identifier.ValueText));
            }
            else if (namespaceDeclaration is InterfaceDeclarationSyntax)
            {
                var it = (InterfaceDeclarationSyntax) namespaceDeclaration;
                fullNamespace.Append(NormalizeName(it.Identifier.ValueText));
            }
        }

        private void AddTypeInfo(
            SyntaxNode syntaxNode,
            List<MemberDeclarationSyntax> namespaceDeclarations,
            List<UsingDirectiveSyntax> usingDirectives)
        {
            var typeDeclarationSyntax = (BaseTypeDeclarationSyntax) syntaxNode;
            var fullNamespace = GetFullTypeNamespace(namespaceDeclarations, typeDeclarationSyntax.Identifier.ValueText);

            var trackedVariableTypeInfo = GetTypeInfo(
                typeDeclarationSyntax.Identifier.ValueText,
                usingDirectives,
                namespaceDeclarations);

            if (trackedVariableTypeInfo == null)
            {
                trackedVariableTypeInfo = new EvaluatedTypeInfo();
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
            EvaluatedTypeInfo currentTypeInfo,
            List<MemberDeclarationSyntax> namespaceDeclarations)
        {
            var constructorDeclarationSyntax = (ConstructorDeclarationSyntax) syntaxNode;
            var fullNamespace = GetFullTypeNamespace(
                namespaceDeclarations,
                constructorDeclarationSyntax.Identifier.ValueText);

            var trackedConstructor = new EvaluatedConstructor();
            trackedConstructor.Declaration = constructorDeclarationSyntax;
            trackedConstructor.FullIdentifierText = fullNamespace;
            trackedConstructor.Identifier = constructorDeclarationSyntax.Identifier;
            trackedConstructor.IdentifierText = constructorDeclarationSyntax.Identifier.ValueText;
            currentTypeInfo.Constructors.Add(trackedConstructor);

            for (var i = 0; i < constructorDeclarationSyntax.ParameterList.Parameters.Count; i++)
            {
                var trackedMethodParameter = new EvaluatedMethodParameter();
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
            EvaluatedTypeInfo currentTypeInfo,
            List<MemberDeclarationSyntax> namespaceDeclarations)
        {
            var fieldDeclarationSyntax = (FieldDeclarationSyntax) syntaxNode;

            foreach (var variableDeclaratorSyntax in fieldDeclarationSyntax.Declaration.Variables)
            {
                var fullNamespace = GetFullTypeNamespace(
                    namespaceDeclarations,
                    variableDeclaratorSyntax.Identifier.ValueText);
                var trackedField = new EvaluatedField();
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
            EvaluatedTypeInfo currentTypeInfo,
            List<MemberDeclarationSyntax> namespaceDeclarations)
        {
            var methodDeclarationSyntax = (MethodDeclarationSyntax) syntaxNode;
            var fullNamespace = GetFullTypeNamespace(
                namespaceDeclarations,
                methodDeclarationSyntax.Identifier.ValueText);

            var trackedMethod = new EvaluatedMethod();
            trackedMethod.Declaration = methodDeclarationSyntax;
            trackedMethod.FullIdentifierText = fullNamespace;
            trackedMethod.Identifier = methodDeclarationSyntax.Identifier;
            trackedMethod.IdentifierText = methodDeclarationSyntax.Identifier.ValueText;
            currentTypeInfo.Methods.Add(trackedMethod);

            for (var i = 0; i < methodDeclarationSyntax.ParameterList.Parameters.Count; i++)
            {
                var trackedMethodParameter = new EvaluatedMethodParameter();
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
            EvaluatedTypeInfo currentTypeInfo,
            List<MemberDeclarationSyntax> namespaceDeclarations)
        {
            var propertyDeclarationSyntax = (PropertyDeclarationSyntax) syntaxNode;
            var fullNamespace = GetFullTypeNamespace(
                namespaceDeclarations,
                propertyDeclarationSyntax.Identifier.ValueText);

            var trackedProperty = new EvaluatedProperty();

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
                    var trackedPropertyGetAccessor = new EvaluatedPropertyGetAccessor();
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
                    var propertySetAccessor = new EvaluatedPropertySetAccessor();
                    propertySetAccessor.Declaration = accessorDeclarationSyntax;
                    propertySetAccessor.Identifier = propertyDeclarationSyntax.Identifier;
                    propertySetAccessor.IdentifierText = propertyDeclarationSyntax.Identifier.ValueText;
                    propertySetAccessor.FullIdentifierText = fullNamespace
                                                             + NormalizeName(
                                                                 "Set" + propertyDeclarationSyntax.Identifier.ValueText);
                    trackedProperty.PropertySetAccessor = propertySetAccessor;

                    var trackedMethodParameter = new EvaluatedMethodParameter();
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
            EvaluatedTypeInfo currentTypeInfo,
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
                    var allTypeInfos = new List<EvaluatedTypeInfo>();

                    allTypeInfos.Add(trackedVariableTypeInfo);

                    bool foundNewTypes;

                    do
                    {
                        foundNewTypes = false;

                        var tempTypeInfo = new List<EvaluatedTypeInfo>();

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
                    } while (foundNewTypes);

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