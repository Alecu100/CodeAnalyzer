namespace CodeEvaluator.Evaluation.Members
{
    using System.Collections.Generic;

    public class EvaluatedDelegate : EvaluatedObject
    {
        private EvaluatedMethodBase _method;

        private EvaluatedTypeInfo _referenceType;

        private readonly List<EvaluatedMethod> _methodGroup = new List<EvaluatedMethod>();

        public EvaluatedDelegate(EvaluatedTypeInfo referenceType, EvaluatedObject evaluatedObject, IEnumerable<EvaluatedMethod> methodGroups,
            EvaluatedMethodBase method)
        {
            var evaluatedObjectReference = new EvaluatedObjectDirectReference();
            evaluatedObjectReference.AssignEvaluatedObject(evaluatedObject);

            _fields.Add(evaluatedObjectReference);
            _method = method;
            _referenceType = referenceType;
            _methodGroup.AddRange(methodGroups);
        }

        public EvaluatedDelegate(EvaluatedTypeInfo referenceType,
            EvaluatedMethodBase method)
        {
            _method = method;
            _referenceType = referenceType;
        }

        public EvaluatedMethodBase Method
        {
            get { return _method; }
        }

        public List<EvaluatedMethod> MethodGroup
        {
            get
            {
                return _methodGroup;
            }
        }

        public override IReadOnlyList<EvaluatedObjectReference> Fields
        {
            get { return _fields; }
        }

        public override EvaluatedTypeInfo TypeInfo
        {
            get { return _referenceType; }
        }

        public override List<EvaluatedObjectHistory> History
        {
            get { return _fields[0].EvaluatedObjects[0].History; }
        }
    }
}