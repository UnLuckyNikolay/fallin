using Fallin.InventorySystem;
using Fallin.Enums;


namespace Fallin.Characters
{
    public abstract class Enemy : Character
    {
        public Dictionary<Item, int> LootTable { get; init; }
        public Dictionary<ResourceType, int> ResourceTable { get; init; }
        public string SpriteAlive { get; init; }
        public string SpriteDead { get; init; }

        protected GameStateManager gsm;

        protected Enemy(GameStateManager GSM, CharacterProperties props, EnemyProperties eprops) :
        base(props)
        {
            gsm = GSM;
            gsm.AddEnemyReference(this);

            LootTable = eprops.LootTable;
            ResourceTable = eprops.ResourceTable;
            SpriteAlive = eprops.SpriteAlive;
            SpriteDead = eprops.SpriteDead;
        }


        public void Spawn()
        {
            gsm.CurrentMap.TrySpawnEnemyAtRandomPosition(this);
        }

        /// <summary>
        /// Gives player resources (money, xp, etc) and rolls for items, and prints it out
        /// </summary>
        /// <param name="player"></param>
        public void DropLoot()
        {
            if (gsm.Player == null) { return; } // To suppress the warning
            gsm.Player.Experience += ResourceTable[ResourceType.Experience];
            gsm.Player.ExperienceBP += ResourceTable[ResourceType.ExperienceBP];
            gsm.Player.Gold += ResourceTable[ResourceType.Gold];

            Console.WriteLine($" Acquired {ResourceTable[ResourceType.Experience]} experience, " +
                                        $"{ResourceTable[ResourceType.ExperienceBP]} BP experience, " +
                                        $"{ResourceTable[ResourceType.Gold]} gold.");

            bool itemDrop = false;
            Random rnd = new();
            foreach (KeyValuePair<Item, int> item in LootTable)
            {
                int roll = rnd.Next(0, 101);
                if (roll < item.Value)
                {
                    gsm.Player.PickupItem(item.Key);
                    if (!itemDrop)
                    {
                        Console.Write($" Items: {item.Key.Name} x {item.Key.Quantity}");
                        itemDrop = true;
                    }
                    else { Console.Write($", {item.Key.Name} x {item.Key.Quantity}"); }
                }
            }
            if (itemDrop) { Console.WriteLine(); }
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
            DropLoot();
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