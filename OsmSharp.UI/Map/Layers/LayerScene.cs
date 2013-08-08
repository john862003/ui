﻿// OsmSharp - OpenStreetMap tools & library.
// Copyright (C) 2013 Abelshausen Ben
// 
// This file is part of OsmSharp.
// 
// OsmSharp is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 2 of the License, or
// (at your option) any later version.
// 
// OsmSharp is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with OsmSharp. If not, see <http://www.gnu.org/licenses/>.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OsmSharp.Osm.Data;
using OsmSharp.Math.Geo;
using OsmSharp.UI.Map.Styles;
using OsmSharp.UI.Renderer;
using OsmSharp.UI.Renderer.Scene2DPrimitives;

namespace OsmSharp.UI.Map.Layers
{
    /// <summary>
    /// A layer drawing scene objects.
    /// </summary>
    public class LayerScene : ILayer
    {
        /// <summary>
        /// Holds the scene primitives source.
        /// </summary>
        private IScene2DPrimitivesSource _index;

        /// <summary>
        /// Creates a new scene layer.
        /// </summary>
        /// <param name="index"></param>
        public LayerScene(IScene2DPrimitivesSource index)
        {
            _index = index;
            this.Scene = new Scene2D();
        }

        /// <summary>
        /// Gets the minimum zoom.
        /// </summary>
        public float? MinZoom { get; private set; }

        /// <summary>
        /// Gets the maximum zoom.
        /// </summary>
        public float? MaxZoom { get; private set; }

        /// <summary>
        /// Gets the scene of this layer containing the objects already projected.
        /// </summary>
        public Scene2D Scene { get; private set; }

        /// <summary>
        /// Called when the view on the map containing this layer has changed.
        /// </summary>
        /// <param name="map"></param>
        /// <param name="zoomFactor"></param>
        /// <param name="center"></param>
        /// <param name="view"></param>
        public void ViewChanged(Map map, float zoomFactor, GeoCoordinate center, View2D view)
        {
            this.BuildScene(map, zoomFactor, center, view);
        }

        /// <summary>
        /// Event raised when this layer's content has changed.
        /// </summary>
        public event Map.LayerChanged LayerChanged;

		/// <summary>
		/// Holds the last box.
		/// </summary>
		private GeoCoordinateBox _lastBox;

        #region Scene Building
        
        /// <summary>
        /// Builds the scene.
        /// </summary>
        /// <param name="map"></param>
        /// <param name="zoomFactor"></param>
        /// <param name="center"></param>
        /// <param name="view"></param>
        private void BuildScene(Map map, float zoomFactor, GeoCoordinate center, View2D view)
        {
			// build the boundingbox.
			var box = new GeoCoordinateBox(map.Projection.ToGeoCoordinates(view.Left, view.Top),
			                               map.Projection.ToGeoCoordinates(view.Right, view.Bottom));
//			if (_lastBox != null && _lastBox.IsInside (box)) {
//				return;
//			}
//			_lastBox = box;

            // reset the scene.
            this.Scene = new Scene2D();

            // get from the index.
            this.Scene.BackColor = SimpleColor.FromKnownColor(KnownColor.White).Value;
            _index.Get(this.Scene, view, zoomFactor);
        }

        #endregion
    }
}