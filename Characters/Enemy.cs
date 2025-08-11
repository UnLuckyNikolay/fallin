using Fallin.InventorySystem;
using Fallin.Enums;
using static Utilities.ConsoleHelper;


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
            // gsm.AddEnemyReference(this); // Dumbass, use GSM.Enemies.Add() instead

            LootTable = eprops.LootTable;
            ResourceTable = eprops.ResourceTable;
            SpriteAlive = eprops.SpriteAlive;
            SpriteDead = eprops.SpriteDead;
        }


        /// <summary>
        /// Spawns the enemy. If no available spawn points are present - the enemy is deleted
        /// </summary>
        public void Spawn()
        {
            gsm.CurrentMap.TrySpawnEnemyAtRandomPosition(this);
        }

        /// <summary>
        /// Gives player resources (money, xp, etc) and rolls for items, and prints it out
        /// </summary>
        public void DropLoot()
        {
            if (gsm.Player == null) { return; } // To suppress the warning

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

            gsm.Player.Experience += ResourceTable[ResourceType.Experience];
            gsm.Player.ExperienceBP += ResourceTable[ResourceType.ExperienceBP];
            gsm.Player.Gold += ResourceTable[ResourceType.Gold];
        }

        /// <summary>
        /// Should have one of the available Moves. Currently: MoveRandomly()
        /// </summary>
        public abstract void Move();

        /// <summary>
        /// Tries to move the enemy in a random direction. If the move fails - tries one more time in the opposite direction
        /// </summary>
        public void MoveRandomly()
        {
            string[] directions = ["up", "right", "down", "left"];
            Random rnd = new();
            int dir = rnd.Next(0, 4);

            bool success = gsm.CurrentMap.MoveEnemy(this, directions[dir]);
            if (!success)
            {
                dir += 2;
                if (dir > 3) { dir -= 4; } // Going to the opposite direction in the list

                gsm.CurrentMap.MoveEnemy(this, directions[dir]);
            }
        }

        public void WriteAttributes()
        {
            Console.Write(" --<");
            WriteColored(Name, NameColor);
            Console.WriteLine(">--");
            Console.WriteLine($" Level: {Level}");
            Console.WriteLine($" Health Points: {Health}/{HealthMax}");
            Console.WriteLine();
            if (Health > 0) { Console.WriteLine(SpriteAlive); }
            else { WriteColored(SpriteDead, Color.DarkRed); }
            Console.WriteLine();
        }

        /// <summary>
        /// Removes enemy reference from GameStateManager and gives player the loot. To remove enemy without dropping loot use .Remove()
        /// </summary>
        public override void Death()
        {
            Console.WriteLine();
            Console.WriteLine($" {Name} has been killed!");
            DropLoot();
            
            //gsm.RemoveEnemyReference(this); Instead removed at the end of the turn to prevent modifying Enemies list while it's used
            gsm.CurrentMap.Layout[Position.y, Position.x].RemoveCharacter();
        }

        /// <summary>
        /// Used to get rid of enemy without "killing" it, e.g. failed spawn
        /// </summary>
        public void DebugRemove()
        {
            gsm.RemoveEnemyReference(this);
            gsm.CurrentMap.Layout[Position.y, Position.x].RemoveCharacter();
        }
    }
}