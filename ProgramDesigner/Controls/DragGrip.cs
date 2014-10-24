using System;
using System.Collections;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using ProgramDesigner.Converters;

namespace ProgramDesigner.Controls
{
    public class DragGrip : Border, INotifyPropertyChanged
    {
        //used in the Multidrag
        public Point InitialPoint { get; set; }

        private bool _isSelected;
        public bool IsDragable { get; set; }

        public bool IsToolBarItem { get; set; }

        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                _isSelected = value;
                NotifyProperty("IsSelected");
            }
        }

        public string ContextMenuName { get; set; }

        public DragGrip()
        {
            MouseDown += DragGrip_MouseDown;
            MouseUp += DragGrip_MouseUp;
            MouseMove += DragGrip_MouseMove;
        }

        private bool _mouseDown;
        private Point _staringPoint;

        private void DragGrip_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton != MouseButtonState.Pressed)
                return;

            _mouseDown = true;
            _isDragged = false;
            _staringPoint = e.GetPosition(this);
            ((FrameworkElement) e.OriginalSource).CaptureMouse();
        }

        public bool _isDragged;
        public Point _dragOffset = new Point(3, 3);

        private void DragGrip_MouseMove(object sender, MouseEventArgs e)
        {
            var currentPosition = e.GetPosition(this);
            if (_mouseDown &&
                (Math.Abs(_staringPoint.X - currentPosition.X) > _dragOffset.X
                 || Math.Abs(_staringPoint.Y - currentPosition.Y) > _dragOffset.Y))
            {
                _isDragged = true;
            }
        }

        private void DragGrip_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (_mouseDown && !_isDragged)
            {
                IsSelected = !IsSelected;
            }
            _mouseDown = false;
            _isDragged = false;
            ((FrameworkElement) e.OriginalSource).ReleaseMouseCapture();
        }

        public DragGrip Clone()
        {
            var newItem = new DragGrip
            {
                Name = "Cloned" + Guid.NewGuid().ToString().Replace("-", ""),
                RenderTransform =
                    new TranslateTransform(((TranslateTransform) RenderTransform).X,
                        ((TranslateTransform) RenderTransform).Y),
                InitialPoint = InitialPoint,
                IsDragable = false,
                IsToolBarItem = true,
                IsSelected = false,
                ContextMenuName = ContextMenuName,
            };

            var binding = new Binding
            {
                RelativeSource = new RelativeSource
                {
                    Mode = RelativeSourceMode.FindAncestor,
                    AncestorType = typeof (DragGrip)
                },
                Path = new PropertyPath("IsSelected"),
                Converter = new BoolToIntConverter(),
                ConverterParameter = "2.5",
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
                Mode = BindingMode.TwoWay
            };
            var border = new Border
            {
                Background = ((Border) Child).Background,
                Width = ActualWidth,
                Height = ActualHeight,
                BorderBrush = Brushes.DeepSkyBlue,
                Child = new TextBlock
                {
                    Text = ((TextBlock) ((Border) Child).Child).Text,
                    FontSize = 16,
                    Foreground = Brushes.White,
                    VerticalAlignment = VerticalAlignment.Center,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    Margin = ((TextBlock) ((Border) Child).Child).Margin
                }
            };

            border.SetBinding(Border.BorderThicknessProperty, binding);
            newItem.ContextMenu = (ContextMenu)((Canvas)Parent).FindResource(ContextMenuName);
            newItem.Child = border;
            return newItem;
        }


        //private ContextMenu GetContextMenu1()
        //{
        //    var mainCanvas = (Canvas)Parent;
        //    var border2 = (Border)mainCanvas.Parent;
        //    var grid = (Grid)border2.Parent;
        //    var mainWindow = (MainWindow)grid.Parent;

        //    var contextMenu = new ContextMenu();
        //    var menueItem = new MenuItem { Header = "Delete" };
        //    menueItem.Click += mainWindow.MenuItemOnDelete;
        //    contextMenu.Items.Add(menueItem);
        //    return contextMenu;
        //}

        //private ContextMenu GetContextMenu2()
        //{
        //    var mainCanvas = (Canvas)Parent;
        //    var border2 = (Border)mainCanvas.Parent;
        //    var grid = (Grid)border2.Parent;
        //    var mainWindow = (MainWindow)grid.Parent;

        //    var contextMenu = new ContextMenu();

        //    var deleteItem = new MenuItem { Header = "Delete" };
        //    deleteItem.Click += mainWindow.MenuItemOnDelete;

        //    var renameItem = new MenuItem { Header = "Rename" };
        //    renameItem.Click += mainWindow.MenuItemOnRename;

        //    contextMenu.Items.Add(deleteItem);
        //    contextMenu.Items.Add(renameItem);
        //    return contextMenu;
        //}

        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyProperty(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this,new PropertyChangedEventArgs(propertyName));
        }
    }
}