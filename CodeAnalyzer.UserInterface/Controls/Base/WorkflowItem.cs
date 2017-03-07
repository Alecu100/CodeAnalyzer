﻿//  Project              : GLP
//  Module               : RomSoft.Client.Debug.dll
//  File                 : WorkflowItem.cs
//  Author               : Alecsandru
//  Last Updated         : 10/02/2016 at 16:58
//  
// 
//  Contains             : Implementation of the WorkflowItem.cs class.
//  Classes              : WorkflowItem.cs
// 
//  
//  ----------------------------------------------------------------------- 
//   <copyright file="WorkflowItem.cs" company="Sysmex"> 
//       Copyright (c) Sysmex. All rights reserved. 
//   </copyright> 
//  -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using CodeAnalyzer.UserInterface.Controls.Base.Enums;

namespace CodeAnalyzer.UserInterface.Controls.Base
{
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
        #region Static Fields

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

        #region Fields

        private readonly EWorkflowItemType _type;

        #endregion

        #region Constructors and Destructors

        public WorkflowItem(EWorkflowItemType workflowItemType)
        {
            _type = workflowItemType;

            var xamlResourcesRepository = ObjectFactory.GetInstance<IXamlResourcesRepository>();

            Style newStyle = null;

            switch (_type)
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

            Loaded += DesignerItem_Loaded;
        }

        public WorkflowItem()
        {
            Loaded += DesignerItem_Loaded;
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

        public EWorkflowItemType Type
        {
            get
            {
                return _type;
            }
        }

        public WorkflowConnector WorkflowConnectorBottom
        {
            get
            {
                WorkflowConnector workflowConnectorBottom =
                    this.FindVisualChildren<WorkflowConnector>()
                        .FirstOrDefault(connector => connector.Orientation == EConnectorOrientation.Bottom);
                return workflowConnectorBottom;
            }
        }

        public WorkflowConnector WorkflowConnectorLeft
        {
            get
            {
                WorkflowConnector workflowConnectorLeft =
                    this.FindVisualChildren<WorkflowConnector>()
                        .FirstOrDefault(connector => connector.Orientation == EConnectorOrientation.Left);
                return workflowConnectorLeft;
            }
        }

        public WorkflowConnector WorkflowConnectorRight
        {
            get
            {
                WorkflowConnector workflowConnectorRight =
                    this.FindVisualChildren<WorkflowConnector>()
                        .FirstOrDefault(connector => connector.Orientation == EConnectorOrientation.Right);
                return workflowConnectorRight;
            }
        }

        public WorkflowConnector WorkflowConnectorTop
        {
            get
            {
                WorkflowConnector workflowConnectorTop =
                    this.FindVisualChildren<WorkflowConnector>()
                        .FirstOrDefault(connector => connector.Orientation == EConnectorOrientation.Top);
                return workflowConnectorTop;
            }
        }

        #endregion

        #region Public Methods and Operators

        public static IEnumerable<T> FindLogicalChildren<T>(DependencyObject depObj) where T : DependencyObject
        {
            if (depObj != null)
            {
                List<DependencyObject> dependencyObjects =
                    LogicalTreeHelper.GetChildren(depObj).Cast<DependencyObject>().ToList();
                for (int i = 0; i < dependencyObjects.Count; i++)
                {
                    DependencyObject child = dependencyObjects[i];
                    if (child is T)
                    {
                        yield return (T)child;
                    }

                    foreach (var childOfChild in FindLogicalChildren<T>(child))
                    {
                        yield return childOfChild;
                    }
                }
            }
        }

        public static T FindParent<T>(DependencyObject child) where T : DependencyObject
        {
            //get parent item
            DependencyObject parentObject = VisualTreeHelper.GetParent(child);

            //we've reached the end of the tree
            if (parentObject == null)
            {
                return null;
            }

            //check if the parent matches the type we're looking for
            var parent = parentObject as T;
            if (parent != null)
            {
                return parent;
            }
            return FindParent<T>(parentObject);
        }

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

        #region Protected Methods and Operators

        protected override void OnPreviewMouseDown(MouseButtonEventArgs e)
        {
            base.OnPreviewMouseDown(e);
            var designer = VisualTreeHelper.GetParent(this) as WorkflowCanvas;

            // update selection
            if (designer != null)
            {
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
                    {
                        item.IsSelected = false;
                    }

                    designer.SelectedItems.Clear();
                    IsSelected = true;
                    designer.SelectedItems.Add(this);
                    designer.OnSelectionChanged();
                }
            }
            e.Handled = false;
        }

        #endregion

        #region Private Methods and Operators

        private void DesignerItem_Loaded(object sender, RoutedEventArgs e)
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
                            ControlTemplate template = GetDragThumbTemplate(contentVisual);
                            if (template != null)
                            {
                                thumb.Template = template;
                            }
                        }

                        if (connectorDecorator != null)
                        {
                            ControlTemplate template = GetConnectorDecoratorTemplate(contentVisual);
                            if (template != null)
                            {
                                connectorDecorator.Template = template;
                            }
                        }
                    }
                }
            }
        }

        private IEnumerable<T> FindVisualChildren<T>(DependencyObject obj) where T : DependencyObject
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(obj, i);
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

        private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            ApplyTemplate();
        }

        #endregion
    }
}