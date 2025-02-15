using System.Dynamic;

namespace Fallin.Characters
{
    public abstract class Character
    {
        public required string Name { get; init; }
        private int health;
        public int Health {
            get => health;
            set {
                health = Math.Clamp(value, 0, HealthMax);
                if (health <= 0) { Death(); }
            }
        }
        public abstract int HealthMax { get ;} 

        public bool IsAlive => Health > 0;

        public int Level { get; protected set; }
        public int Strength { get; protected set; }
        public int Perception { get; protected set; }
        public int Endurance { get; protected set; }
        public int Charisma { get; protected set; }
        public int Intelligence { get; protected set; }
        public int Agility { get; protected set; }
        public int Luck { get; protected set; }

        protected Character(int s, int p, int e, int c, int i, int a, int l)
        {
            Strength = s;
            Perception = p;
            Endurance = e;
            Charisma = c;
            Intelligence = i;
            Agility = a;
            Luck = l;
        }

        public abstract void Death();
    }
}