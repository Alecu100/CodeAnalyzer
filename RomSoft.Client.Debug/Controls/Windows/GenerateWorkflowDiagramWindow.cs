//  Project              : GLP
//  Module               : RomSoft.Client.Debug.dll
//  File                 : GenerateWorkflowDiagramWindow.cs
//  Author               : Alecsandru
//  Last Updated         : 09/12/2015 at 17:17
//  
// 
//  Contains             : Implementation of the GenerateWorkflowDiagramWindow.cs class.
//  Classes              : GenerateWorkflowDiagramWindow.cs
// 
//  
//  ----------------------------------------------------------------------- 
//   <copyright file="GenerateWorkflowDiagramWindow.cs" company="Sysmex"> 
//       Copyright (c) Sysmex. All rights reserved. 
//   </copyright> 
//  -----------------------------------------------------------------------

namespace RomSoft.Client.Debug.Controls.Windows
{
    #region Using

    using System.Runtime.InteropServices;

    using Microsoft.VisualStudio.Shell;

    using RomSoft.Client.Debug.Controls.Views;

    #endregion

    /// <summary>
    ///     This class implements the tool window exposed by this package and hosts a user control.
    ///     In Visual Studio tool windows are composed of a frame (implemented by the shell) and a pane,
    ///     usually implemented by the package implementer.
    ///     This class derives from the ToolWindowPane class provided from the MPF in order to use its
    ///     implementation of the IVsUIElementPane interface.
    /// </summary>
    [Guid("37352c55-6861-4212-a9d3-51af10e7dc1f")]
    public class GenerateWorkflowDiagramWindow : ToolWindowPane
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Standard constructor for the tool window.
        /// </summary>
        public GenerateWorkflowDiagramWindow()
            : base(null)
        {
            Caption = Resources.ToolWindowTitle;

            BitmapResourceID = 301;
            BitmapIndex = 1;

            // This is the user control hosted by the tool window; Note that, even if this class implements IDisposable,
            // we are not calling Dispose on this object. This is because ToolWindowPane calls Dispose on 
            // the object returned by the Content property.
            Content = new GenerateWorkflowDiagramControl();
        }

        #endregion
    }
}