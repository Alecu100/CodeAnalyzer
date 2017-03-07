//  Project              : GLP
//  Module               : RomSoft.Client.Debug.dll
//  File                 : ISyntaxNodeEvaluator.cs
//  Author               : Alecsandru
//  Last Updated         : 16/11/2015 at 15:16
//  
// 
//  Contains             : Implementation of the ISyntaxNodeEvaluator.cs class.
//  Classes              : ISyntaxNodeEvaluator.cs
// 
//  
//  ----------------------------------------------------------------------- 
//   <copyright file="ISyntaxNodeEvaluator.cs" company="Sysmex"> 
//       Copyright (c) Sysmex. All rights reserved. 
//   </copyright> 
//  -----------------------------------------------------------------------

namespace RomSoft.Client.Debug.Library.Interfaces
{
    #region Using

    using Microsoft.CodeAnalysis;

    using RomSoft.Client.Debug.Library.Common;

    #endregion

    public interface ISyntaxNodeEvaluator
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Evaluates the syntax node.
        /// </summary>
        /// <param name="syntaxNode">The syntax node.</param>
        /// <param name="workflowEvaluatorContext">The workflow evaluator stack.</param>
        void EvaluateSyntaxNode(SyntaxNode syntaxNode, StaticWorkflowEvaluatorContext workflowEvaluatorContext);

        #endregion
    }
}