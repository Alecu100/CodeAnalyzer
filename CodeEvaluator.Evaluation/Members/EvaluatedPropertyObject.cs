using System.Collections.Generic;
using CodeEvaluator.Evaluation.Common;

namespace CodeEvaluator.Evaluation.Members
{
    public class EvaluatedPropertyObject : EvaluatedObject
    {
        public EvaluatedPropertyObject(
            EvaluatedTypeInfo referenceType,
            EvaluatedObject evaluatedObject,
            EvaluatedProperty targetProperty,
            CodeEvaluatorExecutionStack evaluatorExecutionStack)
        {
            var evaluatedObjectReference = new EvaluatedObjectDirectReference();
            evaluatedObjectReference.AssignEvaluatedObject(evaluatedObject);

            _fields.Add(evaluatedObjectReference);
            CodeEvaluatorExecutionStack = evaluatorExecutionStack;
            TargetProperty = targetProperty;
            TypeInfo = referenceType;
        }

        public EvaluatedProperty TargetProperty { get; }

        public EvaluatedObject TargetObject => _fields.Count > 0 ? _fields[0].EvaluatedObjects[0] : null;

        public override IReadOnlyList<EvaluatedObjectReference> Fields => _fields;

        public override EvaluatedTypeInfo TypeInfo { get; }

        public override List<EvaluatedObjectHistory> History => _fields[0].EvaluatedObjects[0].History;

        public CodeEvaluatorExecutionStack CodeEvaluatorExecutionStack { get; }
    }
}
