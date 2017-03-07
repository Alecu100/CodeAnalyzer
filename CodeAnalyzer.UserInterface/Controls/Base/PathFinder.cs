﻿//  Project              : GLP
//  Module               : Sysmex.GLP.Client.Debug.dll
//  File                 : PathFinder.cs
//  Author               : Alecsandru
//  Last Updated         : 26/10/2015 at 11:02
//  
// 
//  Contains             : Implementation of the PathFinder.cs class.
//  Classes              : PathFinder.cs
// 
//  
//  ----------------------------------------------------------------------- 
//   <copyright file="PathFinder.cs" company="Sysmex"> 
//       Copyright (c) Sysmex. All rights reserved. 
//   </copyright> 
//  -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Windows.Controls;
using CodeAnalyzer.UserInterface.Controls.Base.Enums;

namespace CodeAnalyzer.UserInterface.Controls.Base
{

    #region Using

    #endregion

    // Note: I couldn't find a useful open source library that does
    // orthogonal routing so started to write something on my own.
    // Categorize this as a quick and dirty short term solution.
    // I will keep on searching.

    // Helper class to provide an orthogonal connection path
    internal class PathFinder
    {
        #region Constants

        private const int Margin = 20;

        #endregion

        #region Public Methods and Operators

        public static List<Point> GetConnectionLine(WorkflowConnectorInfo source, WorkflowConnectorInfo sink, bool showLastLine)
        {
            var linePoints = new List<Point>();

            Rect rectSource = GetRectWithMargin(source, Margin);
            Rect rectSink = GetRectWithMargin(sink, Margin);

            Point startPoint = GetOffsetPoint(source, rectSource);
            Point endPoint = GetOffsetPoint(sink, rectSink);

            linePoints.Add(startPoint);
            Point currentPoint = startPoint;

            if (!rectSink.Contains(currentPoint) && !rectSource.Contains(endPoint))
            {
                while (true)
                {
                    #region source node

                    if (IsPointVisible(currentPoint, endPoint, new[] { rectSource, rectSink }))
                    {
                        linePoints.Add(endPoint);
                        currentPoint = endPoint;
                        break;
                    }

                    Point neighbour = GetNearestVisibleNeighborSink(currentPoint, endPoint, sink, rectSource, rectSink);
                    if (!double.IsNaN(neighbour.X))
                    {
                        linePoints.Add(neighbour);
                        linePoints.Add(endPoint);
                        currentPoint = endPoint;
                        break;
                    }

                    if (currentPoint == startPoint)
                    {
                        bool flag;
                        Point n = GetNearestNeighborSource(source, endPoint, rectSource, rectSink, out flag);
                        linePoints.Add(n);
                        currentPoint = n;

                        if (!IsRectVisible(currentPoint, rectSink, new[] { rectSource }))
                        {
                            Point n1, n2;
                            GetOppositeCorners(source.Orientation, rectSource, out n1, out n2);
                            if (flag)
                            {
                                linePoints.Add(n1);
                                currentPoint = n1;
                            }
                            else
                            {
                                linePoints.Add(n2);
                                currentPoint = n2;
                            }
                            if (!IsRectVisible(currentPoint, rectSink, new[] { rectSource }))
                            {
                                if (flag)
                                {
                                    linePoints.Add(n2);
                                    currentPoint = n2;
                                }
                                else
                                {
                                    linePoints.Add(n1);
                                    currentPoint = n1;
                                }
                            }
                        }
                    }
                        #endregion

                        #region sink node

                    else // from here on we jump to the sink node
                    {
                        Point n1, n2; // neighbour corner
                        Point s1, s2; // opposite corner
                        GetNeighborCorners(sink.Orientation, rectSink, out s1, out s2);
                        GetOppositeCorners(sink.Orientation, rectSink, out n1, out n2);

                        bool n1Visible = IsPointVisible(currentPoint, n1, new[] { rectSource, rectSink });
                        bool n2Visible = IsPointVisible(currentPoint, n2, new[] { rectSource, rectSink });

                        if (n1Visible && n2Visible)
                        {
                            if (rectSource.Contains(n1))
                            {
                                linePoints.Add(n2);
                                if (rectSource.Contains(s2))
                                {
                                    linePoints.Add(n1);
                                    linePoints.Add(s1);
                                }
                                else
                                {
                                    linePoints.Add(s2);
                                }

                                linePoints.Add(endPoint);
                                currentPoint = endPoint;
                                break;
                            }

                            if (rectSource.Contains(n2))
                            {
                                linePoints.Add(n1);
                                if (rectSource.Contains(s1))
                                {
                                    linePoints.Add(n2);
                                    linePoints.Add(s2);
                                }
                                else
                                {
                                    linePoints.Add(s1);
                                }

                                linePoints.Add(endPoint);
                                currentPoint = endPoint;
                                break;
                            }

                            if ((Distance(n1, endPoint) <= Distance(n2, endPoint)))
                            {
                                linePoints.Add(n1);
                                if (rectSource.Contains(s1))
                                {
                                    linePoints.Add(n2);
                                    linePoints.Add(s2);
                                }
                                else
                                {
                                    linePoints.Add(s1);
                                }
                                linePoints.Add(endPoint);
                                currentPoint = endPoint;
                                break;
                            }
                            linePoints.Add(n2);
                            if (rectSource.Contains(s2))
                            {
                                linePoints.Add(n1);
                                linePoints.Add(s1);
                            }
                            else
                            {
                                linePoints.Add(s2);
                            }
                            linePoints.Add(endPoint);
                            currentPoint = endPoint;
                            break;
                        }
                        if (n1Visible)
                        {
                            linePoints.Add(n1);
                            if (rectSource.Contains(s1))
                            {
                                linePoints.Add(n2);
                                linePoints.Add(s2);
                            }
                            else
                            {
                                linePoints.Add(s1);
                            }
                            linePoints.Add(endPoint);
                            currentPoint = endPoint;
                            break;
                        }
                        linePoints.Add(n2);
                        if (rectSource.Contains(s2))
                        {
                            linePoints.Add(n1);
                            linePoints.Add(s1);
                        }
                        else
                        {
                            linePoints.Add(s2);
                        }
                        linePoints.Add(endPoint);
                        currentPoint = endPoint;
                        break;
                    }

                    #endregion
                }
            }
            else
            {
                linePoints.Add(endPoint);
            }

            linePoints = OptimizeLinePoints(
                linePoints,
                new[] { rectSource, rectSink },
                source.Orientation,
                sink.Orientation);

            CheckPathEnd(source, sink, showLastLine, linePoints);
            return linePoints;
        }

