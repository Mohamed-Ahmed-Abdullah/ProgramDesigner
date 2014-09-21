using System;
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

        private void RecOnMouseDown(object sender, MouseButtonEventArgs e)
        {
            ((Rectangle) sender).Tag = true;
        }
            
        private int _offset = 5;//pixel
        private Point? _point = null;
        private Point? _startingPoint = null;
        private void RecOnMouseMove(object sender, MouseEventArgs e)
        {
            if (((bool?) ((Rectangle) sender).Tag) == true && e.LeftButton == MouseButtonState.Pressed)
            {
                if(_startingPoint == null)
                    _startingPoint = e.GetPosition(this);
                _point = e.GetPosition(this);

                if (Math.Abs(_startingPoint.Value.X - _point.Value.X) >= _offset
                    && Math.Abs(_startingPoint.Value.Y - _point.Value.Y) >= _offset)
                {
                    //copy to clipboard and stop the drag
                    LocalClipboard = Utils.Clone( (Rectangle) sender);
                    ((Rectangle)sender).Tag = false;
                    MainCanvas.Children.Add((FrameworkElement) LocalClipboard);
                    Debug.WriteLine("Copied");
                }
            }
            else
            {
                _startingPoint = null;
                _point = null;
            }
        }

        private void RecOnMouseUp(object sender, MouseButtonEventArgs e)
        {
            ((Rectangle)sender).Tag = false;            

        }


        private FrameworkElement _element;
        private bool isDragging;
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
        }

        private Point _snapOffset = new Point {X = 5, Y = 15}; 
        private void CanvasOnMouseMove(object sender, MouseEventArgs e)
        {
            var draggableControl = Utils.GetParent<DragGrip>(e.OriginalSource as FrameworkElement) as DragGrip;
            if (isDragging && draggableControl != null && draggableControl.IsDragable)
            {
                Point currentPosition = e.GetPosition(this.Parent as FrameworkElement);
                var transform = draggableControl.RenderTransform as TranslateTransform;
                if (transform == null)
                {
                    transform = new TranslateTransform();
                    draggableControl.RenderTransform = transform;
                }

                transform.X = (currentPosition.X - clickPosition.X) + initialPoint.X.ZeroBased();
                transform.Y = (currentPosition.Y - clickPosition.Y) + initialPoint.Y.ZeroBased();

                //TODO: Snap to the other element                
                var x = transform.X;
                var y = transform.Y;
                var xbottom = transform.X + draggableControl.ActualWidth.ZeroBased();
                var ybottom = transform.Y + draggableControl.ActualHeight.ZeroBased();
                foreach (FrameworkElement child in MainCanvas.Children)
                {
                    //skip the dragable element
                    if (child.Equals(draggableControl)) { continue; }

                    if (!(child.RenderTransform is TranslateTransform))
                    {
                        continue;
                    }

                    var childX = ((TranslateTransform)child.RenderTransform).X;
                    var childY = ((TranslateTransform)child.RenderTransform).Y;
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