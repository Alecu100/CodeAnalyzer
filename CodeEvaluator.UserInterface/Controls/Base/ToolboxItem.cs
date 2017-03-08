//  Project              : GLP
//  Module               : Sysmex.GLP.Client.Debug.dll
//  File                 : ToolboxItem.cs
//  Author               : Alecsandru
//  Last Updated         : 23/10/2015 at 18:11
//  
// 
//  Contains             : Implementation of the ToolboxItem.cs class.
//  Classes              : ToolboxItem.cs
// 
//  
//  ----------------------------------------------------------------------- 
//   <copyright file="ToolboxItem.cs" company="Sysmex"> 
//       Copyright (c) Sysmex. All rights reserved. 
//   </copyright> 
//  -----------------------------------------------------------------------

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;

namespace CodeAnalyzer.UserInterface.Controls.Base
{
    #region Using

    

    #endregion

    // Represents a selectable item in the Toolbox/>.
    public class ToolboxItem : ContentControl
    {
        // caches the start point of the drag operation

        #region Fields

        private Point? _dragStartPoint;

        #endregion

        #region Constructors and Destructors

        static ToolboxItem()
        {
            // set the key to reference the style for this control
            DefaultStyleKeyProperty.OverrideMetadata(
                typeof(ToolboxItem),
                new FrameworkPropertyMetadata(typeof(ToolboxItem)));
        }

        #endregion

        #region Protected Methods and Operators

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (e.LeftButton != MouseButtonState.Pressed)
            {
                _dragStartPoint = null;
            }

            if (_dragStartPoint.HasValue)
            {
                // XamlWriter.Save() has limitations in exactly what is serialized,
                // see SDK documentation; short term solution only;
                string xamlString = XamlWriter.Save(Content);
                var dataObject = new DragObject();
                dataObject.Xaml = xamlString;

                var panel = VisualTreeHelper.GetParent(this) as WrapPanel;
                if (panel != null)
                {
                    // desired size for DesignerCanvas is the stretched Toolbox item size
                    double scale = 1.3;
                    dataObject.DesiredSize = new Size(panel.ItemWidth * scale, panel.ItemHeight * scale);
                }

                DragDrop.DoDragDrop(this, dataObject, DragDropEffects.Copy);

                e.Handled = true;
            }
        }

        protected override void OnPreviewMouseDown(MouseButtonEventArgs e)
        {
            base.OnPreviewMouseDown(e);
            _dragStartPoint = e.GetPosition(this);
        }

        #endregion
    }

    // Wraps info of the dragged object into a class
    public class DragObject
    {
        // Xaml string that represents the serialized content

        // Defines width and height of the DesignerItem
        // when this DragObject is dropped on the DesignerCanvas

        #region Public Properties

        public Size? DesiredSize { get; set; }

        public String Xaml { get; set; }

        #endregion
    }
}