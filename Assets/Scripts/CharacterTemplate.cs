using System;
using System.Collections;

// Define the enums that represent department and rarity
public enum Department
{
    // Populate with actual departments
    Math,
    Science,
    Literature,
    // etc.
}

public enum Rarity
{
    Common,
    Rare,
    Epic,
    Legendary
    // etc.
}

// Define the Faculty class with the specified variables
public class Faculty
{
    public string Id { get; set; }
    public string Name { get; set; }
    public Department Dept { get; set; }
    public Rarity Rarity { get; set; }
    public double Health { get; set; }
    public double Damage { get; set; }
    public string SpecialAttack { get; set; }
    public Texture2D Image { get; set; }

    // Constructor for the Faculty class
    public Faculty(string id, string name, Department dept, Rarity rarity, double health, double damage, string attack, string imagePath)
    {
        Id = id;
        Name = name;
        Dept = dept;
        Rarity = rarity;
        Health = health;
        Damage = damage;
        SpecialAttack = attack;
        LoadImage(imagePath);
    }

    public Faculty()
    {
        Id = 0001;
        Name = "Mr Varney";
        Dept = Math;
        Rarity = Legendary;
        Health = 150.0;
        Damage = 173;
        SpecialAttack = "The Varney Special";
        LoadImage("varney pixel");
    }

    private void LoadImage(string imagePath)
    {
        // Resources.Load<Texture2D> assumes the path is relative to a Resources folder in your Assets directory.
        // The image must also be marked as readable in the import settings to be loaded at runtime.
        Image = Resources.Load<Texture2D>(imagePath);
    }
}

// Define the CharacterTemplate class that contains an ArrayList of Faculty objects
public class CharacterTemplate
{
    // ArrayList to store Faculty objects
    public ArrayList FacultyList { get; private set; }

    // Constructor for the CharacterTemplate class
    public CharacterTemplate()
    {
        FacultyList = new ArrayList();
    }

    // Method to add a Faculty object to the FacultyList
    public void AddFaculty(Faculty faculty)
    {
        FacultyList.Add(faculty);
    }

    public void AddVarney(Faculty faculty)
    {
        Faculty varney = new Faculty();
        FacultyList.Add(varney);
    }

}