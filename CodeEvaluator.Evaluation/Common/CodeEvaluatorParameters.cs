namespace CodeEvaluator.Evaluation.Common
{
    using System.Collections.Generic;

    using global::CodeEvaluator.Evaluation.Interfaces;

    #region Using

    

    #endregion

    public class CodeEvaluatorParameters
    {
        #region SpecificFields

        private readonly List<ISyntaxNodeEvaluatorListener> _listeners =
            new List<ISyntaxNodeEvaluatorListener>();

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets the listeners.
        /// </summary>
        /// <value>
        ///     The listeners.
        /// </value>
        public List<ISyntaxNodeEvaluatorListener> Listeners
        {
            get
            {
                return _listeners;
            }
        }



        #endregion
    }
}