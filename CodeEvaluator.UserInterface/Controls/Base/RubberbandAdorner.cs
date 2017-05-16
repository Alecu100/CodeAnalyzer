//  Project              : GLP
//  Module               : Sysmex.GLP.Client.Debug.dll
//  File                 : RubberbandAdorner.cs
//  Author               : Alecsandru
//  Last Updated         : 05/11/2015 at 14:09
//  
// 
//  Contains             : Implementation of the RubberbandAdorner.cs class.
//  Classes              : RubberbandAdorner.cs
// 
//  
//  ----------------------------------------------------------------------- 
//   <copyright file="RubberbandAdorner.cs" company="Sysmex"> 
//       Copyright (c) Sysmex. All rights reserved. 
//   </copyright> 
//  -----------------------------------------------------------------------

using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

namespace CodeEvaluator.UserInterface.Controls.Base
{

    #region Using

    #endregion

    public class RubberbandAdorner : Adorner
    {
        #region Constructors and Destructors

        public RubberbandAdorner(WorkflowCanvas workflowCanvas, Point? dragStartPoint)
            : base(workflowCanvas)
        {
            _workflowCanvas = workflowCanvas;
            _startPoint = dragStartPoint;
            _rubberbandPen = new Pen(Brushes.LightSlateGray, 1);
            _rubberbandPen.DashStyle = new DashStyle(new double[] {2}, 1);
        }

        #endregion

        #region Private Methods and Operators

        private void UpdateSelection()
        {
            foreach (var item in _workflowCanvas.SelectedItems)
            {
                item.IsSelected = false;
            }
            _workflowCanvas.SelectedItems.Clear();

            var rubberBand = new Rect(_startPoint.Value, _endPoint.Value);
            foreach (Control item in _workflowCanvas.Children)
            {
                var itemRect = VisualTreeHelper.GetDescendantBounds(item);
                var itemBounds = item.TransformToAncestor(_workflowCanvas).TransformBounds(itemRect);

                if (rubberBand.Contains(itemBounds) && item is ISelectable)
                {
                    var selectableItem = item as ISelectable;
                    selectableItem.IsSelected = true;
                    _workflowCanvas.SelectedItems.Add(selectableItem);
                }
            }
        }

        #endregion

        #region SpecificFields

        private readonly Pen _rubberbandPen;

        private readonly WorkflowCanvas _workflowCanvas;

        private Point? _endPoint;

        private Point? _startPoint;

        #endregion

        #region Protected Methods and Operators

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                if (!IsMouseCaptured)
                {
                    CaptureMouse();
                }

                _endPoint = e.GetPosition(this);
                UpdateSelection();
                InvalidateVisual();
            }
            else
            {
                if (IsMouseCaptured)
                {
                    ReleaseMouseCapture();
                }
            }

            e.Handled = true;
        }

        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            // release mouse capture
            if (IsMouseCaptured)
            {
                ReleaseMouseCapture();
            }

            // remove this adorner from adorner layer
            var adornerLayer = AdornerLayer.GetAdornerLayer(_workflowCanvas);
            if (adornerLayer != null)
            {
                adornerLayer.Remove(this);
            }

            e.Handled = true;
        }

        protected override void OnRender(DrawingContext dc)
        {
            base.OnRender(dc);

            // without a background the OnMouseMove event would not be fired !
            // Alternative: implement a Canvas as a child of this adorner, like
            // the ConnectionAdorner does.
            dc.DrawRectangle(Brushes.Transparent, null, new Rect(RenderSize));

            if (_startPoint.HasValue && _endPoint.HasValue)
            {
                dc.DrawRectangle(Brushes.Transparent, _rubberbandPen, new Rect(_startPoint.Value, _endPoint.Value));
            }
        }

        #endregion
    }
}