//  Project              : GLP
//  Module               : RomSoft.Client.Debug.dll
//  File                 : StaticWorkflowEvaluatorParameters.cs
//  Author               : Alecsandru
//  Last Updated         : 19/12/2015 at 1:11
//  
// 
//  Contains             : Implementation of the StaticWorkflowEvaluatorParameters.cs class.
//  Classes              : StaticWorkflowEvaluatorParameters.cs
// 
//  
//  ----------------------------------------------------------------------- 
//   <copyright file="StaticWorkflowEvaluatorParameters.cs" company="Sysmex"> 
//       Copyright (c) Sysmex. All rights reserved. 
//   </copyright> 
//  -----------------------------------------------------------------------

using System.Collections.Generic;
using CodeAnalysis.Core.Interfaces;

namespace CodeAnalysis.Core.Common
{
    #region Using

    

    #endregion

    public class StaticWorkflowEvaluatorParameters
    {
        #region Fields

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