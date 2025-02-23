using System.Dynamic;
using Fallin.MapSystem;

namespace Fallin.Characters
{
    public abstract class Character
    {
        public string Name { get; init; }
        public string NameMap { get; init; }
        public string NameColor { get; protected set; }

        private int health;
        public int Health {
            get => health;
            set {
                health = Math.Clamp(value, 0, HealthMax);
                if (health <= 0) { Death(); }
            }
        }
        public float HealthMultiplier { get; init; }
        public int HealthMax => (int)Math.Round((50 + 10 * Level + 10 * Endurance) * HealthMultiplier);
        public float AttackMultiplier { get; init; }
        public int Attack => (int)Math.Round((4 * Strength + 2 * Agility) * AttackMultiplier);
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

        public (int y, int x) Position { get; set; }


        protected Character(CharacterProperties props)
        {
            Level = props.Level;
            Name = props.Name;
            NameMap = props.NameMap;
            NameColor = props.NameColor;

            Strength = props.Strength;
            Perception = props.Perception;
            Endurance = props.Endurance;
            Charisma = props.Charisma;
            Intelligence = props.Intelligence;
            Agility = props.Agility;
            Luck = props.Luck;

            HealthMultiplier = props.HealthMultiplier;
            AttackMultiplier = props.AttackMultiplier;

            Health = HealthMax;
        }


        public abstract void Death();

        public void TakeDamage(int damage)
        {
            Health -= (damage - Armor);
        }
    }
}