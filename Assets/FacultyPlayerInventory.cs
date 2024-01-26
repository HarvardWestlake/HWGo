using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FacultyPlayerInventory
{
    private List<Faculty> inventory = new List<Faculty>();

    public void AddFaculty(int facultyId)
    {
        Faculty newFaculty = new Faculty(facultyId);
        inventory.Add(newFaculty);
    }

    public Faculty GetFacultyById(int id)
    {
        return inventory.Find(faculty => faculty.id == id);
    }

    // Method to remove a Faculty member from the inventory by their ID
    public bool RemoveFacultyById(int id)
    {
        var faculty = GetFacultyById(id);
        if (faculty != null)
        {
            inventory.Remove(faculty);
            return true;
        }
        return false;
    }

    public List<Faculty> GetCurrencyFaculty() {
        return inventory;
    }

    // Additional methods as needed...
}
