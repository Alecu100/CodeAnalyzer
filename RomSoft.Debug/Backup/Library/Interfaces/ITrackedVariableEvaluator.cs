//  Project              : GLP
//  Module               : RomSoft.Client.Debug.dll
//  File                 : ITrackedVariableEvaluator.cs
//  Author               : Alecsandru
//  Last Updated         : 11/12/2015 at 17:09
//  
// 
//  Contains             : Implementation of the ITrackedVariableEvaluator.cs class.
//  Classes              : ITrackedVariableEvaluator.cs
// 
//  
//  ----------------------------------------------------------------------- 
//   <copyright file="ITrackedVariableEvaluator.cs" company="Sysmex"> 
//       Copyright (c) Sysmex. All rights reserved. 
//   </copyright> 
//  -----------------------------------------------------------------------

namespace RomSoft.Client.Debug.Library.Interfaces
{
    #region Using

    using Microsoft.CodeAnalysis;

    using RomSoft.Client.Debug.Library.Members;

    #endregion

    public interface ITrackedVariableEvaluator
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Evaluates the variable expression.
        /// </summary>
        /// <param name="reference">The reference.</param>
        /// <param name="expressionSyntax">The expression syntax.</param>
        void EvaluateVariable(TrackedVariableReference reference, SyntaxNode expressionSyntax);

        #endregion
    }
}