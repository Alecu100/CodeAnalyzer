using CodeEvaluator.Evaluation.Extensions;
using CodeEvaluator.Evaluation.Interfaces;
using CodeEvaluator.Evaluation.Members;
using StructureMap;

namespace CodeEvaluator.Evaluation.Common
{
    #region Using

    #endregion

    public class EvaluatorExecutionFrameFactory : IEvaluatorExecutionFrameFactory
    {
        #region Public Methods and Operators

        public CodeEvaluatorExecutionFrame BuildInitialExecutionFrame(EvaluatedTypeInfo evaluatedType,
            EvaluatedMethod startMethod)
        {
            var codeEvaluatorExecutionFrame = new CodeEvaluatorExecutionFrame();

            var thisEvaluatedObjectReference = new EvaluatedObjectDirectReference();
            if (startMethod.IsStatic())
            {
                thisEvaluatedObjectReference.AssignEvaluatedObject(evaluatedType.SharedStaticObject);
            }
            else
            {
                var trackedVariableAllocator = ObjectFactory.GetInstance<IEvaluatedObjectAllocator>();
                var allocateVariable = trackedVariableAllocator.AllocateVariable(evaluatedType);

                thisEvaluatedObjectReference.AssignEvaluatedObject(allocateVariable);
            }

            codeEvaluatorExecutionFrame.ThisReference = thisEvaluatedObjectReference;
            codeEvaluatorExecutionFrame.PassedMethodParameters[-1] = thisEvaluatedObjectReference;
            codeEvaluatorExecutionFrame.CurrentMethod = startMethod;

            return codeEvaluatorExecutionFrame;
        }

        public CodeEvaluatorExecutionFrame BuildNewExecutionFrameForMethodCall(
            EvaluatedMethodBase targetMethod,
            EvaluatedObjectReference thisReference)
        {
            var newExecutionFrameForMethodCall = new CodeEvaluatorExecutionFrame();

            newExecutionFrameForMethodCall.CurrentMethod = targetMethod;
            newExecutionFrameForMethodCall.ThisReference = thisReference;
            newExecutionFrameForMethodCall.CurrentSyntaxNode = targetMethod.Declaration;
            newExecutionFrameForMethodCall.ReturningMethodParameters =
                new EvaluatedObjectDirectReference {TypeInfo = targetMethod.ReturnType};

            return newExecutionFrameForMethodCall;
        }

        #endregion
    }
}