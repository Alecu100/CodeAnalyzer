namespace CodeEvaluator.Evaluation.Members
{
    using System;
    using System.Collections.Generic;

    using CodeEvaluator.Evaluation.Common;
    using CodeEvaluator.Evaluation.Exceptions;
    using CodeEvaluator.Evaluation.Interfaces;

    using Microsoft.CodeAnalysis.CSharp.Syntax;

    using StructureMap;

    #region Using

    #endregion

    [Serializable]
    public class EvaluatedTypeInfo : EvaluatedMember
    {
        private readonly EvaluatedMembersList<EvaluatedField> _accesibleFields =
            new EvaluatedMembersList<EvaluatedField>();

        private readonly EvaluatedMembersList<EvaluatedMethod> _accesibleMethods =
            new EvaluatedMembersList<EvaluatedMethod>();

        private readonly EvaluatedMembersList<EvaluatedProperty> _accesibleProperties =
            new EvaluatedMembersList<EvaluatedProperty>();

        private readonly EvaluatedMembersList<EvaluatedTypeInfo> _baseTypeInfos =
            new EvaluatedMembersList<EvaluatedTypeInfo>();

        private readonly EvaluatedMembersList<EvaluatedConstructor> _constructors =
            new EvaluatedMembersList<EvaluatedConstructor>();

        private readonly Dictionary<int, EvaluatedTypeInfo> _genericTypeInfos = new Dictionary<int, EvaluatedTypeInfo>();

        private readonly EvaluatedMembersList<MemberDeclarationSyntax> _namespaceDeclarations =
            new EvaluatedMembersList<MemberDeclarationSyntax>();

        private readonly EvaluatedMembersList<EvaluatedField> _specificFields =
            new EvaluatedMembersList<EvaluatedField>();

        private readonly EvaluatedMembersList<EvaluatedMethod> _specificMethods =
            new EvaluatedMembersList<EvaluatedMethod>();

        private readonly EvaluatedMembersList<EvaluatedProperty> _specificProperties =
            new EvaluatedMembersList<EvaluatedProperty>();

        private readonly EvaluatedMembersList<UsingDirectiveSyntax> _usingDirectives =
            new EvaluatedMembersList<UsingDirectiveSyntax>();

        private bool isDelegateRelatedType;

        private bool isGenericDefinition;

        private bool isGenericRealization;

        private bool isInterfaceType;

        private bool isReadyToBeFinalized;

        private bool isReferenceType;

        private bool isValueType;

        public EvaluatedTypeInfo()
        {
            SharedStaticObject = new EvaluatedStaticObject(this);
        }

        public bool IsReadyToBeFinalized
        {
            get
            {
                return isReadyToBeFinalized;
            }
            set
            {
                isReadyToBeFinalized = value;
            }
        }

        public EvaluatedStaticObject SharedStaticObject { get; }

        private void FinalizeTypeInfoIfNeeded()
        {
            if (!IsFinalized && IsReadyToBeFinalized)
            {
                var evaluatedTypeInfoFinalizers = ObjectFactory.GetAllInstances<IEvaluatedTypeInfoFinalizer>();

                IsReadyToBeFinalized = false;

                foreach (var evaluatedTypeInfoFinalizer in evaluatedTypeInfoFinalizers) evaluatedTypeInfoFinalizer.FinalizeTypeInfo(this);

                IsFinalized = true;

                _usingDirectives.IsFinalized = true;
                _specificProperties.IsFinalized = true;
                _namespaceDeclarations.IsFinalized = true;
                _specificMethods.IsFinalized = true;
                _specificFields.IsFinalized = true;
                _constructors.IsFinalized = true;
                _baseTypeInfos.IsFinalized = true;
                _accesibleProperties.IsFinalized = true;
                _accesibleMethods.IsFinalized = true;
                _accesibleFields.IsFinalized = true;
            }
        }

        #region SpecificFields

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets all fields.
        /// </summary>
        /// <value>
        ///     All fields.
        /// </value>
        public EvaluatedMembersList<EvaluatedField> AccesibleFields
        {
            get
            {
                FinalizeTypeInfoIfNeeded();

                return _accesibleFields;
            }
        }

        /// <summary>
        ///     Gets all methods.
        /// </summary>
        /// <value>
        ///     All methods.
        /// </value>
        public EvaluatedMembersList<EvaluatedMethod> AccesibleMethods
        {
            get
            {
                FinalizeTypeInfoIfNeeded();

                return _accesibleMethods;
            }
        }

        /// <summary>
        ///     Gets all properties.
        /// </summary>
        /// <value>
        ///     All properties.
        /// </value>
        public EvaluatedMembersList<EvaluatedProperty> AccesibleProperties
        {
            get
            {
                FinalizeTypeInfoIfNeeded();

                return _accesibleProperties;
            }
        }

        /// <summary>
        ///     Gets the base type infos.
        /// </summary>
        /// <value>
        ///     The base type infos.
        /// </value>
        public EvaluatedMembersList<EvaluatedTypeInfo> BaseTypeInfos
        {
            get
            {
                FinalizeTypeInfoIfNeeded();

                return _baseTypeInfos;
            }
        }

        /// <summary>
        ///     Gets the constructors.
        /// </summary>
        /// <value>
        ///     The constructors.
        /// </value>
        public EvaluatedMembersList<EvaluatedConstructor> Constructors
        {
            get
            {
                FinalizeTypeInfoIfNeeded();

                return _constructors;
            }
        }

        /// <summary>
        ///     Gets the fields.
        /// </summary>
        /// <value>
        ///     The fields.
        /// </value>
        public EvaluatedMembersList<EvaluatedField> SpecificFields
        {
            get
            {
                FinalizeTypeInfoIfNeeded();

                return _specificFields;
            }
        }

        /// <summary>
        ///     Gets or sets the tracked generic parameters.
        /// </summary>
        /// <value>
        ///     The tracked generic parameters.
        /// </value>
        public Dictionary<int, EvaluatedTypeInfo> GenericTypeInfos
        {
            get
            {
                FinalizeTypeInfoIfNeeded();

                return _genericTypeInfos;
            }
        }

        /// <summary>
        ///     Gets or sets a value indicating whether this instance is delegate related type.
        /// </summary>
        /// <value>
        ///     <c>true</c> if this instance is delegate related type; otherwise, <c>false</c>.
        /// </value>
        public bool IsDelegateRelatedType
        {
            get
            {
                return isDelegateRelatedType;
            }
            set
            {
                ThrowExceptionIfFinalized();

                isDelegateRelatedType = value;
            }
        }

        /// <summary>
        ///     Gets or sets a value indicating whether this instance is generic defition.
        /// </summary>
        /// <value>
        ///     <c>true</c> if this instance is generic defition; otherwise, <c>false</c>.
        /// </value>
        public bool IsGenericDefinition
        {
            get
            {
                return isGenericDefinition;
            }
            set
            {
                ThrowExceptionIfFinalized();

                isGenericDefinition = value;
            }
        }

        /// <summary>
        ///     Gets or sets a value indicating whether this instance is generic.
        /// </summary>
        /// <value>
        ///     <c>true</c> if this instance is generic; otherwise, <c>false</c>.
        /// </value>
        public bool IsGenericRealization
        {
            get
            {
                FinalizeTypeInfoIfNeeded();

                return isGenericRealization;
            }
            set
            {
                isGenericRealization = value;
            }
        }

        /// <summary>
        ///     Gets or sets a value indicating whether this instance is interface type.
        /// </summary>
        /// <value>
        ///     <c>true</c> if this instance is interface type; otherwise, <c>false</c>.
        /// </value>
        public bool IsInterfaceType
        {
            get
            {
                FinalizeTypeInfoIfNeeded();

                return isInterfaceType;
            }
            set
            {
                ThrowExceptionIfFinalized();

                isInterfaceType = value;
            }
        }

        /// <summary>
        ///     Gets or sets a value indicating whether this instance is reference type.
        /// </summary>
        /// <value>
        ///     <c>true</c> if this instance is reference type; otherwise, <c>false</c>.
        /// </value>
        public bool IsReferenceType
        {
            get
            {
                FinalizeTypeInfoIfNeeded();

                return isReferenceType;
            }
            set
            {
                ThrowExceptionIfFinalized();

                isReferenceType = value;
            }
        }

        /// <summary>
        ///     Gets or sets a value indicating whether this instance is value type.
        /// </summary>
        /// <value>
        ///     <c>true</c> if this instance is value type; otherwise, <c>false</c>.
        /// </value>
        public bool IsValueType
        {
            get
            {
                FinalizeTypeInfoIfNeeded();

                return isValueType;
            }
            set
            {
                ThrowExceptionIfFinalized();

                isValueType = value;
            }
        }

        /// <summary>
        ///     Gets or sets the methods.
        /// </summary>
        /// <value>
        ///     The methods.
        /// </value>
        public EvaluatedMembersList<EvaluatedMethod> SpecificMethods
        {
            get
            {
                FinalizeTypeInfoIfNeeded();

                return _specificMethods;
            }
        }

        /// <summary>
        ///     Gets the namespace declarations.
        /// </summary>
        /// <value>
        ///     The namespace declarations.
        /// </value>
        public EvaluatedMembersList<MemberDeclarationSyntax> NamespaceDeclarations
        {
            get
            {
                FinalizeTypeInfoIfNeeded();

                return _namespaceDeclarations;
            }
        }

        /// <summary>
        ///     Gets the properties.
        /// </summary>
        /// <value>
        ///     The properties.
        /// </value>
        public EvaluatedMembersList<EvaluatedProperty> SpecificProperties
        {
            get
            {
                FinalizeTypeInfoIfNeeded();

                return _specificProperties;
            }
        }

        /// <summary>
        ///     Gets the using directives.
        /// </summary>
        /// <value>
        ///     The using directives.
        /// </value>
        public EvaluatedMembersList<UsingDirectiveSyntax> UsingDirectives
        {
            get
            {
                FinalizeTypeInfoIfNeeded();

                return _usingDirectives;
            }
        }

        #endregion
    }
}