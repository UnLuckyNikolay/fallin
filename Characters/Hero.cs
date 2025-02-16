using Fallin.InventorySystem;

namespace Fallin.Characters
{
    public class Hero : Character
    {
        public int ExperienceMax => 20 + 30 * Level;
        private int experience;
        public int Experience {
            get => experience;
            set {
                experience = value;
                while (experience >= ExperienceMax) { LevelUp(); }
            }
        }
        public int SpecialLeft { get; private set; }

        private readonly Inventory inventory = new();


        public Hero(GameStateManager gst, string name) : 
        base(new CharacterProperties{
            Level = 1,
            Name = name,
            NameMap = "Pl",
            NameColor = "white",

            Strength = 3,
            Perception = 3,
            Endurance = 3,
            Charisma = 3,
            Intelligence = 3,
            Agility = 3,
            Luck = 3,

            HealthMultiplier = 1,
            AttackMultiplier = 1
        })
        {
            gst.SetPlayerReference(this);
        }


        private void LevelUp()
        {
            Experience -= ExperienceMax;
            SpecialLeft += 3;
            Level++;
            HealFull();
        }

        public void HealFull()
        {
            Health = HealthMax;
        }

        public void TryUsingItem(string itemName, Character target)
        {
            bool isSuccessful = inventory.TryUsingItem(itemName, target);
            if (isSuccessful)
            {
                if (target is not Hero)
                {
                    Console.WriteLine($"Used {itemName} on {target.Name}");
                }
                else
                {
                    Console.WriteLine($"Used {itemName}"); 
                }
            }
            else { Console.WriteLine("Item not found!"); }
        }

        public void PickupItem(Item item)
        {
            inventory.AddItem(item);
        }

        public void Move(string direction)
        {
            // ADD MOVEMENT
        }

        public override void Death()
        {
            Console.WriteLine("The brave hero is lying defeated!");
        }
    }
}