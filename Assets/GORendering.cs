using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GORendering : MonoBehaviour
{
    public GameObject smokeOverlayPrefab; // Prefab for the smoke or obscuring element

    private GameObject smokeOverlayInstance; // Reference to the instantiated smoke overlay

    public void ShowCustomArea(Camera mainCamera, float minLatitude, float maxLatitude, float minLongitude, float maxLongitude)
    {
        // Calculate the center coordinates of the specified area
        float centerLatitude = (minLatitude + maxLatitude) / 2f;
        float centerLongitude = (minLongitude + maxLongitude) / 2f;

        // Convert center coordinates (latitude and longitude) to world coordinates
        Vector2 centerWorldPosition = new Vector2(34.140470585811755f, -118.41304575602013f);


        // Calculate the size of the specified area
        float areaSize = Mathf.Max(maxLatitude - minLatitude, maxLongitude - minLongitude);

        // Create or move the smoke overlay to cover the specified area
        if (smokeOverlayInstance == null)
        {
            smokeOverlayInstance = GameObject.Instantiate(smokeOverlayPrefab, Vector3.zero, Quaternion.identity);
        }

        // Set the scale of the smoke overlay to cover the specified area
        smokeOverlayInstance.transform.localScale = new Vector3(areaSize, 1f, areaSize);

        // Move the smoke overlay to cover the specified area
        smokeOverlayInstance.transform.position = new Vector2(centerWorldPosition.x, smokeOverlayInstance.transform.position.y);

        // Move the camera to focus on the center of the specified area
        mainCamera.transform.position = new Vector2(centerWorldPosition.x, mainCamera.transform.position.y);
    }
}
