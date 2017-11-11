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
            _methodDeclarationSyntax = (MethodDeclarationSyntax)syntaxNode;
            WorkflowEvaluatorExecutionStack = workflowEvaluatorExecutionStack;

            InitializeThisVariable();
            InitializeExecutionFrame();
            InitializeParameters();

            var syntaxNodeEvaluator =
                SyntaxNodeEvaluatorFactory.GetSyntaxNodeEvaluator(
                    _methodDeclarationSyntax.Body,
                    EEvaluatorActions.None);

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
            _evaluatedMethod = _thisReference.TypeInfo.AccesibleMethods.FirstOrDefault(
                method =>
                    {
                        var methodDeclarationSyntax = (MethodDeclarationSyntax)method.Declaration;

                        if (methodDeclarationSyntax.Identifier.ValueText
                            != _methodDeclarationSyntax.Identifier.ValueText)
                        {
                            return false;
                        }

                        if (methodDeclarationSyntax.ParameterList.GetText().ToString()
                            != _methodDeclarationSyntax.ParameterList.GetText().ToString())
                        {
                            return false;
                        }

                        if (methodDeclarationSyntax.ReturnType.GetText().ToString()
                            != _methodDeclarationSyntax.ReturnType.GetText().ToString())
                        {
                            return false;
                        }

                        return true;
                    });
        }

        #endregion
    }
}