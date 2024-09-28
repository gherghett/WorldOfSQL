using System;
using System.Linq;
using System.Net.NetworkInformation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

class Program
{
    static void Main(string[] args)
    {
        // Build configuration to access user secrets
        var config = new ConfigurationBuilder()
            .AddUserSecrets<Program>()
            .Build();

        string connectionString = config["ConnectionStrings:MyDbConnectionString"]!;
        //Console.WriteLine(connectionString);

        // Set up context
        //these three have the environment data, that doesnt change
        RoomDataAccess roomDataAccess = new RoomDataAccess(connectionString);
        List<Room> rooms = roomDataAccess.GetRooms();

        DoorDataAccess doorDataAccess = new DoorDataAccess(connectionString);
        List<Door> doors = doorDataAccess.GetDoors(rooms);

        KeyItemsDataAccess keyItemsDataAccess = new KeyItemsDataAccess(connectionString);
        List<InventoryItem> inventoryItems = keyItemsDataAccess.GetItems(doors);

        //player and tables like item, KeyItem, PlayerItem, these change when playing the game.
        PlayerDataAccess playerDataAccess = new PlayerDataAccess(connectionString);
        Player player = playerDataAccess.GetPlayer(1, inventoryItems);

        Console.ReadLine();
    }
}