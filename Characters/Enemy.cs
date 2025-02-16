using Fallin.InventorySystem;

namespace Fallin.Characters
{
    public abstract class Enemy : Character
    {
        public required Dictionary<Item, int> LootTable { get; init; }

        protected Enemy(GameStateManager gst, CharacterProperties props) :
        base(props)
        {
            gst.AddEnemyReference(this);
        }
    }
}