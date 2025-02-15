using Fallin.Characters;

namespace Fallin.InventorySystem
{
    public abstract class Item
    {
        public string Name { get; }
        public int Quantity = 1;

        protected Item(string name)
        {
            Name = name;
        }


        public abstract void UseItem(Character target);
    }
}