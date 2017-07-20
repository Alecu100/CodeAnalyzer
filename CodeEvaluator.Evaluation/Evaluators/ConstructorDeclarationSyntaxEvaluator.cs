namespace CodeEvaluator.Evaluation.Evaluators
{
    using System.Linq;

    using CodeEvaluator.Evaluation.Common;
    using CodeEvaluator.Evaluation.Members;

    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp.Syntax;

    #region Using

    #endregion

    public class ConstructorDeclarationSyntaxEvaluator : BaseMethodDeclarationSyntaxEvaluator
    {
        #region Protected Methods and Operators

        protected override void EvaluateSyntaxNodeInternal(
            SyntaxNode syntaxNode,
            CodeEvaluatorExecutionState workflowEvaluatorExecutionState)
        {
            _baseMethodDeclarationSyntax = (ConstructorDeclarationSyntax) syntaxNode;
            _workflowEvaluatorExecutionState = workflowEvaluatorExecutionState;

            InitializeThisVariable();
            InitializeExecutionFrame();
            InitializeParameters();

            var syntaxNodeEvaluator =
                SyntaxNodeEvaluatorFactory.GetSyntaxNodeEvaluator(_baseMethodDeclarationSyntax.Body);

            if (syntaxNodeEvaluator != null)
            {
                syntaxNodeEvaluator.EvaluateSyntaxNode(_baseMethodDeclarationSyntax.Body, workflowEvaluatorExecutionState);
            }

            ReturnThisReference();

            ResetExecutionFrame();
        }

        #endregion

        #region Private Methods and Operators

        private void InitializeThisVariable()
        {
            var trackedVariableTypeInfo =
                EvaluatedTypesInfoTable.GetTypeInfo(_baseMethodDeclarationSyntax as ConstructorDeclarationSyntax);

            if (trackedVariableTypeInfo != null)
            {
                _thisReference = new EvaluatedObjectReference();
                _thisReference.AssignEvaluatedObject(VariableAllocator.AllocateVariable(trackedVariableTypeInfo));
                _thisReference.TypeInfo = trackedVariableTypeInfo;
                _evaluatedMethod =
                    trackedVariableTypeInfo.Constructors.First(
                        constructor =>
                            ((ConstructorDeclarationSyntax) constructor.Declaration).ParameterList.ToString()
                            == _baseMethodDeclarationSyntax.ParameterList.ToString());
            }
        }

        private void ReturnThisReference()
        {
            _workflowEvaluatorExecutionState.CurrentExecutionFrame.ReturningMethodParameters.AssignEvaluatedObject(
                _thisReference);
        }

        #endregion
    }
}