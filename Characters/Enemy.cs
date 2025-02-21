using Fallin.InventorySystem;

namespace Fallin.Characters
{
    public abstract class Enemy : Character
    {
        public required Dictionary<Item, int> LootTable { get; init; }
        protected GameStateManager gsm;

        protected Enemy(GameStateManager GSM, CharacterProperties props) :
        base(props)
        {
            gsm = GSM;
            gsm.AddEnemyReference(this);
        }


        public void WriteAttributes()
        {
            Console.Write(" --<");
            Tool.WriteColored(Name, NameColor);
            Console.WriteLine(">--");
            Console.WriteLine($" Level: {Level}");
            Console.WriteLine($" Health Points: {Health}/{HealthMax}");
        }

        public override void Death()
        {
            gsm.RemoveEnemyReference(this);
            // ADD loot drops

            Console.WriteLine($" {Name} has been killed!");
        }

        /// <summary>
        /// Used to get rid of enemy without "killing" it, e.g. failed spawn
        /// </summary>
        public void Remove()
        {
            gsm.RemoveEnemyReference(this);
        }
    }
}