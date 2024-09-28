using System;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Data;


public class Door
{
    public int Id { get; set; }

    public required string Description { get; set; }

    public bool Locked { get; set; }

    public int FromRoomId { get; set; }
    public required Room FromRoom { get; set; }

    public required Room ToRoom { get; set; }

    // Navigation property
    //public KeyItem KeyItem { get; set; }
}

public class DoorDataAccess(string connectionString)
{
    private string _connectionString = connectionString;

    public List<Door> GetDoors(List<Room> rooms)
    {
        List<Door> doors = new List<Door>();
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            string itemKeyQuery = "SELECT * FROM Door";
            connection.Open();
            using (SqlCommand command = new SqlCommand(itemKeyQuery, connection))
            {
                //command.Parameters.Add("@playerId", SqlDbType.Int).Value = player.Id;
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    List<Door> inventoryItems = new List<Door>();
                    while(reader.Read())
                    {
                        var door = new Door{
                            Id = reader.GetInt32(0),
                            Description = reader.GetString(1),
                            FromRoom = rooms.Where(r => r.Id == reader.GetInt32(2)).Single(),
                            ToRoom = rooms.Where(r => r.Id == reader.GetInt32(3)).Single(),
                            Locked = reader.GetBoolean(4),
                        };
                        doors.Add(door);
                    }
                    reader.Close();
                }
            }
            connection.Close();
        }
        return doors;
    }

}

                            
