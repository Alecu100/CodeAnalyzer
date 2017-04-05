using System.Collections.Generic;

namespace CodeAnalysis.Core.Members
{
    public class EvaluatedDelegate : EvaluatedObject
    {
        private EvaluatedMethodBase _method;

        public EvaluatedDelegate(EvaluatedObject evaluatedObject, EvaluatedMethodBase method) : base(null)
        {
            var evaluatedObjectReference = new EvaluatedObjectReference();
            evaluatedObjectReference.AssignEvaluatedObject(evaluatedObject);

            _fields.Add(evaluatedObjectReference);
            _method = method;
        }

        public EvaluatedMethodBase Method
        {
            get { return _method; }
            set { _method = value; }
        }

        public override IReadOnlyList<EvaluatedObjectReference> Fields
        {
            get { return _fields[0].EvaluatedObjects[0].Fields; }
        }

        public override EvaluatedTypeInfo TypeInfo
        {
            get { return _fields[0].EvaluatedObjects[0].TypeInfo; }
        }

        public override List<EvaluatedObjectHistory> History
        {
            get { return _fields[0].EvaluatedObjects[0].History; }
        }
    }
}