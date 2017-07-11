using System.Linq;
using CodeAnalysis.Core.Common;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CodeAnalysis.Core.SyntaxNodeEvaluators
{

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
            CodeEvaluatorExecutionState workflowEvaluatorExecutionState)
        {
            _baseMethodDeclarationSyntax = (MethodDeclarationSyntax) syntaxNode;
            _methodDeclarationSyntax = (MethodDeclarationSyntax) syntaxNode;
            _workflowEvaluatorExecutionState = workflowEvaluatorExecutionState;

            InitializeThisVariable();
            InitializeExecutionFrame();
            InitializeParameters();

            var syntaxNodeEvaluator =
                SyntaxNodeEvaluatorFactory.GetSyntaxNodeEvaluator(_baseMethodDeclarationSyntax.Body);

            if (syntaxNodeEvaluator != null)
            {
                syntaxNodeEvaluator.EvaluateSyntaxNode(_methodDeclarationSyntax.Body, workflowEvaluatorExecutionState);
            }

            ResetExecutionFrame();
        }

        #endregion

        #region Private Methods and Operators

        private void InitializeThisVariable()
        {
            _thisReference = _workflowEvaluatorExecutionState.CurrentExecutionFrame.PassedMethodParameters[-1];
            _workflowEvaluatorExecutionState.CurrentExecutionFrame.PassedMethodParameters.Remove(-1);
            _evaluatedMethod =
                _thisReference.TypeInfo.AccesibleMethods.First(
                    method => method.IdentifierText == _methodDeclarationSyntax.Identifier.ValueText);
        }

        #endregion
    }
}