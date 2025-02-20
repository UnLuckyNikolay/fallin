using Fallin.Characters;
using Fallin.MapSystem;

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
        public GameStateManager GSM;
        public Hero Player;
        public Map Map;

        public string commandLast = "";
        public bool GameRunning = true;


        public Game()
        {
            Console.Clear();
            GSM = new();

            Console.WriteLine(" Welcome to Fallin.");
            Console.Write(" Write the character name: ");
            string playerName = "";
            while (playerName == "")
            {
                playerName = Console.ReadLine() ?? "";
                if (playerName == "") { Console.Write(" Invalid input. Try again: "); }
            }
            Player = new(GSM, playerName);

            Map = new(1);
            Player.CurrentMap = Map;
            Map.Spawn(Player);

            // ADD enemy spawns
        }


        public void Run()
        {
            while (GameRunning)
            {
                DrawMenu();
                commandLast = (Console.ReadLine() ?? "").ToLower();
                ProcessCommand(commandLast);
            }
        }

        public void DrawMenu()
        {
            Console.Clear();
            switch (GameStateCurrent)
            {
                case GameState.Map:
                    Player.WriteAttributes();
                    Console.WriteLine("\n --<Current map>--");
                    Map.DrawMap();
                    Console.Write("\n Choose the next command (move (up/down/left/right), character, inventory, exit): ");
                    break;

                case GameState.Fight:
                    // ADD fight menu
                    Console.WriteLine(" You shouldn't be here."); // REMOVE
                    Console.ReadLine(); // REMOVE
                    break;

                case GameState.Character:
                    Player.WriteAttributes();
                    Console.WriteLine();
                    Player.WriteSpecial();
                    if (Player.SpecialLeft > 0) 
                    { Console.Write("\n Choose the next command (items, levelup, battlepass, exit): "); }
                    else { Console.Write("\n Choose the next command (items, battlepass, exit): "); }
                    break;

                case GameState.Leveling:
                    Player.WriteAttributes();
                    Console.WriteLine();
                    Player.WriteSpecial();
                    Console.Write(" Choose the next (S/P/E/C/I/A/L) to upgrade: ");
                    break;

                case GameState.Inventory:
                    Player.WriteAttributes();
                    Console.WriteLine();
                    Player.WriteInventory();
                    Console.Write("\n Write the *name* of the item to use or 'return': ");
                    break;

                case GameState.BattlePass:
                    // ADD battle pass
                    Console.Write("\n Write 'name *color*' to change it or 'return': ");
                    break;
            }
        }

        public void ProcessCommand(string command)
        {
            string[] commandSplit = command.Split();

            if (commandSplit[0] == "cheat")
            {
                CommandCheat(commandSplit);
            }
            else
            {
                switch(GameStateCurrent)
                {
                    case GameState.Map:
                        CommandMap(commandSplit);
                        break;

                    case GameState.Fight:
                        CommandFight(commandSplit);
                        break;
                        
                    case GameState.Character:
                        CommandCharacter(commandSplit);
                        break;
                        
                    case GameState.Leveling:
                        CommandLeveling(commandSplit);
                        break;
                        
                    case GameState.Inventory:
                        CommandInventory(commandSplit);
                        break;
                        
                    case GameState.BattlePass:
                        CommandBattlePass(commandSplit);
                        break;
                }
            }
        }

        private void CommandCheat(string[] commandSplit)
        {
            switch (commandSplit[1])
            {
                case "heal":
                case "fullheal":
                    Player.HealFull();

                    Console.Write($" Cheat used! {Player.Name} fully healed");
                    Utilities.Dots();
                    break;
                
                case "lvlup":
                case "levelup":
                    Player.Experience += Player.ExperienceMax;

                    Console.Write($" Cheat used! {Player.Name} leveled up");
                    Utilities.Dots();
                    break;
                    
                case "gold":
                case "money":
                    if (!int.TryParse(commandSplit[2], out int money)) { goto default; }
                    Player.Money += money;

                    Console.Write(" Cheat used! More money added");
                    Utilities.Dots();
                    break;

                case "kms":
                    Player.Health = 0;

                    Console.Write(" M'okay");
                    Utilities.Dots();
                    break;

                case "colortest":
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.WriteLine(" Black       Test ▒▒▒▒");
                    Console.ForegroundColor = ConsoleColor.DarkBlue;
                    Console.WriteLine(" DarkBlue    Test ▒▒▒▒");
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    Console.WriteLine(" DarkGreen   Test ▒▒▒▒");
                    Console.ForegroundColor = ConsoleColor.DarkCyan;
                    Console.WriteLine(" DarkCyan    Test ▒▒▒▒");
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine(" DarkRed     Test ▒▒▒▒");
                    Console.ForegroundColor = ConsoleColor.DarkMagenta;
                    Console.WriteLine(" DarkMagenta Test ▒▒▒▒");
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.WriteLine(" DarkYellow  Test ▒▒▒▒");
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.WriteLine(" Gray        Test ▒▒▒▒");
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.WriteLine(" DarkGray    Test ▒▒▒▒");
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine(" Green       Test ▒▒▒▒");
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine(" Cyan        Test ▒▒▒▒");
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(" Red         Test ▒▒▒▒");
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    Console.WriteLine(" Magenta     Test ▒▒▒▒");
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine(" Yellow      Test ▒▒▒▒");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine(" White       Test ▒▒▒▒");
                    Console.WriteLine(" Press Enter to continue");
                    Console.ReadLine();
                    break;

                case "info":
                case "enemies":
                case "enemylist":
                    if (GSM.Enemies.Count == 0) 
                    {
                        Console.WriteLine(" But nobody came");
                        Utilities.Dots();
                    }
                    else
                    {
                        for (int i = 0; i < GSM.Enemies.Count; i++)
                        {
                            Console.WriteLine($" {i}: {GSM.Enemies[i].Name} ({GSM.Enemies[i].Position.x}, {GSM.Enemies[i].Position.y})");
                        }
                        Console.WriteLine(" Press Enter to continue");
                        Console.ReadLine();
                    }
                    break;

                case "attack":
                case "engage":
                        if (!int.TryParse(commandSplit[2], out int index)) { goto default; }
                        GameStateCurrent = GameState.Fight;
                        // ADD fight
                        break;

                default:
                    Console.Write(" Do you even know how to cheat?");
                    Utilities.Dots();
                    break;
            }
        }

        private void CommandMap(string[] commandSplit)
        {
            switch (commandSplit[0])   // commandSplit[1] (direction) is checked in MapLibrary.Map.MoveHero()
            {
                case "items":
                case "inventory":
                case "backpack":
                    GameStateCurrent = GameState.Inventory;
                    break;

                case "stats":
                case "character":
                    GameStateCurrent = GameState.Character;
                    break;

                case "move":
                case "go":
                case "run":
                    Player.Move(commandSplit[1]);
                    break;

                case "turnoff":
                case "exit":
                    Console.Write(" Are you sure you want to exit the game? (yes/no): ");
                    while (true) 
                    {
                        string answer = Console.ReadLine() ?? "";
                        if (answer == "yes") 
                        {
                            GameRunning = false;

                            Console.Write(" K, bye");
                            Utilities.Dots();
                            break;
                        }
                        else if (answer == "no") { break; }
                        else { Console.Write(" Invalid input, try again: "); }
                    }
                    break;

                default:
                    Console.Write(" Invalid Input");
                    Utilities.Dots();
                    break;
            }
        }

        private void CommandFight(string[] commandSplit)
        {
            switch (commandSplit[0])
            {
                // ADD fight command handling
                default:
                    Console.Write(" Invalid Input");
                    Utilities.Dots();
                    break;
            }
        }

        private void CommandCharacter(string[] commandSplit)
        {
            switch (commandSplit[0])
            {
                case "items":
                case "inventory":
                case "backpack":
                    GameStateCurrent = GameState.Inventory;
                    break;

                case "levelup":
                case "lvlup":
                case "leveling":
                    if (Player.SpecialLeft > 0) { GameStateCurrent = GameState.Leveling; }
                    else 
                    {
                        Console.Write(" Nothing to level up");
                        Utilities.Dots();
                    }
                    break;
                
                case "return":
                case "exit":
                case "map":
                    GameStateCurrent = GameState.Map;
                    break;

                default:
                    Console.Write(" What are you even trying to do?");
                    Utilities.Dots();
                    break;
            }
        }

        private void CommandLeveling(string[] commandSplit)
        {
            string special = commandSplit[0];
            while (Player.SpecialLeft > 0)
            { 
                Player.IncreaseSpecial(special);
                if (Player.SpecialLeft > 0) 
                { 
                    Console.Write(" Choose the next Special: ");
                    special = (Console.ReadLine() ?? "").ToLower();
                }
            }
            Console.Write(" Returning to the Character menu");
            Utilities.Dots();
            GameStateCurrent = GameState.Character;
        }

        private void CommandInventory(string[] commandSplit)
        {
            switch (commandSplit[0])
            {
                case "stats":
                case "character":
                case "return":
                case "exit":
                    GameStateCurrent = GameState.Character;
                    break;

                case "map":
                    GameStateCurrent = GameState.Map;
                    break;

                default:
                    string itemName = string.Join(" ", commandSplit.Skip(1));
                    Player.UseItem(itemName, Player); // ADD enemy targeting later if needed
                    Utilities.Dots();
                    break;
            }
        }

        private void CommandBattlePass(string[] commandSplit)
        {
            switch (commandSplit[0])
            {
                // ADD battle pass section

                case "stats":
                case "character":
                case "return":
                case "exit":
                    GameStateCurrent = GameState.Character;
                    break;

                case "map":
                    GameStateCurrent = GameState.Map;
                    break;
                    
                default: // ADD COLOR CHANGE HERE
                    Console.Write(" Invalid Input");
                    Utilities.Dots();
                    break;
            }
        }
    }
}