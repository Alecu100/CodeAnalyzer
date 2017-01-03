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

namespace RomSoft.Client.Debug.Controls.Base
{
    #region Using

    using System.Windows;

    using RomSoft.Client.Debug.Controls.Base.Enums;

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