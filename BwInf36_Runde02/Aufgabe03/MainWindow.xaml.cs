using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Aufgabe03.Classes;
using Aufgabe03.Classes.GUI;
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

        public double OverlayZoomLevel = 8;
        private int StrokeThickness = 10;
        public readonly ViewModel Vm;
        public int QuaxPosIndex;
        private Pathfinder _pathfinder;
        private WriteableBitmap _mapImg;
        private WriteableBitmap _mapImgOverlay;
        private Konsole _konsole;

        public MainWindow()
        {
            InitializeComponent();

            Vm = new ViewModel();
            DataContext = Vm;
            Manager.Instance.MainWindow = this;

            MapScaleSlider.ValueChanged += MapScaleSlider_OnValueChanged;
        }

        private void OpenFileBtn_OnClick(object sender, RoutedEventArgs e)
        {
            e.Handled = true;
            var openFileDialog = new OpenFileDialog
            {
                Multiselect = false,
                Filter = "Image files (*.png)|*.png",
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
            };
            var result = openFileDialog.ShowDialog();

            if (result == true)
            {
                Vm.PositionTabs.Clear();
                QuaxPosIndex = 0;
                _mapImg = new WriteableBitmap(new BitmapImage(new Uri(openFileDialog.FileName)));
                _mapImgOverlay = new WriteableBitmap(Convert.ToInt32(_mapImg.PixelWidth * OverlayZoomLevel), Convert.ToInt32(_mapImg.PixelHeight * OverlayZoomLevel), _mapImg.DpiX,
                    _mapImg.DpiY, PixelFormats.Bgra32, null);

                MapDaten.Instance.LoadMapData(_mapImg);

                for (int i = 0; i < MapDaten.Instance.QuaxPositionen.Count; i++)
                {
                    var quaxPos = new PositionTab(i, _mapImg.Clone());
                    Vm.PositionTabs.Add(quaxPos);
                }

                QuaxPosTabControl.SelectedIndex = 0;

                MapScaleSlider.Value = MapScaleSlider.Minimum;
                MapImage.Source = _mapImg;
                MapImageOverlay.Source = _mapImgOverlay;

                Manager.Instance.MapOverlay = _mapImgOverlay;
            }
        }

        private void StartAlgorithm_OnClick(object sender, RoutedEventArgs e)
        {
            e.Handled = true;
            _pathfinder = new Pathfinder(QuaxPosIndex);
            //_mapImgOverlay = new WriteableBitmap(Convert.ToInt32(_mapImg.PixelWidth * OverlayZoomLevel), Convert.ToInt32(_mapImg.PixelHeight * OverlayZoomLevel), _mapImg.DpiX,
            //    _mapImg.DpiY, PixelFormats.Bgra32, null);
            //MapImageOverlay.Source = _mapImgOverlay; //TODO: WTF Clear die Quadtree Sachen wenn er fertig is und tab wechselt

            try
            {
                var timer = new Stopwatch();
                timer.Start();
                var pathInfo = _pathfinder.FindPath();
                timer.Stop();
                Result_Header.Content = $"Ergebnis für {Manager.Instance.CurPositionTab.Header}";
                Result_AlgorithmTime.Content = $"{timer.ElapsedMilliseconds}ms";
                Result_DroneFlights.Content = $"{Manager.Instance.CurPositionTab.AnzahlFluege}";

                if (pathInfo.StadtGefunden)
                {
                    Result_FoundPath.Content = "JA";
                    for (int i = 0; i < pathInfo.Weg.Count; i++)
                    {
                        if (i == 0)
                        {
                            _mapImgOverlay.DrawLineAa(Convert.ToInt32(MapDaten.Instance.QuaxPositionen[QuaxPosIndex].X * OverlayZoomLevel),
                                Convert.ToInt32(MapDaten.Instance.QuaxPositionen[QuaxPosIndex].Y * OverlayZoomLevel),
                                Convert.ToInt32(pathInfo.Weg[i].MapQuadrat.Mittelpunkt.X * OverlayZoomLevel),
                                Convert.ToInt32(pathInfo.Weg[i].MapQuadrat.Mittelpunkt.Y * OverlayZoomLevel), Colors.Cyan, StrokeThickness);
                        }

                        if (i < pathInfo.Weg.Count - 1)
                        {
                            _mapImgOverlay.DrawLineAa(Convert.ToInt32(pathInfo.Weg[i].MapQuadrat.Mittelpunkt.X * OverlayZoomLevel),
                                Convert.ToInt32(pathInfo.Weg[i].MapQuadrat.Mittelpunkt.Y * OverlayZoomLevel),
                                Convert.ToInt32(pathInfo.Weg[i + 1].MapQuadrat.Mittelpunkt.X * OverlayZoomLevel),
                                Convert.ToInt32(pathInfo.Weg[i + 1].MapQuadrat.Mittelpunkt.Y * OverlayZoomLevel), Colors.Cyan, StrokeThickness);
                        }
                        else if (i == pathInfo.Weg.Count - 1)
                        {
                            _mapImgOverlay.DrawLineAa(Convert.ToInt32(pathInfo.Weg[i].MapQuadrat.Mittelpunkt.X * OverlayZoomLevel),
                                Convert.ToInt32(pathInfo.Weg[i].MapQuadrat.Mittelpunkt.Y * OverlayZoomLevel),
                                Convert.ToInt32(pathInfo.StadtPos.X * OverlayZoomLevel),
                                Convert.ToInt32(pathInfo.StadtPos.Y * OverlayZoomLevel), Colors.Cyan, StrokeThickness);
                        }
                    }

                    MessageBox.Show($"Weg Laenge: {pathInfo.Weg.Count}\nIn {timer.ElapsedMilliseconds}ms");
                }
                else
                {
                    Result_FoundPath.Content = "NEIN";
                    MessageBox.Show("Es konnte kein Weg gefunden werden!");
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.StackTrace, exception.Message);
            }

            _pathfinder = null;
        }

        public void ZeichneWeg() { }


        private void QuaxPosTabControl_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.Source is TabControl)
            {
                QuaxPosIndex = QuaxPosTabControl.SelectedIndex;
                e.Handled = true;
            }
        }

        private void OpenConsoleBtn_OnClick(object sender, RoutedEventArgs e)
        {
            if (_konsole == null)
            {
                _konsole = new Konsole();
                Manager.Instance.Konsole = _konsole;
                Console.WriteLine("Konsole gestarted");
            }

            _konsole.Show();
            e.Handled = true;
        }

        private void Selector_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.Source is ListBox)
            {
                var listBox = (ListBox) sender;
                if (listBox.SelectedIndex < 0) return;
                Manager.Instance.CurPositionTab.HighlightedDrohnenFlug =
                    Manager.Instance.CurPositionTab.DrohnenFluege[listBox.SelectedIndex];
                e.Handled = true;
            }
        }

        #region Map Scrolling

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
                MapScaleSlider.Value += .5d;
            }
            if (e.Delta < 0)
            {
                MapScaleSlider.Value -= .5d;
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

        #endregion
    }
}
