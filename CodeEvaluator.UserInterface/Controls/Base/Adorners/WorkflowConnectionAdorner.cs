//  Project              : GLP
//  Module               : Sysmex.GLP.Client.Debug.dll
//  File                 : ConnectionAdorner.cs
//  Author               : Alecsandru
//  Last Updated         : 23/10/2015 at 18:15
//  
// 
//  Contains             : Implementation of the ConnectionAdorner.cs class.
//  Classes              : ConnectionAdorner.cs
// 
//  
//  ----------------------------------------------------------------------- 
//   <copyright file="ConnectionAdorner.cs" company="Sysmex"> 
//       Copyright (c) Sysmex. All rights reserved. 
//   </copyright> 
//  -----------------------------------------------------------------------

using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using CodeEvaluator.UserInterface.Controls.Base.Enums;

namespace CodeEvaluator.UserInterface.Controls.Base.Adorners
{
    #region Using

    

    #endregion

    public class WorkflowConnectionAdorner : Adorner
    {
        #region SpecificFields

        private readonly Canvas _adornerCanvas;

        private readonly WorkflowConnection _workflowConnection;

        private readonly WorkflowCanvas _workflowCanvas;

        private readonly Pen _drawingPen;

        private readonly VisualCollection _visualChildren;

        private WorkflowConnector _dragWorkflowConnector;

        private WorkflowConnector _fixWorkflowConnector;

        private WorkflowConnector _hitWorkflowConnector;

        private WorkflowItem _hitWorkflowItem;

        private PathGeometry _pathGeometry;

        private Thumb _sinkDragThumb;

        private Thumb _sourceDragThumb;

        #endregion

        #region Constructors and Destructors

        public WorkflowConnectionAdorner(WorkflowCanvas workflow, WorkflowConnection workflowConnection)
            : base(workflow)
        {
            _workflowCanvas = workflow;
            _adornerCanvas = new Canvas();
            _visualChildren = new VisualCollection(this);
            _visualChildren.Add(_adornerCanvas);

            _workflowConnection = workflowConnection;
            _workflowConnection.PropertyChanged += AnchorPositionChanged;

            InitializeDragThumbs();

            _drawingPen = new Pen(Brushes.LightSlateGray, 1);
            _drawingPen.LineJoin = PenLineJoin.Round;
        }

        #endregion

        #region Properties

        protected override int VisualChildrenCount
        {
            get
            {
                return _visualChildren.Count;
            }
        }

        private WorkflowConnector HitWorkflowConnector
        {
            get
            {
                return _hitWorkflowConnector;
            }
            set
            {
                if (_hitWorkflowConnector != value)
                {
                    _hitWorkflowConnector = value;
                }
            }
        }

        private WorkflowItem HitWorkflowItem
        {
            get
            {
                return _hitWorkflowItem;
            }
            set
            {
                if (_hitWorkflowItem != value)
                {
                    if (_hitWorkflowItem != null)
                    {
                        _hitWorkflowItem.IsDragConnectionOver = false;
                    }

                    _hitWorkflowItem = value;

                    if (_hitWorkflowItem != null)
                    {
                        _hitWorkflowItem.IsDragConnectionOver = true;
                    }
                }
            }
        }

        #endregion

        #region Protected Methods and Operators

        protected override Size ArrangeOverride(Size finalSize)
        {
            _adornerCanvas.Arrange(new Rect(0, 0, _workflowCanvas.ActualWidth, _workflowCanvas.ActualHeight));
            return finalSize;
        }

        protected override Visual GetVisualChild(int index)
        {
            return _visualChildren[index];
        }

        protected override void OnRender(DrawingContext dc)
        {
            base.OnRender(dc);
            dc.DrawGeometry(null, _drawingPen, _pathGeometry);
        }

        #endregion

        #region Private Methods and Operators

        private void AnchorPositionChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("AnchorPositionSource"))
            {
                Canvas.SetLeft(_sourceDragThumb, _workflowConnection.AnchorPositionSource.X);
                Canvas.SetTop(_sourceDragThumb, _workflowConnection.AnchorPositionSource.Y);
            }

