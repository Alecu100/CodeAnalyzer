using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using CodeEvaluator.UserInterface.Controls.Base.Adorners;
using CodeEvaluator.UserInterface.Controls.Base.Enums;

namespace CodeEvaluator.UserInterface.Controls.Base
{
    #region Using

    #endregion

    public class WorkflowConnection : Control, ISelectable, INotifyPropertyChanged
    {
        #region Constructors and Destructors

        public WorkflowConnection(WorkflowConnector source, WorkflowConnector sink)
        {
            Source = source;
            Sink = sink;
            Unloaded += Connection_Unloaded;
        }

        #endregion

        #region Public Events

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Protected Methods and Operators

        internal void HideAdorner()
        {
            if (_connectionAdorner != null)
                _connectionAdorner.Visibility = Visibility.Collapsed;
        }

        #endregion

        #region SpecificFields

        public EArrowSymbol sinkArrowSymbol = EArrowSymbol.Arrow;

        private double _anchorAngleSink;

        private double _anchorAngleSource;

        private Point _anchorPositionSink;

        private Point _anchorPositionSource;

        private Adorner _connectionAdorner;

        private bool _isSelected;

        private Point _labelPosition;

        // source connector

        // connection path geometry
        private PathGeometry _pathGeometry;

        private WorkflowConnector _sink;

        private WorkflowConnector _source;

        private EArrowSymbol _sourceArrowSymbol = EArrowSymbol.None;

        private DoubleCollection _strokeDashArray;

        #endregion

        #region Public Properties

        public double AnchorAngleSink
        {
            get { return _anchorAngleSink; }
            set
            {
                if (_anchorAngleSink != value)
                {
                    _anchorAngleSink = value;
                    OnPropertyChanged("AnchorAngleSink");
                }
            }
        }

        public double AnchorAngleSource
        {
            get { return _anchorAngleSource; }
            set
            {
                if (_anchorAngleSource != value)
                {
                    _anchorAngleSource = value;
                    OnPropertyChanged("AnchorAngleSource");
                }
            }
        }

        // analogue to source side

        public Point AnchorPositionSink
        {
            get { return _anchorPositionSink; }
            set
            {
                if (_anchorPositionSink != value)
                {
                    _anchorPositionSink = value;
                    OnPropertyChanged("AnchorPositionSink");
                }
            }
        }

