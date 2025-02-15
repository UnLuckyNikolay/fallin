using System.Reflection.Metadata.Ecma335;
using Fallin.InventorySystem;
using Fallin.Characters;

namespace Fallin.InventorySystem.Items
{
    public class PotionHealth : Item
    {
        public PotionHealth() : 
        base("Health Potion")
        {
        }

        public override void UseItem(Character target)
        {
            Quantity--;
            target.Health += 50;
        }
    }
}