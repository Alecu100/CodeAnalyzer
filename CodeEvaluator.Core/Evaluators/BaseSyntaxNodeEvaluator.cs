namespace CodeAnalysis.Core.Evaluators
{
    using CodeAnalysis.Core.Common;
    using CodeAnalysis.Core.Interfaces;

    using Microsoft.CodeAnalysis;

    using StructureMap;

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
        /// <param name="workflowEvaluatorExecutionState">The workflow evaluator stack.</param>
        public virtual void EvaluateSyntaxNode(
            SyntaxNode syntaxNode,
            CodeEvaluatorExecutionState workflowEvaluatorExecutionState)
        {
            var previousSyntaxNode = workflowEvaluatorExecutionState.CurrentExecutionFrame.CurrentSyntaxNode;
            workflowEvaluatorExecutionState.PushSyntaxNodeEvaluator(this);
            workflowEvaluatorExecutionState.CurrentExecutionFrame.CurrentSyntaxNode = syntaxNode;

            EvaluateSyntaxNodeInternal(syntaxNode, workflowEvaluatorExecutionState);

            workflowEvaluatorExecutionState.CurrentExecutionFrame.CurrentSyntaxNode = previousSyntaxNode;
            workflowEvaluatorExecutionState.PopSyntaxNodeEvaluator();
        }

        #endregion

        #region Protected Methods and Operators

        protected virtual void EvaluateSyntaxNodeInternal(
            SyntaxNode syntaxNode,
            CodeEvaluatorExecutionState workflowEvaluatorExecutionState)
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