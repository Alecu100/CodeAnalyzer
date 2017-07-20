namespace CodeEvaluator.Evaluation.Interfaces
{
    using Microsoft.CodeAnalysis;

    #region Using

    #endregion

    public interface ISyntaxNodeEvaluatorFactory
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Builds the syntax node evaluator.
        /// </summary>
        /// <param name="syntaxNode">The syntax node.</param>
        /// <returns></returns>
        ISyntaxNodeEvaluator GetSyntaxNodeEvaluator(SyntaxNode syntaxNode);

        #endregion
    }
}