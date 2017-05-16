﻿//  Project              : GLP
//  Module               : RomSoft.Client.Debug.dll
//  File                 : CodeEvaluatorExecutionFrame.cs
//  Author               : Alecsandru
//  Last Updated         : 18/12/2015 at 22:29
//  
// 
//  Contains             : Implementation of the CodeEvaluatorExecutionFrame.cs class.
//  Classes              : CodeEvaluatorExecutionFrame.cs
// 
//  
//  ----------------------------------------------------------------------- 
//   <copyright file="CodeEvaluatorExecutionFrame.cs" company="Sysmex"> 
//       Copyright (c) Sysmex. All rights reserved. 
//   </copyright> 
//  -----------------------------------------------------------------------

using System.Collections.Generic;
using CodeAnalysis.Core.Members;
using Microsoft.CodeAnalysis;

namespace CodeAnalysis.Core.Common
{

    #region Using

    #endregion

    public class CodeEvaluatorExecutionFrame
    {
        private readonly EvaluatedObjectReference _returningMethodParameters = new EvaluatedObjectReference();

        private EvaluatedObjectReference _memberAccessResult;

        #region Public Properties

        /// <summary>
        ///     Gets or sets the accessed reference.
        /// </summary>
        /// <value>
        ///     The accessed reference.
        /// </value>
        public EvaluatedObjectReference MemberAccessResult
        {
            get { return _memberAccessResult; }
            set { _memberAccessResult = value; }
        }

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