        public static List<Point> GetConnectionLine(
            WorkflowConnectorInfo source,
            Point sinkPoint,
            EConnectorOrientation preferredOrientation)
        {
            var linePoints = new List<Point>();
            Rect rectSource = GetRectWithMargin(source, 10);
            Point startPoint = GetOffsetPoint(source, rectSource);
            Point endPoint = sinkPoint;

            linePoints.Add(startPoint);
            Point currentPoint = startPoint;

            if (!rectSource.Contains(endPoint))
            {
                while (true)
                {
                    if (IsPointVisible(currentPoint, endPoint, new[] { rectSource }))
                    {
                        linePoints.Add(endPoint);
                        break;
                    }

                    bool sideFlag;
                    Point n = GetNearestNeighborSource(source, endPoint, rectSource, out sideFlag);
                    linePoints.Add(n);
                    currentPoint = n;

                    if (IsPointVisible(currentPoint, endPoint, new[] { rectSource }))
                    {
                        linePoints.Add(endPoint);
                        break;
                    }
                    Point n1, n2;
                    GetOppositeCorners(source.Orientation, rectSource, out n1, out n2);
                    if (sideFlag)
                    {
                        linePoints.Add(n1);
                    }
                    else
                    {
                        linePoints.Add(n2);
                    }

                    linePoints.Add(endPoint);
                    break;
                }
            }
            else
            {
                linePoints.Add(endPoint);
            }

            if (preferredOrientation != EConnectorOrientation.None)
            {
                linePoints = OptimizeLinePoints(
                    linePoints,
                    new[] { rectSource },
                    source.Orientation,
                    preferredOrientation);
            }
            else
            {
                linePoints = OptimizeLinePoints(
                    linePoints,
                    new[] { rectSource },
                    source.Orientation,
                    GetOpositeOrientation(source.Orientation));
            }

            return linePoints;
        }

        #endregion

        #region Private Methods and Operators

