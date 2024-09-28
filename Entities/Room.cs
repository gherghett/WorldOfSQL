using System;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Data;


public class Room
{
    public int Id { get; set; }

    public required string Name { get; set; }

    // Navigation properties
    public ICollection<Door>? DoorsFrom { get; set; }
    public ICollection<Door>? DoorsTo { get; set; }
    //public ICollection<ItemInRoom> ItemsInRoom { get; set; }
}

public class RoomDataAccess(string connectionString)
{
    private string _connectionString = connectionString;

    public  List<Room> GetRooms()
    {
        List<Room> rooms = new List<Room>();
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            connection.Open();
            string itemKeyQuery = "SELECT * FROM Room";
            using (SqlCommand command = new SqlCommand(itemKeyQuery, connection))
            {
                //command.Parameters.Add("@playerId", SqlDbType.Int).Value = player.Id;
                using (SqlDataReader reader = command.ExecuteReader())
                {
                        while(reader.Read())
                    {
                        var item = new Room{
                            Id = reader.GetInt32(0),
                            Name = reader.GetString(1)
                        };
                        rooms.Add(item);
                    }
                    reader.Close();
                }
            }
            connection.Close();
        }
        return rooms;
    }
}
