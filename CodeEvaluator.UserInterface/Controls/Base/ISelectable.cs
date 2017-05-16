//  Project              : GLP
//  Module               : Sysmex.GLP.Client.Debug.dll
//  File                 : ISelectable.cs
//  Author               : Alecsandru
//  Last Updated         : 23/10/2015 at 18:12
//  
// 
//  Contains             : Implementation of the ISelectable.cs class.
//  Classes              : ISelectable.cs
// 
//  
//  ----------------------------------------------------------------------- 
//   <copyright file="ISelectable.cs" company="Sysmex"> 
//       Copyright (c) Sysmex. All rights reserved. 
//   </copyright> 
//  -----------------------------------------------------------------------

namespace CodeEvaluator.UserInterface.Controls.Base
{
    // Common interface for items that can be selected
    // on the DesignerCanvas; used by DesignerItem and Connection
    public interface ISelectable
    {
        #region Public Properties

        bool IsSelected { get; set; }

        #endregion
    }
}