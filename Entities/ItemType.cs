public class ItemType
{
    public int Id { get; set; }

    public required string Name { get; set; }

    // Navigation property
    //public required ICollection<Item> Items { get; set; }
}
