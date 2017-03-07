﻿//  Project              : GLP
//  Module               : RomSoft.Client.Debug.dll
//  File                 : IStaticEvaluator.cs
//  Author               : Alecsandru
//  Last Updated         : 16/11/2015 at 17:44
//  
// 
//  Contains             : Implementation of the IStaticEvaluator.cs class.
//  Classes              : IStaticEvaluator.cs
// 
//  
//  ----------------------------------------------------------------------- 
//   <copyright file="IStaticEvaluator.cs" company="Sysmex"> 
//       Copyright (c) Sysmex. All rights reserved. 
//   </copyright> 
//  -----------------------------------------------------------------------

using System.Collections.Generic;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CodeAnalysis.Core.Interfaces
{

    #region Using

    #endregion

    public interface IStaticEvaluator
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Evaluates the workflow.
        /// </summary>
        /// <param name="listeners">The listeners.</param>
        /// <param name="selectedProjets">The selected projets.</param>
        /// <param name="targetClass">The target class.</param>
        /// <param name="startMethod">The start method.</param>
        void EvaluateWorkflow(
            IList<IStaticWorkflowEvaluatorListener> listeners,
            IList<Project> selectedProjets,
            ClassDeclarationSyntax targetClass,
            MethodDeclarationSyntax startMethod);

        #endregion
    }
}