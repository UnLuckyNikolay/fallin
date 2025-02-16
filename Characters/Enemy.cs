using Fallin.InventorySystem;

namespace Fallin.Characters
{
    public abstract class Enemy : Character
    {
        public required Dictionary<Item, int> LootTable { get; init; }

        protected Enemy(int level, string nameColor, int s, int p, int e, int c, int i, int a, int l) :
        base(level, nameColor, s, p, e, c, i, a, l)
        {
        }
    }
}