using Fallin.Characters;

namespace Fallin.MapSystem
{
    public sealed class Cell
    {
        public bool IsWall { get; init; }
        public bool HasEnemy => character != null && character is Enemy;
        public bool HasHero => character != null && character is Hero;
        public bool HasCharacter => character != null && character is Character;
        public Character? character = null;


        public Cell(bool isWall)
        {
            IsWall = isWall;
        }


        public void DrawCell()
        {
            if (IsWall)
            {
                Utilities.WriteColored("▒▒", "darkyellow");
            }
            else if (character == null)
            {
                Console.Write("  ");
            }
            else
            {
                Utilities.WriteColored(character.NameMap, character.NameColor);
            }
        }

        public void AddCharacter(Character Character)
        {
            if (character == null)
            {
                character = Character;
            }
        }

        public void RemoveCharacter()
        {
            character = null;
        }
    }
}