namespace CodeEvaluator.Evaluation.Evaluators
{
    using CodeEvaluator.Evaluation.Common;
    using CodeEvaluator.Evaluation.Interfaces;

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
        /// <param name="workflowEvaluatorExecutionStack">The workflow evaluator stack.</param>
        public void EvaluateSyntaxNode(
            SyntaxNode syntaxNode,
            CodeEvaluatorExecutionStack workflowEvaluatorExecutionStack)
        {
            var previousSyntaxNode = workflowEvaluatorExecutionStack.CurrentExecutionFrame.CurrentSyntaxNode;
            workflowEvaluatorExecutionStack.PushSyntaxNodeEvaluator(this);
            workflowEvaluatorExecutionStack.PushSyntaxNode(syntaxNode);
            workflowEvaluatorExecutionStack.CurrentExecutionFrame.CurrentSyntaxNode = syntaxNode;

            var syntaxNodeEvaluatorListenerArgs = new SyntaxNodeEvaluatorListenerArgs { CancelEvaluation = false, EvaluatedSyntaxNode = syntaxNode, ExecutionStack = workflowEvaluatorExecutionStack };

            foreach (var staticWorkflowListener in workflowEvaluatorExecutionStack.Parameters.EvaluatorListeners)
            {
                staticWorkflowListener.OnBeforeSyntaxNodeEvaluated(this, syntaxNodeEvaluatorListenerArgs);
            }

            if (!syntaxNodeEvaluatorListenerArgs.CancelEvaluation)
            {
                EvaluateSyntaxNodeInternal(syntaxNode, workflowEvaluatorExecutionStack);
            }

            foreach (var staticWorkflowListener in workflowEvaluatorExecutionStack.Parameters.EvaluatorListeners)
            {
                staticWorkflowListener.OnAfterSyntaxNodeEvaluated(this, syntaxNodeEvaluatorListenerArgs);
            }

            workflowEvaluatorExecutionStack.CurrentExecutionFrame.CurrentSyntaxNode = previousSyntaxNode;
            workflowEvaluatorExecutionStack.PopSyntaxNode();
            workflowEvaluatorExecutionStack.PopSyntaxNodeEvaluator();
        }

        #endregion

        #region Protected Methods and Operators

        protected virtual void EvaluateSyntaxNodeInternal(
            SyntaxNode syntaxNode,
            CodeEvaluatorExecutionStack workflowEvaluatorExecutionStack)
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