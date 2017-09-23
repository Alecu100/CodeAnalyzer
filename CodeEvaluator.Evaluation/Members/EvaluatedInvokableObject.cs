using System.Collections.Generic;

namespace CodeEvaluator.Evaluation.Members
{
    public class EvaluatedInvokableObject : EvaluatedObject
    {
        public EvaluatedInvokableObject(
            EvaluatedTypeInfo referenceType,
            EvaluatedObject evaluatedObject,
            IEnumerable<EvaluatedMethodBase> methodGroups)
        {
            var evaluatedObjectReference = new EvaluatedObjectDirectReference();
            evaluatedObjectReference.AssignEvaluatedObject(evaluatedObject);

            _fields.Add(evaluatedObjectReference);
            TypeInfo = referenceType;
            TargetMethodGroup.AddRange(methodGroups);
        }

        public EvaluatedInvokableObject(
            EvaluatedTypeInfo referenceType,
            IEnumerable<EvaluatedMethodBase> methodGroups)
        {
            TypeInfo = referenceType;
            TargetMethodGroup.AddRange(methodGroups);
        }

        public EvaluatedInvokableObject(EvaluatedTypeInfo referenceType)
        {
            TypeInfo = referenceType;
        }

        public List<EvaluatedMethodBase> TargetMethodGroup { get; } = new List<EvaluatedMethodBase>();

        public EvaluatedObject TargetObject
        {
            get { return _fields.Count > 0 ? _fields[0].EvaluatedObjects[0] : null; }
        }

        public override IReadOnlyList<EvaluatedObjectReference> Fields
        {
            get { return _fields; }
        }

        public override EvaluatedTypeInfo TypeInfo { get; }

        public override List<EvaluatedObjectHistory> History
        {
            get { return _fields[0].EvaluatedObjects[0].History; }
        }
    }
}