        public Point AnchorPositionSource
        {
            get { return _anchorPositionSource; }
            set
            {
                if (_anchorPositionSource != value)
                {
                    _anchorPositionSource = value;
                    OnPropertyChanged("AnchorPositionSource");
                }
            }
        }

        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                if (_isSelected != value)
                {
                    _isSelected = value;
                    OnPropertyChanged("IsSelected");
                    if (_isSelected)
                        ShowAdorner();
                    else
                        HideAdorner();
                }
            }
        }

        // analogue to source side

        public Point LabelPosition
        {
            get { return _labelPosition; }
            set
            {
                if (_labelPosition != value)
                {
                    _labelPosition = value;
                    OnPropertyChanged("LabelPosition");
                }
            }
        }

        public PathGeometry PathGeometry
        {
            get { return _pathGeometry; }
            set
            {
                if (_pathGeometry != value)
                {
                    _pathGeometry = value;
                    UpdateAnchorPosition();
                    OnPropertyChanged("PathGeometry");
                }
            }
        }

        public WorkflowConnector Sink
        {
            get { return _sink; }
            set
            {
                if (_sink != value)
                {
                    if (_sink != null)
                    {
                        _sink.PropertyChanged -= OnConnectorPositionChanged;
                        _sink.Connections.Remove(this);
                    }

                    _sink = value;

                    if (_sink != null)
                    {
                        _sink.Connections.Add(this);
                        _sink.PropertyChanged += OnConnectorPositionChanged;
                    }
                    UpdatePathGeometry();
                }
            }
        }

        public EArrowSymbol SinkArrowSymbol
        {
            get { return sinkArrowSymbol; }
            set
            {
                if (sinkArrowSymbol != value)
                {
                    sinkArrowSymbol = value;
                    OnPropertyChanged("SinkArrowSymbol");
                }
            }
        }

        public WorkflowConnector Source
        {
            get { return _source; }
            set
            {
                if (_source != value)
                {
                    if (_source != null)
                    {
                        _source.PropertyChanged -= OnConnectorPositionChanged;
                        _source.Connections.Remove(this);
                    }

                    _source = value;

                    if (_source != null)
                    {
                        _source.Connections.Add(this);
                        _source.PropertyChanged += OnConnectorPositionChanged;
                    }

                    UpdatePathGeometry();
                }
            }
        }

        public EArrowSymbol SourceArrowSymbol
        {
            get { return _sourceArrowSymbol; }
            set
            {
                if (_sourceArrowSymbol != value)
                {
                    _sourceArrowSymbol = value;
                    OnPropertyChanged("SourceArrowSymbol");
                }
            }
        }

        // pattern of dashes and gaps that is used to outline the connection path

        public DoubleCollection StrokeDashArray
        {
            get { return _strokeDashArray; }
            set
            {
                if (_strokeDashArray != value)
                {
                    _strokeDashArray = value;
                    OnPropertyChanged("StrokeDashArray");
                }
            }
        }

        #endregion

        // if connected, the ConnectionAdorner becomes visible

        #region Protected Methods and Operators

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);

            // usual selection business
            var designer = VisualTreeHelper.GetParent(this) as WorkflowCanvas;
            if (designer != null)
                if ((Keyboard.Modifiers & (ModifierKeys.Shift | ModifierKeys.Control)) != ModifierKeys.None)
                {
                    if (IsSelected)
                    {
                        IsSelected = false;
                        designer.SelectedItems.Remove(this);
                    }
                    else
                    {
                        IsSelected = true;
                        designer.SelectedItems.Add(this);
                    }
                }
                else if (!IsSelected)
                {
                    foreach (var item in designer.SelectedItems)
                        item.IsSelected = false;

                    designer.SelectedItems.Clear();
                    IsSelected = true;
                    designer.SelectedItems.Add(this);
                }
            e.Handled = false;
        }

        protected void OnPropertyChanged(string name)
        {
            var handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(name));
        }

        #endregion

        #region Private Methods and Operators

        private void Connection_Unloaded(object sender, RoutedEventArgs e)
        {
            // do some housekeeping when Connection is unloaded

            // remove event handler
            _source.PropertyChanged -= OnConnectorPositionChanged;
            _sink.PropertyChanged -= OnConnectorPositionChanged;

            // remove adorner
            if (_connectionAdorner != null)
            {
                var designer = this.FindParent<WorkflowCanvas>();

                if (designer == null)
                    return;

                var adornerLayer = AdornerLayer.GetAdornerLayer(designer);
                if (adornerLayer != null)
                {
                    adornerLayer.Remove(_connectionAdorner);
                    _connectionAdorner = null;
                }
            }
        }

        private void OnConnectorPositionChanged(object sender, PropertyChangedEventArgs e)
        {
            // whenever the 'Position' property of the source or sink Connector 
            // changes we must update the connection path geometry
            if (e.PropertyName.Equals("Position"))
                UpdatePathGeometry();
        }

        private void ShowAdorner()
        {
            // the ConnectionAdorner is created once for each Connection
            if (_connectionAdorner == null)
            {
                var designer = this.FindParent<WorkflowCanvas>();

                if (designer == null)
                    return;

                var adornerLayer = AdornerLayer.GetAdornerLayer(designer);
                if (adornerLayer != null)
                {
                    _connectionAdorner = new WorkflowConnectionAdorner(designer, this);
                    adornerLayer.Add(_connectionAdorner);
                }
            }
            _connectionAdorner.Visibility = Visibility.Visible;
        }

        private void UpdateAnchorPosition()
        {
            Point pathStartPoint, pathTangentAtStartPoint;
            Point pathEndPoint, pathTangentAtEndPoint;
            Point pathMidPoint, pathTangentAtMidPoint;

            // the PathGeometry.GetPointAtFractionLength method gets the point and a tangent vector 
            // on PathGeometry at the specified fraction of its length
            PathGeometry.GetPointAtFractionLength(0, out pathStartPoint, out pathTangentAtStartPoint);
            PathGeometry.GetPointAtFractionLength(1, out pathEndPoint, out pathTangentAtEndPoint);
            PathGeometry.GetPointAtFractionLength(0.5, out pathMidPoint, out pathTangentAtMidPoint);

            // get angle from tangent vector
            AnchorAngleSource = Math.Atan2(-pathTangentAtStartPoint.Y, -pathTangentAtStartPoint.X) * (180 / Math.PI);
            AnchorAngleSink = Math.Atan2(pathTangentAtEndPoint.Y, pathTangentAtEndPoint.X) * (180 / Math.PI);

            // add some margin on source and sink side for visual reasons only
            pathStartPoint.Offset(-pathTangentAtStartPoint.X * 5, -pathTangentAtStartPoint.Y * 5);
            pathEndPoint.Offset(pathTangentAtEndPoint.X * 5, pathTangentAtEndPoint.Y * 5);

            AnchorPositionSource = pathStartPoint;
            AnchorPositionSink = pathEndPoint;
            LabelPosition = pathMidPoint;
        }

        private void UpdatePathGeometry()
        {
            if (Source != null && Sink != null)
            {
                var geometry = new PathGeometry();
                var linePoints = PathFinder.GetConnectionLine(Source.GetInfo(), Sink.GetInfo(), true);
                if (linePoints.Count > 0)
                {
                    var figure = new PathFigure();
                    figure.StartPoint = linePoints[0];
                    linePoints.Remove(linePoints[0]);
                    figure.Segments.Add(new PolyLineSegment(linePoints, true));
                    geometry.Figures.Add(figure);

                    PathGeometry = geometry;
                }
            }
        }

        #endregion
    }
}