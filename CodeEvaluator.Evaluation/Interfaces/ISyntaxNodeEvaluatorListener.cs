namespace CodeEvaluator.Evaluation.Interfaces
{
    using CodeEvaluator.Evaluation.Common;
    using CodeEvaluator.Evaluation.Members;

    using Microsoft.CodeAnalysis.CSharp.Syntax;

    #region Using

    #endregion

    public interface ISyntaxNodeEvaluatorListener
    {
        #region Public Methods and Operators

        void OnBeforeSyntaxNodeEvaluated(ISyntaxNodeEvaluator syntaxNodeEvaluator, SyntaxNodeEvaluatorListenerArgs args);

        void OnAfterSyntaxNodeEvaluated(ISyntaxNodeEvaluator syntaxNodeEvaluator, SyntaxNodeEvaluatorListenerArgs args);

        #endregion
    }
}