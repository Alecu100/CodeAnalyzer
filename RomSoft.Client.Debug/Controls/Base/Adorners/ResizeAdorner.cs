//  Project              : GLP
//  Module               : Sysmex.GLP.Client.Debug.dll
//  File                 : ResizeAdorner.cs
//  Author               : Alecsandru
//  Last Updated         : 28/10/2015 at 10:30
//  
// 
//  Contains             : Implementation of the ResizeAdorner.cs class.
//  Classes              : ResizeAdorner.cs
// 
//  
//  ----------------------------------------------------------------------- 
//   <copyright file="ResizeAdorner.cs" company="Sysmex"> 
//       Copyright (c) Sysmex. All rights reserved. 
//   </copyright> 
//  -----------------------------------------------------------------------

namespace RomSoft.Client.Debug.Controls.Base.Adorners
{
    #region Using

    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Documents;
    using System.Windows.Media;

    #endregion

    public class ResizeAdorner : Adorner
    {
        #region Fields

        private readonly ResizeChrome _chrome;

        private readonly VisualCollection _visuals;

        #endregion

        #region Constructors and Destructors

        public ResizeAdorner(ContentControl designerItem)
            : base(designerItem)
        {
            _chrome = new ResizeChrome();
            _visuals = new VisualCollection(this);
            _visuals.Add(_chrome);
            _chrome.DataContext = designerItem;
        }

        #endregion

        #region Properties

        protected override int VisualChildrenCount
        {
            get
            {
                return _visuals.Count;
            }
        }

        #endregion

        #region Protected Methods and Operators

        protected override Size ArrangeOverride(Size arrangeBounds)
        {
            _chrome.Arrange(new Rect(arrangeBounds));
            return arrangeBounds;
        }

        protected override Visual GetVisualChild(int index)
        {
            return _visuals[index];
        }

        #endregion
    }
}