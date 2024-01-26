using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class AssignImages : MonoBehaviour
{
    public Button[] inventoryButtons; // Array to store your 15 buttons
    public Image[] buttonImages; // Array to store the images associated with each button

    // List of owned faculty objects
    public List<Faculty> ownedFacultyList = new List<Faculty>();

    void Start()
    {
        // Call a method to update the button images based on the owned faculty list
        UpdateInventoryUI();
    }

    void UpdateInventoryUI()
    {
        // Loop through the inventory buttons and assign faculty images
        for (int i = 0; i < inventoryButtons.Length; i++)
        {
            if (i < ownedFacultyList.Count)
            {
                // If you have a faculty object at this index, assign its image to the button
                // buttonImages[i].sprite = ownedFacultyList[i].facultyImage; Fix This
                inventoryButtons[i].interactable = true; // Enable the button
            }
            else
            {
                // If there are no more faculty objects, disable the button
                inventoryButtons[i].interactable = false;
            }
        }
    }
}