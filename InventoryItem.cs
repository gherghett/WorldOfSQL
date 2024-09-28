// this class is inherited by all items that player can have in inventory
// the Item class cant be in inventory as its a base table for child items like KeyItem
public abstract class InventoryItem
{
    public abstract int Id { get; init;}

    public abstract string Name { get; init;}

    //this represents the type of item
    public abstract int ItemTypeId { get; init;} 
}