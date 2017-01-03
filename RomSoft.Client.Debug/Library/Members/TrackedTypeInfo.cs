//  Project              : GLP
//  Module               : RomSoft.Client.Debug.dll
//  File                 : TrackedTypeInfo.cs
//  Author               : Alecsandru
//  Last Updated         : 18/12/2015 at 18:46
//  
// 
//  Contains             : Implementation of the TrackedTypeInfo.cs class.
//  Classes              : TrackedTypeInfo.cs
// 
//  
//  ----------------------------------------------------------------------- 
//   <copyright file="TrackedTypeInfo.cs" company="Sysmex"> 
//       Copyright (c) Sysmex. All rights reserved. 
//   </copyright> 
//  -----------------------------------------------------------------------

namespace RomSoft.Client.Debug.Library.Members
{
    #region Using

    using System.Collections.Generic;

    using Microsoft.CodeAnalysis.CSharp.Syntax;

    #endregion

    public class TrackedTypeInfo : TrackedMember
    {
        #region Fields

        private readonly List<TrackedField> _allFields = new List<TrackedField>();

        private readonly List<TrackedMethod> _allMethods = new List<TrackedMethod>();

        private readonly List<TrackedProperty> _allProperties = new List<TrackedProperty>();

        private readonly List<TrackedTypeInfo> _baseTypeInfos = new List<TrackedTypeInfo>();

        private readonly List<TrackedConstructor> _constructors = new List<TrackedConstructor>();

        private readonly List<TrackedField> _fields = new List<TrackedField>();

        private readonly Dictionary<int, TrackedTypeInfo> _genericTypeInfos = new Dictionary<int, TrackedTypeInfo>();

        private readonly List<TrackedMethod> _methods = new List<TrackedMethod>();

        private readonly List<MemberDeclarationSyntax> _namespaceDeclarations = new List<MemberDeclarationSyntax>();

        private readonly List<TrackedProperty> _properties = new List<TrackedProperty>();

        private readonly List<UsingDirectiveSyntax> _usingDirectives = new List<UsingDirectiveSyntax>();

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets all fields.
        /// </summary>
        /// <value>
        ///     All fields.
        /// </value>
        public List<TrackedField> AllFields
        {
            get
            {
                return _allFields;
            }
        }

        /// <summary>
        ///     Gets all methods.
        /// </summary>
        /// <value>
        ///     All methods.
        /// </value>
        public List<TrackedMethod> AllMethods
        {
            get
            {
                return _allMethods;
            }
        }

        /// <summary>
        ///     Gets all properties.
        /// </summary>
        /// <value>
        ///     All properties.
        /// </value>
        public List<TrackedProperty> AllProperties
        {
            get
            {
                return _allProperties;
            }
        }

        /// <summary>
        ///     Gets the base type infos.
        /// </summary>
        /// <value>
        ///     The base type infos.
        /// </value>
        public List<TrackedTypeInfo> BaseTypeInfos
        {
            get
            {
                return _baseTypeInfos;
            }
        }

        /// <summary>
        ///     Gets the constructors.
        /// </summary>
        /// <value>
        ///     The constructors.
        /// </value>
        public List<TrackedConstructor> Constructors
        {
            get
            {
                return _constructors;
            }
        }

        /// <summary>
        ///     Gets the fields.
        /// </summary>
        /// <value>
        ///     The fields.
        /// </value>
        public List<TrackedField> Fields
        {
            get
            {
                return _fields;
            }
        }

        /// <summary>
        ///     Gets or sets the tracked generic parameters.
        /// </summary>
        /// <value>
        ///     The tracked generic parameters.
        /// </value>
        public Dictionary<int, TrackedTypeInfo> GenericTypeInfos
        {
            get
            {
                return _genericTypeInfos;
            }
        }

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
        public List<TrackedMethod> Methods
        {
            get
            {
                return _methods;
            }
        }

        /// <summary>
        ///     Gets the namespace declarations.
        /// </summary>
        /// <value>
        ///     The namespace declarations.
        /// </value>
        public List<MemberDeclarationSyntax> NamespaceDeclarations
        {
            get
            {
                return _namespaceDeclarations;
            }
        }

        /// <summary>
        ///     Gets the properties.
        /// </summary>
        /// <value>
        ///     The properties.
        /// </value>
        public List<TrackedProperty> Properties
        {
            get
            {
                return _properties;
            }
        }

        /// <summary>
        ///     Gets the using directives.
        /// </summary>
        /// <value>
        ///     The using directives.
        /// </value>
        public List<UsingDirectiveSyntax> UsingDirectives
        {
            get
            {
                return _usingDirectives;
            }
        }

        #endregion
    }
}