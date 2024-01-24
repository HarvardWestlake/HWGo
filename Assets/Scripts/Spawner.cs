using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoShared;
using GoMap;
using UnityEngine.SceneManagement;
// player prefs

public class PokemonSpawner : MonoBehaviour
{
    public List<GameObject> pokemonPrefabs; // List of all Pokémon prefabs
    public float latitudeMin, latitudeMax, longitudeMin, longitudeMax; // Geofence boundaries
    private GameObject pokemonPrefabInstance; // An instance of the Pokémon prefab

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
            GameObject pokemonPrefab = pokemonPrefabs[Random.Range(0, pokemonPrefabs.Count)];
            // Instantiate the Pokémon at the determined coordinates
            Vector3 position = coordinates.convertCoordinateToVector();
            pokemonPrefabInstance = Instantiate(pokemonPrefab, position, Quaternion.identity);
            // make its name its original + ID
            pokemonPrefabInstance.name = pokemonPrefab.name + " " + (i + 1);

            goMap.dropPin(coordinates, pokemonPrefabInstance);
        }
    }

    public void ClickButton()
    {
        // Find the closest Pokémon to the player, and make sure it's within 2000
        GameObject closestPokemon = FindClosestPokemon();

        if (closestPokemon != null && Vector3.Distance(closestPokemon.transform.position, transform.position) < 2000)
        {
            // Destroy the Pokémon
            Destroy(closestPokemon);

            SceneManager.LoadScene("CatchingScene");
        }

    }

    GameObject FindClosestPokemon()
    {
        GameObject[] pokemon = GameObject.FindGameObjectsWithTag("Pokemon");

        GameObject closestPokemon = null;
        float closestDistance = Mathf.Infinity;

        foreach (GameObject p in pokemon)
        {
            float distance = Vector3.Distance(p.transform.position, transform.position);

            if (distance < closestDistance)
            {
                closestPokemon = p;
                closestDistance = distance;
            }
        }

        Debug.Log("Closest Pokémon is " + closestPokemon.name + " at " + closestDistance + " distance");

        // save name in player prefs
        PlayerPrefs.SetString("pokemonName", closestPokemon.name);

        return closestPokemon;
    }
}