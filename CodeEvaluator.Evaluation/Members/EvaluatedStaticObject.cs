namespace CodeEvaluator.Evaluation.Members
{
    using System.Collections.Generic;

    public class EvaluatedStaticObject : EvaluatedObject
    {
        public EvaluatedStaticObject(EvaluatedTypeInfo typeInfo) : base(typeInfo)
        {
        }

        public override IReadOnlyList<EvaluatedObjectReferenceBase> Fields
        {
            get { return _fields; }
        }

        public List<EvaluatedObjectReferenceBase> ModifiableFields
        {
            get { return _fields; }
        }
    }
}