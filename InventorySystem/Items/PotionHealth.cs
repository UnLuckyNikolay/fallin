using Fallin.Characters;

namespace Fallin.InventorySystem.Items
{
    public class PotionHealth : Item
    {
        public PotionHealth(int quantity=1) : 
        base("Health Potion", quantity)
        {
        }

        public override void UseItem(Character target)
        {
            Quantity--;
            target.Health += 50;
        }
    }
}