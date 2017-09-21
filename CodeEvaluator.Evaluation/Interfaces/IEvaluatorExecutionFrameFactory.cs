namespace CodeEvaluator.Evaluation.Interfaces
{
    using CodeEvaluator.Evaluation.Common;
    using CodeEvaluator.Evaluation.Members;

    #region Using

    #endregion

    public interface IEvaluatorExecutionFrameFactory
    {
        CodeEvaluatorExecutionFrame BuildNewExecutionFrameForMethodCall(
            EvaluatedMethodBase targetMethod,
            EvaluatedObjectReference thisReference);


        CodeEvaluatorExecutionFrame BuildInitialExecutionFrame(EvaluatedTypeInfo evaluatedType, EvaluatedMethod startMethod);
    }
}