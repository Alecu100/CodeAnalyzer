//  Project              : GLP
//  Module               : Sysmex.GLP.Client.Debug.dll
//  File                 : ConnectorAdorner.cs
//  Author               : Alecsandru
//  Last Updated         : 23/10/2015 at 18:14
//  
// 
//  Contains             : Implementation of the ConnectorAdorner.cs class.
//  Classes              : ConnectorAdorner.cs
// 
//  
//  ----------------------------------------------------------------------- 
//   <copyright file="ConnectorAdorner.cs" company="Sysmex"> 
//       Copyright (c) Sysmex. All rights reserved. 
//   </copyright> 
//  -----------------------------------------------------------------------

using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using CodeAnalyzer.UserInterface.Controls.Base.Enums;

namespace CodeAnalyzer.UserInterface.Controls.Base.Adorners
{

    #region Using

    #endregion

    public class WorkflowConnectorAdorner : Adorner
    {
        #region Constructors and Destructors

        public WorkflowConnectorAdorner(WorkflowCanvas workflow, WorkflowConnector sourceWorkflowConnector)
            : base(workflow)
        {
            _workflowCanvas = workflow;
            _sourceWorkflowConnector = sourceWorkflowConnector;
            _drawingPen = new Pen(Brushes.LightSlateGray, 1);
            _drawingPen.LineJoin = PenLineJoin.Round;
            Cursor = Cursors.Cross;
        }

        #endregion

        #region SpecificFields

        private readonly WorkflowCanvas _workflowCanvas;

        private readonly Pen _drawingPen;

        private readonly WorkflowConnector _sourceWorkflowConnector;

        private WorkflowConnector _hitWorkflowConnector;

        private WorkflowItem _hitWorkflowItem;

        private PathGeometry _pathGeometry;

        #endregion

        #region Properties

        private WorkflowConnector HitWorkflowConnector
        {
            get { return _hitWorkflowConnector; }
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
            get { return _hitWorkflowItem; }
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

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                if (!IsMouseCaptured)
                {
                    CaptureMouse();
                }
                HitTesting(e.GetPosition(this));
                _pathGeometry = GetPathGeometry(e.GetPosition(this));
                InvalidateVisual();
            }
            else
            {
                if (IsMouseCaptured)
                {
                    ReleaseMouseCapture();
                }
            }
        }

        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            if (HitWorkflowConnector != null)
            {
                var sourceWorkflowConnector = _sourceWorkflowConnector;
                var sinkWorkflowConnector = HitWorkflowConnector;
                var newConnection = new WorkflowConnection(sourceWorkflowConnector, sinkWorkflowConnector);

                // connections are added with z-index of zero
                _workflowCanvas.Children.Insert(0, newConnection);
            }
            if (HitWorkflowItem != null)
            {
                HitWorkflowItem.IsDragConnectionOver = false;
            }

            if (IsMouseCaptured)
            {
                ReleaseMouseCapture();
            }

            var adornerLayer = AdornerLayer.GetAdornerLayer(_workflowCanvas);
            if (adornerLayer != null)
            {
                adornerLayer.Remove(this);
            }
        }

        protected override void OnRender(DrawingContext dc)
        {
            base.OnRender(dc);
            dc.DrawGeometry(null, _drawingPen, _pathGeometry);

            // without a background the OnMouseMove event would not be fired
            // Alternative: implement a Canvas as a child of this adorner, like
            // the ConnectionAdorner does.
            dc.DrawRectangle(Brushes.Transparent, null, new Rect(RenderSize));
        }

        #endregion

        #region Private Methods and Operators

        private PathGeometry GetPathGeometry(Point position)
        {
            var geometry = new PathGeometry();

            EConnectorOrientation targetOrientation;
            if (HitWorkflowConnector != null)
            {
                targetOrientation = HitWorkflowConnector.Orientation;
            }
            else
            {
                targetOrientation = EConnectorOrientation.None;
            }

            var pathPoints = PathFinder.GetConnectionLine(
                _sourceWorkflowConnector.GetInfo(),
                position,
                targetOrientation);

            if (pathPoints.Count > 0)
            {
                var figure = new PathFigure();
                figure.StartPoint = pathPoints[0];
                pathPoints.Remove(pathPoints[0]);
                figure.Segments.Add(new PolyLineSegment(pathPoints, true));
                geometry.Figures.Add(figure);
            }

            return geometry;
        }

        private void HitTesting(Point hitPoint)
        {
            var hitConnectorFlag = false;

            var hitObject = _workflowCanvas.InputHitTest(hitPoint) as DependencyObject;
            while (hitObject != null && hitObject != _sourceWorkflowConnector.ParentWorkflowItem
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

        #endregion
    }
}