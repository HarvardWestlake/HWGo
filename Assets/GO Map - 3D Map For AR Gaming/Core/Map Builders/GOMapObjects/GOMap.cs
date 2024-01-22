﻿using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;
using UnityEngine.Events;
using System.Linq;

using GoShared;
using LocationManagerEnums;
using UnityEngine.Networking;

#if UNITY_EDITOR
using UnityEditor;
#endif

//GOMAP 3.0

namespace GoMap
{
	[ExecuteInEditMode]
	public class GOMap : MonoBehaviour 
	{

        public BaseLocationManager locationManager;
		[Range (0,8)]  public int tileBuffer = 2;
		[ShowOnly] public int zoomLevel = 0;
		public bool useCache = true;
		[InspectorButton("ClearCache")] public bool clearCache;
		[InspectorButton("OpenCacheFolder")] public bool openCache;

		public GOMapType mapType = GOMapType.Mapbox;
		public enum GOMapType
		{

			Mapbox,
			OSM,
			Esri,
			Nextzen,
			Protomaps,
			CustomTile,
		}
		[Header("Drag here your custom GOTile subclass")]
		public GOTile customTile;
		[Header("Or enter the api key for the selected map provider")]


		[Regex (@"^(?!\s*$).+", "Please insert your MapBox Access Token")]public string mapbox_accessToken = "";
		[Regex (@"^(?!\s*$).+", "Please insert your OSM API key")]public string osm_api_key = "";
		[Regex (@"^(?!\s*$).+", "Please insert your Nextzen API key")] public string nextzen_api_key = "";
		[Regex(@"^(?!\s*$).+", "Please insert your Protomaps API key")] public string protomaps_api_key = "";

		
			
		public Material tileBackground;
		[HideInInspector] public GameObject tempTileBackgorund;
		public GOLayer[] layers;
		public GOPOILayer pois;
		public GOLabelsLayer labels;

        public GOTilesetLayer[] customMapboxTilesets;
        [Header("---")]
        [Header("    ")]

        public bool useElevation = false;
        [Range(0.1f,10)] public float elevationMultiplier = 1.0f;
		[Range(10, 100)]public float terrainResolution = 50.0f;
		public bool runOnMainThread = false;
		public bool combineFeatures = false;
		public bool addGoFeatureComponents = false;
		public bool useSatelliteBackground = false;
		public bool satellite4X = false;
        public GONavMeshSettings navmeshSettings;

		[HideInInspector]
		public bool dynamicLoad = true;

		public GOTileEvent OnTileLoad;

		[HideInInspector] public List <GOTile> tiles = null;

		//features ID
		[HideInInspector]public IList buildingsIds;
		[HideInInspector]public  List <GOCenterContainer> containers;
        private IEnumerator destroyTilesRoutine;

