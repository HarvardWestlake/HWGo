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
    public string attack;


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
        name = "Name: " + faculty.Name;
        department = "Department: " + faculty.Dept.ToString();
        rarity = "Rarity: " + faculty.Rarity.ToString();
        health = "Health: " + faculty.Health.ToString();
        damage = "Damage: " + faculty.Damage.ToString();
        attack = "Special Attack: " + faculty.SpecialAttack;

        //nameText = transform.Find("DisplayName").GetComponent<TextMeshProUGUI>();


        SetName(name);

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
        if (nameText != null) {
            nameText.text = nameInput; // Use the .text property to set the text
        } else {
            Debug.LogError("nameText is not set.");
        }
    }

    public void OnButtonClickWithId1()
    {
        Debug.Log("click ID 1!");
        OnButtonClick("0001");
    }
}
