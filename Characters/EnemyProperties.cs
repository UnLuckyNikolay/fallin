using Fallin.InventorySystem;
using Fallin.Enums;

namespace Fallin.Characters
{
    public class EnemyProperties
    {
        /// <summary>
        /// The dictionary of items as Keys and the chances of drop as Values
        /// </summary>
        public required Dictionary<Item, int> LootTable;

        /// <summary>
        /// The dictionary of resources as Keys and amounts as Values
        /// </summary>
        public required Dictionary<ResourceType, int> ResourceTable;
        
        public required string SpriteAlive;
        public required string SpriteDead;
    }
}