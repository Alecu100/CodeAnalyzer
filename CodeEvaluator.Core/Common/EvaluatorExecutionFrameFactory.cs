using CodeAnalysis.Core.Extensions;
using CodeAnalysis.Core.Interfaces;
using CodeAnalysis.Core.Members;
using StructureMap;

namespace CodeAnalysis.Core.Common
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

            var thisEvaluatedObjectReference = new EvaluatedObjectReference();
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

            return newExecutionFrameForMethodCall;
        }

        #endregion
    }
}