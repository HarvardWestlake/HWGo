using UnityEngine;
using GoShared;
using UnityEngine.Serialization;

namespace GoMap {

	[System.Serializable]
	public class GORenderingOptions
	{
		public GameObject smokeOverlayPrefab;
		private GameObject smokeOverlayInstance;

		public void showArea (Camera mainCamera, float minLatitude, float maxLatitude, float minLongitude, float maxLongitude)
        {
			float centerLatitude = (minLatitude + maxLatitude) / 2f;
			float centerLongitude = (minLongitude + maxLongitude) / 2f;

			Vector2 centerWorldPosition = new Vector2(34.14045f, -118.41306f);
			float areaSize = Mathf.Max(maxLatitude - minLatitude, maxLongitude - minLongitude);

			if (smokeOverlayInstance == null)
            {
				smokeOverlayInstance = GameObject.Instantiate(smokeOverlayPrefab, Vector2.zero, Quaternion.identity);
            }
			smokeOverlayInstance.transform.localScale = new Vector2(areaSize, areaSize);
			smokeOverlayInstance.transform.position = new Vector2(centerWorldPosition.x, smokeOverlayInstance.transform.position.y);
			mainCamera.transform.position = new Vector2(centerWorldPosition.x, mainCamera.transform.position.y);

        }

		public GOFeatureKind kind;
		public Material material;
		[ConditionalHide("parent/layerType", "Roads")] public Material outlineMaterial;
		[ConditionalHide("parent/layerType", "Buildings")] public bool hasRoof;
		[ConditionalHide("parent/layerType", "Buildings")] public Material roofMaterial;


		public Material[] materials;

//		[ConditionalHide("parent/layerType", "Roads")]
		public float lineWidth;
		[ConditionalHide("parent/layerType", "Roads")] public float outlineWidth;
		[ConditionalHide("parent/layerType", "Roads", true)] public float polygonHeight;

		public string tag;

	}

}