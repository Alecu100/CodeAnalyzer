namespace CodeEvaluator.Evaluation.Evaluators
{
    using System.Linq;

    using CodeEvaluator.Evaluation.Common;

    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp.Syntax;

    #region Using

    #endregion

    public class MethodDeclarationSyntaxEvaluator : BaseMethodDeclarationSyntaxEvaluator
    {
        #region SpecificFields

        private MethodDeclarationSyntax _methodDeclarationSyntax;

        #endregion

        #region Protected Methods and Operators

        protected override void EvaluateSyntaxNodeInternal(
            SyntaxNode syntaxNode,
            CodeEvaluatorExecutionStack workflowEvaluatorExecutionStack)
        {
            _baseMethodDeclarationSyntax = (MethodDeclarationSyntax) syntaxNode;
            _methodDeclarationSyntax = (MethodDeclarationSyntax) syntaxNode;
            WorkflowEvaluatorExecutionStack = workflowEvaluatorExecutionStack;

            InitializeThisVariable();
            InitializeExecutionFrame();
            InitializeParameters();

            var syntaxNodeEvaluator =
                SyntaxNodeEvaluatorFactory.GetSyntaxNodeEvaluator(_baseMethodDeclarationSyntax.Body, EEvaluatorActions.None);

            if (syntaxNodeEvaluator != null)
            {
                syntaxNodeEvaluator.EvaluateSyntaxNode(_methodDeclarationSyntax.Body, workflowEvaluatorExecutionStack);
            }

            ResetExecutionFrame();
        }

        #endregion

        #region Private Methods and Operators

        private void InitializeThisVariable()
        {
            _thisReference = WorkflowEvaluatorExecutionStack.CurrentExecutionFrame.PassedMethodParameters[-1];
            WorkflowEvaluatorExecutionStack.CurrentExecutionFrame.PassedMethodParameters.Remove(-1);
            _evaluatedMethod =
                _thisReference.TypeInfo.AccesibleMethods.First(
                    method => method.IdentifierText == _methodDeclarationSyntax.Identifier.ValueText);
        }

        #endregion
    }
}