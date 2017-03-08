namespace CodeAnalysis.Core.Members
{
    public class EvaluatedDelegate : EvaluatedObject
    {
        private EvaluatedMethodBase _method;

        public EvaluatedDelegate(EvaluatedObject evaluatedObject, EvaluatedMethodBase method)
        {
            var evaluatedObjectReference = new EvaluatedObjectReference();
            evaluatedObjectReference.AssignEvaluatedObject(evaluatedObject);

            _fields.Add(evaluatedObjectReference);
            _method = method;
            _typeInfo = evaluatedObject.TypeInfo;
        }

        public EvaluatedMethodBase Method
        {
            get { return _method; }
            set { _method = value; }
        }
    }
}