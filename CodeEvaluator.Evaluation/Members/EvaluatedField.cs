namespace CodeEvaluator.Evaluation.Members
{
    using System;

    using Microsoft.CodeAnalysis.CSharp.Syntax;

    #region Using

    #endregion

    [Serializable]
    public class EvaluatedField : EvaluatedMember
    {
        #region Public Properties

        /// <summary>
        ///     Gets or sets the initializer expression.
        /// </summary>
        /// <value>
        ///     The initializer expression.
        /// </value>
        public EqualsValueClauseSyntax InitializerExpression { get; set; }

        /// <summary>
        ///     Gets or sets the variable type information.
        /// </summary>
        /// <value>
        ///     The variable type information.
        /// </value>
        public EvaluatedTypeInfo TypeInfo { get; set; }

        #endregion
    }
}