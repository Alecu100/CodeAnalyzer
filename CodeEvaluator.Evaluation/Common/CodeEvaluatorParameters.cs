namespace CodeEvaluator.Evaluation.Common
{
    using System.Collections.Generic;

    using global::CodeEvaluator.Evaluation.Interfaces;

    #region Using

    #endregion

    public class CodeEvaluatorParameters
    {
        #region SpecificFields

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets the listeners.
        /// </summary>
        /// <value>
        ///     The listeners.
        /// </value>
        public List<ISyntaxNodeEvaluatorListener> EvaluatorListeners { get; } = new List<ISyntaxNodeEvaluatorListener>();

        public int EvaluatedObjectsHistoryLength { get; set; } = 5;

        #endregion
    }
}