//  Project              : GLP
//  Module               : RomSoft.Client.Debug.dll
//  File                 : TrackedVariableHistory.cs
//  Author               : Alecsandru
//  Last Updated         : 07/12/2015 at 15:07
//  
// 
//  Contains             : Implementation of the TrackedVariableHistory.cs class.
//  Classes              : TrackedVariableHistory.cs
// 
//  
//  ----------------------------------------------------------------------- 
//   <copyright file="TrackedVariableHistory.cs" company="Sysmex"> 
//       Copyright (c) Sysmex. All rights reserved. 
//   </copyright> 
//  -----------------------------------------------------------------------

namespace RomSoft.Client.Debug.Library.Members
{
    #region Using

    using Microsoft.CodeAnalysis;

    using RomSoft.Client.Debug.Library.Common;

    #endregion

    public class TrackedVariableHistory
    {
        #region Public Properties

        /// <summary>
        ///     Gets or sets the execution frame.
        /// </summary>
        /// <value>
        ///     The execution frame.
        /// </value>
        public StaticWorkflowEvaluatorExecutionFrame ExecutionFrame { get; set; }

        /// <summary>
        ///     Gets or sets the syntax node.
        /// </summary>
        /// <value>
        ///     The syntax node.
        /// </value>
        public SyntaxNode SyntaxNode { get; set; }

        #endregion
    }
}