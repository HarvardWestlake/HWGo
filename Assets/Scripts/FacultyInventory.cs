using System;
using System.Collections.Generic;
using UnityEngine;

// Assuming you have a Faculty class already defined as per your previous image
[Serializable]
/*public class Faculty
{
    public string id; // ID associated with faculty
    public Department dept; // Faculty department
    public Rarity rarity; // Rarity level, e.g., common, rare, etc.
    public double health; // Number of health points faculty has
    public double damage; // Number of damage points done per attack
    public List<string> catchPrompt; // Prompts related to specific teacher that catcher must answer to catch teacher
    //Do we need catchPrompt??

    // You can also add a constructor to initialize the properties
    public Faculty(string id, Department dept, Rarity rarity, double health, double damage, List<string> catchPrompt)
    {
        this.id = id;
        this.dept = dept;
        this.rarity = rarity;
        this.health = health;
        this.damage = damage;
        this.catchPrompt = catchPrompt;
    }
}*/

[Serializable]
public class FacultyMemberQuantity
{
    public Faculty faculty;
    public int quantity;

    public FacultyMemberQuantity(Faculty faculty, int quantity)
    {
        this.faculty = faculty;
        this.quantity = quantity;
    }
}

public class FacultyInventory : MonoBehaviour
{
    // Variable to store the list of owned faculties and their quantities
    public List<FacultyMemberQuantity> facInventory = new List<FacultyMemberQuantity>();

    // Method to add a faculty to the inventory
    public void AddFaculty(Faculty facultyToAdd)
    {
        // Check if the faculty is already in the inventory
        var existingFaculty = facInventory.Find(f => f.faculty.id == facultyToAdd.id);//boolean
        if (existingFaculty != null)
        {
            // If found, increase the quantity
            existingFaculty.quantity++;
        }
        else
        {
            // If not found, add it to the list with a quantity of 1
            facInventory.Add(new FacultyMemberQuantity(facultyToAdd, 1));
        }
    }

    // Method to remove a faculty from the inventory
    public bool RemoveFaculty(Faculty facultyToRemove)
    {
        // Check if the faculty is in the inventory
        var existingFaculty = facInventory.Find(f => f.faculty.id == facultyToRemove.id);
        if (existingFaculty != null && existingFaculty.quantity > 0)
        {
            // Decrease the quantity and return true
            existingFaculty.quantity--;
            return true;
        }
        
        // If not found or quantity is 0, return false
        return false;
    }

    // You can add more methods here to handle displaying the faculties, etc.
}