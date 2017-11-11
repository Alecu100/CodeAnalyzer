using System.Linq;
using CodeEvaluator.Evaluation.Common;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CodeEvaluator.Evaluation.Evaluators
{
    public class AccessorDeclarationSyntaxEvaluator : BaseMethodDeclarationSyntaxEvaluator
    {
        #region SpecificFields

        private AccessorDeclarationSyntax _accessorDeclarationSyntax;

        #endregion

        #region Protected Methods and Operators

        protected override void EvaluateSyntaxNodeInternal(
            SyntaxNode syntaxNode,
            CodeEvaluatorExecutionStack workflowEvaluatorExecutionStack)
        {
            _accessorDeclarationSyntax = (AccessorDeclarationSyntax) syntaxNode;
            WorkflowEvaluatorExecutionStack = workflowEvaluatorExecutionStack;

            InitializeThisVariable();
            InitializeExecutionFrame();
            InitializeParameters();

            var syntaxNodeEvaluator =
                SyntaxNodeEvaluatorFactory.GetSyntaxNodeEvaluator(
                    _accessorDeclarationSyntax.Body,
                    EEvaluatorActions.None);

            if (syntaxNodeEvaluator != null)
                syntaxNodeEvaluator.EvaluateSyntaxNode(_accessorDeclarationSyntax.Body,
                    workflowEvaluatorExecutionStack);

            ResetExecutionFrame();
        }

        #endregion

        #region Private Methods and Operators

        private void InitializeThisVariable()
        {
            _thisReference = WorkflowEvaluatorExecutionStack.CurrentExecutionFrame.PassedMethodParameters[-1];
            WorkflowEvaluatorExecutionStack.CurrentExecutionFrame.PassedMethodParameters.Remove(-1);

            var evaluatedProperty = _thisReference.TypeInfo.AccesibleProperties.FirstOrDefault(
                property =>
                {
                    var propertyDeclarationSyntax = (PropertyDeclarationSyntax) property.Declaration;

                    if (propertyDeclarationSyntax.AccessorList.Accessors.Any(
                        accesor => accesor == _accessorDeclarationSyntax))
                        return true;

                    return false;
                });

            if (evaluatedProperty != null && evaluatedProperty.PropertyGetAccessor.Declaration ==
                _accessorDeclarationSyntax)
                _evaluatedMethod = evaluatedProperty.PropertyGetAccessor;

            if (evaluatedProperty != null && evaluatedProperty.PropertySetAccessor.Declaration ==
                _accessorDeclarationSyntax)
                _evaluatedMethod = evaluatedProperty.PropertySetAccessor;
        }

        #endregion
    }
}