		void Start () 
	    {

			buildingsIds = new List<object>();
			tiles = new List<GOTile>();
			containers = new List<GOCenterContainer>();


			if (useElevation && !CheckIfLayerMaskExists ("GOTerrain")) {
				ShowAlert("GO Map needs a Unity Layer named \"GOTerrain\" to be set up. Open the layers tab in the top right section of your Unity editor.", true);
				return;
			}

			zoomLevel = locationManager.zoomLevel;

			if (zoomLevel > 15 && useElevation) {
				ShowAlert("Elevation is available only for zoomlevel <= 15. Zoom level 15 is set.", false);
				zoomLevel = 15;
			} 

			if (mapType == GOMapType.OSM && locationManager.zoomLevel > 14) {
				ShowAlert("Zoom level " +zoomLevel+"is not available with this map provider. Zoom level 14 is set.", false);
				zoomLevel = 14;
				tileBuffer = tileBuffer == 0? 0 : tileBuffer- 1;
			}

			if (mapType == GOMapType.Protomaps && locationManager.zoomLevel > 15)
			{
				ShowAlert("Zoom level " + zoomLevel + "is not available with this map provider. Zoom level 15 is set.", false);
				zoomLevel = 15;
			}

			if (mapType == GOMapType.Nextzen && locationManager.zoomLevel > 14 && useElevation == true)
            {
				ShowAlert("Elevation at zoom level " + zoomLevel + "is not available with this map provider", false);
                zoomLevel = 14;
                tileBuffer = tileBuffer == 0 ? 0 : tileBuffer - 1;
            }

            if (mapType == GOMapType.Esri && locationManager.zoomLevel > 15) {
				ShowAlert("Zoom level " +zoomLevel+"is not available with this map provider. Zoom level 15 is set.", false);
				zoomLevel = 15;
			}

			if (zoomLevel == 0) {
				zoomLevel = locationManager.zoomLevel;	
			}

			if ( mapType == GOMapType.Nextzen && (nextzen_api_key == null || nextzen_api_key == "")) {
				ShowAlert("Nextzen api key is missing, GET iT HERE: https://developers.nextzen.org/keys", true);
				return;
			}

			else if (mapType == GOMapType.Mapbox  && (mapbox_accessToken == null || mapbox_accessToken == "")) {
				ShowAlert("Mapbox access token is missing, GET IT HERE: https://www.mapbox.com/help/create-api-access-token/", true);
				return;
			}
			else if (mapType == GOMapType.OSM && (osm_api_key == null || osm_api_key == "")) {
				ShowAlert("Open Street Maps api key is missing, GET IT HERE: https://cloud.maptiler.com", true); 
				return;
			}
			else if (mapType == GOMapType.Protomaps && (protomaps_api_key == null || protomaps_api_key == ""))
			{
				ShowAlert("Protomaps api key is missing, GET IT HERE: https://protomaps.com/dashboard", true);
				return;
			}
			else if (mapType == GOMapType.Protomaps && (protomaps_api_key == null || protomaps_api_key == ""))
			{
				ShowAlert("Protomaps api key is missing, GET IT HERE: https://protomaps.com/dashboard", true);
				return;
			}
			else if (mapType == GOMapType.CustomTile && customTile == null)
			{
				ShowAlert("Custom tile is missing, link your own custom GOTile subclass to continue", true);
				return;
			}

			if (useSatelliteBackground && (mapType == GOMapType.Nextzen || mapType == GOMapType.OSM) && string.IsNullOrEmpty (mapbox_accessToken)) {
				ShowAlert("Satellite background is not supported by this API, insert a Mapbox access token to use it, GET IT HERE: https://www.mapbox.com/help/create-api-access-token/", true); 
				return;
			}

			if (combineFeatures && zoomLevel <= 15) {
				combineFeatures = false;
				ShowAlert("Combine features is available only at zoom levels higher than 15", false); 
			}


			locationManager.onOriginSet.AddListener((Coordinates) => {OnOriginSet(Coordinates);});
			locationManager.onLocationChanged.AddListener((Coordinates) => {OnLocationChanged(Coordinates);});
				

			if (tileBackground != null && Application.isPlaying) {
				CreateTemporaryMapBackground ();
			}
	    }

        internal void BuildMapPortionInsideEditor(Coordinates location, GOMapInspector.CustomCoordinatesBounds portionBounds)
        {
            throw new NotImplementedException();
        }


		#region Location Manager Events
		public event Action<Coordinates> OnLocationChangedEvent;
		public event Action<Coordinates> OnOriginSetEvent;

		public void OnLocationChanged (Coordinates currentLocation) {
			if (OnLocationChangedEvent != null)
			{
				OnLocationChangedEvent.Invoke(currentLocation);
			}
			if (tileBackground != null /*&& Application.isMobilePlatform*/) {
				DestroyTemporaryMapBackground ();
			}

			StartCoroutine(ReloadMap (currentLocation,true));
		}

		public void OnOriginSet (Coordinates currentLocation) {

			if (OnOriginSetEvent != null)
			{
				OnOriginSetEvent.Invoke(currentLocation);
			}
			//if (locationManager.demoLocation == DemoLocation.SearchMode)
			DestroyCurrentMap ();

			if (tileBackground != null /*&& Application.isMobilePlatform*/) {
				DestroyTemporaryMapBackground ();
			}
			StartCoroutine(ReloadMap (currentLocation,false));
		}

