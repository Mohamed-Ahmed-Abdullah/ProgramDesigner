﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ProgramDesigner.Controls;

namespace ProgramDesigner
{
    public partial class MainWindow
    {
        public static object LocalClipboard { get; set; }
        public Utils Utils { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            GreenDragGrip.RenderTransform = new TranslateTransform { Y = 70 };
            Utils = new Utils();
        }

        private int _offset = 5;//pixel
        private Point? _point = null;
        private Point? _startingPoint = null;
        private FrameworkElement _element;
        private bool isDragging;
        private bool isCanvasDragging;
        private Point clickPosition;
        private Point initialPoint;
        private void CanvasOnMouseDown(object sender, MouseButtonEventArgs e)
        {
            var draggableControl = e.OriginalSource as FrameworkElement;
            draggableControl = Utils.GetParent<DragGrip>(draggableControl) as DragGrip;
            
            if (draggableControl != null)
            {
                isDragging = true;
                clickPosition = e.GetPosition(this);

                var transform = draggableControl.RenderTransform as TranslateTransform;
                if (transform != null)
                    initialPoint = new Point(transform.X, transform.Y);

                draggableControl.CaptureMouse();
            }

            var canvas = e.OriginalSource as Canvas;
            if (canvas != null)
            {
                clickPosition = e.GetPosition(this);
                isCanvasDragging = true;
                canvas.CaptureMouse();
            }
        }

        private Point _snapOffset = new Point {X = 5, Y = 15}; 
        private void CanvasOnMouseMove(object sender, MouseEventArgs e)
        {
            if (isDragging && e.OriginalSource is DragGrip)
            {
                var draggableControl = Utils.GetParent<DragGrip>(e.OriginalSource as FrameworkElement) as DragGrip;
                if (draggableControl == null || !draggableControl.IsDragable)
                { return; }

                var currentPosition = e.GetPosition(Parent as FrameworkElement);
                var transform = draggableControl.RenderTransform as TranslateTransform;
                if (transform == null)
                {
                    transform = new TranslateTransform();
                    draggableControl.RenderTransform = transform;
                }

                //TODO:if the dragable element selected -> drag and selected elment with it  if not-> drag him normally (just use the current code)
                //Drag
                transform.X = (currentPosition.X - clickPosition.X) + initialPoint.X.ZeroBased();
                transform.Y = (currentPosition.Y - clickPosition.Y) + initialPoint.Y.ZeroBased();

                #region Snapping

                var x = transform.X;
                var y = transform.Y;

                var xbottom = transform.X + draggableControl.ActualWidth.ZeroBased();
                var ybottom = transform.Y + draggableControl.ActualHeight.ZeroBased();

                foreach (FrameworkElement child in MainCanvas.Children)
                {
                    //skip the dragable element
                    if (child.Equals(draggableControl))
                    {
                        continue;
                    }

                    if (!(child.RenderTransform is TranslateTransform))
                    {
                        continue;
                    }

                    var childX = ((TranslateTransform) child.RenderTransform).X;
                    var childY = ((TranslateTransform) child.RenderTransform).Y;
                    var childXBottom = childX + child.ActualWidth.ZeroBased();
                    var childYBottom = childY + child.ActualHeight.ZeroBased();

                    //Reversed Snapping
                    //T2 <--> B1
                    if (y.Between(Math.Abs(childYBottom - _snapOffset.Y), Math.Abs(childYBottom + _snapOffset.Y)))
                    {
                        transform.Y = childYBottom;
                    }
                    //B2 <--> T1
                    if (ybottom.Between(Math.Abs(childY - _snapOffset.Y), Math.Abs(childY + _snapOffset.Y)))
                    {
                        transform.Y = childY - draggableControl.ActualHeight.ZeroBased();
                    }
                    //L2 <--> R1
                    if (x.Between(Math.Abs(childXBottom - _snapOffset.X), Math.Abs(childXBottom + _snapOffset.X)))
                    {
                        transform.X = childXBottom;
                    }
                    //R2 <--> L1
                    if (xbottom.Between(Math.Abs(childX - _snapOffset.X), Math.Abs(childX + _snapOffset.X)))
                    {
                        transform.X = childX - draggableControl.ActualWidth.ZeroBased();
                    }

                    //Similar Snapping
                    //L2 <--> L1
                    if (x.Between(Math.Abs(childX - _snapOffset.X), Math.Abs(childX + _snapOffset.X)))
                    {
                        transform.X = childX;
                    }
                    //B2 <--> B1
                    if (y.Between(Math.Abs(childY - _snapOffset.Y), Math.Abs(childY + _snapOffset.Y)))
                    {
                        transform.Y = childY;
                    }
                }

                #endregion

            }

            if (isCanvasDragging && e.OriginalSource is Canvas)
            {
                //Multi select 
                var currentPosition = e.GetPosition(Parent as FrameworkElement);
                var x1 = currentPosition.X;
                var y1 = currentPosition.Y;
                var x2 = clickPosition.X.ZeroBased() + initialPoint.X.ZeroBased();
                var y2 = clickPosition.Y + initialPoint.Y.ZeroBased();

                if (Math.Abs(x1 - x2) > 3 && Math.Abs(y1 - y2) > 3)
                {
                    SelectionRectangle.Visibility = Visibility.Visible;
                    Canvas.SetLeft(SelectionRectangle, (x1 < x2) ? x1 : x2);
                    Canvas.SetTop(SelectionRectangle, (y1 < y2) ? y1 : y2);
                    SelectionRectangle.Width = Math.Abs(x1 - x2);
                    SelectionRectangle.Height = Math.Abs(y1 - y2);
                }
            }
        }

        private void CanvasOnMouseUp(object sender, MouseButtonEventArgs e)
        {
            var draggable = e.OriginalSource as DragGrip;
            if (draggable != null)
            {
                isDragging = false;
                draggable.ReleaseMouseCapture();
            }

            var canvas = e.OriginalSource as Canvas;
            if (canvas != null)
            {
                isCanvasDragging = false;
                canvas.ReleaseMouseCapture();
                SelectionRectangle.Visibility = Visibility.Collapsed;
            }
        }
    }

    public class Utils
    {
        public Rectangle Clone(Rectangle rectangle)
        {
            return new Rectangle
            {
                Width = rectangle.Width,
                Height = rectangle.Height,
                Fill = rectangle.Fill
            };
        }

        public FrameworkElement GetParent<T>(FrameworkElement current)
        {
            if (current == null)
                return null;
            if (current is T)
                return current;
            return GetParent<T>((FrameworkElement)current.Parent);
        }
    }

    public static class Extentions
    {
        public static bool Between(this Point point, Point topLeft, Point bottomRight)
        {
            if (topLeft.X.ZeroBased() <= point.X.ZeroBased() && bottomRight.X.ZeroBased() >= point.X.ZeroBased() &&
                topLeft.Y.ZeroBased() <= point.Y.ZeroBased() && bottomRight.Y.ZeroBased() >= point.Y.ZeroBased())
                return true;
            return false;
        }
        public static double ZeroBased(this double d)
        {
            return (double.IsNaN(d) ? 0 : d);
        }

        public static bool Between(this double current, double a, double b)
        {
            if ((current >= a && current <= b) || (current <= a && current >= b))
            {
                return true;
            }
            return false;
        }
    }
}