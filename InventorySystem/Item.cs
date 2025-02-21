using Fallin.Characters;

namespace Fallin.InventorySystem
{
    public abstract class Item
    {
        public string Name { get; }
        public int Quantity;

        protected Item(string name, int quantity=1)
        {
            Name = name;
            Quantity = quantity;
        }


        public abstract void UseItem(Character target);
    }
}