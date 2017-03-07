//  Project              : GLP
//  Module               : Sysmex.GLP.Client.Debug.dll
//  File                 : ResizeChrome.cs
//  Author               : Alecsandru
//  Last Updated         : 28/10/2015 at 10:29
//  
// 
//  Contains             : Implementation of the ResizeChrome.cs class.
//  Classes              : ResizeChrome.cs
// 
//  
//  ----------------------------------------------------------------------- 
//   <copyright file="ResizeChrome.cs" company="Sysmex"> 
//       Copyright (c) Sysmex. All rights reserved. 
//   </copyright> 
//  -----------------------------------------------------------------------

namespace RomSoft.Client.Debug.Controls.Base.Adorners
{
    #region Using

    using System.Windows;
    using System.Windows.Controls;

    #endregion

    public class ResizeChrome : Control
    {
        #region Constructors and Destructors

        static ResizeChrome()
        {
            DefaultStyleKeyProperty.OverrideMetadata(
                typeof(ResizeChrome),
                new FrameworkPropertyMetadata(typeof(ResizeChrome)));
        }

        #endregion
    }
}