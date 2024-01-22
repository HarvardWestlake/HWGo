using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoShared;
using GoMap;

public class PokemonSpawner : MonoBehaviour
{
    public List<GameObject> pokemonPrefabs; // List of all Pokémon prefabs
    public float latitudeMin, latitudeMax, longitudeMin, longitudeMax; // Geofence boundaries

    public GOMap goMap;

    IEnumerator Start()
    {
        yield return StartCoroutine(goMap.locationManager.WaitForOriginSet());

        SpawnPokemon();
    }

    void SpawnPokemon()
    {
        for (int i = 0; i < 15; i++)
        {
            // Get a random location within the geofence
            float latitude = Random.Range(latitudeMin, latitudeMax);
            float longitude = Random.Range(longitudeMin, longitudeMax);

            Coordinates coordinates = new Coordinates(latitude, longitude);

            // Choose a random Pokémon prefab
            // GameObject pokemonPrefab = pokemonPrefabs[Random.Range(0, pokemonPrefabs.Count)];

            GameObject pokemonPrefab = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            pokemonPrefab.transform.localScale = new Vector3(10, 10, 10);
            pokemonPrefab.GetComponent<MeshRenderer>().material.color = Color.red;


            goMap.dropPin(coordinates, pokemonPrefab);
        }
    }
}