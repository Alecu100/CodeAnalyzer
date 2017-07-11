using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Xml;

namespace CodeEvaluator.UserInterface.Controls.Base
{
    #region Using

    

    #endregion

    public class WorkflowCanvasSelectionChangedEventArgs : EventArgs
    {
        #region Public Properties

        public List<ISelectable> SelectedItems { get; set; }

        #endregion
    }

    public delegate void SelectionChangedHandler(object sender, WorkflowCanvasSelectionChangedEventArgs args);

    public class WorkflowCanvas : Canvas
    {
        // start point of the rubberband drag operation

        #region Constants

        private const double ScaleRate = 0.1;

        #endregion

        #region SpecificFields

        private readonly ScaleTransform _scaleTransform;

        private Point? _rubberbandSelectionStartPoint;

        // keep track of selected items 
        private List<ISelectable> _selectedItems;

        #endregion

        #region Constructors and Destructors

        public WorkflowCanvas()
        {
            AllowDrop = true;
            _scaleTransform = new ScaleTransform(1, 1);
            LayoutTransform = _scaleTransform;
        }

        #endregion

        #region Public Events

        public event SelectionChangedHandler SelectionChanged;

        #endregion

        #region Public Properties

        public List<ISelectable> SelectedItems
        {
            get
            {
                if (_selectedItems == null)
                {
                    _selectedItems = new List<ISelectable>();
                }
                return _selectedItems;
            }
            set
            {
                _selectedItems = value;
            }
        }

        #endregion

        #region Public Methods and Operators

        public void OnSelectionChanged()
        {
            if (SelectionChanged != null)
            {
                SelectionChanged(this, new WorkflowCanvasSelectionChangedEventArgs { SelectedItems = _selectedItems });
            }
        }

        #endregion

        #region Protected Methods and Operators

        protected override Size MeasureOverride(Size constraint)
        {
            var size = new Size();
            foreach (UIElement element in base.Children)
            {
                double left = GetLeft(element);
                double top = GetTop(element);
                left = double.IsNaN(left) ? 0 : left;
                top = double.IsNaN(top) ? 0 : top;

                //measure desired size for each child
                element.Measure(constraint);

                Size desiredSize = element.DesiredSize;
                if (!double.IsNaN(desiredSize.Width) && !double.IsNaN(desiredSize.Height))
                {
                    size.Width = Math.Max(size.Width, left + desiredSize.Width);
                    size.Height = Math.Max(size.Height, top + desiredSize.Height);
                }
            }
            // add margin 
            size.Width += 10;
            size.Height += 10;
            return size;
        }

        protected override void OnDrop(DragEventArgs e)
        {
            base.OnDrop(e);
            var dragObject = e.Data.GetData(typeof(DragObject)) as DragObject;
            if (dragObject != null && !String.IsNullOrEmpty(dragObject.Xaml))
            {
                WorkflowItem newItem = null;
                Object content = XamlReader.Load(XmlReader.Create(new StringReader(dragObject.Xaml)));

                if (content != null)
                {
                    newItem = new WorkflowItem();
                    newItem.Content = content;

                    Point position = e.GetPosition(this);

                    if (dragObject.DesiredSize.HasValue)
                    {
                        Size desiredSize = dragObject.DesiredSize.Value;
                        newItem.Width = desiredSize.Width;
                        newItem.Height = desiredSize.Height;

                        SetLeft(newItem, Math.Max(0, position.X - newItem.Width / 2));
                        SetTop(newItem, Math.Max(0, position.Y - newItem.Height / 2));
                    }
                    else
                    {
                        SetLeft(newItem, Math.Max(0, position.X));
                        SetTop(newItem, Math.Max(0, position.Y));
                    }

                    Children.Add(newItem);

                    //update selection
                    foreach (var item in SelectedItems)
                    {
                        item.IsSelected = false;
                    }
                    SelectedItems.Clear();
                    newItem.IsSelected = true;
                    SelectedItems.Add(newItem);
                }

                e.Handled = true;
            }
        }

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);
            if (e.Source == this)
            {
                // in case that this click is the start for a 
                // drag operation we cache the start point
                _rubberbandSelectionStartPoint = e.GetPosition(this);

                // if you click directly on the canvas all 
                // selected items are 'de-selected'
                foreach (var item in SelectedItems)
                {
                    item.IsSelected = false;
                }
                _selectedItems.Clear();

                e.Handled = true;
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            // if mouse button is not pressed we have no drag operation, ...
            if (e.LeftButton != MouseButtonState.Pressed)
            {
                _rubberbandSelectionStartPoint = null;
            }

            // ... but if mouse button is pressed and start
            // point value is set we do have one
            if (_rubberbandSelectionStartPoint.HasValue)
            {
                // create rubberband adorner
                AdornerLayer adornerLayer = AdornerLayer.GetAdornerLayer(this);
                if (adornerLayer != null)
                {
                    var adorner = new RubberbandAdorner(this, _rubberbandSelectionStartPoint);
                    if (adorner != null)
                    {
                        adornerLayer.Add(adorner);
                    }
                }
            }
            e.Handled = true;
        }

        #endregion
    }
}