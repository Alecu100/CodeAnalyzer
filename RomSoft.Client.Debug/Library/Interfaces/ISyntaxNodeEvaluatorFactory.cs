//  Project              : GLP
//  Module               : RomSoft.Client.Debug.dll
//  File                 : ISyntaxNodeEvaluatorFactory.cs
//  Author               : Alecsandru
//  Last Updated         : 16/11/2015 at 15:17
//  
// 
//  Contains             : Implementation of the ISyntaxNodeEvaluatorFactory.cs class.
//  Classes              : ISyntaxNodeEvaluatorFactory.cs
// 
//  
//  ----------------------------------------------------------------------- 
//   <copyright file="ISyntaxNodeEvaluatorFactory.cs" company="Sysmex"> 
//       Copyright (c) Sysmex. All rights reserved. 
//   </copyright> 
//  -----------------------------------------------------------------------

namespace RomSoft.Client.Debug.Library.Interfaces
{
    #region Using

    using Microsoft.CodeAnalysis;

    #endregion

    public interface ISyntaxNodeEvaluatorFactory
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Builds the syntax node evaluator.
        /// </summary>
        /// <param name="syntaxNode">The syntax node.</param>
        /// <returns></returns>
        ISyntaxNodeEvaluator GetSyntaxNodeEvaluator(SyntaxNode syntaxNode);

        #endregion
    }
}