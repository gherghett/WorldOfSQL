using System;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Data;

public class Player
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Email { get; set; } 
    public required string Password { get; set; }

    // Navigation property
    public required ICollection<InventoryItem> PlayerItems { get; set; }

    public override string ToString() =>
        $"Id: {Id}, Name:{Name}, Items: {string.Join("\n", PlayerItems.ToList().Select(pi => "\t"+pi.ToString()))}";
    
}

public class PlayerDataAccess(string connectionString)
{
    private string _connectionString = connectionString;

    public Player GetPlayer(int playerId, List<InventoryItem> items)
    {
        Player? player = null;

        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            connection.Open();
            List<InventoryItem> inventoryItems = new();
            string itemKeyQuery = "SELECT pi.ItemId FROM PlayerItem pi "+
                                "WHERE pi.PlayerId = @playerId";
            using (SqlCommand command = new SqlCommand(itemKeyQuery, connection))
            {
                command.Parameters.Add("@playerId", SqlDbType.Int).Value = playerId;
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while(reader.Read())
                    {
                        inventoryItems.Add(items.Where(i => i.Id == reader.GetInt32(0)).Single());
                    }
                    reader.Close();
                }
            }
            string queryString = "SELECT Id, Name, Email, Password FROM Player WHERE id = @id";
            using( SqlCommand command = new SqlCommand(queryString, connection))
            {
                command.Parameters.Add("@Id", SqlDbType.Int).Value = playerId;

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        player = new Player
                        {
                            Id = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            Email = reader.GetString(2),
                            Password = reader.GetString(3),
                            PlayerItems = inventoryItems,
                        };
                    }
                    reader.Close();
                }
            }

            connection.Close();
        }
        return player ?? throw new Exception();
    }

    public static void SavePlayer(Player player)
    {
        
    }
}
