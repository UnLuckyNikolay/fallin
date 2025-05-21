using static Utilities.ConsoleHelper;

namespace Fallin.Characters
{
    public class CharacterProperties
    {
        public required int Level;
        public required string Name;
        public required string NameMap;
        public required Color NameColor;

        public required int Strength;
        public required int Perception;
        public required int Endurance;
        public required int Charisma;
        public required int Intelligence;
        public required int Agility;
        public required int Luck;

        public required float HealthMultiplier;
        public required float DamageMultiplier;
    }
}