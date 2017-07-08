using System.Collections.Generic;
using CodeAnalysis.Core.Interfaces;

namespace CodeAnalysis.Core.Common
{
    #region Using

    

    #endregion

    public class CodeEvaluatorParameters
    {
        #region SpecificFields

        private readonly List<ICodeEvaluatorListener> _listeners =
            new List<ICodeEvaluatorListener>();

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets the listeners.
        /// </summary>
        /// <value>
        ///     The listeners.
        /// </value>
        public List<ICodeEvaluatorListener> Listeners
        {
            get
            {
                return _listeners;
            }
        }



        #endregion
    }
}