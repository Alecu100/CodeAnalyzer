//  Project              : GLP
//  Module               : RomSoft.Client.Debug.dll
//  File                 : IStaticWorkflowEvaluator.cs
//  Author               : Alecsandru
//  Last Updated         : 16/11/2015 at 17:44
//  
// 
//  Contains             : Implementation of the IStaticWorkflowEvaluator.cs class.
//  Classes              : IStaticWorkflowEvaluator.cs
// 
//  
//  ----------------------------------------------------------------------- 
//   <copyright file="IStaticWorkflowEvaluator.cs" company="Sysmex"> 
//       Copyright (c) Sysmex. All rights reserved. 
//   </copyright> 
//  -----------------------------------------------------------------------

namespace RomSoft.Client.Debug.Library.Interfaces
{
    #region Using

    using System.Collections.Generic;

    using EnvDTE;

    using Microsoft.CodeAnalysis.CSharp.Syntax;

    #endregion

    public interface IStaticWorkflowEvaluator
    {
        #region Public Methods and Operators

        /// <summary>
        /// Evaluates the workflow.
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