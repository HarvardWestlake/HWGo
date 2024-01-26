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

    private TextMeshProUGUI facultyName;

    private void Awake() {
        Debug.Log("This is a debug message!");

        //facultyName = transform.Find("DisplayName").GetComponent<TextMeshProUGUI>();

        //SetName("Mr Varney");

        if (CharacterTemplate.Instance != null)
        {
            FacultyList = CharacterTemplate.Instance.FacultyList;
        }
    }

    public void OnButtonClick(string facultyId)
    {
        foreach (Faculty faculty in FacultyList)
        {
            if (faculty.Id == facultyId)
            {
                DisplayFacultyDetails(faculty);
                break;
            }
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

        // Assuming you are using Texture2D for faculty images
        if (faculty.Image != null)
        {
            facultyImage.texture = faculty.Image;
            facultyImage.enabled = true;
        }
        else
        {
            facultyImage.enabled = false; // Hide the image if not available
        }

        nameText = transform.Find("DisplayName").GetComponent<TextMeshProUGUI>();

        SetName(name);
    }

    private void SetName(string nameInput) {
        nameText.SetText(nameInput);
    }

    public void OnButtonClickWithId1()
    {
        Debug.Log("click ID 1!");
        OnButtonClick("0001");
    }
}
