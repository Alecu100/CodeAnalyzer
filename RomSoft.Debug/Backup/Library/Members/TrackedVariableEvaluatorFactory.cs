//  Project              : GLP
//  Module               : RomSoft.Client.Debug.dll
//  File                 : TrackedVariableExpressionEvaluatorFactory.cs
//  Author               : Alecsandru
//  Last Updated         : 11/12/2015 at 17:08
//  
// 
//  Contains             : Implementation of the TrackedVariableExpressionEvaluatorFactory.cs class.
//  Classes              : TrackedVariableExpressionEvaluatorFactory.cs
// 
//  
//  ----------------------------------------------------------------------- 
//   <copyright file="TrackedVariableExpressionEvaluatorFactory.cs" company="Sysmex"> 
//       Copyright (c) Sysmex. All rights reserved. 
//   </copyright> 
//  -----------------------------------------------------------------------

namespace RomSoft.Client.Debug.Library.Members
{
    #region Using

    using Microsoft.CodeAnalysis;

    using RomSoft.Client.Debug.Library.Interfaces;

    #endregion

    public class TrackedVariableEvaluatorFactory : ITrackedVariableEvaluatorFactory
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Gets the variable expression evaluator.
        /// </summary>
        /// <param name="variableReference">The variable reference.</param>
        /// <param name="expression">The expression.</param>
        /// <returns></returns>
        public ITrackedVariableEvaluator GetVariableExpressionEvaluator(
            TrackedVariableReference variableReference,
            SyntaxNode expression)
        {
            return null;
        }

        #endregion
    }
}