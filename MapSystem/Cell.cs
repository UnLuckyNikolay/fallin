using Fallin.Characters;

namespace Fallin.MapSystem
{
    public sealed class Cell
    {
        public bool IsWall { get; init; }
        public bool HasEnemy => character != null && character is Enemy;
        private Character? character = null;


        public Cell(bool isWall)
        {
            IsWall = isWall;
        }


        public void DrawCell()
        {
            if (IsWall)
            {
                Console.Write("▒▒");
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