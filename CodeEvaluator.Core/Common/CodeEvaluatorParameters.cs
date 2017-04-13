//  Project              : GLP
//  Module               : RomSoft.Client.Debug.dll
//  File                 : CodeEvaluatorParameters.cs
//  Author               : Alecsandru
//  Last Updated         : 19/12/2015 at 1:11
//  
// 
//  Contains             : Implementation of the CodeEvaluatorParameters.cs class.
//  Classes              : CodeEvaluatorParameters.cs
// 
//  
//  ----------------------------------------------------------------------- 
//   <copyright file="CodeEvaluatorParameters.cs" company="Sysmex"> 
//       Copyright (c) Sysmex. All rights reserved. 
//   </copyright> 
//  -----------------------------------------------------------------------

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