        private static void CheckPathEnd(
            WorkflowConnectorInfo source,
            WorkflowConnectorInfo sink,
            bool showLastLine,
            List<Point> linePoints)
        {
            if (showLastLine)
            {
                var startPoint = new Point(0, 0);
                var endPoint = new Point(0, 0);
                double marginPath = 15;
                switch (source.Orientation)
                {
                    case EConnectorOrientation.Left:
                        startPoint = new Point(source.Position.X - marginPath, source.Position.Y);
                        break;
                    case EConnectorOrientation.Top:
                        startPoint = new Point(source.Position.X, source.Position.Y - marginPath);
                        break;
                    case EConnectorOrientation.Right:
                        startPoint = new Point(source.Position.X + marginPath, source.Position.Y);
                        break;
                    case EConnectorOrientation.Bottom:
                        startPoint = new Point(source.Position.X, source.Position.Y + marginPath);
                        break;
                    default:
                        break;
                }

                switch (sink.Orientation)
                {
                    case EConnectorOrientation.Left:
                        endPoint = new Point(sink.Position.X - marginPath, sink.Position.Y);
                        break;
                    case EConnectorOrientation.Top:
                        endPoint = new Point(sink.Position.X, sink.Position.Y - marginPath);
                        break;
                    case EConnectorOrientation.Right:
                        endPoint = new Point(sink.Position.X + marginPath, sink.Position.Y);
                        break;
                    case EConnectorOrientation.Bottom:
                        endPoint = new Point(sink.Position.X, sink.Position.Y + marginPath);
                        break;
                    default:
                        break;
                }
                linePoints.Insert(0, startPoint);
                linePoints.Add(endPoint);
            }
            else
            {
                linePoints.Insert(0, source.Position);
                linePoints.Add(sink.Position);
            }
        }

        private static double Distance(Point p1, Point p2)
        {
            return Point.Subtract(p1, p2).Length;
        }

        private static Point GetNearestNeighborSource(
            WorkflowConnectorInfo source,
            Point endPoint,
            Rect rectSource,
            Rect rectSink,
            out bool flag)
        {
            Point n1, n2; // neighbors
            GetNeighborCorners(source.Orientation, rectSource, out n1, out n2);

            if (rectSink.Contains(n1))
            {
                flag = false;
                return n2;
            }

            if (rectSink.Contains(n2))
            {
                flag = true;
                return n1;
            }

            if ((Distance(n1, endPoint) <= Distance(n2, endPoint)))
            {
                flag = true;
                return n1;
            }
            flag = false;
            return n2;
        }

        private static Point GetNearestNeighborSource(
            WorkflowConnectorInfo source,
            Point endPoint,
            Rect rectSource,
            out bool flag)
        {
            Point n1, n2; // neighbors
            GetNeighborCorners(source.Orientation, rectSource, out n1, out n2);

            if ((Distance(n1, endPoint) <= Distance(n2, endPoint)))
            {
                flag = true;
                return n1;
            }
            flag = false;
            return n2;
        }

        private static Point GetNearestVisibleNeighborSink(
            Point currentPoint,
            Point endPoint,
            WorkflowConnectorInfo sink,
            Rect rectSource,
            Rect rectSink)
        {
            Point s1, s2; // neighbors on sink side
            GetNeighborCorners(sink.Orientation, rectSink, out s1, out s2);

            bool flag1 = IsPointVisible(currentPoint, s1, new[] { rectSource, rectSink });
            bool flag2 = IsPointVisible(currentPoint, s2, new[] { rectSource, rectSink });

            if (flag1) // s1 visible
            {
                if (flag2) // s1 and s2 visible
                {
                    if (rectSink.Contains(s1))
                    {
                        return s2;
                    }

                    if (rectSink.Contains(s2))
                    {
                        return s1;
                    }

                    if ((Distance(s1, endPoint) <= Distance(s2, endPoint)))
                    {
                        return s1;
                    }
                    return s2;
                }
                return s1;
            }
            if (flag2) // only s2 visible
            {
                return s2;
            }
            return new Point(double.NaN, double.NaN);
        }

