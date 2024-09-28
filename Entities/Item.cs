using System;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Data;

public class Item
{
    public int Id { get; set; }

    public required string Name { get; set; }

    public int ItemTypeId { get; set; }
    //public ItemType ItemType { get; set; }

    // Navigation properties
    // public KeyItem KeyItem { get; set; }
    // //public WeaponItem WeaponItem { get; set; }
    // public ICollection<PlayerItem> PlayerItems { get; set; }
    // public ICollection<ItemInRoom> ItemsInRoom { get; set; }
}

public class ItemDataAccess(string connectionString)
{
    private string _connectionString = connectionString;

    public List<Item> GetItems()
    {
        List<Item> items = new List<Item>();
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            string itemKeyQuery = "SELECT * FROM Item;";
            using (SqlCommand command = new SqlCommand(itemKeyQuery, connection))
            {
                //command.Parameters.Add("@playerId", SqlDbType.Int).Value = player.Id;
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    items = new List<Item>();
                    while(reader.Read())
                    {
                        var item = new Item{
                            Id = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            ItemTypeId = reader.GetInt32(2),
                        };
                        items.Add(item);
                    }
                    reader.Close();
                }
            }
            return items;
        }

    }
}
