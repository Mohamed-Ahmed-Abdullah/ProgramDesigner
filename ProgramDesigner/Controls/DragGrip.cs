using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace ProgramDesigner.Controls
{
    public class DragGrip : Border,INotifyPropertyChanged
    {
        //used in the Multidrag
        public Point InitialPoint { get; set; }

        private bool _isSelected;
        public bool IsDragable { get; set; }

        public bool IsSelected
        {
            get { return _isSelected; }
            set { _isSelected = value;
            NotifyProperty("IsSelected");
            }
        }

        public DragGrip()
        {
            MouseDown += DragGrip_MouseDown;
            MouseUp += DragGrip_MouseUp;
            MouseMove += DragGrip_MouseMove;
        }
        private bool _mouseDown;
        private Point _staringPoint;
        void DragGrip_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            _mouseDown = true;
            _isDragged = false;
            _staringPoint = e.GetPosition(this);
            ((FrameworkElement)e.OriginalSource).CaptureMouse();
        }

        public bool _isDragged;
        public Point _dragOffset= new Point(3,3);
        void DragGrip_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            var currentPosition = e.GetPosition(this);
            if (_mouseDown && 
                ( Math.Abs(_staringPoint.X - currentPosition.X) > _dragOffset.X
                || Math.Abs(_staringPoint.Y - currentPosition.Y) > _dragOffset.Y))
            {
                _isDragged = true;
            }
        }

        void DragGrip_MouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (_mouseDown && !_isDragged)
            {
                IsSelected = !IsSelected;
            }
            _mouseDown = false;
            _isDragged = false;
            ((FrameworkElement)e.OriginalSource).ReleaseMouseCapture();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyProperty(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this,new PropertyChangedEventArgs(propertyName));
        }
    }
}