using Fallin.Characters;
using Fallin.Characters.Enemies;
using Fallin.Enums;
using static Utilities.ConsoleHelper;

namespace Fallin
{
    public sealed class Game
    {
        public GameStateManager GSM;
        public Hero Player;

        public string commandLast = "";
        public bool GameRunning = true;


        public Game()
        {
            Console.Clear();
            GSM = new(this);

            Console.WriteLine(" Welcome to Fallin.");
            Console.Write(" Write the character name: ");
            string playerName = "";
            while (playerName == "")
            {
                playerName = Console.ReadLine() ?? "";
                if (playerName == "") { Console.Write(" Invalid input. Try again: "); }
            }
            Player = new(GSM, playerName);
            Player.Spawn();

            GSM.Enemies.Add(new SmallRat(GSM));
            GSM.Enemies.Add(new SmallRat(GSM));
            GSM.Enemies.Add(new SmallRat(GSM));
            GSM.Enemies.Add(new SmallRat(GSM));

            for (int i = GSM.Enemies.Count - 1; i >= 0; i--)   // Iterating backwards bcs if spawn fails - enemy is immediately removed
            {
                GSM.Enemies[i].Spawn();
            }
        }


        public void Run()
        {
            while (GameRunning)
            {
                while (GSM.PlayerTurn)
                {
                    DrawMenu();
                    commandLast = (Console.ReadLine() ?? "").ToLower();
                    ProcessCommand(commandLast);

                    // Player turn is finished by specific commands. At the time of writing - successful movement (moves player or starts a fight)
                }

                while (!GSM.PlayerTurn)
                {
                    foreach (Enemy enemy in GSM.Enemies)
                    {
                        enemy.Move();
                    }

                    GSM.PlayerTurn = true;
                }
            }
        }

        public void DrawMenu()
        {
            Console.Clear();
            switch (GSM.GameState)
            {
                case GameStates.Map:
                    Player.WriteAttributes();
                    Console.WriteLine("\n --<Current map>--");
                    GSM.CurrentMap.DrawMap();
                    Console.Write("\n Choose the next command (move (up/down/left/right), character, inventory, exit): ");
                    break;

                case GameStates.Fight:
                    GSM.Fight.Enemy?.WriteAttributes();
                    Console.WriteLine();
                    Player.WriteAttributes();
                    Console.WriteLine();
                    break;

                case GameStates.Character:
                    Player.WriteAttributes();
                    Console.WriteLine();
                    Player.WriteSpecial();
                    if (Player.SpecialLeft > 0)
                    { Console.Write("\n Choose the next command (items, levelup, battlepass, exit): "); }
                    else { Console.Write("\n Choose the next command (items, battlepass, exit): "); }
                    break;

                case GameStates.Leveling:
                    Player.WriteAttributes();
                    Console.WriteLine();
                    Player.WriteSpecial();
                    Console.Write("\n Choose the next (S/P/E/C/I/A/L) to upgrade: ");
                    break;

                case GameStates.Inventory:
                    Player.WriteAttributes();
                    Console.WriteLine();
                    Player.WriteInventory();
                    Console.Write("\n 'Write 'use *item name*' to use it or 'return': ");
                    break;

                case GameStates.BattlePass:
                    Player.WriteAttributes();
                    Console.WriteLine();
                    Player.WriteBP();
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
                switch (GSM.GameState)
                {
                    case GameStates.Map:
                        CommandMap(commandSplit);
                        break;

                    case GameStates.Fight:
                        CommandFight(commandSplit);
                        break;

                    case GameStates.Character:
                        CommandCharacter(commandSplit);
                        break;

                    case GameStates.Leveling:
                        CommandLeveling(commandSplit);
                        break;

                    case GameStates.Inventory:
                        CommandInventory(commandSplit);
                        break;

                    case GameStates.BattlePass:
                        CommandBattlePass(commandSplit);
                        break;
                }
            }
        }

