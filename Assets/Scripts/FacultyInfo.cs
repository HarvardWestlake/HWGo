using System;
using System.Collections.Generic;
using System.Linq;

public class FacultyInfo
{
    private static Random random = new Random();

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
        { "Stout", Department.Math },
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
    static string GenerateUniqueId()
    {
        string lastName = GetRandomUnusedLastName();

        // Use the first six letters of the last name, or the entire last name if less than six letters
        string shortenedLastName = lastName.Length >= 6 ? lastName.Substring(0, 6) : lastName;

        // Generate random numbers to complete the 12-character ID
        int randomNumbersLength = 12 - shortenedLastName.Length;
        string randomNumbers = random.Next((int)Math.Pow(10, randomNumbersLength), (int)Math.Pow(10, randomNumbersLength + 1) - 1).ToString();

        // Ensure the random numbers part is the correct length
        randomNumbers = randomNumbers.PadLeft(randomNumbersLength, '0');

        // Combine shortened last name and random numbers to form the ID
        return shortenedLastName + randomNumbers;
    }

    string GetRandomUnusedLastName()
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

    public static List<Faculty> GenerateAllFaculty()
    {
        List<Faculty> allFaculties = new List<Faculty>();

        // Specify the number of faculties you want to generate
        int numberOfFaculties = 10;

        for (int i = 0; i < numberOfFaculties; i++)
        {
            string randomId = GenerateUniqueId();
            string lastName = randomId.Substring(0, randomId.Length - 6); // Extract last name from ID
            Department department = GenerateDepartmentForLastName(lastName);
            Rarity randomRarity = (Rarity)random.Next(Enum.GetValues(typeof(Rarity)).Length);

            // Assuming health, damage, and catchPrompt have some default values or logic
            double defaultHealth = 100.0;
            double defaultDamage = 10.0;
            List<string> defaultCatchPrompt = new List<string> { "DefaultPrompt1", "DefaultPrompt2" };

            // Create a new faculty with random attributes
            Faculty faculty = new Faculty(randomId, department, randomRarity, defaultHealth, defaultDamage, defaultCatchPrompt);

            // Add the faculty to the list
            allFaculties.Add(faculty);
        }

        return allFaculties;
    }
}