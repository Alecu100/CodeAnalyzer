namespace CodeEvaluator.Evaluation.Members
{
    public class EvaluatedLiteralObject : EvaluatedObject
    {
        public EvaluatedLiteralObject(EvaluatedTypeInfo typeInfo)
            : base(typeInfo)
        {
        }

        public object LiteralValue { get; set; }
    }
}