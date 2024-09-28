public class ItemInRoom
{
    public int Id { get; set; }

    public required string Description { get; set; }

    public int RoomId { get; set; }
    //public Room Room { get; set; }

    public int ItemId { get; set; }
    //public Item Item { get; set; }
}
