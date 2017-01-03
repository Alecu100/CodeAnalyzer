﻿//  Project              : GLP
//  Module               : RomSoft.Client.Debug.dll
//  File                 : IWorkflowDiagramGenerator.cs
//  Author               : Alecsandru
//  Last Updated         : 25/01/2016 at 16:27
//  
// 
//  Contains             : Implementation of the IWorkflowDiagramGenerator.cs class.
//  Classes              : IWorkflowDiagramGenerator.cs
// 
//  
//  ----------------------------------------------------------------------- 
//   <copyright file="IWorkflowDiagramGenerator.cs" company="Sysmex"> 
//       Copyright (c) Sysmex. All rights reserved. 
//   </copyright> 
//  -----------------------------------------------------------------------

namespace RomSoft.Client.Debug.Controls.Diagrams.Interfaces
{
    #region Using

    using RomSoft.Client.Debug.Controls.Base;
    using RomSoft.Debug.Diagrams;

    #endregion

    public interface IWorkflowDiagramGenerator
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Generates the workflow diagram.
        /// </summary>
        /// <param name="snapshot">The snapshot.</param>
        /// <param name="canvas">The canvas.</param>
        void GenerateWorkflowDiagram(WorkflowExecutionSnapshot snapshot, WorkflowCanvas canvas);

        #endregion
    }
}