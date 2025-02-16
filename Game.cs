using System.Collections;
using System.Diagnostics.Contracts;

namespace Fallin
{
    public sealed class Game
    {
        private enum GameState {
            Map,
            Fight,
            Character,
            Leveling,
            Inventory,
            BattlePass
        }
        private GameState GameStateCurrent = GameState.Map;
        // ADD REFERENCES TO MAP, PLAYER, GSM

        public Game()
        {

        }


        public void Initialize()
        {
            // ADD METHOD
        }

        public void Run()
        {
            // ADD METHOD
        }

        public void DrawMenu()
        {
            // ADD METHOD
        }

        public void ProcessCommand()
        {
            string command = (Console.ReadLine() ?? "").ToLower();
            string[] commandSplit = command.Split();

            if (commandSplit[0] == "cheat")
            {
                CommandCheat(command);
            }
            else
            {
                switch(GameStateCurrent)
                {
                    
                }
            }
        }

        public void CommandCheat(string command)
        {

        }
    }
}