using Fallin.Characters;

namespace Fallin
{
    public sealed class GameStateManager
    {
        public Hero? Player { get; private set; }
        public List<Enemy> Enemies { get; private set; } = [];


        // ADD CONSTRUCTOR WHICH WILL REQUIRE GAME CLASS


        public void SetPlayerReference(Hero player)
        {
            Player = player;
        }

        public void AddEnemyReference(Enemy enemy)
        {
            Enemies.Add(enemy);
        }
    }
}