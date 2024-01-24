/*using System;
using System.Collections.Generic;
using UnityEngine;



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
        var existingFaculty = facInventory.Find(f => f.faculty.id == facultyToAdd.id);
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
}*/