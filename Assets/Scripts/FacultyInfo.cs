using System;
using System.Collections.Generic;
using System.Linq;

public class FacultyInfo
{
    private static Random random = new Random();
    List<Faculty> allFaculty = new List<Faculty>();

    // List of last names for generating unique IDs
    private static Dictionary<string, Department> lastNameDepartments = new Dictionary<string, Department>
    {
        { "Campbell", Department.Math },
        { "Helston", Department.Math },
        { "Kochar", Department.Math },
        { "Lamberto-Egan", Department.Math },
        { "Lieberman", Department.Math },
        { "Lopez", Department.Math },
        { "Mori", Department.Math },
        { "Movsisian", Department.Math },
        { "AStout", Department.Math },
        { "Stout", Department.Math }, // Assuming both "Stout" entries should have Department.Math
        { "Nealis", Department.Math },
        { "O’Connor", Department.Math },
        { "Palmer", Department.Math },
        { "Sim", Department.Math },
        { "Theiss", Department.Math },
        { "Varney", Department.Math },
        { "Weis", Department.Math },
        { "Williams", Department.Math },
        { "An", Department.English },
        { "Cohen", Department.English },
        { "Gasparino", Department.English },
        { "Long", Department.English },
        { "Medawar", Department.English },
        { "Eisenkolb", Department.Science },
        { "Ellington", Department.Science },
        { "Lee", Department.Science },
        { "Park", Department.Science },
        { "Reiner", Department.Science },
        { "Hilt", Department.Theater },
        { "Martin", Department.Theater },
        { "Peters", Department.Theater },
        { "Spears", Department.Theater },
        { "Washburn", Department.Theater }
    };

    private static Dictionary<string, bool> usedNames = new Dictionary<string, bool>();

    // Method to generate a unique ID using the first six letters of last names (or the entire last name if less than six letters) and 6 random numbers
    static int GenerateUniqueId(String lastName)
    {
        int randomNumber = random.Next(1, 16); // Generate random number between 1 and 15
        return randomNumber;


    }

    static string GetRandomUnusedLastName()
    {
        List<string> unusedLastNames = lastNameDepartments.Keys.Where(name => !usedNames.ContainsKey(name) || !usedNames[name]).ToList();
        if (unusedLastNames.Count == 0)
        {
            throw new InvalidOperationException("All last names have been used.");
        }

        string randomLastName = unusedLastNames[random.Next(unusedLastNames.Count)];
        usedNames.Add(randomLastName, true); // Mark the last name as used

        return randomLastName;
    }

    private static Department GenerateDepartmentForLastName(string lastName)
    {
        return lastNameDepartments.ContainsKey(lastName) ? lastNameDepartments[lastName] : Department.History; // Default department if not specified
    }

    List<Faculty> GenerateAllFaculty()
    { 

        // Specify the number of faculties you want to generate
        int numberOfFaculties = 15;

        for (int i = 0; i < numberOfFaculties; i++)
        {
            String lastName = GetRandomUnusedLastName();
            int randomId = GenerateUniqueId(lastName);
            Department department = GenerateDepartmentForLastName(lastName);
            Rarity randomRarity = (Rarity)random.Next(Enum.GetValues(typeof(Rarity)).Length);

            // Assuming health, damage, and catchPrompt have some default values or logic
            double defaultHealth = 100.0;
            double defaultDamage = 10.0;
            List<string> defaultCatchPrompt = new List<string> { "DefaultPrompt1", "DefaultPrompt2" };

            // Create a new faculty with random attributes
            Faculty faculty = new Faculty(randomId, department, randomRarity, defaultHealth, defaultDamage, defaultCatchPrompt);

            // Add the faculty to the list
            allFaculty.Add(faculty);
        }

        return allFaculty;
    }
    public Faculty GetFacultyById(int facultyId)
    {
        return allFaculty.FirstOrDefault(faculty => faculty.id == facultyId);
    }
}
