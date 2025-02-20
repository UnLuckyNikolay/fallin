using Fallin.Characters;

namespace Fallin
{
    public sealed class GameStateManager
    {
        public Hero? Player { get; private set; }
        public List<Enemy> Enemies { get; private set; } = [];


        public void SetPlayerReference(Hero player)
        // Used in Hero constructor
        {
            Player = player;
        }

        public void AddEnemyReference(Enemy enemy)
        // Used in Enemy constructor
        {
            Enemies.Add(enemy);
        }
    }
}