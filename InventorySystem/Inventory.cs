using Fallin.Characters;

namespace Fallin.InventorySystem
{
    public sealed class Inventory
    {
        private readonly List<Item> items = [];
        public int Count => items.Count();

        public void AddItem(Item item)
        {
            Item? itemFound = items.Find(itemSearched => itemSearched.Name.ToLower() == item.Name.ToLower());
            if (itemFound != null)
            {
                itemFound.Quantity++;
            }
            else { items.Add(item); }
        }

        public bool TryUsingItem(string itemName, Character target)
        {
            Item? itemFound = items.Find(itemSearched => itemSearched.Name.ToLower() == itemName.ToLower());
            if (itemFound != null)
            {
                itemFound.UseItem(target);
                if (itemFound.Quantity <= 0) { items.Remove(itemFound); }
                return true;
            }
            else { return false; }
        }

        public bool HasItem(string name)
        {
            return items.Find(itemSearched => itemSearched.Name.ToLower() == name.ToLower()) != null;
        }

        public void WriteItems()
        {
            if (Count == 0) { Console.WriteLine(" Quite empty in here for now"); }
            else
            {
                foreach (Item item in items)
                {
                    Console.WriteLine($" {item.Name} x {item.Quantity}");
                }
            }
        }
    }
}