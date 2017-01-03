//  Project              : GLP
//  Module               : Sysmex.GLP.Client.Debug.dll
//  File                 : DragableScrollViewer.cs
//  Author               : Alecsandru
//  Last Updated         : 28/10/2015 at 16:31
//  
// 
//  Contains             : Implementation of the DragableScrollViewer.cs class.
//  Classes              : DragableScrollViewer.cs
// 
//  
//  ----------------------------------------------------------------------- 
//   <copyright file="DragableScrollViewer.cs" company="Sysmex"> 
//       Copyright (c) Sysmex. All rights reserved. 
//   </copyright> 
//  -----------------------------------------------------------------------

namespace RomSoft.Client.Debug.Controls.Base
{
    #region Using

    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;

    #endregion

    public class DragableScrollViewer : ScrollViewer
    {
        #region Constants

        private const double Slowdown = 0.4; //The number 200 is found from experiments, it should be corrected

        #endregion

        #region Fields

        private bool _isDeferredMovingStarted;

        //True - Mouse down -> Mouse up without moving -> Move; False - Mouse down -> Move

        private bool _isMoving; //False - ignore mouse movements and don't scroll

        private double _startHorizontalOffset;

        private Point? _startPosition;

        private double _startVerticallOffset;

        #endregion

        #region Constructors and Destructors

        public DragableScrollViewer()
        {
            MouseLeave += OnMouseLeave;
            MouseUp += OnMouseUp;
            MouseMove += OnMouseMove;
            MouseDown += OnMouseDown;
        }

        #endregion

        #region Private Methods and Operators

        private void CancelScrolling()
        {
            _isMoving = false;
            _startPosition = null;
            _isDeferredMovingStarted = false;
            Cursor = Cursors.Arrow;
        }

        private void OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            var sv = sender as ScrollViewer;

            if (_isMoving) //Moving with a released wheel and pressing a button
            {
                CancelScrolling();
            }
            else if (e.ChangedButton == MouseButton.Middle && e.ButtonState == MouseButtonState.Pressed && sv != null)
            {
                if (_isMoving == false) //Pressing a wheel the first time
                {
                    _isMoving = true;
                    _startPosition = e.GetPosition(sender as IInputElement);
                    _isDeferredMovingStarted = true; //the default value is true until the opposite value is set
                    _startVerticallOffset = sv.VerticalOffset;
                    _startHorizontalOffset = sv.HorizontalOffset;

                    Cursor = Cursors.Hand;
                }
            }
        }

        private void OnMouseLeave(object sender, MouseEventArgs e)
        {
            CancelScrolling();
        }

        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            var sv = sender as ScrollViewer;

            if (_isMoving && sv != null)
            {
                _isDeferredMovingStarted = false; //standard scrolling (Mouse down -> Move)

                var currentPosition = e.GetPosition(sv);
                var offset = currentPosition - _startPosition.Value;
                offset.Y /= Slowdown;
                offset.X /= Slowdown;

                //if(Math.Abs(offset.Y) > 25.0/slowdown)  //Some kind of a dead space, uncomment if it is neccessary
                sv.ScrollToVerticalOffset(_startVerticallOffset - offset.Y);
                sv.ScrollToHorizontalOffset(_startHorizontalOffset - offset.X);
            }
        }

        private void OnMouseUp(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Middle && e.ButtonState == MouseButtonState.Released
                && _isDeferredMovingStarted != true)
            {
                CancelScrolling();
            }
        }

        #endregion
    }
}