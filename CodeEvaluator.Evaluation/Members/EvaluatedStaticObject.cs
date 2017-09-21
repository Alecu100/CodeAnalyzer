namespace CodeEvaluator.Evaluation.Members
{
    using System.Collections.Generic;

    public class EvaluatedStaticObject : EvaluatedObject
    {
        public EvaluatedStaticObject(EvaluatedTypeInfo typeInfo) : base(typeInfo)
        {
        }

        public override IReadOnlyList<EvaluatedObjectReference> Fields
        {
            get { return _fields; }
        }

        public List<EvaluatedObjectReference> ModifiableFields
        {
            get { return _fields; }
        }
    }
}