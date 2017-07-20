namespace CodeEvaluator.Evaluation.Members
{
    using System;
    using System.Collections.Generic;

    using Microsoft.CodeAnalysis.CSharp.Syntax;

    #region Using

    #endregion

    [Serializable]
    public class EvaluatedTypeInfo : EvaluatedMember
    {
        public EvaluatedTypeInfo()
        {
            SharedStaticObject = new EvaluatedStaticObject(this);
        }

        public EvaluatedStaticObject SharedStaticObject { get; }

        #region SpecificFields

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets all fields.
        /// </summary>
        /// <value>
        ///     All fields.
        /// </value>
        public List<EvaluatedField> AccesibleFields { get; } = new List<EvaluatedField>();

        /// <summary>
        ///     Gets all methods.
        /// </summary>
        /// <value>
        ///     All methods.
        /// </value>
        public List<EvaluatedMethod> AccesibleMethods { get; } = new List<EvaluatedMethod>();

        /// <summary>
        ///     Gets all properties.
        /// </summary>
        /// <value>
        ///     All properties.
        /// </value>
        public List<EvaluatedProperty> AccesibleProperties { get; } = new List<EvaluatedProperty>();

        /// <summary>
        ///     Gets the base type infos.
        /// </summary>
        /// <value>
        ///     The base type infos.
        /// </value>
        public List<EvaluatedTypeInfo> BaseTypeInfos { get; } = new List<EvaluatedTypeInfo>();

        /// <summary>
        ///     Gets the constructors.
        /// </summary>
        /// <value>
        ///     The constructors.
        /// </value>
        public List<EvaluatedConstructor> Constructors { get; } = new List<EvaluatedConstructor>();

        /// <summary>
        ///     Gets the fields.
        /// </summary>
        /// <value>
        ///     The fields.
        /// </value>
        public List<EvaluatedField> SpecificFields { get; } = new List<EvaluatedField>();

        /// <summary>
        ///     Gets or sets the tracked generic parameters.
        /// </summary>
        /// <value>
        ///     The tracked generic parameters.
        /// </value>
        public Dictionary<int, EvaluatedTypeInfo> GenericTypeInfos { get; } = new Dictionary<int, EvaluatedTypeInfo>();

        /// <summary>
        ///     Gets or sets a value indicating whether this instance is delegate related type.
        /// </summary>
        /// <value>
        ///     <c>true</c> if this instance is delegate related type; otherwise, <c>false</c>.
        /// </value>
        public bool IsDelegateRelatedType { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether this instance is generic defition.
        /// </summary>
        /// <value>
        ///     <c>true</c> if this instance is generic defition; otherwise, <c>false</c>.
        /// </value>
        public bool IsGenericDefinition { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether this instance is generic.
        /// </summary>
        /// <value>
        ///     <c>true</c> if this instance is generic; otherwise, <c>false</c>.
        /// </value>
        public bool IsGenericRealization { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether this instance is interface type.
        /// </summary>
        /// <value>
        ///     <c>true</c> if this instance is interface type; otherwise, <c>false</c>.
        /// </value>
        public bool IsInterfaceType { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether this instance is reference type.
        /// </summary>
        /// <value>
        ///     <c>true</c> if this instance is reference type; otherwise, <c>false</c>.
        /// </value>
        public bool IsReferenceType { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether this instance is value type.
        /// </summary>
        /// <value>
        ///     <c>true</c> if this instance is value type; otherwise, <c>false</c>.
        /// </value>
        public bool IsValueType { get; set; }

        /// <summary>
        ///     Gets or sets the methods.
        /// </summary>
        /// <value>
        ///     The methods.
        /// </value>
        public List<EvaluatedMethod> SpecificMethods { get; } = new List<EvaluatedMethod>();

        /// <summary>
        ///     Gets the namespace declarations.
        /// </summary>
        /// <value>
        ///     The namespace declarations.
        /// </value>
        public List<MemberDeclarationSyntax> NamespaceDeclarations { get; } = new List<MemberDeclarationSyntax>();

        /// <summary>
        ///     Gets the properties.
        /// </summary>
        /// <value>
        ///     The properties.
        /// </value>
        public List<EvaluatedProperty> Properties { get; } = new List<EvaluatedProperty>();

        /// <summary>
        ///     Gets the using directives.
        /// </summary>
        /// <value>
        ///     The using directives.
        /// </value>
        public List<UsingDirectiveSyntax> UsingDirectives { get; } = new List<UsingDirectiveSyntax>();

        #endregion
    }
}