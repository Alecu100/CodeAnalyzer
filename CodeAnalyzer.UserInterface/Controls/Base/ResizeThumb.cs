//  Project              : GLP
//  Module               : Sysmex.GLP.Client.Debug.dll
//  File                 : ResizeThumb.cs
//  Author               : Alecsandru
//  Last Updated         : 23/10/2015 at 18:16
//  
// 
//  Contains             : Implementation of the ResizeThumb.cs class.
//  Classes              : ResizeThumb.cs
// 
//  
//  ----------------------------------------------------------------------- 
//   <copyright file="ResizeThumb.cs" company="Sysmex"> 
//       Copyright (c) Sysmex. All rights reserved. 
//   </copyright> 
//  -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace CodeAnalyzer.UserInterface.Controls.Base
{
    #region Using

    

    #endregion

    public class ResizeThumb : Thumb
    {
        #region Constructors and Destructors

        public ResizeThumb()
        {
            base.DragDelta += ResizeThumb_DragDelta;
        }

        #endregion

        #region Private Methods and Operators

        private static void CalculateDragLimits(
            IEnumerable<ISelectable> selectedDesignerItems,
            out double minLeft,
            out double minTop,
            out double minDeltaHorizontal,
            out double minDeltaVertical)
        {
            minLeft = double.MaxValue;
            minTop = double.MaxValue;
            minDeltaHorizontal = double.MaxValue;
            minDeltaVertical = double.MaxValue;

            // drag limits are set by these parameters: canvas top, canvas left, minHeight, minWidth
            // calculate min value for each parameter for each item
            foreach (WorkflowItem item in selectedDesignerItems)
            {
                double left = Canvas.GetLeft(item);
                double top = Canvas.GetTop(item);

                minLeft = double.IsNaN(left) ? 0 : Math.Min(left, minLeft);
                minTop = double.IsNaN(top) ? 0 : Math.Min(top, minTop);

                minDeltaVertical = Math.Min(minDeltaVertical, item.ActualHeight - item.MinHeight);
                minDeltaHorizontal = Math.Min(minDeltaHorizontal, item.ActualWidth - item.MinWidth);
            }
        }

        private void ResizeThumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            var designerItem = DataContext as WorkflowItem;
            var designer = VisualTreeHelper.GetParent(designerItem) as WorkflowCanvas;

            if (designerItem != null && designer != null && designerItem.IsSelected)
            {
                double minLeft, minTop, minDeltaHorizontal, minDeltaVertical;
                double dragDeltaVertical, dragDeltaHorizontal;

                // only resize DesignerItems
                var selectedDesignerItems = from item in designer.SelectedItems where item is WorkflowItem select item;

                CalculateDragLimits(
                    selectedDesignerItems,
                    out minLeft,
                    out minTop,
                    out minDeltaHorizontal,
                    out minDeltaVertical);

                foreach (WorkflowItem item in selectedDesignerItems)
                {
                    if (item != null)
                    {
                        switch (base.VerticalAlignment)
                        {
                            case VerticalAlignment.Bottom:
                                dragDeltaVertical = Math.Min(-e.VerticalChange, minDeltaVertical);
                                item.Height = item.ActualHeight - dragDeltaVertical;
                                break;
                            case VerticalAlignment.Top:
                                double top = Canvas.GetTop(item);
                                dragDeltaVertical = Math.Min(Math.Max(-minTop, e.VerticalChange), minDeltaVertical);
                                Canvas.SetTop(item, top + dragDeltaVertical);
                                item.Height = item.ActualHeight - dragDeltaVertical;
                                break;
                            default:
                                break;
                        }

                        switch (base.HorizontalAlignment)
                        {
                            case HorizontalAlignment.Left:
                                double left = Canvas.GetLeft(item);
                                dragDeltaHorizontal = Math.Min(
                                    Math.Max(-minLeft, e.HorizontalChange),
                                    minDeltaHorizontal);
                                Canvas.SetLeft(item, left + dragDeltaHorizontal);
                                item.Width = item.ActualWidth - dragDeltaHorizontal;
                                break;
                            case HorizontalAlignment.Right:
                                dragDeltaHorizontal = Math.Min(-e.HorizontalChange, minDeltaHorizontal);
                                item.Width = item.ActualWidth - dragDeltaHorizontal;
                                break;
                            default:
                                break;
                        }
                    }
                }
                e.Handled = true;
            }
        }

        #endregion
    }
}