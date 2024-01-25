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

    public ArrayList FacultyList;

    private TextMeshProUGUI facultyName;

    private void Awake() {
        Debug.Log("This is a debug message!");

        facultyName = transform.Find("testest").GetComponent<TextMeshProUGUI>();

        SetName("Mr Varney");

        if (CharacterTemplate.Instance != null)
        {
            FacultyList = CharacterTemplate.Instance.FacultyList;
        }
    }

    private void SetName(string inputText) {
        facultyName.text = inputText; // Use '.text' instead of '.setText'
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
        nameText.text = "Name: " + faculty.Name;
        departmentText.text = "Department: " + faculty.Dept.ToString();
        rarityText.text = "Rarity: " + faculty.Rarity.ToString();
        healthText.text = "Health: " + faculty.Health.ToString();
        damageText.text = "Damage: " + faculty.Damage.ToString();
        specialAttackText.text = "Special Attack: " + faculty.SpecialAttack;

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
    }

    public void OnButtonClickWithId1()
    {
        OnButtonClick("0001");
    }
}
