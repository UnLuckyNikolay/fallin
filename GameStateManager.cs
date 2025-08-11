using Fallin.Characters;
using Fallin.MapSystem;
using Fallin.Enums;

namespace Fallin
{
    public sealed class GameStateManager
    {
        public GameStates GameState = GameStates.Map;
        public Hero? Player { get; private set; }
        public List<Enemy> Enemies { get; private set; } = [];
        public Map CurrentMap;
        public CombatSystem Fight;
        public Game Game;
        public bool PlayerTurn = true;


        public GameStateManager(Game game)
        {
            CurrentMap = new(this);
            Fight = new(this);
            Game = game;
        }

        public void SetPlayerReference(Hero player)
        // Used in Hero constructor
        {
            Player = player;
            Fight.Player = player;
        }

        public void AddEnemyReference(Enemy enemy)
        // Used in Enemy constructor
        {
            Enemies.Add(enemy);
        }

        public void RemoveEnemyReference(Enemy enemy)
        {
            Enemies.Remove(enemy);
        }

        /// <summary>
        /// Used at the end of a turn to remove dead enemies.
        /// </summary>
        public void RemoveDeadEnemies()
        {
            for (int i = Enemies.Count - 1; i >= 0; i--)
            {
                if (Enemies[i].IsAlive == false)
                {
                    Enemies.Remove(Enemies[i]);
                }
            }
        }
    }
}