        private void CommandCheat(string[] commandSplit)
        {
            switch (commandSplit[1])
            {
                case "kill":
                    if (GSM.Fight.Enemy == null) { goto default; }
                    GSM.Fight.Enemy.Health = 0;
                    GSM.Fight.PlayerTurn = false;
                    Console.Write(" Avada Kedavra!");
                    Dots(200, 10);
                    break;

                case "heal":
                case "fullheal":
                    Player.HealFull();

                    Console.Write($" Cheat used! {Player.Name} fully healed");
                    Dots();
                    break;

                case "lvlup":
                case "levelup":
                    Player.Experience += Player.ExperienceMax;

                    Console.Write($" Cheat used! {Player.Name} leveled up");
                    Dots();
                    break;

                case "bpup":
                case "battlepassup":
                    Player.ExperienceBP += 1000;

                    Console.Write($" Cheat used! Added 1000 BP Experience");
                    Dots();
                    break;

                case "gold":
                case "money":
                    if (!int.TryParse(commandSplit[2], out int gold)) { goto default; }
                    Player.Gold += gold;

                    Console.Write(" Cheat used! More money added");
                    Dots();
                    break;

                case "kms":
                    Player.Health = 0;

                    Console.Write(" M'okay");
                    Dots();
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
                        Dots();
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
                    GSM.GameState = GameStates.Fight;
                    // ADD fight
                    break;

                default:
                    Console.Write(" Do you even know how to cheat?");
                    Dots();
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
                    GSM.GameState = GameStates.Inventory;
                    break;

                case "stats":
                case "character":
                    GSM.GameState = GameStates.Character;
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
                            Dots();
                            break;
                        }
                        else if (answer == "no") { break; }
                        else { Console.Write(" Invalid input, try again: "); }
                    }
                    break;

                default:
                    Console.Write(" Invalid Input");
                    Dots();
                    break;
            }
        }

        private void CommandFight(string[] commandSplit)
        {
            switch (commandSplit[0])
            {
                case "attack":
                    if (GSM.Fight.Enemy == null) { break; }
                    Player.Attack(GSM.Fight.Enemy);
                    GSM.Fight.PlayerTurn = false;
                    break;

                case "defence":
                case "defense":
                case "block":
                    Player.IsBlocking = true;
                    GSM.Fight.PlayerTurn = false;
                    Console.Write(" You take a defensive position");
                    Dots();
                    break;

                default:
                    Console.Write(" Invalid Input. Try again: ");
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
                    GSM.GameState = GameStates.Inventory;
                    break;

                case "levelup":
                case "lvlup":
                case "leveling":
                    if (Player.SpecialLeft > 0) { GSM.GameState = GameStates.Leveling; }
                    else
                    {
                        Console.Write(" Nothing to level up");
                        Dots();
                    }
                    break;

                case "battlepass":
                case "bp":
                    GSM.GameState = GameStates.BattlePass;
                    break;

                case "return":
                case "exit":
                case "map":
                    GSM.GameState = GameStates.Map;
                    break;

                default:
                    Console.Write(" What are you even trying to do?");
                    Dots();
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
            Dots();
            GSM.GameState = GameStates.Character;
        }

        private void CommandInventory(string[] commandSplit)
        {
            switch (commandSplit[0])
            {
                case "stats":
                case "character":
                case "return":
                case "exit":
                    GSM.GameState = GameStates.Character;
                    break;

                case "map":
                    GSM.GameState = GameStates.Map;
                    break;

                default:
                    string itemName = string.Join(" ", commandSplit.Skip(1));
                    Player.UseItem(itemName, Player); // ADD enemy targeting later if needed
                    Dots();
                    break;
            }
        }

        private void CommandBattlePass(string[] commandSplit)
        {
            switch (commandSplit[0])
            {
                case "name":
                    Player.TryChangeColorName(string.Join("", commandSplit.Skip(1)));
                    break;

                case "stats":
                case "character":
                case "return":
                case "exit":
                    GSM.GameState = GameStates.Character;
                    break;

                case "map":
                    GSM.GameState = GameStates.Map;
                    break;

                default:
                    Console.Write(" Invalid Input");
                    Dots();
                    break;
            }
        }
    }
}