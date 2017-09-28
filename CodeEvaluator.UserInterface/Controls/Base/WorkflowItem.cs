namespace CodeEvaluator.UserInterface.Controls.Base
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Media;

    using CodeEvaluator.UserInterface.Controls.Base.Enums;
    using CodeEvaluator.UserInterface.Interfaces;

    using StructureMap;

    #region Using

    #endregion

    #region Using

    #region Using

    #region Using

    #endregion

    #endregion

    #endregion

    //These attributes identify the types of the named parts that are used for templating
    [TemplatePart(Name = "PART_DragThumb", Type = typeof(DragThumb))]
    [TemplatePart(Name = "PART_ResizeDecorator", Type = typeof(Control))]
    [TemplatePart(Name = "PART_ConnectorDecorator", Type = typeof(Control))]
    [TemplatePart(Name = "PART_ContentPresenter", Type = typeof(ContentPresenter))]
    public class WorkflowItem : ContentControl, ISelectable
    {
        #region SpecificFields

        #endregion

        #region Static SpecificFields

        public static readonly DependencyProperty ConnectorDecoratorTemplateProperty =
            DependencyProperty.RegisterAttached(
                "ConnectorDecoratorTemplate",
                typeof(ControlTemplate),
                typeof(WorkflowItem));

        public static readonly DependencyProperty DragThumbTemplateProperty =
            DependencyProperty.RegisterAttached("DragThumbTemplate", typeof(ControlTemplate), typeof(WorkflowItem));

        public static readonly DependencyProperty IsDragConnectionOverProperty =
            DependencyProperty.Register(
                "IsDragConnectionOver",
                typeof(bool),
                typeof(WorkflowItem),
                new FrameworkPropertyMetadata(false));

        public static readonly DependencyProperty IsSelectedProperty = DependencyProperty.Register(
            "IsSelected",
            typeof(bool),
            typeof(WorkflowItem),
            new FrameworkPropertyMetadata(false));


        public static readonly DependencyProperty LabelProperty = DependencyProperty.Register(
            "Label",
            typeof(string),
            typeof(WorkflowItem),
            new FrameworkPropertyMetadata(null));

        #endregion

        #region Constructors and Destructors

        public WorkflowItem(EWorkflowItemType workflowItemType)
        {
            Type = workflowItemType;

            var xamlResourcesRepository = ObjectFactory.GetInstance<IXamlResourcesRepository>();

            Style newStyle = null;

            switch (Type)
            {
                case EWorkflowItemType.Process:
                    newStyle = (Style)xamlResourcesRepository.FindResource("WorkflowItemProcess");
                    break;
                case EWorkflowItemType.Decission:
                    newStyle = (Style)xamlResourcesRepository.FindResource("WorkflowItemDecission");
                    break;
                case EWorkflowItemType.Start:
                case EWorkflowItemType.Stop:
                    newStyle = (Style)xamlResourcesRepository.FindResource("WorkflowItemStartStop");
                    break;
            }

            Style = newStyle;

            Initialized += OnInitialized;

            Loaded += WorkflowItem_Loaded;
        }

        private void WorkflowItem_MouseLeave(object sender, MouseEventArgs mouseEventArgs)
        {
            var grdConnectors = this.FindVisualChildren<Grid>().First(x => x.Name == "grdConnectors");

            if (IsDragConnectionOver == false)
            {
                grdConnectors.Visibility = Visibility.Hidden;
            }
        }

        private void WorkflowItem_MouseEnter(object sender, MouseEventArgs mouseEventArgs)
        {
            var grdConnectors = this.FindVisualChildren<Grid>().First(x => x.Name == "grdConnectors");

            grdConnectors.Visibility = Visibility.Visible;
        }

        public WorkflowItem()
        {
            Loaded += WorkflowItem_Loaded;
        }

        static WorkflowItem()
        {
            // set the key to reference the style for this control
            DefaultStyleKeyProperty.OverrideMetadata(
                typeof(WorkflowItem),
                new FrameworkPropertyMetadata(typeof(WorkflowItem)));
        }

        #endregion

        #region Public Properties

        public bool IsDragConnectionOver
        {
            get
            {
                return (bool)GetValue(IsDragConnectionOverProperty);
            }
            set
            {
                SetValue(IsDragConnectionOverProperty, value);

                var grdConnectors = this.FindVisualChildren<Grid>().First(x => x.Name == "grdConnectors");
                if (value == false)
                {
                    grdConnectors.Visibility = Visibility.Hidden;
                }
                else
                {
                    grdConnectors.Visibility = Visibility.Visible;
                }
            }
        }

        public bool IsSelected
        {
            get
            {
                return (bool)GetValue(IsSelectedProperty);
            }
            set
            {
                SetValue(IsSelectedProperty, value);
            }
        }

        public string Label
        {
            get
            {
                return (string)GetValue(LabelProperty);
            }
            set
            {
                SetValue(LabelProperty, value);
            }
        }

        public EWorkflowItemType Type { get; }

        public WorkflowConnector WorkflowConnectorBottom
        {
            get
            {
                var workflowConnectorBottom =
                    this.FindVisualChildren<WorkflowConnector>()
                        .FirstOrDefault(connector => connector.Orientation == EConnectorOrientation.Bottom);
                return workflowConnectorBottom;
            }
        }

        public WorkflowConnector WorkflowConnectorLeft
        {
            get
            {
                var workflowConnectorLeft =
                    this.FindVisualChildren<WorkflowConnector>()
                        .FirstOrDefault(connector => connector.Orientation == EConnectorOrientation.Left);
                return workflowConnectorLeft;
            }
        }

        public WorkflowConnector WorkflowConnectorRight
        {
            get
            {
                var workflowConnectorRight =
                    this.FindVisualChildren<WorkflowConnector>()
                        .FirstOrDefault(connector => connector.Orientation == EConnectorOrientation.Right);
                return workflowConnectorRight;
            }
        }

        public WorkflowConnector WorkflowConnectorTop
        {
            get
            {
                var workflowConnectorTop =
                    this.FindVisualChildren<WorkflowConnector>()
                        .FirstOrDefault(connector => connector.Orientation == EConnectorOrientation.Top);
                return workflowConnectorTop;
            }
        }

        #endregion

        #region Public Methods and Operators

        public static ControlTemplate GetConnectorDecoratorTemplate(UIElement element)
        {
            return (ControlTemplate)element.GetValue(ConnectorDecoratorTemplateProperty);
        }

        public static ControlTemplate GetDragThumbTemplate(UIElement element)
        {
            return (ControlTemplate)element.GetValue(DragThumbTemplateProperty);
        }

        public static void SetConnectorDecoratorTemplate(UIElement element, ControlTemplate value)
        {
            element.SetValue(ConnectorDecoratorTemplateProperty, value);
        }

        public static void SetDragThumbTemplate(UIElement element, ControlTemplate value)
        {
            element.SetValue(DragThumbTemplateProperty, value);
        }

        #endregion

        #region Private Methods and Operators

        private void WorkflowItem_Loaded(object sender, RoutedEventArgs e)
        {
            // if DragThumbTemplate and ConnectorDecoratorTemplate properties of this class
            // are set these templates are applied; 
            // Note: this method is only executed when the Loaded event is fired, so
            // setting DragThumbTemplate or ConnectorDecoratorTemplate properties after
            // will have no effect.
            if (Template != null)
            {
                var contentPresenter = Template.FindName("PART_ContentPresenter", this) as ContentPresenter;
                if (contentPresenter != null)
                {
                    var contentVisual = VisualTreeHelper.GetChild(contentPresenter, 0) as UIElement;
                    if (contentVisual != null)
                    {
                        var thumb = Template.FindName("PART_DragThumb", this) as DragThumb;
                        var connectorDecorator = Template.FindName("PART_ConnectorDecorator", this) as Control;

                        if (thumb != null)
                        {
                            var template = GetDragThumbTemplate(contentVisual);
                            if (template != null)
                            {
                                thumb.Template = template;
                            }
                        }

                        if (connectorDecorator != null)
                        {
                            var template = GetConnectorDecoratorTemplate(contentVisual);
                            if (template != null)
                            {
                                connectorDecorator.Template = template;
                            }
                        }
                    }
                }
            }

            var pathIcon = this.FindVisualChildren<Grid>().First(x => x.Name == "grdContent");

            pathIcon.MouseEnter += WorkflowItem_MouseEnter;

            pathIcon.MouseLeave += WorkflowItem_MouseLeave;
        }

        private IEnumerable<T> FindVisualChildren<T>(DependencyObject obj) where T : DependencyObject
        {
            for (var i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
            {
                var child = VisualTreeHelper.GetChild(obj, i);
                if (child is T)
                {
                    yield return (T)child;
                }

                foreach (var childOfChild in FindVisualChildren<T>(child))
                {
                    yield return childOfChild;
                }
            }
        }

        private void OnInitialized(object sender, EventArgs eventArgs)
        {
            ApplyTemplate();
        }

        #endregion
    }
}