﻿using System;
using System.Collections.Generic;

namespace CodeAnalysis.Core.Members
{

    #region Using

    #endregion

    [Serializable]
    public class EvaluatedMethodBase : EvaluatedMember
    {
        #region SpecificFields

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets the parameters.
        /// </summary>
        /// <value>
        ///     The parameters.
        /// </value>
        public List<EvaluatedMethodParameter> Parameters { get; } = new List<EvaluatedMethodParameter>();

        public EvaluatedTypeInfo ReturnType { get; set; }

        #endregion
    }
}