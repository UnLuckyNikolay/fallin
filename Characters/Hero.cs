namespace Fallin.Characters
{
    public class Hero : Character
    {
        public override int HealthMax => 100 + 10 * Level;
        public int ExperienceMax => 20 + 30 * Level;

        protected Hero(string name) : base(3, 3, 3, 3, 3, 3, 3)
        {
            Name = name;
        }

        public override void Death()
        {
            Console.WriteLine("The brave hero is lying defeated!");
        }
    }
}