        private static void GetNeighborCorners(EConnectorOrientation orientation, Rect rect, out Point n1, out Point n2)
        {
            switch (orientation)
            {
                case EConnectorOrientation.Left:
                    n1 = rect.TopLeft;
                    n2 = rect.BottomLeft;
                    break;
                case EConnectorOrientation.Top:
                    n1 = rect.TopLeft;
                    n2 = rect.TopRight;
                    break;
                case EConnectorOrientation.Right:
                    n1 = rect.TopRight;
                    n2 = rect.BottomRight;
                    break;
                case EConnectorOrientation.Bottom:
                    n1 = rect.BottomLeft;
                    n2 = rect.BottomRight;
                    break;
                default:
                    throw new Exception("No neighour corners found!");
            }
        }

        private static Point GetOffsetPoint(WorkflowConnectorInfo workflowConnector, Rect rect)
        {
            var offsetPoint = new Point();

            switch (workflowConnector.Orientation)
            {
                case EConnectorOrientation.Left:
                    offsetPoint = new Point(rect.Left, workflowConnector.Position.Y);
                    break;
                case EConnectorOrientation.Top:
                    offsetPoint = new Point(workflowConnector.Position.X, rect.Top);
                    break;
                case EConnectorOrientation.Right:
                    offsetPoint = new Point(rect.Right, workflowConnector.Position.Y);
                    break;
                case EConnectorOrientation.Bottom:
                    offsetPoint = new Point(workflowConnector.Position.X, rect.Bottom);
                    break;
                default:
                    break;
            }

            return offsetPoint;
        }

        private static EConnectorOrientation GetOpositeOrientation(EConnectorOrientation eConnectorOrientation)
        {
            switch (eConnectorOrientation)
            {
                case EConnectorOrientation.Left:
                    return EConnectorOrientation.Right;
                case EConnectorOrientation.Top:
                    return EConnectorOrientation.Bottom;
                case EConnectorOrientation.Right:
                    return EConnectorOrientation.Left;
                case EConnectorOrientation.Bottom:
                    return EConnectorOrientation.Top;
                default:
                    return EConnectorOrientation.Top;
            }
        }

        private static void GetOppositeCorners(EConnectorOrientation orientation, Rect rect, out Point n1, out Point n2)
        {
            switch (orientation)
            {
                case EConnectorOrientation.Left:
                    n1 = rect.TopRight;
                    n2 = rect.BottomRight;
                    break;
                case EConnectorOrientation.Top:
                    n1 = rect.BottomLeft;
                    n2 = rect.BottomRight;
                    break;
                case EConnectorOrientation.Right:
                    n1 = rect.TopLeft;
                    n2 = rect.BottomLeft;
                    break;
                case EConnectorOrientation.Bottom:
                    n1 = rect.TopLeft;
                    n2 = rect.TopRight;
                    break;
                default:
                    throw new Exception("No opposite corners found!");
            }
        }

        private static EConnectorOrientation GetOrientation(Point p1, Point p2)
        {
            if (p1.X == p2.X)
            {
                if (p1.Y >= p2.Y)
                {
                    return EConnectorOrientation.Bottom;
                }
                return EConnectorOrientation.Top;
            }
            if (p1.Y == p2.Y)
            {
                if (p1.X >= p2.X)
                {
                    return EConnectorOrientation.Right;
                }
                return EConnectorOrientation.Left;
            }
            throw new Exception("Failed to retrieve orientation");
        }

        private static Orientation GetOrientation(EConnectorOrientation sourceOrientation)
        {
            switch (sourceOrientation)
            {
                case EConnectorOrientation.Left:
                    return Orientation.Horizontal;
                case EConnectorOrientation.Top:
                    return Orientation.Vertical;
                case EConnectorOrientation.Right:
                    return Orientation.Horizontal;
                case EConnectorOrientation.Bottom:
                    return Orientation.Vertical;
                default:
                    throw new Exception("Unknown ConnectorOrientation");
            }
        }

        private static Rect GetRectWithMargin(WorkflowConnectorInfo workflowConnectorThumb, double margin)
        {
            var rect = new Rect(
                workflowConnectorThumb.DesignerItemLeft,
                workflowConnectorThumb.DesignerItemTop,
                workflowConnectorThumb.DesignerItemSize.Width,
                workflowConnectorThumb.DesignerItemSize.Height);

            rect.Inflate(margin, margin);

            return rect;
        }

        private static bool IsPointVisible(Point fromPoint, Point targetPoint, Rect[] rectangles)
        {
            foreach (var rect in rectangles)
            {
                if (RectangleIntersectsLine(rect, fromPoint, targetPoint))
                {
                    return false;
                }
            }
            return true;
        }

