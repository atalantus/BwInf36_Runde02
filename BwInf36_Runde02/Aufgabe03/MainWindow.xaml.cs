using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Aufgabe03.Classes;
using Aufgabe03.Classes.Pathfinding;
using Microsoft.Win32;

namespace Aufgabe03
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Point? _lastCenterPositionOnTarget;
        private Point? _lastMousePositionOnTarget;
        private Point? _lastDragPoint;

        public MainWindow()
        {
            InitializeComponent();

            MapScaleSlider.ValueChanged += MapScaleSlider_OnValueChanged;
        }

        private void OpenFileBtn_OnClick(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog
            {
                Multiselect = false,
                Filter = "Image files (*.png)|*.png",
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
            };
            bool? result = openFileDialog.ShowDialog();
            ProgressBar.Value = 0d;

            if (result == true)
            {
                BitmapImage image = new BitmapImage(new Uri(openFileDialog.FileName));
                MapDaten.Instance.Map = new WriteableBitmap(image);
                ImageSource source = image;
                MapScaleSlider.Value = MapScaleSlider.Minimum;
                MapImage.Source = source;
                ConsoleFlowDocument.Blocks.Add(new Paragraph(new Run("Loaded image!")));
            }
        }

        private void MapScrollViewer_OnScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            if (e.ExtentHeightChange != 0 || e.ExtentWidthChange != 0)
            {
                Point? targetBefore = null;
                Point? targetNow = null;

                if (!_lastMousePositionOnTarget.HasValue)
                {
                    if (_lastCenterPositionOnTarget.HasValue)
                    {
                        var centerOfViewport = new Point(MapScrollViewer.ViewportWidth / 2, MapScrollViewer.ViewportHeight / 2);
                        Point centerOfTargetNow = MapScrollViewer.TranslatePoint(centerOfViewport, MapGrid);

                        targetBefore = _lastCenterPositionOnTarget;
                        targetNow = centerOfTargetNow;
                    }
                }
                else
                {
                    targetBefore = _lastMousePositionOnTarget;
                    targetNow = Mouse.GetPosition(MapGrid);

                    _lastMousePositionOnTarget = null;
                }

                if (targetBefore.HasValue)
                {
                    double dXInTargetPixels = targetNow.Value.X - targetBefore.Value.X;
                    double dYInTargetPixels = targetNow.Value.Y - targetBefore.Value.Y;

                    double multiplicatorX = e.ExtentWidth / MapGrid.Width;
                    double multiplicatorY = e.ExtentHeight / MapGrid.Height;

                    double newOffsetX = MapScrollViewer.HorizontalOffset - dXInTargetPixels * multiplicatorX;
                    double newOffsetY = MapScrollViewer.VerticalOffset - dYInTargetPixels * multiplicatorY;

                    if (double.IsNaN(newOffsetX) || double.IsNaN(newOffsetY))
                    {
                        return;
                    }

                    MapScrollViewer.ScrollToHorizontalOffset(newOffsetX);
                    MapScrollViewer.ScrollToVerticalOffset(newOffsetY);
                }
            }
        }

        private void MapScrollViewer_OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            MapScrollViewer.Cursor = Cursors.Arrow;
            MapScrollViewer.ReleaseMouseCapture();
            _lastDragPoint = null;
        }

        private void MapScrollViewer_OnPreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            MapScrollViewer.Cursor = Cursors.Arrow;
            MapScrollViewer.ReleaseMouseCapture();
            _lastDragPoint = null;
        }

        private void MapScrollViewer_OnPreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            _lastMousePositionOnTarget = Mouse.GetPosition(MapGrid);

            if (e.Delta > 0)
            {
                MapScaleSlider.Value += 5;
            }
            if (e.Delta < 0)
            {
                MapScaleSlider.Value -= 5;
            }

            e.Handled = true;
        }

        private void MapScrollViewer_OnPreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var mousePos = e.GetPosition(MapScrollViewer);
            if (mousePos.X <= MapScrollViewer.ViewportWidth && mousePos.Y < MapScrollViewer.ViewportHeight) //make sure we still can use the scrollbars
            {
                MapScrollViewer.Cursor = Cursors.SizeAll;
                _lastDragPoint = mousePos;
                Mouse.Capture(MapScrollViewer);
            }
        }

        private void MapScrollViewer_OnMouseMove(object sender, MouseEventArgs e)
        {
            if (_lastDragPoint.HasValue)
            {
                Point posNow = e.GetPosition(MapScrollViewer);

                double dX = posNow.X - _lastDragPoint.Value.X;
                double dY = posNow.Y - _lastDragPoint.Value.Y;

                _lastDragPoint = posNow;

                MapScrollViewer.ScrollToHorizontalOffset(MapScrollViewer.HorizontalOffset - dX);
                MapScrollViewer.ScrollToVerticalOffset(MapScrollViewer.VerticalOffset - dY);
            }
        }

        private void MapScaleSlider_OnValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            MapGridScaleTransform.ScaleX = e.NewValue;
            MapGridScaleTransform.ScaleY = e.NewValue;

            var centerOfViewport = new Point(MapScrollViewer.ViewportWidth / 2, MapScrollViewer.ViewportHeight / 2);
            _lastCenterPositionOnTarget = MapScrollViewer.TranslatePoint(centerOfViewport, MapGrid);
        }
    }
}
