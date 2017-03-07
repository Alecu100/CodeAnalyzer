//  Project              : GLP
//  Module               : RomSoft.Client.Debug.dll
//  File                 : DelegateRelatedTypeEvaluator.cs
//  Author               : Alecsandru
//  Last Updated         : 11/12/2015 at 17:11
//  
// 
//  Contains             : Implementation of the DelegateRelatedTypeEvaluator.cs class.
//  Classes              : DelegateRelatedTypeEvaluator.cs
// 
//  
//  ----------------------------------------------------------------------- 
//   <copyright file="DelegateRelatedTypeEvaluator.cs" company="Sysmex"> 
//       Copyright (c) Sysmex. All rights reserved. 
//   </copyright> 
//  -----------------------------------------------------------------------

using CodeAnalysis.Core.Interfaces;
using Microsoft.CodeAnalysis;

namespace CodeAnalysis.Core.Members
{
    #region Using

    

    #endregion

    public class DelegateRelatedTypeEvaluator : ITrackedVariableEvaluator
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Evaluates the variable expression.
        /// </summary>
        /// <param name="reference">The reference.</param>
        /// <param name="expressionSyntax">The expression syntax.</param>
        public void EvaluateVariable(EvaluatedObjectReference reference, SyntaxNode expressionSyntax)
        {
        }

        #endregion
    }
}