            if (e.PropertyName.Equals("AnchorPositionSink"))
            {
                Canvas.SetLeft(_sinkDragThumb, _workflowConnection.AnchorPositionSink.X);
                Canvas.SetTop(_sinkDragThumb, _workflowConnection.AnchorPositionSink.Y);
            }
        }

        private void HitTesting(Point hitPoint)
        {
            bool hitConnectorFlag = false;

            var hitObject = _workflowCanvas.InputHitTest(hitPoint) as DependencyObject;
            while (hitObject != null && hitObject != _fixWorkflowConnector.ParentWorkflowItem
                   && hitObject.GetType() != typeof(WorkflowCanvas))
            {
                if (hitObject is WorkflowConnector)
                {
                    HitWorkflowConnector = hitObject as WorkflowConnector;
                    hitConnectorFlag = true;
                }

                if (hitObject is WorkflowItem)
                {
                    HitWorkflowItem = hitObject as WorkflowItem;
                    if (!hitConnectorFlag)
                    {
                        HitWorkflowConnector = null;
                    }
                    return;
                }
                hitObject = VisualTreeHelper.GetParent(hitObject);
            }

            HitWorkflowConnector = null;
            HitWorkflowItem = null;
        }

        private void InitializeDragThumbs()
        {
            var dragThumbStyle = _workflowConnection.FindResource("ConnectionAdornerThumbStyle") as Style;

            //source drag thumb
            _sourceDragThumb = new Thumb();
            Canvas.SetLeft(_sourceDragThumb, _workflowConnection.AnchorPositionSource.X);
            Canvas.SetTop(_sourceDragThumb, _workflowConnection.AnchorPositionSource.Y);
            _adornerCanvas.Children.Add(_sourceDragThumb);
            if (dragThumbStyle != null)
            {
                _sourceDragThumb.Style = dragThumbStyle;
            }

            _sourceDragThumb.DragDelta += thumbDragThumb_DragDelta;
            _sourceDragThumb.DragStarted += thumbDragThumb_DragStarted;
            _sourceDragThumb.DragCompleted += thumbDragThumb_DragCompleted;

            // sink drag thumb
            _sinkDragThumb = new Thumb();
            Canvas.SetLeft(_sinkDragThumb, _workflowConnection.AnchorPositionSink.X);
            Canvas.SetTop(_sinkDragThumb, _workflowConnection.AnchorPositionSink.Y);
            _adornerCanvas.Children.Add(_sinkDragThumb);
            if (dragThumbStyle != null)
            {
                _sinkDragThumb.Style = dragThumbStyle;
            }

            _sinkDragThumb.DragDelta += thumbDragThumb_DragDelta;
            _sinkDragThumb.DragStarted += thumbDragThumb_DragStarted;
            _sinkDragThumb.DragCompleted += thumbDragThumb_DragCompleted;
        }

        private PathGeometry UpdatePathGeometry(Point position)
        {
            var geometry = new PathGeometry();

            EConnectorOrientation targetOrientation;
            if (HitWorkflowConnector != null)
            {
                targetOrientation = HitWorkflowConnector.Orientation;
            }
            else
            {
                targetOrientation = _dragWorkflowConnector.Orientation;
            }

            List<Point> linePoints = PathFinder.GetConnectionLine(_fixWorkflowConnector.GetInfo(), position, targetOrientation);

            if (linePoints.Count > 0)
            {
                var figure = new PathFigure();
                figure.StartPoint = linePoints[0];
                linePoints.Remove(linePoints[0]);
                figure.Segments.Add(new PolyLineSegment(linePoints, true));
                geometry.Figures.Add(figure);
            }

            return geometry;
        }

        private void thumbDragThumb_DragCompleted(object sender, DragCompletedEventArgs e)
        {
            if (HitWorkflowConnector != null)
            {
                if (_workflowConnection != null)
                {
                    if (_workflowConnection.Source == _fixWorkflowConnector)
                    {
                        _workflowConnection.Sink = HitWorkflowConnector;
                    }
                    else
                    {
                        _workflowConnection.Source = HitWorkflowConnector;
                    }
                }
            }

            HitWorkflowItem = null;
            HitWorkflowConnector = null;
            _pathGeometry = null;
            _workflowConnection.StrokeDashArray = null;
            InvalidateVisual();
        }

        private void thumbDragThumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            Point currentPosition = Mouse.GetPosition(this);
            HitTesting(currentPosition);
            _pathGeometry = UpdatePathGeometry(currentPosition);
            InvalidateVisual();
        }

        private void thumbDragThumb_DragStarted(object sender, DragStartedEventArgs e)
        {
            HitWorkflowItem = null;
            HitWorkflowConnector = null;
            _pathGeometry = null;
            Cursor = Cursors.Cross;
            _workflowConnection.StrokeDashArray = new DoubleCollection(new double[] { 1, 2 });

            if (sender == _sourceDragThumb)
            {
                _fixWorkflowConnector = _workflowConnection.Sink;
                _dragWorkflowConnector = _workflowConnection.Source;
            }
            else if (sender == _sinkDragThumb)
            {
                _dragWorkflowConnector = _workflowConnection.Sink;
                _fixWorkflowConnector = _workflowConnection.Source;
            }
        }

        #endregion
    }
}