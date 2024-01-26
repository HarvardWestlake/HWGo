using System.Collections.Generic;

public class Player
{
    public string username;
    public string password;
    public int playerID;
    public int level;
    public List<Faculty> inventory; // A list of Faculty objects

    // Constructor to initialize the Player object
    public Player(string username, string password, int playerID, int level)
    {
        this.username = username;
        this.password = password;
        this.playerID = playerID;
        this.level = level;
        this.inventory = new List<Faculty>();
    }
}

