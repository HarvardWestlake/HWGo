using UnityEngine;
using UnityEngine.UI; // For UI components like Button
using TMPro; // Make sure you have this line to use TextMeshProUGUI
using System.Collections;

public class InventOverlayDisplay : MonoBehaviour
{
    // Reference to UI Text elements
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI departmentText;
    public TextMeshProUGUI rarityText;
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI damageText;
    public TextMeshProUGUI specialAttackText;
    public RawImage facultyImage; // For displaying the image

    private string name;
    private string department;
    private string rarity;
    private string health;
    private string damage;
    private string attack;


    public ArrayList FacultyList;

    //private TextMeshProUGUI testText;

    private void Awake() {
        Debug.Log("InventOverlayDisplay awakened");

        //facultyName = transform.Find("DisplayName").GetComponent<TextMeshProUGUI>();

        //SetName("Mr Varney");
        //testText = transform.Find("testest").GetComponent<TextMeshProUGUI>();

        //testText.SetText("Mr Varney");

        if (CharacterTemplate.Instance != null)
        {
            Debug.Log("CharacterTemplate instance works");
            FacultyList = CharacterTemplate.Instance.FacultyList;
        }
        else
        {
            Debug.LogError("CharacterTemplate instance is null.");
        }

        if(gameObject.activeInHierarchy) {
            Debug.Log("ACTIVE");
        }
        if (!gameObject.activeInHierarchy) {
            Debug.LogError(gameObject.name + " is not active in the hierarchy.");
        }
        if (!nameText.gameObject.activeInHierarchy) {
            Debug.LogError(nameText.gameObject.name + " is not active in the hierarchy.");
        }
    }

    public void OnButtonClick(string facultyId)
    {
        // Check if FacultyList is not null and has elements
        if (FacultyList != null && FacultyList.Count > 0)
        {
            foreach (Faculty faculty in FacultyList)
            {
                if (faculty != null && faculty.Id == facultyId)
                {
                    DisplayFacultyDetails(faculty);
                    break;
                }
            }
        }
        else
        {
            Debug.LogError("FacultyList is null or empty.");
        }
    }


    private void DisplayFacultyDetails(Faculty faculty)
    {
        // Update each UI element with the corresponding faculty property
        name = faculty.Name;
        department = "Department: " + faculty.Dept.ToString();
        rarity = "Rarity: " + faculty.Rarity.ToString();
        health = faculty.Health.ToString();
        damage = faculty.Damage.ToString();
        attack = "Special Attack: " + faculty.SpecialAttack;

        //nameText = transform.Find("DisplayName").GetComponent<TextMeshProUGUI>();
        SetName(name);
        SetDepartment(department);
        SetRarity(department);
        SetHealth(health);
        SetDamage(damage);
        SetAttack(attack);

        if (faculty.Image != null)
        {
            facultyImage.texture = faculty.Image;
            facultyImage.enabled = true;
        }
        else
        {
            facultyImage.enabled = false; // Hide the image if not available
        }

        //debugging
        if (facultyImage == null)
        {
            Debug.LogError("facultyImage is not set. Please assign it in the Inspector.");
            return;
        }
    }

    private void SetName(string nameInput) {
        if(nameText == null) {
            Debug.LogError("this is WRONG");
        }
        if (nameText != null) {
            nameText.text = nameInput; // Use the .text property to set the text
        } 
        else {
            Debug.LogError("nameText is not set.");
        }
    }

    private void SetDepartment(string departmentInput) {
        if(departmentText == null) {
            Debug.LogError("this is WRONG");
        }
        if (departmentText != null) {
            departmentText.text = departmentInput; // Use the .text property to set the text
        } 
        else {
            Debug.LogError("departmentText is not set.");
        }
    }

    private void SetAttack(string attackInput) {
        if(specialAttackText == null) {
            Debug.LogError("this is WRONG");
        }
        if (specialAttackText != null) {
            specialAttackText.text = attackInput; // Use the .text property to set the text
        } 
        else {
            Debug.LogError("specialAttackText is not set.");
        }
    }

    private void SetRarity(string rarityInput) {
        if(rarityText == null) {
            Debug.LogError("this is WRONG");
        }
        if (rarityText != null) {
            rarityText.text = rarityInput; // Use the .text property to set the text
        } 
        else {
            Debug.LogError("rarityText is not set.");
        }
    }

    private void SetHealth(string healthInput) {
        if(healthText == null) {
            Debug.LogError("this is WRONG");
        }
        if (healthText != null) {
            healthText.text = healthInput; // Use the .text property to set the text
        } 
        else {
            Debug.LogError("healthText is not set.");
        }
    }

    private void SetDamage(string damageInput) {
        if(damageText == null) {
            Debug.LogError("this is WRONG");
        }
        if (damageText != null) {
            damageText.text = damageInput; // Use the .text property to set the text
        } 
        else {
            Debug.LogError("damageText is not set.");
        }
    }

    public void OnButtonClickWithId1()
    {
        Debug.Log("click ID 1!");
        OnButtonClick("0001");
    }
}
