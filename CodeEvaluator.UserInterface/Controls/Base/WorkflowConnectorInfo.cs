//  Project              : GLP
//  Module               : Sysmex.GLP.Client.Debug.dll
//  File                 : ConnectorInfo.cs
//  Author               : Alecsandru
//  Last Updated         : 23/10/2015 at 18:17
//  
// 
//  Contains             : Implementation of the ConnectorInfo.cs class.
//  Classes              : ConnectorInfo.cs
// 
//  
//  ----------------------------------------------------------------------- 
//   <copyright file="ConnectorInfo.cs" company="Sysmex"> 
//       Copyright (c) Sysmex. All rights reserved. 
//   </copyright> 
//  -----------------------------------------------------------------------

using System.Windows;
using CodeEvaluator.UserInterface.Controls.Base.Enums;

namespace CodeEvaluator.UserInterface.Controls.Base
{

    #region Using

    #endregion

    public struct WorkflowConnectorInfo
    {
        #region Public Properties

        public double DesignerItemLeft { get; set; }

        public Size DesignerItemSize { get; set; }

        public double DesignerItemTop { get; set; }

        public EConnectorOrientation Orientation { get; set; }

        public Point Position { get; set; }

        #endregion
    }
}