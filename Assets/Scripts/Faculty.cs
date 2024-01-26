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
    public string id; // ID associated with faculty
    public Department dept; // Faculty department
    public Rarity rarity; // Rarity level, e.g., common, rare, etc.
    public double health; // Number of health points faculty has
    public double damage; // Number of damage points done per attack
    public List<string> catchPrompt; // Prompts related to specific teacher that catcher must answer to catch teacher

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

    // You might also want to add methods to handle the faculty's behaviors, like taking damage or performing an action.
}