		#endregion

		#region GoMap Load

		public IEnumerator ReloadMap (Coordinates location, bool delayed) {

			if (!dynamicLoad) {
				yield break;
			}
				
			GOFeature.BuildingElevationOffset *= locationManager.worldScale;
			GOFeature.RoadsHeightForElevation *= locationManager.worldScale;

			//Get SmartTiles
			List <Vector2> tileList = location.adiacentNTiles(zoomLevel,tileBuffer);
				
			List <GOTile> newTiles = new List<GOTile> ();

			// Create new tiles
			foreach (Vector2 tileCoords in tileList) {

				if (!isSmartTileAlreadyCreated (tileCoords, zoomLevel)) {

					GOTile adiacentSmartTile = createSmartTileObject (tileCoords, zoomLevel);
					adiacentSmartTile.gameObject.transform.position = adiacentSmartTile.goTile.tileCenter.convertCoordinateToVector();
					adiacentSmartTile.map = this;

					newTiles.Add (adiacentSmartTile);

					if (tileBackground != null) {
						adiacentSmartTile.createTileBackground();
					}
				}
			}

			foreach (GOTile tile in newTiles) {

                if (Application.isPlaying)
                    yield return tile.StartCoroutine(tile.LoadTileData(layers, delayed));
                else {

                    GORoutine routine = GORoutine.start(tile.LoadTileData(layers, delayed), tile);
                    while (!routine.finished) {
                        yield return null;
                    }
                }
			}
				
			//Destroy far tiles
			List <Vector2> tileListForDestroy = location.adiacentNTiles(zoomLevel,tileBuffer+1);
            if (destroyTilesRoutine!= null)
                yield return (destroyTilesRoutine);
            destroyTilesRoutine = DestroyTiles(tileListForDestroy);
            yield return StartCoroutine (destroyTilesRoutine);

		}

		public List <string> layerNames () {
		
			List <string> l = new List<string>();
			for (int i = 0; i < layers.ToList().Count; i++) {
				if (layers [i].disabled == false) {
					l.Add(layers [i].json());
				}
			}
			return l;
		}

		public IEnumerator DestroyTiles (List <Vector2> list) {

			try {
				List <string> tileListNames = new List <string> ();
				foreach (Vector2 v in list) {
					string s = GOTileObj.TileNamePrototype(v,zoomLevel);
					tileListNames.Add (s);
				}

				List <GOTile> toDestroy = new List<GOTile> ();
				foreach (GOTile tile in tiles) {
					if (tile != null && !tileListNames.Contains (tile.name) && tile.goTile.status == GOTileObj.GOTileStatus.Done) {
                        //Debug.Log(tile.goTile.status.ToString());
                        toDestroy.Add (tile);
					}
				}
				for (int i = 0; i < toDestroy.Count; i++) {

					GOTile tile = toDestroy [i];
					tiles.Remove (tile);
					GameObject.Destroy (tile.gameObject,i);
				}
			} catch (Exception ex) {
				Debug.LogWarning (ex);
			}
			yield return null;
		}

		bool isSmartTileAlreadyCreated (Vector2 tileCoords, int Zoom) {

			string name = GOTileObj.TileNamePrototype(tileCoords,zoomLevel);
			return transform.Find (name);
		}

