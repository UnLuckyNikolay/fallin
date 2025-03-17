namespace Fallin.Characters
{
    public abstract class Character
    {
        public string Name { get; init; }
        public string NameMap { get; init; }
        public Color NameColor { get; protected set; }

        private int health;
        public int Health {
            get => health;
            set {
                health = Math.Clamp(value, 0, HealthMax);
                //if (health <= 0) { Death(); }
            }
        }
        public float HealthMultiplier { get; init; }
        public int HealthMax => (int)Math.Round((50 + 10 * Level + 10 * Endurance) * HealthMultiplier);
        public float DamageMultiplier { get; init; }
        private Random rnd = new();
        private float Damage => (4 * Strength + 2 * Agility) * DamageMultiplier;
        public int DamageRandom => (int)Math.Round(Damage * (rnd.Next(75, 126) / 100f));
        public int DamageMin => (int)Math.Round(Damage * 0.75f);
        public int DamageMax => (int)Math.Round(Damage * 1.25f);
        public int Armor => Endurance;
        public int DodgeChanceBase => 20 + Agility * 3;

        public bool IsAlive => Health > 0;
        public bool IsBlocking = false;
        public int SpecialAttackCD;
        private bool SpecialAttackCharged = false;

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
            DamageMultiplier = props.DamageMultiplier;

            Health = HealthMax;
        }


        public abstract void Death();

        /// <summary>
        /// Calculates and deals damage, checks for dodge, crit and block
        /// </summary>
        public void TakeDamageFrom(Character attacker, int damageMult=1)
        {
            Random rnd = new();
            int dodgeRoll = rnd.Next(101);
            if (dodgeRoll <= (DodgeChanceBase - attacker.Perception * 3))
            {
                if (this is Enemy)
                {
                    Console.Write($" {Name} dodges your attack");
                }
                else 
                {
                    Console.Write($" You dodge {attacker.Name}'s attack");
                }
                Utilities.Dots();
            }
            else
            {
                int critMult = 1;
                bool critSuccess = false;
                int critRoll = rnd.Next(101);
                if (critRoll < Luck * 3)
                {
                    critSuccess = true;
                    critMult = 2;
                }

                int damage = attacker.DamageRandom * damageMult * critMult - Armor;
                if (IsBlocking) { damage = (int)Math.Round(damage * 0.25f); }
                Health -= damage;

                if (this is Enemy)
                {
                    if (critSuccess) { Console.Write(" Critical hit!"); }
                    Console.Write($" {Name} takes {damage} point(s) of damage");
                }
                else 
                {
                    if (damageMult > 1) { Console.WriteLine($" The {attacker.Name} hits you with a charged attack!"); }
                    if (critSuccess) { Console.Write(" Critical hit!"); }
                    Console.Write($" You take {damage} point(s) of damage");
                }
                Utilities.Dots(1200);
            }
        }

        /// <summary>
        /// Abstract, used to choose the attack pattern. Damage calculation is done in Character.TakeDamageFrom()
        /// </summary>
        public abstract void Attack(Character target);

        public void AttackNormal(Character target)
        {
            target.TakeDamageFrom(this);
        }

        public void AttackCharged(Character target)
        {
            if (SpecialAttackCharged)
            {
                target.TakeDamageFrom(this, 3);
                SpecialAttackCD = 2;
                SpecialAttackCharged = false;
                return;
            }

            if (SpecialAttackCD <= 0)
            {
                Random rnd = new();
                int roll = rnd.Next(101);
                if (roll <= 40)
                {
                    SpecialAttackCharged = true;
                    Console.Write($" The {Name} prepares a devastating attack");
                    Utilities.Dots(1000);
                    return;
                }
            }
            else
            {
                AttackNormal(target);
                SpecialAttackCD--;
            }
        }
    }
}