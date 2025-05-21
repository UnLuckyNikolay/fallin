using Fallin.InventorySystem;
using static Utilities.ConsoleHelper;

namespace Fallin.Characters
{
    public class Hero : Character
    {
        public int ExperienceMax => 20 + 30 * Level;
        private int experience;
        public int Experience {
            get => experience;
            set {
                experience = Math.Max(value, 0);
                while (experience >= ExperienceMax) { LevelUp(); }
            }
        }

        public int LevelBP { get; protected set; }
        private int experienceBP;
        public int ExperienceBP {
            get => experienceBP;
            set {
                if (LevelBP < BPRewards.Count)
                {
                    experienceBP = Math.Max(value, 0);
                    while (experienceBP >= 1000) { LevelUpBP(); }
                }
                else { experienceBP = 0; }
            }
        }

        public int SpecialLeft { get; protected set; }
        public int Gold { get; set; }
        private GameStateManager gsm;

        private readonly Inventory inventory = new();
        public Dictionary<Color, bool> NameColors = new Dictionary<Color, bool>
        {
            {Color.White, true},
            {Color.DarkCyan, false},
            {Color.Yellow, false},
            {Color.Magenta, false},
            {Color.DarkRed, false},
            {Color.Pride, false},
        };
        private readonly List<Color> BPRewards = [ Color.DarkCyan, Color.Yellow, Color.Magenta, Color.DarkRed, Color.Pride ];


        public Hero(GameStateManager GSM, string name) : 
        base(new CharacterProperties{
            Level = 1,
            Name = name,
            NameMap = "Pl",
            NameColor = Color.White,

            Strength = 3,
            Perception = 3,
            Endurance = 3,
            Charisma = 3,
            Intelligence = 3,
            Agility = 3,
            Luck = 3,

            HealthMultiplier = 1,
            DamageMultiplier = 1
        })
        {
            gsm = GSM;
            gsm.SetPlayerReference(this);
            
            LevelBP = 0;
        }


        private void LevelUp()
        {
            Experience -= ExperienceMax;
            SpecialLeft += 3;
            Level++;
            HealFull();
            
            Console.WriteLine(" LEVEL UP! 3 new Special points available!");
        }

        private void LevelUpBP()
        {
            ExperienceBP -= 1000;
            LevelBP++;
            NameColors[BPRewards[LevelBP-1]] = true;

            Console.WriteLine(" BATTLE PASS LEVEL UP! New customisation available!");
        }

        public void HealFull()
        {
            Health = HealthMax;
        }

        public void UseItem(string itemName, Character target)
        {
            bool isSuccessful = inventory.TryUsingItem(itemName, target);
            if (isSuccessful)
            {
                if (target is not Hero)
                {
                    Console.Write($"Used {itemName} on {target.Name}");
                }
                else
                {
                    Console.Write($"Used {itemName}"); 
                }
            }
            else { Console.Write("Item not found!"); }
            Dots();
        }

        public void PickupItem(Item item)
        {
            inventory.AddItem(item);
        }

        /// <summary>
        /// Spawns the hero. Assumes there is an available spawn point
        /// </summary>
        public void Spawn()
        {
            gsm.CurrentMap.SpawnHeroAtRandomPosition(this);
        }

        /// <summary>
        /// Tries to move player. Finishes player's turn if successful
        /// </summary>
        public void Move(string direction)
        {
            bool success = gsm.CurrentMap.MoveHero(this, direction);
            if (success) { gsm.PlayerTurn = false; }
        }

        public override void Attack(Character target)
        {
            AttackNormal(target);
        }

        public void IncreaseSpecial(string specialName)
        {
            switch (specialName)
            {
                case "s":
                case "strength":
                    Strength++;
                    SpecialLeft--;
                    Console.Write($" Strength increased to {Strength}!");
                    break;
                    
                case "p":
                case "perception":
                    Perception++;
                    SpecialLeft--;
                    Console.Write($" Perception increased to {Perception}!");
                    break;

                case "e":
                case "endurance":
                    Endurance++;
                    SpecialLeft--;
                    Console.Write($" Endurance increased to {Endurance}!");
                    break;

                case "c":
                case "charisma":
                    Charisma++;
                    SpecialLeft--;
                    Console.Write($" Charisma increased to {Charisma}!");
                    break;

                case "i":
                case "intelligence":
                    Intelligence++;
                    SpecialLeft--;
                    Console.Write($" Intelligence increased to {Intelligence}!");
                    break;

                case "a":
                case "agility":
                    Agility++;
                    SpecialLeft--;
                    Console.Write($" Agility increased to {Agility}!");
                    break;

                case "l":
                case "luck":
                    Luck++;
                    SpecialLeft--;
                    Console.Write($" Luck increased to {Luck}!");
                    break;


                default:
                    Console.Write(" Invalid input.");
                    break;
            }
        }

