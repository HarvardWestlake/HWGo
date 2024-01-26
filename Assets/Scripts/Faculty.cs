using System;
using System.Collections.Generic;

public enum Department
{
    Math,
    Science,
    History,
    English,
    Theater
    // ... add other departments
}

public enum Rarity
{
    Common,
    Rare,
    Epic,
    Legendary
    // ... add other rarity levels if needed
}

[Serializable]
public class Faculty
{
    public int id; // ID associated with faculty
    public Department dept; // Faculty department
    public Rarity rarity; // Rarity level, e.g., common, rare, etc.
    public double health; // Number of health points faculty has
    public double damage; // Number of damage points done per attack
    public List<string> catchPrompt; // Prompts related to specific teacher that catcher must answer to catch teacher

    private static Random random = new Random(); // Random number generator

    // You can also add a constructor to initialize the properties
    public Faculty(int id, Department dept, Rarity rarity, double health, double damage, List<string> catchPrompt)
    {
        this.id = id;
        this.dept = dept;
        this.rarity = rarity;
        this.health = health;
        this.damage = damage;
        this.catchPrompt = catchPrompt;
    }

    // Temp constructor for basic critical usage
    public Faculty(int id) {
        this.id = id;
        Array departments = Enum.GetValues(typeof(Department));
        Department randomDepartment = (Department)departments.GetValue(random.Next(departments.Length));
        this.dept = randomDepartment;
        Array rarities = Enum.GetValues(typeof(Rarity));
        Rarity randomRarity = (Rarity)rarities.GetValue(random.Next(rarities.Length));
        this.rarity = randomRarity;
        this.health = 1.0;
        this.damage = 1.0;
        this.catchPrompt = null;
    }

    // You might also want to add methods to handle the faculty's behaviors, like taking damage or performing an action.
}
