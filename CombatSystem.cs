using Fallin.Characters;
using Fallin.Enums;

namespace Fallin
{
    public sealed class CombatSystem
    {
        private GameStateManager gsm;
        public Hero? Player;
        public Enemy? Enemy;
        public bool PlayerTurn;


        public CombatSystem(GameStateManager GSM)
        {
            gsm = GSM;
        }


        public void StartFight(Enemy enemy, bool didPlayerAttack)
        {
            Enemy = enemy;
            Player!.SpecialAttackCD = 0;
            enemy.SpecialAttackCD = 0;

            gsm.GameState = GameStates.Fight;
            Console.Clear();

            FightLoop(didPlayerAttack);

            gsm.GameState = GameStates.Map;
        }

        private void FightLoop(bool didPlayerAttack)
        {
            PlayerTurn = didPlayerAttack;

            while (Player!.IsAlive && Enemy!.IsAlive)
            {
                gsm.Game.DrawMenu();
                while (PlayerTurn)
                {
                    Player.IsBlocking = false;
                    Console.Write(" Choose your next action (attack/defence): ");
                    gsm.Game.ProcessCommand(Console.ReadLine() ?? "");
                }
                if (Enemy.IsAlive)
                {
                    Enemy.Attack(Player);
                    PlayerTurn = true;
                }
            }

            if (!Enemy!.IsAlive) 
            {
                Console.Clear();
                gsm.Game.DrawMenu();

                Console.WriteLine($" The {Enemy.Name} has been defeated!");
                Enemy.DropLoot();
                Console.WriteLine();
                Console.Write(" Press Enter to return to the Map ");

                gsm.GameState = GameStates.Map;
                Enemy.Death();
                Enemy = null;

                Console.ReadLine();
            }
            else if (!Player.IsAlive)
            {
                // ADD game restart
            }
        }
    }
}