		GOTile createSmartTileObject (Vector2 tileCoords, int Zoom) {
        
            GOTileObj goTile = new GOTileObj (tileCoords,zoomLevel, mapType,useElevation, elevationMultiplier, new Vector2 (terrainResolution,terrainResolution), useCache, addGoFeatureComponents,locationManager.worldScale,useSatelliteBackground,satellite4X,combineFeatures,navmeshSettings);
			GameObject tileObj = new GameObject(goTile.name);

			tileObj.transform.parent = gameObject.transform;
			GOTile tile;

            if (mapType == GOMapType.Mapbox)
            {
                tile = tileObj.AddComponent<GOMapboxTile>();
                goTile.apiKey = mapbox_accessToken;
            }
            else if (mapType == GOMapType.Nextzen)
            {
                tile = tileObj.AddComponent<GOMapzenProtoTile>();
                goTile.apiKey = nextzen_api_key;
            }
            else if (mapType == GOMapType.OSM)
            {
                tile = tileObj.AddComponent<GOOSMTile>();
                goTile.apiKey = osm_api_key;
            }
			else if (mapType == GOMapType.Esri) {
				tile = tileObj.AddComponent<GOEsriTIle> ();
			}
			else if (mapType == GOMapType.Protomaps)
			{
				tile = tileObj.AddComponent<GOProtomapsTile>();
				goTile.apiKey = protomaps_api_key;
			}
			else if (mapType == GOMapType.CustomTile)
			{
				var type = customTile.GetType();
				tile = (GOTile)tileObj.AddComponent(type);
				goTile.apiKey = protomaps_api_key;
			}
			else {
				tile = tileObj.AddComponent<GOTile> ();
			}

			goTile.position = goTile.tileCenter.convertCoordinateToVector();
//			goTile.verticesInWorld = goTile.vertsInWorld (zoomLevel);
			tile.gameObject.transform.position = goTile.position;
			tile.map = this;

			tile.goTile = goTile;
			tiles.Add(tile);

			return tile;
		}



		void OnApplicationQuit()
		{
			tiles.Clear ();
		}

		#endregion

		#region Utils

		public void dropPin(double lat, double lng, GameObject go) {

			Coordinates coordinates = new Coordinates (lat, lng,0);
			go.transform.localPosition = coordinates.convertCoordinateToVector(go.transform.position.y);

		}

		//Drop a game object ad fixed latitude/longitude
		public void dropPin(Coordinates coordinates, GameObject go) {

			go.transform.localPosition = coordinates.convertCoordinateToVector(go.transform.position.y);

		}

		//Draws a line given a list of latitudes/longitudes
        public GameObject dropLine(List<Coordinates> polyline, float width, float height, Material material, GOUVMappingStyle uvMappingStyle) {

			List <Vector3> converted = new List<Vector3> ();
			foreach (Coordinates coordinates in polyline) {

				Vector3 v = coordinates.convertCoordinateToVector ();
				if (useElevation)
					v = GOMap.AltitudeToPoint (v);

				converted.Add (v);
			}

            return dropLine (converted, width, height, material, uvMappingStyle);
		}

		//Draws a line given a list of vector 3
        public GameObject dropLine(List<Vector3> polyline, float witdh, float height, Material material, GOUVMappingStyle uvMappingStyle,bool curved = false) {
		
			GameObject line = new GameObject ("Polyline");

			MeshFilter filter = line.AddComponent<MeshFilter>();
			MeshRenderer renderer = line.AddComponent<MeshRenderer>();

			GOLineMesh lineMesh = new GOLineMesh (polyline,curved);
			lineMesh.width = witdh;
			lineMesh.load (line);

            GOMesh goMesh = lineMesh.CreatePremesh();
            goMesh.uvMappingStyle = uvMappingStyle;

			if (height > 0) {
                goMesh = SimpleExtruder.SliceExtrudePremesh (goMesh, height, 4f,4f,10f);
			}

            filter.sharedMesh = goMesh.ToMesh();
			renderer.material = material;

			line.AddComponent<MeshCollider> ();

			return line;
		}

		//Draws a polygon given a list of latitudes/longitudes that is a closed shape
        public GameObject dropPolygon(List<Coordinates> shape, float height, Material material, GOUVMappingStyle uvMappingStyle) {

			List <Vector3> converted = new List<Vector3> ();
			foreach (Coordinates coordinates in shape) {

				Vector3 v = coordinates.convertCoordinateToVector ();
				if (useElevation)
					v = GOMap.AltitudeToPoint (v);

				converted.Add (v);
			}

			if (!GOFeature.IsClockwise (converted)) {
				converted.Reverse ();
			}

            return dropPolygon (converted, height, material, uvMappingStyle);

		}

