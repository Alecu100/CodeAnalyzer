namespace CodeEvaluator.Evaluation.Evaluators
{
    using CodeEvaluator.Evaluation.Common;
    using CodeEvaluator.Evaluation.Interfaces;
    using CodeEvaluator.Evaluation.Members;

    using Microsoft.CodeAnalysis.CSharp.Syntax;

    using StructureMap;

    #region Using

    #endregion

    public class BaseMethodDeclarationSyntaxEvaluator : SyntaxNodeEvaluator
    {
        #region Public Properties

        public IEvaluatedObjectAllocator VariableAllocator { get; set; }

        #endregion

        #region SpecificFields

        protected BaseMethodDeclarationSyntax _baseMethodDeclarationSyntax;

        protected EvaluatedObjectReference _thisReference;

        protected EvaluatedMethodBase _evaluatedMethod;

        protected CodeEvaluatorExecutionStack WorkflowEvaluatorExecutionStack;

        #endregion

        #region Protected Methods and Operators

        protected void InitializeExecutionFrame()
        {
            var staticWorkflowEvaluatorExecutionFrameFactory =
                ObjectFactory.GetInstance<IEvaluatorExecutionFrameFactory>();
            var buildNewExecutionFrameForMethodCall =
                staticWorkflowEvaluatorExecutionFrameFactory.BuildNewExecutionFrameForMethodCall(
                    _evaluatedMethod,
                    _thisReference);
            WorkflowEvaluatorExecutionStack.PushFramePassingParametersFromPreviousFrame(buildNewExecutionFrameForMethodCall);
        }

        protected void InitializeParameters()
        {
            for (var i = 0; i < _evaluatedMethod.Parameters.Count; i++)
            {
                if (WorkflowEvaluatorExecutionStack.CurrentExecutionFrame.PassedMethodParameters.ContainsKey(i))
                {
                    var trackedMethodParameter = _evaluatedMethod.Parameters[i];
                    var passedParameters = WorkflowEvaluatorExecutionStack.CurrentExecutionFrame.PassedMethodParameters[i];

                    if (trackedMethodParameter.TypeInfo == null || passedParameters == null)
                    {
                        continue;
                    }

                    passedParameters.Declaration = trackedMethodParameter.Declaration;
                    passedParameters.Identifier = trackedMethodParameter.Identifier;
                    passedParameters.IdentifierText = trackedMethodParameter.IdentifierText;
                    passedParameters.TypeInfo = trackedMethodParameter.TypeInfo;

                    WorkflowEvaluatorExecutionStack.CurrentExecutionFrame.LocalReferences.Add(passedParameters);
                    WorkflowEvaluatorExecutionStack.CurrentExecutionFrame.PassedMethodParameters.Remove(i);
                }
                else
                {
                    var trackedMethodParameter = _evaluatedMethod.Parameters[i];

                    if (trackedMethodParameter.TypeInfo == null)
                    {
                        continue;
                    }

                    var trackedVariableReference = new EvaluatedObjectDirectReference();
                    trackedVariableReference.Declaration = trackedMethodParameter.Declaration;
                    trackedVariableReference.TypeInfo = trackedMethodParameter.TypeInfo;
                    trackedVariableReference.Identifier = trackedMethodParameter.Identifier;
                    trackedVariableReference.IdentifierText = trackedMethodParameter.IdentifierText;
                    trackedVariableReference.AssignEvaluatedObject(
                        VariableAllocator.AllocateVariable(trackedMethodParameter.TypeInfo));
                    WorkflowEvaluatorExecutionStack.CurrentExecutionFrame.LocalReferences.Add(trackedVariableReference);
                }
            }
        }

        protected void ResetExecutionFrame()
        {
            WorkflowEvaluatorExecutionStack.PopFramePassingReturnedObjectsToPreviousFrame();
        }

        #endregion
    }
}