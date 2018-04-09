using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Aufgabe03.Classes.Pathfinding;

namespace Aufgabe03.Classes.GUI
{
    public class PositionTab : INotifyPropertyChanged
    {
        private ObservableCollection<DrohnenFlug> _drohnenFluege;
        private WriteableBitmap _posPreviewOverlay;
        private DrohnenFlug _highlightedDrohnenFlug;

        /// <summary>
        /// Der Index der Quax Position
        /// </summary>
        public int QuaxPosIndex { get; }


        /// <summary>
        /// Der Header des Tabs
        /// </summary>
        public string Header => $"Quax {QuaxPosIndex + 1}";

        public WriteableBitmap PosPreview { get; }

        public WriteableBitmap PosPreviewOverlay
        {
            get => _posPreviewOverlay;
            set
            {
                _posPreviewOverlay = value;
                OnPropertyChanged();
            }
        }

        public DrohnenFlug HighlightedDrohnenFlug
        {
            get => _highlightedDrohnenFlug;
            set
            {
                if (value == null) return;
                _highlightedDrohnenFlug?.DrawMapSquare();
                _highlightedDrohnenFlug = value;
                _highlightedDrohnenFlug.HighlightMapSquare();
            }
        }

        public int AnzahlFluege { get; set; }

        /// <summary>
        /// Die Drohnen Fluege
        /// </summary>
        public ObservableCollection<DrohnenFlug> DrohnenFluege
        {
            get => _drohnenFluege;
            set { _drohnenFluege = value; OnPropertyChanged(); }
        }

        public PositionTab(int quaxIndex, WriteableBitmap map)
        {
            QuaxPosIndex = quaxIndex;
            DrohnenFluege = new ObservableCollection<DrohnenFlug>();
            PosPreview = map;
            PosPreviewOverlay = RegisterQuaxPos(map);
            AnzahlFluege = 0;
        }

        public int AddDrohnenFlug(MapQuadrat mapQuadrat)
        {
            AnzahlFluege++;
            var id = DrohnenFluege.Count + 1;
            DrohnenFluege.Add(new DrohnenFlug(id, mapQuadrat));
            return id;
        }

        private int OverlayZoomLevel = 2;

        private WriteableBitmap RegisterQuaxPos(WriteableBitmap map)
        {
            var overlayMap = new WriteableBitmap(map.PixelWidth * OverlayZoomLevel, map.PixelHeight * OverlayZoomLevel, map.DpiX, map.DpiY, PixelFormats.Bgra32, null);
            var quaxPos = MapDaten.Instance.QuaxPositionen[QuaxPosIndex];
            var radius = overlayMap.PixelWidth / 6;
            overlayMap.FillRectangle(Convert.ToInt32(quaxPos.X * OverlayZoomLevel - radius), Convert.ToInt32(quaxPos.Y * OverlayZoomLevel - radius), Convert.ToInt32(quaxPos.X * OverlayZoomLevel + radius), Convert.ToInt32(quaxPos.Y * OverlayZoomLevel + radius), Color.FromArgb(150, 255, 255, 0));
            return overlayMap;
        } 

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}