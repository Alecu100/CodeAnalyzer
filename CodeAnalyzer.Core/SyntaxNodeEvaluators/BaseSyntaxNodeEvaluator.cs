//  Project              : GLP
//  Module               : RomSoft.Client.Debug.dll
//  File                 : BaseSyntaxNodeEvaluator.cs
//  Author               : Alecsandru
//  Last Updated         : 15/12/2015 at 10:07
//  
// 
//  Contains             : Implementation of the BaseSyntaxNodeEvaluator.cs class.
//  Classes              : BaseSyntaxNodeEvaluator.cs
// 
//  
//  ----------------------------------------------------------------------- 
//   <copyright file="BaseSyntaxNodeEvaluator.cs" company="Sysmex"> 
//       Copyright (c) Sysmex. All rights reserved. 
//   </copyright> 
//  -----------------------------------------------------------------------

using CodeAnalysis.Core.Common;
using CodeAnalysis.Core.Interfaces;
using Microsoft.CodeAnalysis;
using StructureMap;

namespace CodeAnalysis.Core.SyntaxNodeEvaluators
{

    #region Using

    #endregion

    public abstract class BaseSyntaxNodeEvaluator : ISyntaxNodeEvaluator
    {
        #region Constructors and Destructors

        protected BaseSyntaxNodeEvaluator()
        {
            ObjectFactory.BuildUp(this);
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Evaluates the syntax node.
        /// </summary>
        /// <param name="syntaxNode">The syntax node.</param>
        /// <param name="workflowEvaluatorContext">The workflow evaluator stack.</param>
        public virtual void EvaluateSyntaxNode(
            SyntaxNode syntaxNode,
            StaticWorkflowEvaluatorContext workflowEvaluatorContext)
        {
            var previousSyntaxNode = workflowEvaluatorContext.CurrentExecutionFrame.CurrentSyntaxNode;
            workflowEvaluatorContext.PushSyntaxNodeEvaluator(this);
            workflowEvaluatorContext.CurrentExecutionFrame.CurrentSyntaxNode = syntaxNode;

            EvaluateSyntaxNodeInternal(syntaxNode, workflowEvaluatorContext);

            workflowEvaluatorContext.CurrentExecutionFrame.CurrentSyntaxNode = previousSyntaxNode;
            workflowEvaluatorContext.PopSyntaxNodeEvaluator();
        }

        #endregion

        #region Protected Methods and Operators

        protected virtual void EvaluateSyntaxNodeInternal(
            SyntaxNode syntaxNode,
            StaticWorkflowEvaluatorContext workflowEvaluatorContext)
        {
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets or sets the syntax node evaluator factory.
        /// </summary>
        /// <value>
        ///     The syntax node evaluator factory.
        /// </value>
        public ISyntaxNodeEvaluatorFactory SyntaxNodeEvaluatorFactory { get; set; }

        /// <summary>
        ///     Gets or sets the syntax node namespace provider.
        /// </summary>
        /// <value>
        ///     The syntax node namespace provider.
        /// </value>
        public ISyntaxNodeNamespaceProvider SyntaxNodeNamespaceProvider { get; set; }

        /// <summary>
        ///     Gets or sets the tracked variable type infos cache.
        /// </summary>
        /// <value>
        ///     The tracked variable type infos cache.
        /// </value>
        public IEvaluatedTypesInfoTable EvaluatedTypesInfoTable { get; set; }

        #endregion
    }
}