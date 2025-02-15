namespace Fallin.InventorySystem
{
    public sealed class Inventory
    {
        private readonly List<Item> items = [];

        public void AddItem(Item item)
        {
            Item? itemFound = items.Find(itemSearched => itemSearched.Name == item.Name);
            if (itemFound != null)
            {
                itemFound.Quantity++;
            }
            else { items.Add(item); }
        }

        public bool TryUsingItem(string itemName, Character target)
        {
            Item? itemFound = items.Find(itemSearched => itemSearched.Name == itemName);
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
            return items.Find(itemSearched => itemSearched.Name == name) != null;
        }
    }
}