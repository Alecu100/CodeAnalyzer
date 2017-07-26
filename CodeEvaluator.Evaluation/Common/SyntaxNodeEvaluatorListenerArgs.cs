namespace CodeEvaluator.Evaluation.Common
{
    using Microsoft.CodeAnalysis;

    public class SyntaxNodeEvaluatorListenerArgs
    {
        public SyntaxNode EvaluatedSyntaxNode { get; set; }

        public bool CancelEvaluation { get; set; }

        public CodeEvaluatorExecutionStack ExecutionStack { get; set; }
    }
}