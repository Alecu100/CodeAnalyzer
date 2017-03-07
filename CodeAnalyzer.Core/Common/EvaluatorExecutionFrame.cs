﻿//  Project              : GLP
//  Module               : RomSoft.Client.Debug.dll
//  File                 : EvaluatorExecutionFrame.cs
//  Author               : Alecsandru
//  Last Updated         : 18/12/2015 at 22:29
//  
// 
//  Contains             : Implementation of the EvaluatorExecutionFrame.cs class.
//  Classes              : EvaluatorExecutionFrame.cs
// 
//  
//  ----------------------------------------------------------------------- 
//   <copyright file="EvaluatorExecutionFrame.cs" company="Sysmex"> 
//       Copyright (c) Sysmex. All rights reserved. 
//   </copyright> 
//  -----------------------------------------------------------------------

using System.Collections.Generic;
using CodeAnalysis.Core.Members;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CodeAnalysis.Core.Common
{

    #region Using

    #endregion

    public class EvaluatorExecutionFrame
    {
        private readonly EvaluatedObjectReference _returningMethodParameters = new EvaluatedObjectReference();

        private EvaluatedMember _accessedMember;

        #region Public Properties

        /// <summary>
        ///     Gets or sets the accessed reference.
        /// </summary>
        /// <value>
        ///     The accessed reference.
        /// </value>
        public EvaluatedMember AccessedMember
        {
            get { return _accessedMember ?? ThisReference; }
            set { _accessedMember = value; }
        }

        /// <summary>
        ///     Gets or sets the accessed reference member.
        /// </summary>
        /// <value>
        ///     The accessed reference member.
        /// </value>
        public SimpleNameSyntax AccessedReferenceMember { get; set; }

        /// <summary>
        ///     Gets or sets the current method.
        /// </summary>
        /// <value>
        ///     The current method.
        /// </value>
        public EvaluatedMethodBase CurrentMethod { get; set; }

        /// <summary>
        ///     Gets or sets the current syntax node.
        /// </summary>
        /// <value>
        ///     The current syntax node.
        /// </value>
        public SyntaxNode CurrentSyntaxNode { get; set; }

        /// <summary>
        ///     Gets the local variables.
        /// </summary>
        /// <value>
        ///     The local variables.
        /// </value>
        public List<EvaluatedObjectReference> LocalReferences { get; } = new List<EvaluatedObjectReference>();

        /// <summary>
        ///     Gets or sets the stack variables.
        /// </summary>
        /// <value>
        ///     The stack variables.
        /// </value>
        public Dictionary<int, EvaluatedObjectReference> PassedMethodParameters { get; } =
            new Dictionary<int, EvaluatedObjectReference>();

        /// <summary>
        ///     Gets the returning method parameters.
        /// </summary>
        /// <value>
        ///     The returning method parameters.
        /// </value>
        public EvaluatedObjectReference ReturningMethodParameters
        {
            get { return _returningMethodParameters; }
        }

        /// <summary>
        ///     Gets or sets the this reference.
        /// </summary>
        /// <value>
        ///     The this reference.
        /// </value>
        public EvaluatedObjectReference ThisReference { get; set; }

        #endregion
    }
}