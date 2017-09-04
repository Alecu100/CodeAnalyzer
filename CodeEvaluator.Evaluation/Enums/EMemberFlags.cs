namespace CodeEvaluator.Evaluation
{
    using System;

    [Flags]
    public enum EMemberFlags
    {
        None = 0,

        Static = 1,

        Virtual = 2,

        Override = 4,

        Abstract = 8,

        Private = 16,

        Protected = 32,

        Public = 64,

        Internal = 128,

        New = 256,

        External = 512,

        ProtectedInternal = 1024,

        Sealed = 2048,
    }
}