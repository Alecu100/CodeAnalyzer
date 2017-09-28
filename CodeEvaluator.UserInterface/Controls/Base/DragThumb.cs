using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace CodeEvaluator.UserInterface.Controls.Base
{
    #region Using

    

    #endregion

    public class DragThumb : Thumb
    {
        #region Constructors and Destructors

        public DragThumb()
        {
            base.DragDelta += DragThumb_DragDelta;
        }

        #endregion

        #region Private Methods and Operators

        private void DragThumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            var designerItem = this.FindParent<WorkflowItem>();
            var designer = VisualTreeHelper.GetParent(designerItem) as WorkflowCanvas;
            if (designerItem != null && designer != null && designerItem.IsSelected)
            {
                double minLeft = double.MaxValue;
                double minTop = double.MaxValue;

                // we only move DesignerItems
                IEnumerable<ISelectable> designerItems = from item in designer.SelectedItems
                                                         where item is WorkflowItem
                                                         select item;

                foreach (WorkflowItem item in designerItems)
                {
                    double left = Canvas.GetLeft(item);
                    double top = Canvas.GetTop(item);

                    minLeft = double.IsNaN(left) ? 0 : Math.Min(left, minLeft);
                    minTop = double.IsNaN(top) ? 0 : Math.Min(top, minTop);
                }

                double deltaHorizontal = Math.Max(-minLeft, e.HorizontalChange);
                double deltaVertical = Math.Max(-minTop, e.VerticalChange);

                foreach (WorkflowItem item in designerItems)
                {
                    double left = Canvas.GetLeft(item);
                    double top = Canvas.GetTop(item);

                    if (double.IsNaN(left))
                    {
                        left = 0;
                    }
                    if (double.IsNaN(top))
                    {
                        top = 0;
                    }

                    Canvas.SetLeft(item, left + deltaHorizontal);
                    Canvas.SetTop(item, top + deltaVertical);
                }

                designer.InvalidateMeasure();
                e.Handled = true;
            }
        }

        #endregion
    }
}