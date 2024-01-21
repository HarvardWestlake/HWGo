using System.Collections;
using GoMap;
using GoShared;
using UnityEngine;

public class MapBoundsController : MonoBehaviour
{
    public GOMap goMap;
    public float minLatitude = 40.0f;
    public float maxLatitude = 41.0f;
    public float minLongitude = -75.0f;
    public float maxLongitude = -74.0f;

    void Start()
    {
        if (goMap != null)
        {
            // Subscribe to GoMap events with parameters
            goMap.OnLocationChangedEvent += OnLocationChanged;
            goMap.OnOriginSetEvent += OnOriginSet;
        }
        else
        {
            Debug.LogError("GoMap reference is null. Please assign a valid GoMap instance to goMap.");
        }
    }
    private void OnLocationChanged(Coordinates currentLocation)
    {
        // Handle OnLocationChanged event with parameters
        Debug.Log("Location changed: " + currentLocation);
    }

    // Callback for GoMap OnOriginSet event with parameters
    private void OnOriginSet(Coordinates currentLocation)
    {
        // Handle OnOriginSet event with parameters
        Debug.Log("Origin set: " + currentLocation);
    }

    void Update()
    {
        // You can update the map based on the current location during the Update loop
        if (goMap != null)
        {
            Coordinates currentLocation = goMap.locationManager.currentLocation;
            LoadMapBasedOnBounds(currentLocation);
        }
    }

    void OnLocationChangedHandler(Coordinates currentLocation)
    {
        // Handle location changed event
        LoadMapBasedOnBounds(currentLocation);
    }

    void OnOriginSetHandler(Coordinates currentLocation)
    {
        // Handle origin set event
        LoadMapBasedOnBounds(currentLocation);
    }

    IEnumerator LoadMapBasedOnBounds(Coordinates location)
    {
        if (IsLocationInBounds(location))
        {
            // Load the map only if the location is within bounds
            goMap.DestroyCurrentMap();
            yield return goMap.ReloadMap(location, true);
        }
        else
        {
            Debug.LogWarning("Current location is outside the specified bounds.");
        }
    }

    bool IsLocationInBounds(Coordinates location)
    {
        return location.latitude >= minLatitude &&
               location.latitude <= maxLatitude &&
               location.longitude >= minLongitude &&
               location.longitude <= maxLongitude;
    }
}
