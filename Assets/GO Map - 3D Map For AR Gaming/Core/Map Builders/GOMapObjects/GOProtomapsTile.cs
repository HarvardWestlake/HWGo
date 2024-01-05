using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

using GoShared;
using Mapbox.VectorTile;
using static GoMap.GOLayer;

namespace GoMap
{
    [ExecuteInEditMode]
    public class GOProtomapsTile : GOPBFTileAsync
    {


        public override string GetLayersStrings(GOLayer layer)
        {
            switch (layer.layerType)
            {
                case GOLayerType.Buildings:
                    return "buildings";
                case GOLayerType.Landuse:
                    return "landuse";
                case GOLayerType.Water:
                    return "water";
                case GOLayerType.Earth:
                    return "earth";
                case GOLayerType.Roads:
                    return "roads";
                case GOLayerType.Pois:
                    return "pois";
                default:
                    return "";
            }
        }
        public override string GetPoisStrings()
        {
            return "pois";
        }

        public override string GetLabelsStrings()
        {
            return "labels";
        }

        public override string GetPoisKindKey()
        {
            return "pmap:kind";
        }

        public override GOFeature EditLabelData(GOFeature goFeature)
        {

            goFeature.name = (string)goFeature.properties["name"];
            goFeature.kind = GOEnumUtils.MapboxToKind((string)goFeature.properties["pmap:kind"]);


            Int64 sort = 0;
            goFeature.y = sort / 1000.0f;
            goFeature.sort = sort;

            return goFeature;
        }

        public override GOFeature EditFeatureData(GOFeature goFeature)
        {

            if (goFeature.goFeatureType == GOFeatureType.Point)
            {
                goFeature.name = (string)goFeature.properties["name"];
                return goFeature;
            }

            IDictionary properties = goFeature.properties;

            goFeature.kind = GOEnumUtils.MapboxToKind((string)properties["pmap:kind"]);
            goFeature.name = (string)properties["name"];

            if (goFeature.layer != null && goFeature.layer.layerType == GOLayer.GOLayerType.Roads)
            {

                ((GORoadFeature)goFeature).isBridge = properties.Contains("structure") && (string)properties["structure"] == "bridge";
                if (((GORoadFeature)goFeature).isBridge) {
                    goFeature.kind = GOFeatureKind.bridge;
                }
				((GORoadFeature)goFeature).isTunnel = properties.Contains ("structure") && (string)properties ["structure"] == "tunnel";
                if (((GORoadFeature)goFeature).isTunnel)
                {
                    goFeature.kind = GOFeatureKind.tunnel;
                }
				((GORoadFeature)goFeature).isLink = properties.Contains ("structure") && (string)properties ["structure"] == "link";


                //Fix for v8 streetnames
                if (goFeature.properties.Contains("name") && !string.IsNullOrEmpty((string)goFeature.properties["name"]))
                {
                    goFeature.name = (string)goFeature.properties["name"];
                }
                else {
                    goFeature.name = null;
                }
            } 

//			goFeature.y = (goFeature.index / 50.0f) + goFeature.getLayerDefaultY() /150.0f;
//			float fraction = goFeature.layer.layerType == GOLayer.GOLayerType.Buildings? 100f:10f;
			float fraction = 20f;
			goFeature.y = (1 + goFeature.layerIndex + goFeature.featureIndex/goFeature.featureCount)/fraction;

			goFeature.setRenderingOptions ();
			goFeature.height = goFeature.renderingOptions.polygonHeight;

			if (goFeature.layer.useRealHeight && properties.Contains("height")) {
				double h =  Convert.ToDouble(properties["height"]);
				goFeature.height = (float)h;
			}

			if (goFeature.layer.useRealHeight && properties.Contains("min_height")) {
				double minHeight = Convert.ToDouble(properties["min_height"]);
				goFeature.y = (float)minHeight;
				goFeature.height = (float)goFeature.height - (float)minHeight;
			} 

            if (goFeature.layer.forceMinHeight && goFeature.height < goFeature.renderingOptions.polygonHeight && goFeature.y < 0.5f)
            {
                goFeature.height = goFeature.renderingOptions.polygonHeight;
            }

			return goFeature;

		}

        #region NETWORK

        public override string vectorUrl()
        {
            var baseUrl = "https://api.protomaps.com/tiles/v3/";
            var extension = ".mvt";

            //Download vector data
            Vector2 realPos = goTile.tileCoordinates;
            var tileurl = goTile.zoomLevel + "/" + realPos.x + "/" + realPos.y;

            var completeUrl = baseUrl + tileurl + extension;
            //			var filename = "[MapzenProtoVector]" + gameObject.name;

            if (goTile.apiKey != null && goTile.apiKey != "")
            {
                string u = completeUrl + "?key=" + goTile.apiKey;
                completeUrl = u;
            }

            return completeUrl;

        }

        public override string demUrl ()
		{
            return null;
		}

		public override string normalsUrl ()
		{
			return null;
		}

		public override string satelliteUrl (Vector2? tileCoords = null)
		{
            return null;
        }


		#endregion

	}
}
