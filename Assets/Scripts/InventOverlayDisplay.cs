using UnityEngine;
using UnityEngine.UI; // For UI components like Button
using System.Collections;

public class InventOverlayDisplay : MonoBehaviour
{
    // Let's assume you have a way to access all your faculty members
    // For this example, let's use an ArrayList similar to the CharacterTemplate class
    public ArrayList FacultyList;

    // Unity's Start method to set up your FacultyList with all faculty members
    void Start()
    {
        // Initialize your FacultyList here with all faculty members
        FacultyList = ...;
    }

    // This method will be called when the button is clicked
    public void OnButtonClick(string facultyId)
    {
        // Iterate through your faculty list to find the one with the matching ID
        foreach (Faculty faculty in FacultyList)
        {
            if (faculty.Id == facultyId)
            {
                // Access the faculty variables here
                // For example, display their name or other properties
                DisplayFacultyDetails(faculty);
                break; // Stop the loop once we've found the right faculty
            }
        }
    }

    // Method to display faculty details (modify this according to your needs)
    private void DisplayFacultyDetails(Faculty faculty)
    {
        // Display the faculty details on the UI
        // For example:
        // NameText.text = faculty.Name;
        // DepartmentText.text = faculty.Dept.ToString();
        // and so on...
    }
}