
    using System;
    using System.Data.SqlClient;
    using System.Collections.Generic;
    using System.Data;


    public class KeyItem :InventoryItem
    {
        public override int Id { get; init;}

        public required override string Name { get; init; }

        public override int ItemTypeId { get; init;}
        public required string Description { get; set; }


        public int? DoorId { get; set; }
        public required Door Door { get; set; }

        // Navigation property
        //public Item Item { get; set; }
    }

    public class KeyItemsDataAccess(string connectionString)
    {
        private string _connectionString = connectionString;
        public  List<InventoryItem> GetItems(List<Door> doors)
        {
            List<InventoryItem>inventoryItems = new List<InventoryItem>();
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    string itemKeyQuery = "SELECT i.Id, i.Name, i.ItemTypeId, k.Description, k.DoorId " +
                                        "FROM Item i " +
                                        "JOIN KeyItem k ON i.Id = k.Id " +
                                        "JOIN PlayerItem pi ON i.Id = pi.ItemId";

                    //string itemKeyQuery = @"SELECT i.Id, i.Name, i.ItemTypeId, k.Description, k.DoorId 
                    // FROM Item i
                    // JOIN KeyItem k on i.Id = k.Id
                    // JOIN PlayerItem pi on  i.Id = pi.ItemId";

                    Console.WriteLine(itemKeyQuery);
                                        
                    connection.Open();

                    using (SqlCommand command = new SqlCommand(itemKeyQuery, connection))
                    {
                        //command.Parameters.Add("@playerId", SqlDbType.Int).Value = player.Id;
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while(reader.Read())
                            {
                                var item = new KeyItem{
                                    Id = reader.GetInt32(0),
                                    Name = reader.GetString(1),
                                    ItemTypeId = reader.GetInt32(2),
                                    Description = reader.GetString(3),
                                    Door = doors.Where(d => d.Id == reader.GetInt32(4)).Single()
                                };
                                inventoryItems.Add(item);
                            }
                            reader.Close();
                        }
                    }
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return inventoryItems;
        }
    }
