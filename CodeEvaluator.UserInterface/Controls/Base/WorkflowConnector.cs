//  Project              : GLP
//  Module               : Sysmex.GLP.Client.Debug.dll
//  File                 : Connector.cs
//  Author               : Alecsandru
//  Last Updated         : 05/11/2015 at 11:23
//  
// 
//  Contains             : Implementation of the Connector.cs class.
//  Classes              : Connector.cs
// 
//  
//  ----------------------------------------------------------------------- 
//   <copyright file="Connector.cs" company="Sysmex"> 
//       Copyright (c) Sysmex. All rights reserved. 
//   </copyright> 
//  -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using CodeAnalyzer.UserInterface.Controls.Base.Adorners;
using CodeAnalyzer.UserInterface.Controls.Base.Enums;

namespace CodeAnalyzer.UserInterface.Controls.Base
{

    #region Using

    #endregion

    public class WorkflowConnector : Control, INotifyPropertyChanged
    {
        #region Constructors and Destructors

        public WorkflowConnector()
        {
            // fired when layout changes
            LayoutUpdated += Connector_LayoutUpdated;
        }

        #endregion

        #region Public Events

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Protected Methods and Operators

        internal WorkflowConnectorInfo GetInfo()
        {
            var info = new WorkflowConnectorInfo();
            info.DesignerItemLeft = Canvas.GetLeft(ParentWorkflowItem);
            info.DesignerItemTop = Canvas.GetTop(ParentWorkflowItem);
            info.DesignerItemSize = new Size(ParentWorkflowItem.ActualWidth, ParentWorkflowItem.ActualHeight);
            info.Orientation = Orientation;
            info.Position = Position;
            return info;
        }

        #endregion

        // drag start point, relative to the DesignerCanvas

        #region SpecificFields

        private List<WorkflowConnection> _connections;

        private Point? _dragStartPoint;

        // the DesignerItem this Connector belongs to;
        // retrieved from DataContext, which is set in the
        // DesignerItem template
        private WorkflowItem _parentWorkflowItem;

        private Point _position;

        #endregion

        #region Public Properties

        public List<WorkflowConnection> Connections
        {
            get
            {
                if (_connections == null)
                {
                    _connections = new List<WorkflowConnection>();
                }
                return _connections;
            }
        }

        public EConnectorOrientation Orientation { get; set; }

        public WorkflowItem ParentWorkflowItem
        {
            get
            {
                if (_parentWorkflowItem == null)
                {
                    _parentWorkflowItem = this.FindParent<WorkflowItem>();
                }

                return _parentWorkflowItem;
            }
        }

        public Point Position
        {
            get { return _position; }
            set
            {
                if (_position != value)
                {
                    _position = value;
                    OnPropertyChanged("Position");
                }
            }
        }

        #endregion

        // when the layout changes we update the position property

        #region Protected Methods and Operators

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            var canvas = GetDesignerCanvas(this);
            if (canvas != null)
            {
                // position relative to DesignerCanvas
                _dragStartPoint = e.GetPosition(canvas);
                e.Handled = true;
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            // if mouse button is not pressed we have no drag operation, ...
            if (e.LeftButton != MouseButtonState.Pressed)
            {
                _dragStartPoint = null;
            }

            // but if mouse button is pressed and start point value is set we do have one
            if (_dragStartPoint.HasValue)
            {
                // create connection adorner 
                var canvas = GetDesignerCanvas(this);
                if (canvas != null)
                {
                    var adornerLayer = AdornerLayer.GetAdornerLayer(canvas);
                    if (adornerLayer != null)
                    {
                        var adorner = new WorkflowConnectorAdorner(canvas, this);
                        if (adorner != null)
                        {
                            adornerLayer.Add(adorner);
                            e.Handled = true;
                        }
                    }
                }
            }
        }

        protected void OnPropertyChanged(string name)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }

        #endregion

        #region Private Methods and Operators

        private void Connector_LayoutUpdated(object sender, EventArgs e)
        {
            var workflow = GetDesignerCanvas(this);
            if (workflow != null)
            {
                //get centre position of this Connector relative to the DesignerCanvas
                Position = TransformToAncestor(workflow).Transform(new Point(Width/2, Height/2));
            }
        }

        private WorkflowCanvas GetDesignerCanvas(DependencyObject element)
        {
            while (element != null && !(element is WorkflowCanvas))
            {
                element = VisualTreeHelper.GetParent(element);
            }

            return element as WorkflowCanvas;
        }

        #endregion
    }

    // provides compact info about a connector; used for the 
    // routing algorithm, instead of hand over a full fledged Connector
}