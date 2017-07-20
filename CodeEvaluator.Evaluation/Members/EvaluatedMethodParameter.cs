namespace CodeEvaluator.Evaluation.Members
{
    using System;

    [Serializable]
    public class EvaluatedMethodParameter : EvaluatedMember
    {
        #region Public Properties

        /// <summary>
        ///     Gets or sets the index.
        /// </summary>
        /// <value>
        ///     The index.
        /// </value>
        public int Index { get; set; }

        /// <summary>
        ///     Gets or sets the type information.
        /// </summary>
        /// <value>
        ///     The type information.
        /// </value>
        public EvaluatedTypeInfo TypeInfo { get; set; }

        #endregion
    }
}