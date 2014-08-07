﻿using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ArcGISRuntimeSDKDotNet_DesktopSamples.Samples
{
    /// <summary>
    /// This sample shows how to display a map tip for a feature layer.  In this example, the MouseMove event of the MapView is handled with code that performs a FeatureLayer HitTest / Query combination which returns a single feature for display in the mapTip element defined in the XAML.
    /// </summary>
    /// <title>Map Tips</title>
	/// <category>Layers</category>
	/// <subcategory>Feature Layers</subcategory>
	public partial class FeatureLayerMapTips : UserControl
    {
		private bool _isMapReady;

        /// <summary>Construct Map Tips sample</summary>
        public FeatureLayerMapTips()
        {
            InitializeComponent();

			MyMapView.SpatialReferenceChanged += MyMapView_SpatialReferenceChanged;
        }

		private async void MyMapView_SpatialReferenceChanged(object sender, System.EventArgs e)
		{
			await MyMapView.LayersLoadedAsync();
			_isMapReady = true;
		}

		private async void MyMapView_MouseMove(object sender, MouseEventArgs e)
        {
			if (!_isMapReady)
				return;

			try
            {
				Point screenPoint = e.GetPosition(MyMapView);
				var rows = await earthquakes.HitTestAsync(MyMapView, screenPoint);
                if (rows != null && rows.Length > 0)
                {
                    var features = await earthquakes.FeatureTable.QueryAsync(rows);
                    var feature = features.FirstOrDefault();

                    maptipTransform.X = screenPoint.X + 4;
                    maptipTransform.Y = screenPoint.Y - mapTip.ActualHeight;
                    mapTip.DataContext = feature;
                    mapTip.Visibility = System.Windows.Visibility.Visible;
                }
                else
                    mapTip.Visibility = System.Windows.Visibility.Hidden;
            }
            catch
            {
                mapTip.Visibility = System.Windows.Visibility.Hidden;
            }
        }
    }
}
