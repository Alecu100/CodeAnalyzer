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
            CodeEvaluatorExecutionStack workflowEvaluatorExecutionStack)
        {
            _baseMethodDeclarationSyntax = (ConstructorDeclarationSyntax)syntaxNode;
            WorkflowEvaluatorExecutionStack = workflowEvaluatorExecutionStack;

            InitializeThisVariable();
            InitializeExecutionFrame();
            InitializeParameters();
            AddHistoryToThisVariable(workflowEvaluatorExecutionStack);

            var syntaxNodeEvaluator =
                SyntaxNodeEvaluatorFactory.GetSyntaxNodeEvaluator(
                    _baseMethodDeclarationSyntax.Body,
                    EEvaluatorActions.None);

            if (syntaxNodeEvaluator != null)
            {
                syntaxNodeEvaluator.EvaluateSyntaxNode(
                    _baseMethodDeclarationSyntax.Body,
                    workflowEvaluatorExecutionStack);
            }

            ReturnThisReference();

            ResetExecutionFrame();
        }

        private void AddHistoryToThisVariable(CodeEvaluatorExecutionStack workflowEvaluatorExecutionStack)
        {
            _thisReference.EvaluatedObjects[0].PushHistory(workflowEvaluatorExecutionStack);
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
                        ((ConstructorDeclarationSyntax)constructor.Declaration).ParameterList.ToString()
                        == _baseMethodDeclarationSyntax.ParameterList.ToString());
            }
        }

        private void ReturnThisReference()
        {
            WorkflowEvaluatorExecutionStack.CurrentExecutionFrame.ReturningMethodParameters.AssignEvaluatedObject(
                _thisReference);
        }

        #endregion
    }
}