﻿//  Project              : GLP
//  Module               : Sysmex.GLP.Client.Debug.dll
//  File                 : RelativePositionPanel.cs
//  Author               : Alecsandru
//  Last Updated         : 23/10/2015 at 18:00
//  
// 
//  Contains             : Implementation of the RelativePositionPanel.cs class.
//  Classes              : RelativePositionPanel.cs
// 
//  
//  ----------------------------------------------------------------------- 
//   <copyright file="RelativePositionPanel.cs" company="Sysmex"> 
//       Copyright (c) Sysmex. All rights reserved. 
//   </copyright> 
//  -----------------------------------------------------------------------

namespace RomSoft.Client.Debug.Controls.Base
{
    #region Using

    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;

    #endregion

    public class RelativePositionPanel : Panel
    {
        #region Static Fields

        public static readonly DependencyProperty RelativePositionProperty =
            DependencyProperty.RegisterAttached(
                "RelativePosition",
                typeof(Point),
                typeof(RelativePositionPanel),
                new FrameworkPropertyMetadata(new Point(0, 0), OnRelativePositionChanged));

        #endregion

        #region Public Methods and Operators

        public static Point GetRelativePosition(UIElement element)
        {
            if (element == null)
            {
                throw new ArgumentNullException("element");
            }
            return (Point)element.GetValue(RelativePositionProperty);
        }

        public static void SetRelativePosition(UIElement element, Point value)
        {
            if (element == null)
            {
                throw new ArgumentNullException("element");
            }
            element.SetValue(RelativePositionProperty, value);
        }

        #endregion

        #region Protected Methods and Operators

        protected override Size ArrangeOverride(Size arrangeSize)
        {
            foreach (UIElement element in base.InternalChildren)
            {
                if (element != null)
                {
                    Point relPosition = GetRelativePosition(element);
                    double x = (arrangeSize.Width - element.DesiredSize.Width) * relPosition.X;
                    double y = (arrangeSize.Height - element.DesiredSize.Height) * relPosition.Y;

                    if (double.IsNaN(x))
                    {
                        x = 0;
                    }
                    if (double.IsNaN(y))
                    {
                        y = 0;
                    }

                    element.Arrange(new Rect(new Point(x, y), element.DesiredSize));
                }
            }
            return arrangeSize;
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            var size = new Size(double.PositiveInfinity, double.PositiveInfinity);

            // SDK docu says about InternalChildren Property: 'Classes that are derived from Panel 
            // should use this property, instead of the Children property, for internal overrides 
            // such as MeasureCore and ArrangeCore.

            foreach (UIElement element in InternalChildren)
            {
                if (element != null)
                {
                    element.Measure(size);
                }
            }

            return base.MeasureOverride(availableSize);
        }

        #endregion

        #region Private Methods and Operators

        private static void OnRelativePositionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var reference = d as UIElement;
            if (reference != null)
            {
                var parent = VisualTreeHelper.GetParent(reference) as RelativePositionPanel;
                if (parent != null)
                {
                    parent.InvalidateArrange();
                }
            }
        }

        #endregion
    }
}