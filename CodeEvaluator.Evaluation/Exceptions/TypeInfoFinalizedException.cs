namespace CodeEvaluator.Evaluation.Exceptions
{
    using System;

    public class TypeInfoFinalizedException : Exception
    {
        public TypeInfoFinalizedException(string message)
            : base(message)
        {
        }
    }
}