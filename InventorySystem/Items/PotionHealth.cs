using System.Reflection.Metadata.Ecma335;
using Fallin.InventorySystem;

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
            target.health += 50;
        }
    }
}