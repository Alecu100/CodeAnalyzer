namespace CodeEvaluator.Evaluation.Members
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using CodeEvaluator.Dto;
    using CodeEvaluator.Evaluation.Extensions;
    using CodeEvaluator.Evaluation.Interfaces;

    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp.Syntax;

    using StructureMap;

    #region Using

    #endregion

    public class EvaluatedTypesInfoTable : IEvaluatedTypesInfoTable
    {
        #region Public Properties

        /// <summary>
        ///     Gets the well known types.
        /// </summary>
        /// <value>
        ///     The well known types.
        /// </value>
        public IReadOnlyList<EvaluatedTypeInfo> EvaluatedTypeInfos => _evaluatedTypeInfos;

        #endregion

        #region SpecificFields

        private readonly List<EvaluatedTypeInfo> _evaluatedTypeInfos = new List<EvaluatedTypeInfo>();

        private readonly Dictionary<string, EvaluatedTypeInfo> _evaluatedTypeInfosHashMap =
            new Dictionary<string, EvaluatedTypeInfo>();

        private IKeywordToTypeInfoRemapper _keywordToTypeInfoRemapper;

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

            if (_evaluatedTypeInfosHashMap.ContainsKey(fullNamespace))
            {
                return _evaluatedTypeInfosHashMap[fullNamespace];
            }

            foreach (var usingDirectiveSyntax in usingDirectives)
            {
                fullNamespace = GetFullTypeNamespace(usingDirectiveSyntax, typeName);

                if (_evaluatedTypeInfosHashMap.ContainsKey(fullNamespace))
                {
                    return _evaluatedTypeInfosHashMap[fullNamespace];
                }
            }

            return null;
        }

        public EvaluatedTypeInfo GetTypeInfo(string typeName, EvaluatedTypeInfo evaluatedTypeInfo)
        {
            return GetTypeInfo(typeName, evaluatedTypeInfo.UsingDirectives, evaluatedTypeInfo.NamespaceDeclarations);
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
                    var baseTypeDeclarationSyntax = (MemberDeclarationSyntax)parent;
                    namespaces.Add(baseTypeDeclarationSyntax);
                }

                parent = parent.Parent;
            }
            namespaces.Reverse();

            return GetTypeInfo(typeDeclaration.Identifier.ValueText, new List<UsingDirectiveSyntax>(), namespaces);
        }

        public EvaluatedTypeInfo GetTypeInfo(string typeName)
        {
            if (_evaluatedTypeInfosHashMap.ContainsKey(typeName)) return _evaluatedTypeInfosHashMap[typeName];

            return null;
        }

        /// <summary>
        ///     Rebuilds the well known methods.
        /// </summary>
        /// <param name="syntaxTrees">The syntax trees.</param>
        public void RebuildWellKnownTypesWithMethods(IList<SyntaxTree> syntaxTrees)
        {
            BuildEvaluatedTypeInfos(syntaxTrees);

            BuildEvaluatedTypeInfosMemberTypeInfos();

            BuildEvaluatedTypeInfosInheritedMembers();

            BuildEvaluatedTypeInfosStaticSharedObjects();
        }

        public void ClearTypeInfos()
        {
            _evaluatedTypeInfos.Clear();
            _evaluatedTypeInfosHashMap.Clear();
        }

        public void RebuildExternalTypeInfos(IList<EvaluatedTypeInfoDto> externalTypeInfos)
        {
            //_evaluatedTypeInfos.AddRange(externalTypeInfos);

            var evaluatedTypeInfosDtosMappings = new Dictionary<EvaluatedTypeInfoDto, EvaluatedTypeInfo>();

            var evaluatedMethodMappings = new Dictionary<EvaluatedMethod, EvaluatedMethodDto>();
            var evaluatedConstructorsMappings = new Dictionary<EvaluatedConstructor, EvaluatedMethodDto>();
            var evaluatedFieldMappings = new Dictionary<EvaluatedField, EvaluatedTypedMemberDto>();
            var evaluatedPropertyMappings = new Dictionary<EvaluatedProperty, EvaluatedTypedMemberDto>();
            var evaluatedMethodParameterMappings = new Dictionary<EvaluatedMethodParameter, EvaluatedTypedMemberDto>();

            foreach (var evaluatedTypeInfoDto in externalTypeInfos)
            {
                var evaluatedTypeInfo = new EvaluatedTypeInfo();

                evaluatedTypeInfo.IsValueType = evaluatedTypeInfoDto.IsValueType;
                evaluatedTypeInfo.IsReferenceType = evaluatedTypeInfoDto.IsReferenceType;
                evaluatedTypeInfo.IsInterfaceType = evaluatedTypeInfoDto.IsInterfaceType;
                evaluatedTypeInfo.FullIdentifierText = evaluatedTypeInfoDto.FullIdentifierText;
                evaluatedTypeInfo.MemberFlags = (EMemberFlags)evaluatedTypeInfoDto.MemberFlags;

                foreach (var evaluatedMethodDto in evaluatedTypeInfoDto.Methods)
                {
                    var evaluatedMethod = new EvaluatedMethod();

                    evaluatedMethod.IdentifierText = evaluatedMethodDto.IdentifierText;
                    evaluatedMethod.FullIdentifierText = evaluatedMethodDto.FullIdentifierText;
                    evaluatedMethod.MemberFlags = (EMemberFlags)evaluatedTypeInfoDto.MemberFlags;

                    for (var i = 0; i < evaluatedMethodDto.Parameters.Count; i++)
                    {
                        var evaluatedTypedMemberDto = evaluatedMethodDto.Parameters[i];

                        var evaluatedMethodParameter = new EvaluatedMethodParameter();

                        evaluatedMethodParameter.Index = i;
                        evaluatedMethodParameter.MemberFlags = (EMemberFlags)evaluatedTypedMemberDto.MemberFlags;
                        evaluatedMethodParameter.IdentifierText = evaluatedTypedMemberDto.IdentifierText;
                        evaluatedMethodParameter.FullIdentifierText = evaluatedTypedMemberDto.FullIdentifierText;

                        evaluatedMethod.Parameters.Add(evaluatedMethodParameter);

                        evaluatedMethodParameterMappings[evaluatedMethodParameter] = evaluatedTypedMemberDto;
                    }

                    evaluatedTypeInfo.AccesibleMethods.Add(evaluatedMethod);

                    evaluatedMethodMappings[evaluatedMethod] = evaluatedMethodDto;
                }

                foreach (var evaluatedTypedMemberDto in evaluatedTypeInfoDto.Fields)
                {
                    var evaluatedField = new EvaluatedField();

                    evaluatedField.IdentifierText = evaluatedTypedMemberDto.IdentifierText;
                    evaluatedField.FullIdentifierText = evaluatedTypeInfoDto.FullIdentifierText;
                    evaluatedField.MemberFlags = (EMemberFlags)evaluatedTypeInfoDto.MemberFlags;

                    evaluatedTypeInfo.AccesibleFields.Add(evaluatedField);

                    evaluatedFieldMappings[evaluatedField] = evaluatedTypedMemberDto;
                }

                foreach (var evaluatedMethodDto in evaluatedTypeInfoDto.Constructors)
                {
                    var evaluatedConstructor = new EvaluatedConstructor();

                    evaluatedConstructor.IdentifierText = evaluatedMethodDto.IdentifierText;
                    evaluatedConstructor.FullIdentifierText = evaluatedMethodDto.FullIdentifierText;
                    evaluatedConstructor.MemberFlags = (EMemberFlags)evaluatedTypeInfoDto.MemberFlags;

                    for (var i = 0; i < evaluatedMethodDto.Parameters.Count; i++)
                    {
                        var evaluatedTypedMemberDto = evaluatedMethodDto.Parameters[i];

                        var evaluatedMethodParameter = new EvaluatedMethodParameter();

                        evaluatedMethodParameter.Index = i;
                        evaluatedMethodParameter.MemberFlags = (EMemberFlags)evaluatedTypedMemberDto.MemberFlags;
                        evaluatedMethodParameter.IdentifierText = evaluatedTypedMemberDto.IdentifierText;
                        evaluatedMethodParameter.FullIdentifierText = evaluatedTypedMemberDto.FullIdentifierText;

                        evaluatedConstructor.Parameters.Add(evaluatedMethodParameter);
                    }

                    evaluatedTypeInfo.Constructors.Add(evaluatedConstructor);

                    evaluatedConstructorsMappings[evaluatedConstructor] = evaluatedMethodDto;
                }

                foreach (var evaluatedPropertyDto in evaluatedTypeInfoDto.Properties)
                {
                    var evaluatedProperty = new EvaluatedProperty();

                    evaluatedProperty.IdentifierText = evaluatedPropertyDto.IdentifierText;
                    evaluatedProperty.FullIdentifierText = evaluatedPropertyDto.FullIdentifierText;
                    evaluatedProperty.MemberFlags = (EMemberFlags)evaluatedPropertyDto.MemberFlags;

                    if (evaluatedPropertyDto.Getter != null)
                    {
                        var evaluatedPropertyGetAccessor = new EvaluatedPropertyGetAccessor();

                        evaluatedPropertyGetAccessor.MemberFlags = (EMemberFlags)evaluatedPropertyDto.Getter.MemberFlags;
                        evaluatedPropertyGetAccessor.IdentifierText = evaluatedPropertyDto.Getter.IdentifierText;

                        evaluatedProperty.PropertyGetAccessor = evaluatedPropertyGetAccessor;
                    }

                    if (evaluatedPropertyDto.Setter != null)
                    {
                        var evaluatedPropertySetAccessor = new EvaluatedPropertySetAccessor();

                        evaluatedPropertySetAccessor.MemberFlags = (EMemberFlags)evaluatedPropertyDto.Setter.MemberFlags;
                        evaluatedPropertySetAccessor.IdentifierText = evaluatedPropertyDto.Setter.IdentifierText;

                        var evaluatedMethodParameter = new EvaluatedMethodParameter();

                        evaluatedMethodParameter.Index = 0;
                        evaluatedMethodParameter.MemberFlags =
                            (EMemberFlags)evaluatedPropertyDto.Setter.Parameters[0].MemberFlags;

                        evaluatedPropertySetAccessor.Parameters.Add(evaluatedMethodParameter);

                        evaluatedMethodParameterMappings[evaluatedMethodParameter] =
                            evaluatedPropertyDto.Setter.Parameters[0];

                        evaluatedProperty.PropertySetAccessor = evaluatedPropertySetAccessor;
                    }

                    evaluatedTypeInfo.AccesibleProperties.Add(evaluatedProperty);

                    evaluatedPropertyMappings[evaluatedProperty] = evaluatedPropertyDto;
                }

                evaluatedTypeInfosDtosMappings[evaluatedTypeInfoDto] = evaluatedTypeInfo;

                _evaluatedTypeInfos.Add(evaluatedTypeInfo);
                _evaluatedTypeInfosHashMap[evaluatedTypeInfo.FullIdentifierText] = evaluatedTypeInfo;
            }

            foreach (var evaluatedTypeInfo in _evaluatedTypeInfos)
            {
                foreach (var accesibleField in evaluatedTypeInfo.AccesibleFields)
                    accesibleField.TypeInfo =
                        evaluatedTypeInfosDtosMappings[evaluatedFieldMappings[accesibleField].TypeInfo];

                foreach (var accesibleProperty in evaluatedTypeInfo.AccesibleProperties)
                {
                    accesibleProperty.TypeInfo =
                        evaluatedTypeInfosDtosMappings[evaluatedPropertyMappings[accesibleProperty].TypeInfo];

                    if (accesibleProperty.PropertySetAccessor != null)
                        accesibleProperty.PropertySetAccessor.Parameters[0].TypeInfo =
                            evaluatedTypeInfosDtosMappings[
                                evaluatedMethodParameterMappings[accesibleProperty.PropertySetAccessor.Parameters[0]]
                                    .TypeInfo];
                }

                foreach (var accesibleMethod in evaluatedTypeInfo.AccesibleMethods)
                    foreach (var evaluatedMethodParameter in accesibleMethod.Parameters)
                        evaluatedMethodParameter.TypeInfo =
                            evaluatedTypeInfosDtosMappings[
                                evaluatedMethodParameterMappings[evaluatedMethodParameter].TypeInfo];

                foreach (var evaluatedConstructor in evaluatedTypeInfo.Constructors)
                    foreach (var evaluatedMethodParameter in evaluatedConstructor.Parameters)
                        evaluatedMethodParameter.TypeInfo =
                            evaluatedTypeInfosDtosMappings[
                                evaluatedMethodParameterMappings[evaluatedMethodParameter].TypeInfo];
            }

            foreach (var evaluatedTypeInfoDto in externalTypeInfos)
            {
                var evaluatedTypeInfosDtosMapping = evaluatedTypeInfosDtosMappings[evaluatedTypeInfoDto];

                foreach (var baseTypeInfo in evaluatedTypeInfoDto.BaseTypeInfos) evaluatedTypeInfosDtosMapping.BaseTypeInfos.Add(evaluatedTypeInfosDtosMappings[baseTypeInfo]);
            }
        }

        private void BuildEvaluatedTypeInfosStaticSharedObjects()
        {
            foreach (var evaluatedTypeInfo in _evaluatedTypeInfos.Where(typeInfo => typeInfo.IsWellKnown()))
                foreach (var evaluatedField in evaluatedTypeInfo.SpecificFields)
                    if ((evaluatedField.MemberFlags & EMemberFlags.Static) != 0)
                    {
                        var evaluatedObjectReference = new EvaluatedObjectDirectReference();

                        evaluatedObjectReference.Declaration = evaluatedField.Declaration;
                        evaluatedObjectReference.TypeInfo = evaluatedField.TypeInfo;
                        evaluatedObjectReference.Identifier = evaluatedField.Identifier;
                        evaluatedObjectReference.IdentifierText = evaluatedField.IdentifierText;
                        evaluatedObjectReference.FullIdentifierText = evaluatedField.FullIdentifierText;

                        evaluatedTypeInfo.SharedStaticObject.ModifiableFields.Add(evaluatedObjectReference);
                    }

            foreach (var evaluatedTypeInfo in _evaluatedTypeInfos.Where(typeInfo => typeInfo.IsWellKnown()))
            {
                var baseType =
                    evaluatedTypeInfo.BaseTypeInfos.FirstOrDefault(
                        typeInfo => typeInfo.IsReferenceType && !typeInfo.IsInterfaceType);

                while (baseType != null)
                {
                    evaluatedTypeInfo.SharedStaticObject.ModifiableFields.AddRange(baseType.SharedStaticObject.Fields);

                    var newBaseType =
                        baseType.BaseTypeInfos.FirstOrDefault(
                            typeInfo => typeInfo.IsReferenceType && !typeInfo.IsInterfaceType);

                    if (newBaseType != null && newBaseType != baseType)
                    {
                        baseType = newBaseType;
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }

        private void LoadKeywordToTypeInfoRemapperIfNeeded()
        {
            if (_keywordToTypeInfoRemapper == null)
            {
                _keywordToTypeInfoRemapper = ObjectFactory.GetInstance<IKeywordToTypeInfoRemapper>();
            }
        }

        #endregion

        #region Private Methods and Operators

        private void AddCompilationUnit(
            SyntaxNode syntaxNode,
            EvaluatedTypeInfo currentTypeInfo,
            List<MemberDeclarationSyntax> namespaceDeclarations,
            List<UsingDirectiveSyntax> usingDirectives)
        {
            var compilationUnitSyntax = (CompilationUnitSyntax)syntaxNode;
            var namespaceDeclarationSyntaxesClone = namespaceDeclarations.ToArray().ToList();
            var usingDirectiveSyntaxesClone = usingDirectives.ToArray().ToList();

            usingDirectiveSyntaxesClone.AddRange(compilationUnitSyntax.Usings);

            foreach (var childNode in compilationUnitSyntax.ChildNodes())
                BuildEvaluatedTypeInfos(
                    childNode,
                    currentTypeInfo,
                    namespaceDeclarationSyntaxesClone,
                    usingDirectiveSyntaxesClone);
        }

        private void AddNamespace(
            SyntaxNode syntaxNode,
            EvaluatedTypeInfo currentTypeInfo,
            List<MemberDeclarationSyntax> namespaceDeclarations,
            List<UsingDirectiveSyntax> usingDirectives)
        {
            var namespaceDeclarationSyntax = (NamespaceDeclarationSyntax)syntaxNode;
            var namespaceDeclarationSyntaxesClone = namespaceDeclarations.ToArray().ToList();
            var usingDirectiveSyntaxesClone = usingDirectives.ToArray().ToList();

            namespaceDeclarationSyntaxesClone.Add(namespaceDeclarationSyntax);
            usingDirectiveSyntaxesClone.AddRange(namespaceDeclarationSyntax.Usings);

            foreach (var childNode in namespaceDeclarationSyntax.ChildNodes())
                BuildEvaluatedTypeInfos(
                    childNode,
                    currentTypeInfo,
                    namespaceDeclarationSyntaxesClone,
                    usingDirectiveSyntaxesClone);
        }

        private void AddNameToFullNamespace(MemberDeclarationSyntax namespaceDeclaration, StringBuilder fullNamespace)
        {
            fullNamespace.Append(".");

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

        private void AddMemberFlagsToMember(EvaluatedMember member, SyntaxTokenList modifierList)
        {
            foreach (var modifier in modifierList)
                if (modifier.ValueText == "public") member.MemberFlags |= EMemberFlags.Public;
                else if (modifier.ValueText == "protected") member.MemberFlags |= EMemberFlags.Protected;
                else if (modifier.ValueText == "private") member.MemberFlags |= EMemberFlags.Private;
                else if (modifier.ValueText == "static") member.MemberFlags |= EMemberFlags.Static;
                else if (modifier.ValueText == "virtual") member.MemberFlags |= EMemberFlags.Virtual;
                else if (modifier.ValueText == "override") member.MemberFlags |= EMemberFlags.Override;
                else if (modifier.ValueText == "new") member.MemberFlags |= EMemberFlags.New;
                else if (modifier.ValueText == "abstract") member.MemberFlags |= EMemberFlags.Abstract;
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

            if (!_evaluatedTypeInfosHashMap.ContainsKey(fullNamespace))
            {
                trackedVariableTypeInfo = new EvaluatedTypeInfo();
                trackedVariableTypeInfo.Declaration = typeDeclarationSyntax;
                trackedVariableTypeInfo.IsValueType = typeDeclarationSyntax is StructDeclarationSyntax;
                trackedVariableTypeInfo.IsReferenceType = typeDeclarationSyntax is ClassDeclarationSyntax;
                trackedVariableTypeInfo.IsInterfaceType = typeDeclarationSyntax is InterfaceDeclarationSyntax;
                trackedVariableTypeInfo.FullIdentifierText = fullNamespace;
                trackedVariableTypeInfo.Identifier = typeDeclarationSyntax.Identifier;
                trackedVariableTypeInfo.IdentifierText = typeDeclarationSyntax.Identifier.ValueText;

                AddMemberFlagsToMember(trackedVariableTypeInfo, typeDeclarationSyntax.Modifiers);
            }

            trackedVariableTypeInfo.NamespaceDeclarations.AddRange(namespaceDeclarations);
            trackedVariableTypeInfo.UsingDirectives.AddRange(usingDirectives);

            var namespaceDeclarationSyntaxesClone = namespaceDeclarations.ToArray().ToList();
            var usingDirectiveSyntaxesClone = usingDirectives.ToArray().ToList();

            namespaceDeclarationSyntaxesClone.Add(typeDeclarationSyntax);

            if (!_evaluatedTypeInfosHashMap.ContainsKey(fullNamespace))
            {
                _evaluatedTypeInfos.Add(trackedVariableTypeInfo);
                _evaluatedTypeInfosHashMap[fullNamespace] = trackedVariableTypeInfo;
            }

            foreach (var childNode in typeDeclarationSyntax.ChildNodes())
                BuildEvaluatedTypeInfos(
                    childNode,
                    trackedVariableTypeInfo,
                    namespaceDeclarationSyntaxesClone,
                    usingDirectiveSyntaxesClone);
        }

        private void AddTypeInfoConstructor(
            SyntaxNode syntaxNode,
            EvaluatedTypeInfo currentTypeInfo,
            List<MemberDeclarationSyntax> namespaceDeclarations)
        {
            var constructorDeclarationSyntax = (ConstructorDeclarationSyntax)syntaxNode;
            var fullNamespace = GetFullTypeNamespace(
                namespaceDeclarations,
                constructorDeclarationSyntax.Identifier.ValueText);

            var evaluatedConstructor = new EvaluatedConstructor();
            evaluatedConstructor.Declaration = constructorDeclarationSyntax;
            evaluatedConstructor.FullIdentifierText = fullNamespace;
            evaluatedConstructor.Identifier = constructorDeclarationSyntax.Identifier;
            evaluatedConstructor.IdentifierText = constructorDeclarationSyntax.Identifier.ValueText;

            AddMemberFlagsToMember(evaluatedConstructor, constructorDeclarationSyntax.Modifiers);

            currentTypeInfo.Constructors.Add(evaluatedConstructor);

            for (var i = 0; i < constructorDeclarationSyntax.ParameterList.Parameters.Count; i++)
            {
                var evaluatedMethodParameter = new EvaluatedMethodParameter();
                evaluatedMethodParameter.Declaration = constructorDeclarationSyntax.ParameterList.Parameters[i];
                evaluatedMethodParameter.Index = i;
                evaluatedMethodParameter.Identifier =
                    constructorDeclarationSyntax.ParameterList.Parameters[i].Identifier;
                evaluatedMethodParameter.IdentifierText =
                    constructorDeclarationSyntax.ParameterList.Parameters[i].Identifier.ValueText;
                evaluatedConstructor.Parameters.Add(evaluatedMethodParameter);
            }
        }

        private void AddTypeInfoField(
            SyntaxNode syntaxNode,
            EvaluatedTypeInfo currentTypeInfo,
            List<MemberDeclarationSyntax> namespaceDeclarations)
        {
            var fieldDeclarationSyntax = (FieldDeclarationSyntax)syntaxNode;

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

                AddMemberFlagsToMember(trackedField, fieldDeclarationSyntax.Modifiers);

                currentTypeInfo.SpecificFields.Add(trackedField);
            }
        }

        private void AddTypeInfoMethod(
            SyntaxNode syntaxNode,
            EvaluatedTypeInfo currentTypeInfo,
            List<MemberDeclarationSyntax> namespaceDeclarations)
        {
            var methodDeclarationSyntax = (MethodDeclarationSyntax)syntaxNode;
            var fullNamespace = GetFullTypeNamespace(
                namespaceDeclarations,
                methodDeclarationSyntax.Identifier.ValueText);

            var evaluatedMethod = new EvaluatedMethod();
            evaluatedMethod.Declaration = methodDeclarationSyntax;
            evaluatedMethod.FullIdentifierText = fullNamespace;
            evaluatedMethod.Identifier = methodDeclarationSyntax.Identifier;
            evaluatedMethod.IdentifierText = methodDeclarationSyntax.Identifier.ValueText;

            AddMemberFlagsToMember(evaluatedMethod, methodDeclarationSyntax.Modifiers);

            currentTypeInfo.SpecificMethods.Add(evaluatedMethod);

            for (var i = 0; i < methodDeclarationSyntax.ParameterList.Parameters.Count; i++)
            {
                var evaluatedMethodParameter = new EvaluatedMethodParameter();
                var parameterSyntax = methodDeclarationSyntax.ParameterList.Parameters[i];

                evaluatedMethodParameter.Declaration = parameterSyntax;
                evaluatedMethodParameter.Index = i;
                evaluatedMethodParameter.Identifier = parameterSyntax.Identifier;
                evaluatedMethodParameter.HasDefault = parameterSyntax.Default != null;
                evaluatedMethodParameter.IdentifierText = parameterSyntax.Identifier.ValueText;

                evaluatedMethod.Parameters.Add(evaluatedMethodParameter);
            }
        }

        private void AddTypeInfoProperty(
            SyntaxNode syntaxNode,
            EvaluatedTypeInfo currentTypeInfo,
            List<MemberDeclarationSyntax> namespaceDeclarations)
        {
            var propertyDeclarationSyntax = (PropertyDeclarationSyntax)syntaxNode;
            var fullNamespace = GetFullTypeNamespace(
                namespaceDeclarations,
                propertyDeclarationSyntax.Identifier.ValueText);

            var evaluatedProperty = new EvaluatedProperty();

            evaluatedProperty.Declaration = propertyDeclarationSyntax;
            evaluatedProperty.FullIdentifierText = fullNamespace;
            evaluatedProperty.Identifier = propertyDeclarationSyntax.Identifier;
            evaluatedProperty.IdentifierText = propertyDeclarationSyntax.Identifier.ValueText;
            evaluatedProperty.IsAutoProperty = true;

            AddMemberFlagsToMember(evaluatedProperty, propertyDeclarationSyntax.Modifiers);

            currentTypeInfo.SpecificProperties.Add(evaluatedProperty);

            foreach (var accessorDeclarationSyntax in propertyDeclarationSyntax.AccessorList.Accessors)
            {
                if (accessorDeclarationSyntax.Keyword.ValueText == "get")
                {
                    var evaluatedPropertyGetAccessor = new EvaluatedPropertyGetAccessor();
                    evaluatedPropertyGetAccessor.Declaration = accessorDeclarationSyntax;
                    evaluatedPropertyGetAccessor.Identifier = propertyDeclarationSyntax.Identifier;
                    evaluatedPropertyGetAccessor.IdentifierText = propertyDeclarationSyntax.Identifier.ValueText;
                    evaluatedPropertyGetAccessor.FullIdentifierText = fullNamespace
                                                                      + NormalizeName(
                                                                          "Get"
                                                                          + propertyDeclarationSyntax.Identifier
                                                                                .ValueText);

                    AddMemberFlagsToMember(evaluatedPropertyGetAccessor, accessorDeclarationSyntax.Modifiers);

                    evaluatedProperty.PropertyGetAccessor = evaluatedPropertyGetAccessor;

                    if (accessorDeclarationSyntax.Body != null) evaluatedProperty.IsAutoProperty = false;
                    continue;
                }

                if (accessorDeclarationSyntax.Keyword.ValueText == "set")
                {
                    var evaluatedPropertySetAccessor = new EvaluatedPropertySetAccessor();
                    evaluatedPropertySetAccessor.Declaration = accessorDeclarationSyntax;
                    evaluatedPropertySetAccessor.Identifier = propertyDeclarationSyntax.Identifier;
                    evaluatedPropertySetAccessor.IdentifierText = propertyDeclarationSyntax.Identifier.ValueText;
                    evaluatedPropertySetAccessor.FullIdentifierText = fullNamespace
                                                                      + NormalizeName(
                                                                          "Set"
                                                                          + propertyDeclarationSyntax.Identifier
                                                                                .ValueText);

                    AddMemberFlagsToMember(evaluatedPropertySetAccessor, accessorDeclarationSyntax.Modifiers);

                    evaluatedProperty.PropertySetAccessor = evaluatedPropertySetAccessor;

                    var evaluatedMethodParameter = new EvaluatedMethodParameter();
                    evaluatedMethodParameter.Index = 0;
                    evaluatedMethodParameter.IdentifierText = "value";
                    evaluatedMethodParameter.Declaration = accessorDeclarationSyntax;

                    if (accessorDeclarationSyntax.Body != null) evaluatedProperty.IsAutoProperty = false;
                }
            }
        }

        private void BuildEvaluatedTypeInfos(IList<SyntaxTree> syntaxTrees)
        {
            foreach (var syntaxTree in syntaxTrees)
                BuildEvaluatedTypeInfos(
                    syntaxTree.GetRoot(),
                    null,
                    new List<MemberDeclarationSyntax>(),
                    new List<UsingDirectiveSyntax>());
        }

        private void BuildEvaluatedTypeInfos(
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

            if (syntaxNode is CompilationUnitSyntax) AddCompilationUnit(syntaxNode, currentTypeInfo, namespaceDeclarations, usingDirectives);
        }

        private void BuildEvaluatedTypeInfosInheritedMembers()
        {
            foreach (var trackedVariableTypeInfo in _evaluatedTypeInfos)
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
                            foreach (var baseVariableTypeInfo in trackedTypeInfo.BaseTypeInfos)
                                if (!allTypeInfos.Contains(baseVariableTypeInfo) && baseVariableTypeInfo.IsReferenceType)
                                {
                                    tempTypeInfo.Add(baseVariableTypeInfo);
                                    foundNewTypes = true;
                                }

                        allTypeInfos.AddRange(tempTypeInfo);
                    }
                    while (foundNewTypes);

                    var methodSignatureComparer = ObjectFactory.GetInstance<IMethodSignatureComparer>();

                    foreach (var trackedTypeInfo in allTypeInfos)
                    {
                        var newMethods =
                            trackedTypeInfo.SpecificMethods.Where(
                                baseMethod =>
                                trackedVariableTypeInfo.AccesibleMethods.All(
                                    specificMethod =>
                                    !methodSignatureComparer.HaveSameSignature(baseMethod, specificMethod)));

                        trackedVariableTypeInfo.AccesibleMethods.AddRange(newMethods);

                        trackedVariableTypeInfo.AccesibleFields.AddRange(trackedTypeInfo.SpecificFields);

                        var newProperties =
                            trackedTypeInfo.SpecificProperties.Where(
                                baseProperty =>
                                trackedVariableTypeInfo.AccesibleProperties.All(
                                    specificProperty =>
                                    !string.Equals(
                                        baseProperty.IdentifierText,
                                        specificProperty.IdentifierText,
                                        StringComparison.Ordinal)));

                        trackedVariableTypeInfo.AccesibleProperties.AddRange(newProperties);
                    }
                }
        }

        private void BuildEvaluatedTypeInfosMemberTypeInfos()
        {
            foreach (var trackedVariableTypeInfo in _evaluatedTypeInfos)
            {
                var baseTypeDeclarationSyntax = trackedVariableTypeInfo.Declaration as BaseTypeDeclarationSyntax;

                if (baseTypeDeclarationSyntax != null && trackedVariableTypeInfo.IsWellKnown())
                {
                    if (baseTypeDeclarationSyntax.BaseList != null)
                        foreach (var baseTypeSyntax in baseTypeDeclarationSyntax.BaseList.Types)
                        {
                            var typeName = baseTypeSyntax.Type.GetText().ToString();

                            var baseTypeInfo = GetTypeInfo(
                                typeName,
                                trackedVariableTypeInfo.UsingDirectives,
                                trackedVariableTypeInfo.NamespaceDeclarations);

                            if (baseTypeInfo != null) trackedVariableTypeInfo.BaseTypeInfos.Add(baseTypeInfo);
                        }

                    foreach (var trackedField in trackedVariableTypeInfo.SpecificFields)
                    {
                        var fieldDeclarationSyntax = trackedField.Declaration as FieldDeclarationSyntax;

                        if (fieldDeclarationSyntax != null)
                        {
                            var typeName = fieldDeclarationSyntax.Declaration.Type.GetText().ToString();
                            var wellKnownTypeInfo = GetTypeInfo(
                                typeName,
                                trackedVariableTypeInfo.UsingDirectives,
                                trackedVariableTypeInfo.NamespaceDeclarations);

                            if (wellKnownTypeInfo != null) trackedField.TypeInfo = wellKnownTypeInfo;
                        }
                    }

                    foreach (var trackedProperty in trackedVariableTypeInfo.SpecificProperties)
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
                                    && trackedProperty.PropertySetAccessor.Parameters.Count > 0) trackedProperty.PropertySetAccessor.Parameters[0].TypeInfo = wellKnownTypeInfo;
                            }
                        }
                    }

                    foreach (var evaluatedMethod in trackedVariableTypeInfo.SpecificMethods)
                    {
                        foreach (var evaluatedMethodParameter in evaluatedMethod.Parameters)
                        {
                            var parameterSyntax = evaluatedMethodParameter.Declaration as ParameterSyntax;

                            if (parameterSyntax != null)
                            {
                                var typeName = parameterSyntax.Type.GetText().ToString();
                                var wellKnownTypeInfo = GetTypeInfo(
                                    typeName,
                                    trackedVariableTypeInfo.UsingDirectives,
                                    trackedVariableTypeInfo.NamespaceDeclarations);

                                if (wellKnownTypeInfo != null) evaluatedMethodParameter.TypeInfo = wellKnownTypeInfo;
                            }
                        }

                        var returnedTypeName =
                            ((MethodDeclarationSyntax)evaluatedMethod.Declaration).ReturnType.GetText().ToString();
                        var returnedTypeInfo = GetTypeInfo(
                            returnedTypeName,
                            trackedVariableTypeInfo.UsingDirectives,
                            trackedVariableTypeInfo.NamespaceDeclarations);
                        evaluatedMethod.ReturnType = returnedTypeInfo;
                    }
                }
            }
        }

        private string GetFullTypeNamespace(
            List<MemberDeclarationSyntax> namespaceDeclarations,
            string typeDeclarationName)
        {
            LoadKeywordToTypeInfoRemapperIfNeeded();

            if (_keywordToTypeInfoRemapper.IsKeywordTypeInfo(typeDeclarationName))
            {
                return _keywordToTypeInfoRemapper.GetMappedTypeInfo(typeDeclarationName);
            }

            var fullNamespace = new StringBuilder();

            foreach (var namespaceDeclaration in namespaceDeclarations) AddNameToFullNamespace(namespaceDeclaration, fullNamespace);

            fullNamespace.Append(".");
            fullNamespace.Append(NormalizeName(typeDeclarationName));

            if (fullNamespace.Length > 0 && fullNamespace.ToString().StartsWith(".")) fullNamespace.Remove(0, 1);
            return fullNamespace.ToString();
        }

        private string GetFullTypeNamespace(UsingDirectiveSyntax usingDirective, string typeDeclarationName)
        {
            LoadKeywordToTypeInfoRemapperIfNeeded();

            if (_keywordToTypeInfoRemapper.IsKeywordTypeInfo(typeDeclarationName))
            {
                return _keywordToTypeInfoRemapper.GetMappedTypeInfo(typeDeclarationName);
            }

            var fullNamespace = new StringBuilder();

            fullNamespace.Append(usingDirective.Name);
            fullNamespace.Append(".");
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