        private static bool IsRectVisible(Point fromPoint, Rect targetRect, Rect[] rectangles)
        {
            if (IsPointVisible(fromPoint, targetRect.TopLeft, rectangles))
            {
                return true;
            }

            if (IsPointVisible(fromPoint, targetRect.TopRight, rectangles))
            {
                return true;
            }

            if (IsPointVisible(fromPoint, targetRect.BottomLeft, rectangles))
            {
                return true;
            }

            if (IsPointVisible(fromPoint, targetRect.BottomRight, rectangles))
            {
                return true;
            }

            return false;
        }

        private static List<Point> OptimizeLinePoints(
            List<Point> linePoints,
            Rect[] rectangles,
            EConnectorOrientation sourceOrientation,
            EConnectorOrientation sinkOrientation)
        {
            var points = new List<Point>();
            int cut = 0;

            for (int i = 0; i < linePoints.Count; i++)
            {
                if (i >= cut)
                {
                    for (int k = linePoints.Count - 1; k > i; k--)
                    {
                        if (IsPointVisible(linePoints[i], linePoints[k], rectangles))
                        {
                            cut = k;
                            break;
                        }
                    }
                    points.Add(linePoints[i]);
                }
            }

            #region Line

            for (int j = 0; j < points.Count - 1; j++)
            {
                if (points[j].X != points[j + 1].X && points[j].Y != points[j + 1].Y)
                {
                    EConnectorOrientation orientationFrom;
                    EConnectorOrientation orientationTo;

                    // orientation from point
                    if (j == 0)
                    {
                        orientationFrom = sourceOrientation;
                    }
                    else
                    {
                        orientationFrom = GetOrientation(points[j], points[j - 1]);
                    }

                    // orientation to pint 
                    if (j == points.Count - 2)
                    {
                        orientationTo = sinkOrientation;
                    }
                    else
                    {
                        orientationTo = GetOrientation(points[j + 1], points[j + 2]);
                    }

                    if ((orientationFrom == EConnectorOrientation.Left || orientationFrom == EConnectorOrientation.Right)
                        && (orientationTo == EConnectorOrientation.Left || orientationTo == EConnectorOrientation.Right))
                    {
                        double centerX = Math.Min(points[j].X, points[j + 1].X)
                                         + Math.Abs(points[j].X - points[j + 1].X) / 2;
                        points.Insert(j + 1, new Point(centerX, points[j].Y));
                        points.Insert(j + 2, new Point(centerX, points[j + 2].Y));
                        if (points.Count - 1 > j + 3)
                        {
                            points.RemoveAt(j + 3);
                        }
                        return points;
                    }

                    if ((orientationFrom == EConnectorOrientation.Top || orientationFrom == EConnectorOrientation.Bottom)
                        && (orientationTo == EConnectorOrientation.Top || orientationTo == EConnectorOrientation.Bottom))
                    {
                        double centerY = Math.Min(points[j].Y, points[j + 1].Y)
                                         + Math.Abs(points[j].Y - points[j + 1].Y) / 2;
                        points.Insert(j + 1, new Point(points[j].X, centerY));
                        points.Insert(j + 2, new Point(points[j + 2].X, centerY));
                        if (points.Count - 1 > j + 3)
                        {
                            points.RemoveAt(j + 3);
                        }
                        return points;
                    }

                    if ((orientationFrom == EConnectorOrientation.Left || orientationFrom == EConnectorOrientation.Right)
                        && (orientationTo == EConnectorOrientation.Top || orientationTo == EConnectorOrientation.Bottom))
                    {
                        points.Insert(j + 1, new Point(points[j + 1].X, points[j].Y));
                        return points;
                    }

                    if ((orientationFrom == EConnectorOrientation.Top || orientationFrom == EConnectorOrientation.Bottom)
                        && (orientationTo == EConnectorOrientation.Left || orientationTo == EConnectorOrientation.Right))
                    {
                        points.Insert(j + 1, new Point(points[j].X, points[j + 1].Y));
                        return points;
                    }
                }
            }

            #endregion

            return points;
        }

        private static bool RectangleIntersectsLine(Rect rect, Point startPoint, Point endPoint)
        {
            rect.Inflate(-1, -1);
            return rect.IntersectsWith(new Rect(startPoint, endPoint));
        }

        #endregion
    }
}