		//Draws a polygon given a list of Vector3 that is a closed shape
        public GameObject dropPolygon(List<Vector3> shape, float height, Material material, GOUVMappingStyle uvMappingStyle) {

			GameObject polygon = new GameObject ("Polygon");

			MeshFilter filter = polygon.AddComponent<MeshFilter>();
			MeshRenderer renderer = polygon.AddComponent<MeshRenderer>();

			Poly2Mesh.Polygon poly = new Poly2Mesh.Polygon();
			poly.outside = shape;

            GOMesh goMesh = Poly2Mesh.CreateMeshInBackground(poly);
            goMesh.uvMappingStyle = uvMappingStyle;
            goMesh.ApplyUV(shape);

			if (height > 0) {
                goMesh = SimpleExtruder.SliceExtrudePremesh (goMesh, height, 4f,4f,10f);
			}

            Mesh mesh = goMesh.ToSubmeshes();

			filter.sharedMesh = mesh;
			renderer.material = material;

			polygon.AddComponent<MeshCollider> ();

			return polygon;

		}
			

		public static float AltitudeForPoint (Vector3 v) {

			RaycastHit hit;
			LayerMask masks = GetActiveMasks ();

			if (CheckIfLayerMaskExists ("Roads"))
				masks = (int)masks | (int)LayerMask.GetMask ("Roads");

			Ray ray = new Ray (v+ new Vector3(0,10000,0), Vector3.down );
			if (Physics.Raycast (ray, out hit,Mathf.Infinity,masks)) {
				return hit.point.y;
			}

			return v.y;
		} 

		public static Vector3 AltitudeToPoint (Vector3 v) {

			Vector3 np = v;
			np.y = AltitudeForPoint (v);
			return np;
		} 
			
		public static bool IsPointAboveWater(Vector3 vector) {

			LayerMask watermask = LayerMask.GetMask ("Water");
			RaycastHit hit;
			if ( Physics.Raycast(vector+ new Vector3(0,10000,0), Vector3.down, out hit, Mathf.Infinity, watermask) )
			{
				return true;
			} 

			return false;

		}

		public static LayerMask GetActiveMasks() {

			bool tm = CheckIfLayerMaskExists ("GOTerrain");
			bool bm = CheckIfLayerMaskExists ("Buildings");

			if (tm && bm)
				return LayerMask.GetMask("GOTerrain", "Buildings");
			if (tm && !bm)
				return LayerMask.GetMask("GOTerrain");
			if (!tm && bm)
				return LayerMask.GetMask("Buildings");

			return 1;

		}

		public static bool CheckIfLayerMaskExists(string name) {

			LayerMask mask = LayerMask.NameToLayer (name);
			if (mask.value > 0 && mask.value < 31) {
				return true;
			}
			return false;
		}



		public void DestroyCurrentMap () {
		
			while (transform.childCount > 0) {
				foreach (Transform child in transform) {
					GameObject.DestroyImmediate (child.gameObject);
				}
			}

			GOEnvironment env = GameObject.FindObjectOfType<GOEnvironment>();
			if (env == null) {
				return;
			}

			while (env.transform.childCount > 0) {
				foreach (Transform child in env.transform) {
					GameObject.DestroyImmediate (child.gameObject);
				}
			}
		}



		#endregion

		#region Temporary Tile Background

		private void CreateTemporaryMapBackground () {

			float size = 2000;

			tempTileBackgorund = new GameObject ("Temporary tile background");

			MeshFilter filter = tempTileBackgorund.AddComponent<MeshFilter>();
			MeshRenderer renderer = tempTileBackgorund.AddComponent<MeshRenderer>();
		
			filter.sharedMesh = GOGridMaker.CreateGrid(size,10).ToMesh();
			renderer.material = tileBackground;

			tempTileBackgorund.AddComponent<MeshCollider> ();

		} 

		private void DestroyTemporaryMapBackground () {
			
			GameObject.DestroyImmediate (tempTileBackgorund);
			tempTileBackgorund = null;
		}

		#endregion

		#region GoTerrain Link

