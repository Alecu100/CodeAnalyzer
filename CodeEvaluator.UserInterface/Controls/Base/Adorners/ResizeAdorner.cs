﻿using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace CodeEvaluator.UserInterface.Controls.Base.Adorners
{

    #region Using

    #endregion

    public class ResizeAdorner : Adorner
    {
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
            get { return _visuals.Count; }
        }

        #endregion

        #region SpecificFields

        private readonly ResizeChrome _chrome;

        private readonly VisualCollection _visuals;

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