        public void TryChangeColorName(string color)
        {
            if (!Enum.TryParse<Color>(color, true, out Color colorNew)) 
            {
                Console.Write(" Invalid input");
                Dots();
            }
            else
            {
                if (NameColors.ContainsKey(colorNew) && NameColors[colorNew] == true)
                {
                    NameColor = colorNew;
                    Console.Write($" Name color changed to {colorNew}");
                    Dots();
                }
                else
                {
                    Console.Write(" Invalid input");
                    Dots();
                }
            }
        }
        
        public override void Death()
        {
            Console.WriteLine("The brave hero is lying defeated!"); // ADD DEATH
        }


        public void WriteAttributes()
        {
            Console.Write(" --<");
            WriteColored(Name, NameColor);
            Console.WriteLine(">--");

            Console.WriteLine($" Level: {Level}, experience: {Experience}/{ExperienceMax}");
            Console.WriteLine($" Health Points: {Health}/{HealthMax}");
            Console.WriteLine($" Money: {Gold}");
        }

        public void WriteSpecial()
        {
            Console.WriteLine(" --<S.P.E.C.I.A.L.>--");
            Console.WriteLine($" Strength: {Strength}");
            Console.WriteLine($" Perception: {Perception}");
            Console.WriteLine($" Endurance: {Endurance}");
            Console.WriteLine($" Charisma: {Charisma}");
            Console.WriteLine($" Intelligence: {Intelligence}");
            Console.WriteLine($" Agility: {Agility}");
            Console.WriteLine($" Luck: {Luck}");
            if (SpecialLeft > 0) { Console.WriteLine($" You have {SpecialLeft} spare points."); }
        }
    
        public void WriteInventory()
        {
            Console.WriteLine(" --<Inventory>--");
            inventory.WriteItems();
        }
    
        public void WriteBP()
        {
            List<string> colorsList = new();
            foreach (KeyValuePair<Color, bool> pair in NameColors)
            {
                if (pair.Value == true)
                {
                    colorsList.Add(pair.Key.ToString());
                }
            }
            string colors = string.Join(", ", colorsList);

            Console.WriteLine($" Your current level: {LevelBP}");
            if (LevelBP < BPRewards.Count) { Console.WriteLine($" XP till next level: {ExperienceBP}/1000"); }
            Console.WriteLine(" ╔══════════╦══════════╦══════════╦══════════╦══════════╗");
            WriteBPLevelProgress(1, 5);
            Console.WriteLine(" ╠══════════╬══════════╬══════════╬══════════╬══════════╣");

            Console.Write(" ║");
            WriteColored("Name Color", BPRewards[0]);
            Console.Write("║");
            WriteColored("Name Color", BPRewards[1]);
            Console.Write("║");
            WriteColored("Name Color", BPRewards[2]);
            Console.Write("║");
            WriteColored("Name Color", BPRewards[3]);
            Console.Write("║");
            WriteColored("Name Color", BPRewards[4]);
            Console.WriteLine("║");

            Console.WriteLine(" ╚══════════╩══════════╩══════════╩══════════╩══════════╝");

            Console.WriteLine();
            Console.WriteLine($" Available colors: {colors}");
        }

        public void WriteBPLevelProgress(int startLevel, int endLevel)
        {
            Console.Write(" ");
            while (startLevel <= endLevel)
            {
                Console.Write("║");
                if (startLevel <= LevelBP)
                {
                    WriteColored("▓▓▓▓▓▓▓▓▓▓", Color.Green);
                }
                else if (startLevel == LevelBP + 1)
                {
                    int i = ExperienceBP / 100;
                    int j = 10;
                    while (i > 0 && j > 0)
                    {
                        Console.Write("▓");
                        i--;
                        j--;
                    }
                    while (j > 0)
                    {
                        Console.Write("░");
                        j--;
                    }
                }
                else
                {
                    Console.Write("░░░░░░░░░░");
                }
                startLevel++;
            }
            Console.WriteLine("║");
        }
    }
}