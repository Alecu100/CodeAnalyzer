using System.Collections.Generic;

namespace CodeAnalysis.Core.Members
{
    public class EvaluatedDelegate : EvaluatedObject
    {
        private EvaluatedMethodBase _method;

        private EvaluatedTypeInfo _referenceType;

        public EvaluatedDelegate(EvaluatedTypeInfo referenceType, EvaluatedObject evaluatedObject,
            EvaluatedMethodBase method)
        {
            var evaluatedObjectReference = new EvaluatedObjectReference();
            evaluatedObjectReference.AssignEvaluatedObject(evaluatedObject);

            _fields.Add(evaluatedObjectReference);
            _method = method;
            _referenceType = referenceType;
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
            set { _method = value; }
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