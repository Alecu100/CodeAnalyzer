namespace CodeEvaluator.UserInterface.Controls.Base
{
    using System;
    using System.Linq;
    using System.Windows.Controls;
    using System.Windows.Controls.Primitives;
    using System.Windows.Input;
    using System.Windows.Media;

    #region Using

    #endregion

    public class DragThumb : Thumb
    {
        #region Private Methods and Operators

        private void DragThumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            var workflowItem = this.FindParent<WorkflowItem>();
            var designer = VisualTreeHelper.GetParent(workflowItem) as WorkflowCanvas;
            if (workflowItem != null && designer != null && workflowItem.IsSelected)
            {
                var minLeft = double.MaxValue;
                var minTop = double.MaxValue;

                // we only move DesignerItems
                var workflowItems = from item in designer.SelectedItems where item is WorkflowItem select item;

                foreach (WorkflowItem item in workflowItems)
                {
                    var left = Canvas.GetLeft(item);
                    var top = Canvas.GetTop(item);

                    minLeft = double.IsNaN(left) ? 0 : Math.Min(left, minLeft);
                    minTop = double.IsNaN(top) ? 0 : Math.Min(top, minTop);
                }

                var deltaHorizontal = Math.Max(-minLeft, e.HorizontalChange);
                var deltaVertical = Math.Max(-minTop, e.VerticalChange);

                foreach (WorkflowItem item in workflowItems)
                {
                    var left = Canvas.GetLeft(item);
                    var top = Canvas.GetTop(item);

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

        #region Constructors and Destructors

        public DragThumb()
        {
            DragDelta += DragThumb_DragDelta;

            MouseEnter += WorkflowItem_OnMouseEnter;

            MouseLeave += WorkflowItem_OnMouseLeave;
        }

        private void WorkflowItem_OnMouseLeave(object sender, MouseEventArgs mouseEventArgs)
        {
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        private void WorkflowItem_OnMouseEnter(object sender, MouseEventArgs mouseEventArgs)
        {
            var workflowItem = this.FindParent<WorkflowItem>();

            if (workflowItem.IsSelected)
            {
                Mouse.OverrideCursor = Cursors.SizeAll;
            }
            else
            {
                Mouse.OverrideCursor = Cursors.Hand;
            }
        }

        #endregion
    }
}