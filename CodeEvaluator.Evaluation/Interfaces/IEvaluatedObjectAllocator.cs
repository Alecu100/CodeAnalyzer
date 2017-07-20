namespace CodeEvaluator.Evaluation.Interfaces
{
    using CodeEvaluator.Evaluation.Members;

    #region Using

    

    #endregion

    public interface IEvaluatedObjectAllocator
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Allocates the variable.
        /// </summary>
        /// <param name="typeInfo">The type information.</param>
        /// <returns></returns>
        EvaluatedObject AllocateVariable(EvaluatedTypeInfo typeInfo);

        #endregion
    }
}