		public IEnumerator createTileWithPreloadedData(Vector2 tileCoords, int Zoom, bool delayedLoad, object mapData) {

			#if GOLINK
			switch (goTerrain.goTerrainOptions.vectorAPI) {
			case  GOTerrainOptions.VectorAPI.MapzenJSON:
					mapType = GOMapType.MapzenJson;
					break;
			case  GOTerrainOptions.VectorAPI.Mapzen:
					mapType = GOMapType.Mapzen;
					break;
			case  GOTerrainOptions.VectorAPI.Mapbox:
					mapType = GOMapType.Mapbox;
					break;
			}
			#endif

			if (!isSmartTileAlreadyCreated (tileCoords, zoomLevel)) {

				GOTile tile = createSmartTileObject (tileCoords, zoomLevel);
//				tile.gameObject.transform.position = tile.goTile.tileCenter.convertCoordinateToVector();
//				tile.map = this;

				if (tileBackground != null) {
					tile.createTileBackground();
				}

				yield return tile.StartCoroutine(tile.ParseTileData(layers,delayedLoad,layerNames()));

				if (OnTileLoad != null) {
					OnTileLoad.Invoke(tile);
				}
			}
			yield return null;
		}

		#endregion

		#region CacheControl

		public void ClearCache() {
			FileHandler.ClearEverythingInFolder (FileHandler.GoCachePath());
		}


		public void OpenCacheFolder () {
			#if UNITY_EDITOR
			EditorUtility.RevealInFinder (FileHandler.GoCachePath());
			#endif
		}

		#endregion

		#region Editor Map Builder

		public void BuildInsideEditor () {
        
            dynamicLoad = true;

            //This fixes the map origin
            ((LocationManager)locationManager).LoadDemoLocation ();

			//Wipe buildings id list
			buildingsIds = new List<object>();
			tiles = new List<GOTile>();
			containers = new List<GOCenterContainer>();

			//Start load routine (This might take some time...)
            if (locationManager.worldOrigin == null) {
				Debug.LogWarning ("[GOMap Editor] Warning, check if you have \"No GPS Test\" set on the Location Manager.");
				return;
			}

            IEnumerator routine = ReloadMap (locationManager.worldOrigin.tileCenter (locationManager.zoomLevel), true);
			GORoutine.start (routine,this);

		}

        public void BuildMapPortionInsideEditor(Coordinates location, Coordinates origin)
        {

            dynamicLoad = true;

            //This fixes the map origin
            ((LocationManager)locationManager).SetOriginAndLocation(origin, location);

            //Wipe buildings id list
            buildingsIds = new List<object>();
            tiles = new List<GOTile>();
            containers = new List<GOCenterContainer>();

            //Start load routine (This might take some time...)
            if (locationManager.worldOrigin == null)
            {
                Debug.LogWarning("[GOMap Editor] Warning, check if you have \"No GPS Test\" set on the Location Manager.");
                return;
            }

            IEnumerator routine = ReloadMap(location, true);
            GORoutine.start(routine, this);

        }

        public IEnumerator TestEditorUnityWebRequest() {

#if UNITY_EDITOR

            var www = UnityWebRequest.Get("https://tile.nextzen.org/tilezen/vector/v1/all/17/70076/48701.json");
            yield return www.SendWebRequest();

            ContinuationManager.Add(() => www.isDone, () =>
				{
					if (!string.IsNullOrEmpty(www.error)) Debug.Log("UnityWebRequest failed: " + www.error);
					Debug.Log("[GOMap Editor] Request success: " + www.downloadHandler.text);
				});
#endif
            yield return null;
		}
		#endregion


		private void ShowAlert(String message, bool stopEditor) {
#if UNITY_EDITOR
			EditorUtility.DisplayDialog("Go Map",message, "Ok");
			if (EditorApplication.isPlaying && stopEditor)
			{
				UnityEditor.EditorApplication.isPlaying = false;
			}
#endif
		} 

	}



	#region Events

	[Serializable]
	public class GOFeatureEvent : UnityEvent <GOFeature,GameObject> {


	}

	[Serializable]
	public class GOEvent : UnityEvent <Mesh,GOFeature,Vector3> {


	}

	[Serializable]
	public class GOTileEvent : UnityEvent <GOTile> {


	}
	#endregion
}