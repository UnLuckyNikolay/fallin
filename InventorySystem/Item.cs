namespace Fallin.InventorySystem
{
    public abstract class Item
    {
        public required string Name { get; init; }
        public int Quantity = 1;


        public abstract void UseItem();
    }
}