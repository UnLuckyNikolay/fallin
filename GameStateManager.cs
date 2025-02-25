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
        public Game Game;
        public bool PlayerTurn = true;


        public GameStateManager(Game game)
        {
            CurrentMap = new();
            Game = game;
        }

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

        public void RemoveEnemyReference(Enemy enemy)
        {
            Enemies.Remove(enemy);
        }
    }
}