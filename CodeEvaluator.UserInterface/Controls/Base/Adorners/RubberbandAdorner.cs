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

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace CodeEvaluator.UserInterface.Controls.Base.Adorners
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

            _adornerCanvas = new Canvas();
            _adornerCanvas.Background = Brushes.Transparent;
            _visuals = new VisualCollection(this);
            _visuals.Add(_adornerCanvas);

            _rubberband = new Rectangle();
            _rubberband.Stroke = Brushes.Navy;
            _rubberband.StrokeThickness = 1;
            _rubberband.StrokeDashArray = new DoubleCollection(new double[] {2});

            _adornerCanvas.Children.Add(_rubberband);
        }

        #endregion

        #region Properties

        protected override int VisualChildrenCount
        {
            get { return _visuals.Count; }
        }

        #endregion

        #region SpecificFields

        private readonly Canvas _adornerCanvas;

        private readonly Rectangle _rubberband;

        private readonly VisualCollection _visuals;

        private readonly WorkflowCanvas _workflowCanvas;

        private Point? _endPoint;

        private Point? _startPoint;

        #endregion

        #region Protected Methods and Operators

        protected override Size ArrangeOverride(Size arrangeBounds)
        {
            _adornerCanvas.Arrange(new Rect(arrangeBounds));
            return arrangeBounds;
        }

        protected override Visual GetVisualChild(int index)
        {
            return _visuals[index];
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                if (!IsMouseCaptured)
                {
                    CaptureMouse();
                }

                _endPoint = e.GetPosition(this);
                UpdateRubberband();
                UpdateSelection();
                e.Handled = true;
            }
        }

        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            if (IsMouseCaptured)
            {
                ReleaseMouseCapture();
            }

            var adornerLayer = Parent as AdornerLayer;
            if (adornerLayer != null)
            {
                adornerLayer.Remove(this);
            }
        }

        #endregion

        #region Private Methods and Operators

        private void UpdateRubberband()
        {
            var left = Math.Min(_startPoint.Value.X, _endPoint.Value.X);
            var top = Math.Min(_startPoint.Value.Y, _endPoint.Value.Y);

            var width = Math.Abs(_startPoint.Value.X - _endPoint.Value.X);
            var height = Math.Abs(_startPoint.Value.Y - _endPoint.Value.Y);

            _rubberband.Width = width;
            _rubberband.Height = height;
            Canvas.SetLeft(_rubberband, left);
            Canvas.SetTop(_rubberband, top);
        }

        private void UpdateSelection()
        {
            var rubberBand = new Rect(_startPoint.Value, _endPoint.Value);
            foreach (WorkflowItem item in _workflowCanvas.Children)
            {
                var itemRect = VisualTreeHelper.GetDescendantBounds(item);
                var itemBounds = item.TransformToAncestor(_workflowCanvas).TransformBounds(itemRect);

                if (rubberBand.Contains(itemBounds))
                {
                    item.IsSelected = true;
                }
                else
                {
                    item.IsSelected = false;
                }
            }
        }

        #endregion
    }
}