using UnityEngine;
using UnityEngine.UI;
using TMPro; // Add this line
using System.Collections; // Needed for IEnumerator
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Ability {
    public string name;
    public int value; // This can be damage or healing based on isDamage
    public bool isDamage; // true for damage, false for heal
    public int minHits; // Minimum number of hits for multiattack
    public int maxHits; // Maximum number of hits for multiattack
    public string description; // Description for UI or tooltips

    public Ability(string name, int value, bool isDamage, int minHits, int maxHits, string description) {
        this.name = name;
        this.value = value;
        this.isDamage = isDamage;
        this.minHits = minHits;
        this.maxHits = maxHits;
        this.description = description;
    }

    // Add methods for executing the ability, if necessary
}

[System.Serializable]
public class Pokemon
{
    public string name;
    public List<Ability> abilities;
    public int currentHealth;
    public int totalHealth;

    public Pokemon(string name, List<Ability> abilities, int totalHealth)
    {
        this.name = name;
        this.abilities = abilities;
        this.currentHealth = this.totalHealth = totalHealth;
    }

    // Additional properties and methods for each Pokemon can be added here
}

public class SimpleCombatSystem : MonoBehaviour
{
    public List<Pokemon> allPokemons; // List of all available Pokemons
    private Pokemon currentPokemon; // The currently selected Pokemon
    private Pokemon enemyPokemon; // The enemy Pokemon

    public Button[] abilityButtons; // Assign these in the inspector
    public TMP_Text pokemonNameText; // Assign this in the inspector
    public Slider playerHealthBar; // Assign this in the inspector
    public Slider enemyHealthBar; // Assign this in the inspector

    void Start()
    {
        // Define Abilities
        Ability tackle = new Ability("Tackle", 10, true, 1, 1, "A basic attack.");
        Ability heal = new Ability("Heal", 15, false, 1, 1, "Restores health.");
        Ability furySwipes = new Ability("Fury Swipes", 5, true, 2, 4, "Hits 2 to 4 times.");
        Ability temp = new Ability("Test", 0, true, 1, 1, "Test Attack");

        // Create a list of abilities for each Pokemon
        List<Ability> pikachuAbilities = new List<Ability> { tackle, furySwipes, temp, temp};
        List<Ability> bulbasaurAbilities = new List<Ability> { tackle, heal, temp, temp};

        // Define Pokemons with their abilities
        Pokemon pikachu = new Pokemon("Pikachu", pikachuAbilities, 100);
        Pokemon bulbasaur = new Pokemon("Bulbasaur", bulbasaurAbilities, 100);

        // Add these Pokemons to the allPokemons list
        allPokemons = new List<Pokemon> { pikachu, bulbasaur };

        // Select initial Pokemon and enemy for demonstration
        currentPokemon = allPokemons[0];
        enemyPokemon = allPokemons[1];

        UpdateUI();
    }

    public void SelectAbility(int abilityIndex)
    {
        if (abilityIndex < 0 || abilityIndex >= currentPokemon.abilities.Count)
            return;

        Ability selectedAbility = currentPokemon.abilities[abilityIndex];
        ExecuteAbility(selectedAbility, ref enemyPokemon.currentHealth, enemyPokemon.totalHealth);

        // Update UI to reflect changes
        UpdateHealthBar(enemyHealthBar, enemyPokemon.currentHealth, enemyPokemon.totalHealth);

        // Example of enemy turn (simplified for demonstration)
        StartCoroutine(EnemyTurn());
    }

    IEnumerator EnemyTurn()
    {
        yield return new WaitForSeconds(2); // Wait for 2 seconds
        // Enemy selects a random ability and uses it
        Ability enemyAbility = enemyPokemon.abilities[Random.Range(0, enemyPokemon.abilities.Count)];
        ExecuteAbility(enemyAbility, ref currentPokemon.currentHealth, currentPokemon.totalHealth);

        // Update UI to reflect changes
        UpdateHealthBar(playerHealthBar, currentPokemon.currentHealth, currentPokemon.totalHealth);
    }

    private void ExecuteAbility(Ability ability, ref int targetHealth, int totalHealth)
    {
        int numberOfHits = Random.Range(ability.minHits, ability.maxHits + 1);

        for (int i = 0; i < numberOfHits; i++)
        {
            if (ability.isDamage)
            {
                targetHealth -= ability.value;
            }
            else
            {
                targetHealth += ability.value;
            }
            targetHealth = Mathf.Clamp(targetHealth, 0, totalHealth);
        }
    }

    private void UpdateUI()
    {
        pokemonNameText.text = currentPokemon.name;

        for (int i = 0; i < abilityButtons.Length; i++)
        {
            if (i < currentPokemon.abilities.Count)
            {
                abilityButtons[i].gameObject.SetActive(true);
                abilityButtons[i].GetComponentInChildren<TMP_Text>().text = currentPokemon.abilities[i].name;
            }
            else
            {
                abilityButtons[i].gameObject.SetActive(false);
            }
        }

        UpdateHealthBar(playerHealthBar, currentPokemon.currentHealth, currentPokemon.totalHealth);
        UpdateHealthBar(enemyHealthBar, enemyPokemon.currentHealth, enemyPokemon.totalHealth);
    }

    private void UpdateHealthBar(Slider healthBar, int currentHealth, int totalHealth)
    {
        healthBar.value = (float)currentHealth / totalHealth;
    }

    // Additional methods and game logic can be added here
}
