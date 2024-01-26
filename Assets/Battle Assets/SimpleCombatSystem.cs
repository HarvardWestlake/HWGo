using UnityEngine;
using UnityEngine.UI;
using TMPro; // Add this line
using System.Collections; // Needed for IEnumerator

public class SimpleCombatSystem : MonoBehaviour
{
    public int healthMultiplier = 7;
    public int healthBase = 20;
    private int currentPlayerHealth;
    private int currentEnemyHealth;
    private int totalPlayerHealth;
    private int totalEnemyHealth;
    private int playerLevel;
    private int enemyLevel;

    public Slider playerHealthBar; // Assign this in the inspector
    public TMP_Text playerHealthText; // Assign this in the inspector
    public TMP_Text playerLevelText; // Assign this in the inspector

    public Slider enemyHealthBar; // Assign this in the inspector
    public TMP_Text enemyHealthText; // Assign this in the inspector
    public TMP_Text enemyLevelText; // Assign this in the inspector

    private bool isPlayerTurn = true; // Flag to track the current turn

    void Start()
    {
        int randomIntForPlayer = Random.Range(1, 101); // Generates a random integer from 1 to 100
        int randomIntForEnemy = Random.Range(1, 101); // Generates a random integer from 1 to 100

        playerLevel = randomIntForPlayer;
        enemyLevel = randomIntForEnemy;

        currentPlayerHealth = healthBase + ((randomIntForPlayer - 1) * healthMultiplier);
        currentEnemyHealth = healthBase + ((randomIntForEnemy - 1) * healthMultiplier);

        totalPlayerHealth = currentPlayerHealth;
        totalEnemyHealth = currentEnemyHealth;

        UpdateLevels();
        UpdateHealthBars();
    }

    public void PlayerDamage()
    {
        if (isPlayerTurn == true) {
            int damage = 20;
            currentEnemyHealth -= damage;
            currentEnemyHealth = Mathf.Clamp(currentEnemyHealth, 0, totalEnemyHealth);
            UpdateHealthBars();
            StartCoroutine(EndPlayerTurn());
        }
    }

    public void PlayerHeal()
    {
        if (isPlayerTurn == true) {
            int amount = 10;
            currentPlayerHealth += amount;
            currentPlayerHealth = Mathf.Clamp(currentPlayerHealth, 0, totalPlayerHealth);
            UpdateHealthBars();
            StartCoroutine(EndPlayerTurn());
        }
    }

    private void EnemyDamage()
    {
        int damage = 20;
        currentPlayerHealth -= damage;
        currentPlayerHealth = Mathf.Clamp(currentPlayerHealth, 0, totalPlayerHealth);
        UpdateHealthBars();
    }

    private void EnemyHeal()
    {
        int amount = 10;
        currentEnemyHealth += amount;
        currentEnemyHealth = Mathf.Clamp(currentEnemyHealth, 0, totalEnemyHealth);
        UpdateHealthBars();
    }

    IEnumerator EndPlayerTurn()
    {
        isPlayerTurn = false;
        yield return new WaitForSeconds(2); // Wait for 2 seconds after player's action
        StartCoroutine(StartEnemiesTurn());
    }

    IEnumerator StartEnemiesTurn()
    {
        // Enemy's action logic
        if (Random.Range(0, 2) == 0) // Randomly choose 0 or 1
        {
            EnemyDamage();
        }
        else
        {
            EnemyHeal();
        }

        yield return new WaitForSeconds(2); // Wait for 2 seconds

        isPlayerTurn = true; // Give control back to the player
    }

    void UpdateLevels()
    {
        if (playerLevelText != null)
        {
            playerLevelText.text = "Lv. " + playerLevel;
        }
        if (enemyLevelText != null)
        {
            enemyLevelText.text = "Lv. " + enemyLevel;
        }
    }

    void UpdateHealthBars()
    {
        if (playerHealthBar != null)
        {
            playerHealthBar.value = (float)currentPlayerHealth / totalPlayerHealth;
        }
        if (playerHealthText != null)
        {
            playerHealthText.text = currentPlayerHealth + "/" + totalPlayerHealth;
        }
        if (enemyHealthBar != null)
        {
            enemyHealthBar.value = (float)currentEnemyHealth / totalEnemyHealth;
        }
        if (enemyHealthText != null)
        {
            enemyHealthText.text = currentEnemyHealth + "/" + totalEnemyHealth;
        }
    }
}
