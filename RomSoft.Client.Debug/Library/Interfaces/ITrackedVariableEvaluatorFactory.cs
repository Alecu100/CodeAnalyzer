//  Project              : GLP
//  Module               : RomSoft.Client.Debug.dll
//  File                 : ITrackedVariableExpressionEvaluatorFactory.cs
//  Author               : Alecsandru
//  Last Updated         : 07/12/2015 at 15:39
//  
// 
//  Contains             : Implementation of the ITrackedVariableExpressionEvaluatorFactory.cs class.
//  Classes              : ITrackedVariableExpressionEvaluatorFactory.cs
// 
//  
//  ----------------------------------------------------------------------- 
//   <copyright file="ITrackedVariableExpressionEvaluatorFactory.cs" company="Sysmex"> 
//       Copyright (c) Sysmex. All rights reserved. 
//   </copyright> 
//  -----------------------------------------------------------------------

namespace RomSoft.Client.Debug.Library.Interfaces
{
    #region Using

    using Microsoft.CodeAnalysis;

    using RomSoft.Client.Debug.Library.Common;
    using RomSoft.Client.Debug.Library.Members;

    #endregion

    public interface ITrackedVariableEvaluatorFactory
    {
        #region Public Methods and Operators

        /// <summary>
        /// Gets the variable expression evaluator.
        /// </summary>
        /// <param name="variableReference">The variable reference.</param>
        /// <param name="expression">The expression.</param>
        /// <returns></returns>
        ITrackedVariableEvaluator GetVariableExpressionEvaluator(
            TrackedVariableReference variableReference,
            SyntaxNode expression);

        #endregion
    }
}