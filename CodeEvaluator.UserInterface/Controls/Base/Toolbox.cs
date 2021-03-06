﻿using System.Windows;
using System.Windows.Controls;

namespace CodeEvaluator.UserInterface.Controls.Base
{

    #region Using

    #endregion

    // Implements ItemsControl for ToolboxItems    
    public class Toolbox : ItemsControl
    {
        // Defines the ItemHeight and ItemWidth properties of
        // the WrapPanel used for this Toolbox

        #region SpecificFields

        private Size _itemSize = new Size(50, 50);

        #endregion

        #region Public Properties

        public Size ItemSize
        {
            get { return _itemSize; }
            set { _itemSize = value; }
        }

        #endregion

        // Creates or identifies the element that is used to display the given item.        

        #region Protected Methods and Operators

        protected override DependencyObject GetContainerForItemOverride()
        {
            return new ToolboxItem();
        }

        // Determines if the specified item is (or is eligible to be) its own container.        
        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            return item is ToolboxItem;
        }

        #endregion
    }
}