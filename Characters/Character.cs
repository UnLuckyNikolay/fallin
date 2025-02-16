using System.Dynamic;
using Fallin.MapSystem;

namespace Fallin.Characters
{
    public abstract class Character
    {
        public required string Name { get; init; }
        public required string NameMap { get; init; }
        public string NameColor { get; protected set; }

        private int health;
        public int Health {
            get => health;
            set {
                health = Math.Clamp(value, 0, HealthMax);
                if (health <= 0) { Death(); }
            }
        }
        public required int HealthMultiplier { get; init; }
        public int HealthMax => (50 + 10 * Level + 10 * Endurance) * HealthMultiplier;
        public required int AttackMultiplier { get; init; }
        public int Attack => (4 * Strength + 2 * Agility) * AttackMultiplier;
        public int Armor => Endurance;

        public bool IsAlive => Health > 0;

        public int Level { get; protected set; }
        public int Strength { get; protected set; }
        public int Perception { get; protected set; }
        public int Endurance { get; protected set; }
        public int Charisma { get; protected set; }
        public int Intelligence { get; protected set; }
        public int Agility { get; protected set; }
        public int Luck { get; protected set; }

        protected Map? mapCurrent;
        public (int x, int y) Position { get; set; }


        protected Character(int level, string nameColor, int s, int p, int e, int c, int i, int a, int l)
        {
            Level = level;
            NameColor = nameColor;
            Strength = s;
            Perception = p;
            Endurance = e;
            Charisma = c;
            Intelligence = i;
            Agility = a;
            Luck = l;

            Health = HealthMax;
        }


        public abstract void Death();

        public void TakeDamage(int damage)
        {
            Health -= (damage - Armor);
        }

        public void SetMapReference(Map map)
        {
            mapCurrent = map;
        }
    }
}