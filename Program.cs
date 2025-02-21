using System;
using System.Drawing;
using System.Numerics;
using System.Reflection.Metadata;
using System.Runtime;
using System.Security.Cryptography.X509Certificates;
using System.Xml.Linq;
using Fallin.Characters;
using Fallin.MapSystem;

// DONE --- Possibly remove player.location, only use Game_old.characterLocation
// Maybe change it so SPECIAL scale with level (calculated in .CalculateStats) for monsters and then command would be Character_old("name", level)
// DONE --- Try: change map so each row is another array so there is no need to have movement array. Would be easier to have different sizes, including non-square
// DONE --- Rewrite Special back to names instead of an array to fuck my own brains less
// DONE --- Finish rewritig DrawMap block (change all usages to v2), change code for movement
// DONE --- Add Move block that can be used by player (with up/down etc) and enemies (random, maybe towards player)
// DONE --- Add time to dots
// DONE --- Possibly one big blok for commands that would give ability to have test commands
// DONE --- Colored text?
// DONE --- Change health to int, round attack
// Merchant, charisma
// Command/block for level generation
// Exit
// DONE --- Legend for map and special
// DONE --- Dead enemies
// DONE --- Battle pass and name color customisation
// Inventory and drop tables
// DONE --- Game_old restart is broken
// DONE --- Maybe min and max damage
// DONE --- Add second unpopulated copy of the map and change movement to allow exit/merchant/etc
// Enemy movement
// Maybe fog of war
// DONE --- Attack cheat doesn't remove enemy from the map
// Split Character_old into Player and NPC

// add class Item
// maybe a list of items as inventory
// inventory as a list of [id, amount]?

namespace Fallin
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Game game = new();
            game.Run();
            
            while (Game_old.gameOn)
            {
                Console.Clear();
                GameStateManager GSM = new();
                Hero Player = new(GSM, "Bred");
                Map mapTest = new(0);
                mapTest.Spawn(Player);
                mapTest.DrawMap();
                Console.ReadLine();

                Game_old.roundOn = true;
                Random rnd = new Random();
                Console.WriteLine(" Welcome to Fallin.");

                // --- Generates map
                Game_old.mapCurrentId = rnd.Next(0, Map_old.mapList.Length);
                Game_old.mapCurrent = new string[Map_old.mapList[Game_old.mapCurrentId].Length][];
                Game_old.mapCurrentUnpopulated = new string[Map_old.mapList[Game_old.mapCurrentId].Length][];
                Tool.Copy2DArray(Map_old.mapList[Game_old.mapCurrentId], Game_old.mapCurrent);
                Tool.Copy2DArray(Map_old.mapList[Game_old.mapCurrentId], Game_old.mapCurrentUnpopulated);

                // --- Spawns player
                Character_old player = new Character_old("Player");
                Game_old.character[0] = player;
                Spawn(0);
                bool naming = true;
                Console.Write(" Write the character name: ");
                while (naming)
                {
                    player.name = Console.ReadLine() ?? "";
                    if (player.name != "")
                    {
                        naming = false;
                    }
                    else { Console.Write(" Invalid input. Try again: "); }
                }

                // --- Spawns enemies
                Character_old enemy1 = new Character_old("Small Rat");
                Game_old.character[1] = enemy1;
                Spawn(1);

                Character_old enemy2 = new Character_old("Small Rat");
                Game_old.character[2] = enemy2;
                Spawn(2);

                Character_old enemy3 = new Character_old("Big Rat");
                Game_old.character[3] = enemy3;
                Spawn(3);

                while (Game_old.roundOn)
                {
                    switch (Game_old.menuCurrent)
                    {
                        case "exploration":
                            Console.Clear();
                            player.WriteAttributes();
                            Console.WriteLine("\n --<Current map>--");
                            DrawMap(Game_old.mapCurrent);
                            Console.Write("\n Choose the next command (inventory, move (up/down/left/right), legend, turnoff): ");
                            PlayerCommand(Console.ReadLine());
                            break;

                        case "fight": 
                            Fight(true);
                            break;

                        case "inventory": 
                            Console.Clear();
                            player.WriteAttributes();
                            Console.WriteLine();
                            player.WriteSpecial();
                            Console.Write("\n Choose the next command (items, levelup, battlepass, exit): ");
                            PlayerCommand(Console.ReadLine());
                            break;

                        case "items":
                            Console.Clear();
                            player.WriteAttributes();
                            Console.WriteLine();
                            player.WriteInventory();
                            Console.Write("\n Write the *name* of the item to use or 'exit': ");
                            PlayerCommand(Console.ReadLine());
                            break;

                        case "leveling": 
                            Console.Clear();
                            player.WriteAttributes();
                            Console.WriteLine();
                            player.WriteSpecial();
                            Console.Write("\n Choose an attribute to level up (S/P/E/C/I/A/L) or open \"legend\": ");
                            while (player.specialLeft > 0)
                            {
                                PlayerCommand(Console.ReadLine());
                            }
                            Console.Write(" Press Enter to exit the menu ");
                            Console.ReadLine();
                            player.CalculateStats();
                            Game_old.menuCurrent = "inventory";
                            break;

                        case "battlepass":
                            Console.Clear();
                            Game_old.character[0].WriteAttributes();
                            Console.WriteLine();
                            Game_old.character[0].WriteBP();
                            Console.WriteLine();
                            Console.Write(" Available name colors: ");
                            for (int i = 0; i < player.colorNameAvailable.Length; i++ )
                            {
                                if (player.colorNameAvailable[i])
                                {
                                    Console.Write($"{Game_old.color[i]}, ");
                                }
                            }
                            Console.WriteLine("\b\b  ");
                            Console.Write("\n Write 'name *color*' to change it of 'exit' to return to the inventory: ");
                            PlayerCommand(Console.ReadLine());
                            break;
                    }
                    

                    // --- Death, game restart
                    if (player.health <= 0)
                    {
                        Console.Clear();
                        Console.WriteLine("\n\n\n     Y O U   D I E D     \n\n\n");
                        Console.Write(" Do you want to try again? (yes/no): ");
                        bool questionGameRepeat = true;
                        while (questionGameRepeat)
                        {
                            switch (Console.ReadLine())
                            {
                                case "yes":
                                    questionGameRepeat = false;
                                    Game_old.roundOn = false;
                                    for ( int i = 0; i < Game_old.character.Length; i++ )
                                    {
                                        Game_old.character[i] = null;
                                        Game_old.characterLocation[i] = null;
                                    }
                                    Game_old.menuCurrent = "exploration";
                                    Game_old.mapCurrent = null;
                                    Console.Clear();
                                    break;

                                case "no":
                                    questionGameRepeat = false;
                                    Game_old.roundOn = false;
                                    Game_old.gameOn = false;
                                    break;

                                default:
                                    Console.Write(" Invaild input. Try again: ");
                                    break;
                            }
                        }
                    }
                }
            }
        }

        static void PlayerCommand(string command)
        {
            command = command.ToLower();
            string[] commandSplit = command.Split(" ");

            switch (commandSplit[0])
            {
                case "cheat":
                    switch (commandSplit[1])
                    {
                        case "fullheal":
                            Game_old.character[0].Heal();
                            Console.Write(" Cheat used! Character_old fully healed");
                            Tool.Dots(800);
                            break;

                        case "lvlup":
                        case "levelup":
                            Console.WriteLine(" Cheat used! Level increased.");
                            Game_old.character[0].xpCurrent = Game_old.character[0].xpMax;
                            Game_old.character[0].CalculateLevel();
                            Tool.Dots(800);
                            break;

                        case "bpup":
                            Console.WriteLine(" Cheat used! BP level increased.");
                            Game_old.character[0].xpBpCurrent += 1000;
                            Game_old.character[0].CalculateLevel();
                            Tool.Dots(800);
                            break;

                        case "money":
                            Console.Write(" Cheat used! More money added");
                            if (!int.TryParse(commandSplit[2], out int money)) { goto default; }
                            Game_old.character[0].money += money;
                            Tool.Dots(800);
                            break;

                        case "attack":
                            if (!int.TryParse(commandSplit[2], out int index)) { goto default; }
                            Game_old.opponentIndex = index;
                            Game_old.menuCurrent = "fight";
                            Fight(true);
                            break;

                        case "color":
                            Console.ForegroundColor = ConsoleColor.Black;
                            Console.WriteLine("Black       Test ▒▒▒▒");
                            Console.ForegroundColor = ConsoleColor.DarkBlue;
                            Console.WriteLine("DarkBlue    Test ▒▒▒▒");
                            Console.ForegroundColor = ConsoleColor.DarkGreen;
                            Console.WriteLine("DarkGreen   Test ▒▒▒▒");
                            Console.ForegroundColor = ConsoleColor.DarkCyan;
                            Console.WriteLine("DarkCyan    Test ▒▒▒▒");
                            Console.ForegroundColor = ConsoleColor.DarkRed;
                            Console.WriteLine("DarkRed     Test ▒▒▒▒");
                            Console.ForegroundColor = ConsoleColor.DarkMagenta;
                            Console.WriteLine("DarkMagenta Test ▒▒▒▒");
                            Console.ForegroundColor = ConsoleColor.DarkYellow;
                            Console.WriteLine("DarkYellow  Test ▒▒▒▒");
                            Console.ForegroundColor = ConsoleColor.Gray;
                            Console.WriteLine("Gray        Test ▒▒▒▒");
                            Console.ForegroundColor = ConsoleColor.DarkGray;
                            Console.WriteLine("DarkGray    Test ▒▒▒▒");
                            Console.ForegroundColor = ConsoleColor.Blue;
                            Console.WriteLine("Blue        Test ▒▒▒▒");
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("Green       Test ▒▒▒▒");
                            Console.ForegroundColor = ConsoleColor.Cyan;
                            Console.WriteLine("Cyan        Test ▒▒▒▒");
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Red         Test ▒▒▒▒");
                            Console.ForegroundColor = ConsoleColor.Magenta;
                            Console.WriteLine("Magenta     Test ▒▒▒▒");
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.WriteLine("Yellow      Test ▒▒▒▒");
                            Console.ForegroundColor = ConsoleColor.White;
                            Console.WriteLine("White       Test ▒▒▒▒");
                            Console.WriteLine();
                            Console.ReadLine();
                            break;

                        case "kms":
                            Game_old.character[0].health = 0;
                            Console.Write(" M'okay");
                            Tool.Dots(800);
                            break;

                        case "info":
                            Console.WriteLine($" Slots - {Game_old.character.Length}");
                            for ( int i = 0; i < Game_old.character.Length; i++ ) 
                            { 
                                if (Game_old.character[i] != null)
                                {
                                    Console.WriteLine($" Slot {i}: {Game_old.character[i].name}, {Game_old.characterLocation[i][0]}.{Game_old.characterLocation[i][1]}");
                                }
                            }
                            Console.Write(" Press Enter to continue ");
                            Console.ReadLine();
                            break;

                        default:
                            Console.Write(" Do you even know how to cheat?");
                            Tool.Dots(600);
                            break;
                    }
                    break;

                default:
                    switch (Game_old.menuCurrent)
                    {
                        case "exploration":
                            switch (commandSplit[0])
                            {
                                case "stats":
                                case "inventory":
                                    Game_old.menuCurrent = "inventory";
                                    break;

                                case "go":
                                case "move":
                                    Move(0, commandSplit[1]);
                                    break;

                                case "turnoff":
                                    Game_old.roundOn = false;
                                    Game_old.gameOn = false;
                                    Console.Write(" K, bye");
                                    Tool.Dots(800);
                                    break;

                                case "legend":
                                    Console.Clear();
                                    Console.WriteLine(" --<Legend>--");
                                    Console.WriteLine(" Pl - player");
                                    Console.WriteLine(" rt, Rt, RT, - rats");
                                    Console.WriteLine(" ┌┐ - exit");
                                    Console.Write("\n Press Enter to continue ");
                                    Console.ReadLine();
                                    break;

                                default:
                                    Game_old.character[0].health -= 5;
                                    Console.Write(" Invalid input. HP -5");
                                    Tool.Dots(800);
                                    break;
                            }
                            break;

                        case "fight":
                            switch (commandSplit[0])
                            {
                                case "attack":
                                    FightCurrent.playerTurn = false;
                                    Attack(0, Game_old.opponentIndex, 1, FightCurrent.enemyBlocking);
                                    if (Game_old.character[Game_old.opponentIndex].health <= 0)
                                    {
                                        Game_old.character[Game_old.opponentIndex].health = 0;
                                        FightCurrent.ongoing = false;
                                        FightCurrent.won = true;
                                    }
                                    break;

                                case "defence":
                                case "defense":
                                    FightCurrent.playerTurn = false;
                                    FightCurrent.playerBlocking = true;
                                    Console.Write(" You take a defensive position");
                                    Tool.Dots(800);
                                    break;

                                default:
                                    Console.Write(" Invalid input. Try again: ");
                                    break;
                            }
                            break;

                        case "inventory":
                            switch (commandSplit[0])
                            {
                                case "items":
                                    Game_old.menuCurrent = "items";
                                    break;

                                case "lvlup":
                                case "levelup":
                                    if (Game_old.character[0].specialLeft > 0)
                                    {
                                        Game_old.menuCurrent = "leveling";
                                        break;
                                    }
                                    else
                                    {
                                        Console.Write(" Nothing to level up");
                                        Tool.Dots(800);
                                    }
                                    break;

                                case "exit":
                                    Game_old.menuCurrent = "exploration";
                                    break;

                                case "bp":
                                case "battlepass":
                                    Game_old.menuCurrent = "battlepass";
                                    break;

                                default:
                                    Game_old.character[0].health -= 5;
                                    Console.Write(" What are you even trying to do? Moral down. HP -5");
                                    Tool.Dots(800);
                                    break;
                            }
                            break;

                        case "items":
                            switch (command) 
                            {
                                case "exit":
                                    Game_old.menuCurrent = "inventory";
                                    break;

                                case "health potion":
                                    if (Game_old.character[0].potionHealth > 0)
                                    {
                                        Game_old.character[0].potionHealth--;
                                        Game_old.character[0].health += MathF.Round(Game_old.character[0].healthMax * 0.5f);
                                        if (Game_old.character[0].health > Game_old.character[0].healthMax)
                                        { Game_old.character[0].health = Game_old.character[0].healthMax; }
                                        Console.Write(" You drink a health potion");
                                        Tool.Dots(600);
                                    }
                                    else 
                                    {
                                        Console.Write(" You are out of health potions");
                                        Tool.Dots(600);
                                    }
                                    break;

                                case "key":
                                    if (Game_old.character[0].key > 0)
                                    {
                                        Console.Write(" You can use this to open the exit");
                                        Tool.Dots(600);
                                    }
                                    else { goto default; }
                                    break;

                                default:
                                    Console.Write(" You don't have this item");
                                    Tool.Dots(400);
                                    break;
                            }
                            break;

                        case "leveling":
                            if (command != "" && Game_old.specialChar.Contains(char.ToUpper(command[0])))
                            {
                                switch (command[0])
                                {
                                    case 's':
                                        Game_old.character[0].strength++;
                                        break;
                                    case 'p':
                                        Game_old.character[0].perception++;
                                        break;
                                    case 'e':
                                        Game_old.character[0].endurance++;
                                        break;
                                    case 'c':
                                        Game_old.character[0].charisma++;
                                        break;
                                    case 'i':
                                        Game_old.character[0].intelligence++;
                                        break;
                                    case 'a':
                                        Game_old.character[0].agility++;
                                        break;
                                    case 'l':
                                        Game_old.character[0].luck++;
                                        break;
                                }
                                Game_old.character[0].specialLeft--;
                                Console.Write($" {Game_old.specialName[Array.IndexOf(Game_old.specialChar, char.ToUpper(command[0]))]} increased!");
                                if (Game_old.character[0].specialLeft > 1) { Console.Write($" {Game_old.character[0].specialLeft} more points left. Choose the next attribute: "); }
                                else if (Game_old.character[0].specialLeft == 1) { Console.Write($" {Game_old.character[0].specialLeft} more point left. Choose the next attribute: "); }
                            }
                            else if (command == "legend")
                            {
                                Console.WriteLine();
                                Console.WriteLine(" Strength - damage");
                                Console.WriteLine(" Perception - hit chance");
                                Console.WriteLine(" Endurance - health and armor");
                                Console.WriteLine(" Charisma - WIP");
                                Console.WriteLine(" Intelligence - WIP");
                                Console.WriteLine(" Agility - damage and dodge chance");
                                Console.WriteLine(" Luck - crit chance");
                                Console.WriteLine();
                                Console.Write(" Choose an attribute to level up (S/P/E/C/I/A/L): ");
                            }
                            else { Console.Write(" Invalid input, try again: "); }
                            break;

                        case "battlepass":
                            switch (commandSplit[0])
                            {
                                case "name":
                                    if (Game_old.colorToLower.Contains(commandSplit[1]) && Game_old.character[0].colorNameAvailable[Array.IndexOf(Game_old.colorToLower, commandSplit[1])] == true)
                                    {
                                        Game_old.character[0].colorName = commandSplit[1];
                                        Console.Write(" Name color changed");
                                        Tool.Dots(800);
                                    }
                                    else { goto default; }
                                    break;
                                case "exit":
                                    Game_old.menuCurrent = "inventory";
                                    break;
                                default:
                                    Console.Write(" Invalid input");
                                    Tool.Dots(400);
                                    break;
                            }
                            break;
                    }
                    break;
            }
        }

        static void DrawMap(string[][] map)
        {
            for (int i = 0; i < map.Length; i++)
            {
                Console.Write(" ");
                for (int j = 0; j < map[0].Length; j++)
                {
                    if (map[i][j].Equals("▒▒"))
                    {
                        Tool.WriteColored(map[i][j], "darkyellow");
                    }
                    else if (map[i][j].Equals(CharInfo.nameMap[0]))
                    {
                        Tool.WriteColored(map[i][j], Game_old.character[0].colorName);
                    }
                    else if (CharInfo.nameMap.Contains(map[i][j]))
                    {
                        Tool.WriteColored(map[i][j], CharInfo.colorName[Array.IndexOf(CharInfo.nameMap, map[i][j])]);
                    }
                    else { Console.Write(map[i][j]); }
                }
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.White;
            }
        }

        static void Spawn(int index)
        {
            Random rnd = new Random();
            bool spawning = true;
            int i = rnd.Next(0, Map_old.mapSpawns[Game_old.mapCurrentId].Length), j = i;
            int[] location;
            do
            {
                location = Map_old.mapSpawns[Game_old.mapCurrentId][i];
                if (!Game_old.characterLocation.Contains(location))                            // CHECK IF THIS EVEN WORKS
                {
                    spawning = false;
                    Game_old.characterLocation[index] = location;
                }
                else
                {
                    if (i < Map_old.mapSpawns[Game_old.mapCurrentId].Length - 1)
                    {
                        i++;
                    }
                    else { i = 0; }
                }
            }
            while (i != j && spawning);
            if (!spawning)
            {
                Game_old.mapCurrent[location[0]][location[1]] = CharInfo.nameMap[Game_old.character[index].id];
            }
            else
            {
                Console.WriteLine(" ERROR! FAILED TO SPAWN THE ENEMY");
                Console.ReadLine();
                Game_old.character[index] = null;
            }
        }

        static void Fight(bool isPlayerAttacking)
        {
            Random rnd = new Random();
            FightCurrent.playerTurn = true;
            FightCurrent.playerBlocking = false;
            FightCurrent.enemyBlocking = false;
            FightCurrent.enemyCharging = false;
            FightCurrent.won = false;
            FightCurrent.enemyChargeCD = 1;
            FightCurrent.ongoing = true;

            int id = Game_old.character[Game_old.opponentIndex].id;

            if (isPlayerAttacking) { FightCurrent.playerTurn = true; }
            else { FightCurrent.playerTurn = false; }

            while (FightCurrent.ongoing)
            {
                Console.Clear();
                Game_old.character[0].WriteAttributes();
                Console.WriteLine();
                Game_old.character[Game_old.opponentIndex].WriteAttributes();
                Console.WriteLine();
                Console.WriteLine(CharInfo.sprite[id][0]);

                Console.Write("\n Choose your next action (attack/defence): ");

                while (FightCurrent.playerTurn)
                {
                    PlayerCommand(Console.ReadLine());
                    Console.WriteLine();
                }
                FightCurrent.enemyBlocking = false;

                if (FightCurrent.ongoing)
                {
                    if (FightCurrent.enemyCharging)
                    {
                        Console.Write($" The {Game_old.character[Game_old.opponentIndex].name} hits you with a charged attack!");
                        Tool.Dots(500);
                        Console.WriteLine();
                        Attack(Game_old.opponentIndex, 0, 3, FightCurrent.playerBlocking);
                        FightCurrent.enemyChargeCD = 2;
                        FightCurrent.enemyCharging = false;
                    }

                    else if (FightCurrent.enemyChargeCD == 0 && rnd.Next(0, 100) > 30) 
                    {
                        FightCurrent.enemyCharging = true;
                        Console.Write($" The {Game_old.character[Game_old.opponentIndex].name} prepares a devastating attack");
                        Tool.Dots(1000);
                    }

                    else
                    {
                        Console.Write($" The {Game_old.character[Game_old.opponentIndex].name} attacks you!");
                        Tool.Dots(500);
                        Console.WriteLine();
                        Attack(Game_old.opponentIndex, 0, 1, FightCurrent.playerBlocking);
                        FightCurrent.enemyChargeCD--; 
                    }
                    FightCurrent.playerTurn = true;
                    FightCurrent.playerBlocking = false;
                    if (Game_old.character[0].health <= 0) { FightCurrent.ongoing = false; }
                }
            }

            // Fight won
            if (FightCurrent.won)
            {
                Console.Clear();
                Game_old.character[0].WriteAttributes();
                Console.WriteLine();
                Game_old.character[Game_old.opponentIndex].WriteAttributes();
                Console.WriteLine();
                Tool.WriteColored(CharInfo.sprite[id][1], "red");
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine($" The {Game_old.character[Game_old.opponentIndex].name} has been defeated!");

                // --- Rewards
                Game_old.character[0].xpCurrent += 15 + 15 * Game_old.character[Game_old.opponentIndex].level;
                bool lvlup = Game_old.character[0].CalculateLevel();
                if (lvlup) { Console.WriteLine(); } 
                Game_old.character[0].xpBpCurrent += 100;
                lvlup = Game_old.character[0].CalculateLevel();
                if (lvlup) { Console.WriteLine(); }
                Console.Write($" Acquired {15 + 15 * Game_old.character[Game_old.opponentIndex].level} XP, 100 Battle Pass XP");
                Tool.GenerateLoot(Game_old.character[Game_old.opponentIndex].name);
                Game_old.menuCurrent = "exploration";

                // --- Removes enemy from the map if it was attacked using 'cheat attack N'
                if (Game_old.mapCurrent[Game_old.characterLocation[Game_old.opponentIndex][0]][Game_old.characterLocation[Game_old.opponentIndex][1]] != "Pl")
                {
                    Game_old.mapCurrent[Game_old.characterLocation[Game_old.opponentIndex][0]][Game_old.characterLocation[Game_old.opponentIndex][1]] = "  ";
                }

                // --- Deletes enemy
                Game_old.character[Game_old.opponentIndex] = null;
                Game_old.characterLocation[Game_old.opponentIndex] = null;

                Console.Write("\n Press Enter to continue ");
                Console.ReadLine();
            }
        }

        static void Attack(int indexAttacker, int indexDefender, int attackPower, bool isBlocking)
        {
            int crit = 1;
            float block = 1, attackLast;
            Random rnd = new Random();
            // --- Dodge check
            if (rnd.Next(0, 100) > 20 + Game_old.character[indexDefender].agility * 3 - Game_old.character[indexAttacker].perception * 3)
            {
                // --- Crit check
                if (rnd.Next(0, 100) < Game_old.character[indexAttacker].luck * 3) { crit = 2; }
                // --- Blocking check
                if (isBlocking) { block = 0.25f; }
                attackLast = MathF.Round(Game_old.character[indexAttacker].attack * crit * attackPower * block * rnd.Next(75, 126) / 100 - Game_old.character[indexDefender].armor);
                Game_old.character[indexDefender].health -= attackLast;
                if (crit > 1) { Console.Write(" Critical hit!"); }
                if (indexAttacker == 0)
                {
                    Console.Write($" {Game_old.character[indexDefender].name} takes {attackLast} point(s) of damage");
                    Tool.Dots(800);
                }
                else
                {
                    Console.Write($" You take {attackLast} point(s) of damage");
                    Tool.Dots(1000);
                }
            }
            else 
            {
                if (indexAttacker == 0) 
                { 
                    Console.Write($" {Game_old.character[indexDefender].name} evades your attack");
                    Tool.Dots(800);
                }
                else 
                { 
                    Console.Write($" You evade {Game_old.character[indexAttacker].name}'s attack");
                    Tool.Dots(800);
                }
            }
        }

        static void Move(int index, string direction)
        {
            string[] directions = { "up", "down", "left", "right" };
            if (directions.Contains(direction))
            {
                int[] goalXY = [0, 0];
                switch (direction)
                {
                    case "up":
                        goalXY = [Game_old.characterLocation[index][0] - 1, Game_old.characterLocation[index][1]];
                        break;
                    case "down":
                        goalXY = [Game_old.characterLocation[index][0] + 1, Game_old.characterLocation[index][1]];
                        break;
                    case "left":
                        goalXY = [Game_old.characterLocation[index][0], Game_old.characterLocation[index][1] - 1];
                        break;
                    case "right":
                        goalXY = [Game_old.characterLocation[index][0], Game_old.characterLocation[index][1] + 1];
                        break;
                }
                string goal = Game_old.mapCurrent[goalXY[0]][goalXY[1]];
                if (!Map_old.walls.Contains(goal))
                {
                    // --- Enemy detection
                    if (CharInfo.nameMap.Contains(goal))
                    {
                        Game_old.menuCurrent = "fight";
                        if (index == 0)
                        {
                            Game_old.opponentIndex = Tool.IndexOfArray(Game_old.characterLocation, goalXY);
                            Console.Write($" You approach the {CharInfo.name[Array.IndexOf(CharInfo.nameMap, goal)]}");
                            Tool.Dots(800);
                        }
                        else 
                        {
                            Game_old.opponentIndex = index;
                            Console.Write($" The {CharInfo.name[Array.IndexOf(CharInfo.nameMap, Game_old.mapCurrent [Game_old.characterLocation[index][0]] [Game_old.characterLocation[index][1]] )]} approaches you");
                            Tool.Dots(800);
                        }
                    }
                    else 
                    {
                        if (index == 0)
                        {
                            Console.Write($" You move {direction}");
                            Tool.Dots(300);
                        }
                        else
                        {
                            Console.Write($" The {CharInfo.name[Array.IndexOf(CharInfo.nameMap, Game_old.mapCurrent[Game_old.characterLocation[index][0]][Game_old.characterLocation[index][1]])]} moves {direction}");
                            Tool.Dots(300);
                        }
                    }
                    Game_old.mapCurrent [Game_old.characterLocation[index][0]] [Game_old.characterLocation[index][1]] = Game_old.mapCurrentUnpopulated [Game_old.characterLocation[index][0]] [Game_old.characterLocation[index][1]];
                    Game_old.characterLocation[index] = goalXY;
                    if (!goal.Equals("Pl"))
                    {
                        Game_old.mapCurrent[goalXY[0]][goalXY[1]] = Game_old.character[index].nameShort;
                    }
                }
                else if (index == 0)
                {
                    Game_old.character[0].health -= 5;
                    Console.Write(" You bonk into a wall. HP -5");
                    Tool.Dots(800);
                }
            }
            else if (index == 0)
            {
                Game_old.character[0].health -= 5;
                Console.Write(" You slip on a banana peel. HP -5");
                Tool.Dots(800);
            }
        }
    }


    public static class Tool
    {
        public static void WriteColored(string text, string color) // --- For coloring text
        {
            switch (color.ToLower().Replace(" ", "")) // TEST
            {
                case "darkblue":
                    Console.ForegroundColor = ConsoleColor.DarkBlue;
                    Console.Write(text);
                    break;
                case "darkgreen":
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    Console.Write(text);
                    break;
                case "darkcyan":
                    Console.ForegroundColor = ConsoleColor.DarkCyan;
                    Console.Write(text);
                    break;
                case "darkred":
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.Write(text);
                    break;
                case "darkmagenta":
                    Console.ForegroundColor = ConsoleColor.DarkMagenta;
                    Console.Write(text);
                    break;
                case "darkyellow":
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.Write(text);
                    break;
                case "gray":
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.Write(text);
                    break;
                case "darkgray":
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.Write(text);
                    break;
                case "blue":
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.Write(text);
                    break;
                case "green":
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write(text);
                    break;
                case "cyan":
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.Write(text);
                    break;
                case "red":
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write(text);
                    break;
                case "magenta":
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    Console.Write(text);
                    break;
                case "yellow":
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write(text);
                    break;
                case "pride":
                    char[] textChar = text.ToCharArray();
                    int j = 0;
                    foreach (char i in textChar)
                    {
                        switch (j) 
                        {
                            case 0:
                                Console.ForegroundColor = ConsoleColor.Red;
                                break;
                            case 1:
                                Console.ForegroundColor = ConsoleColor.DarkYellow;
                                break;
                            case 2:
                                Console.ForegroundColor = ConsoleColor.Yellow;
                                break;
                            case 3:
                                Console.ForegroundColor = ConsoleColor.Green;
                                break;
                            case 4:
                                Console.ForegroundColor = ConsoleColor.Cyan;
                                break;
                            case 5:
                                Console.ForegroundColor = ConsoleColor.Blue;
                                break;
                            case 6:
                                Console.ForegroundColor = ConsoleColor.Magenta;
                                break;
                        }
                        if (j < 6) { j++; }
                        else { j = 0; }
                        Console.Write(i);
                    }
                    break;
                default:
                    Console.Write(text);
                    break;
            }
            Console.ForegroundColor = ConsoleColor.White;
        }

        public static int IndexOfArray(int[][] array2d, int[] array1d)
        {
            for (int i = 0; i < array2d.Length; i++ )
            {
                if (array2d[i] != null)
                {
                    for (int j = 0; j < array2d[i].Length; j++)
                    {
                        if (array2d[i][j] != array1d[j]) { break; }
                        else if (j == array1d.Length - 1) { return i; }
                    }
                }
            }
            return -1;
        }

        public static void Copy2DArray(string[][] original, string[][] target)
        {
            for ( int i = 0; i < original.Length; i++ )
            {
                target[i] = new string[original[i].Length];
                Array.Copy(original[i], target[i], original[i].Length);
            }
        }

        public static void GenerateLoot(string enemyName)
        {
            Random rnd = new Random();
            switch (enemyName)
            {
                case "Small Rat":
                    if (rnd.Next(0,100) < 50)
                    {
                        Game_old.character[0].potionHealth++;
                        Console.Write(", Health potion x 1");
                    }
                    break;

                case "Rat":
                    Game_old.character[0].potionHealth++;
                    Console.Write(", Health potion x 1");
                    break;

                case "Big Rat":
                    Game_old.character[0].potionHealth += 2;
                    Game_old.character[0].key++;
                    Console.Write(", Health potion x 2, Key");
                    break;

                default:
                    Console.Write(" Invalid loot table!");
                    Dots(1000);
                    Console.ReadLine();
                    break;
            }
        }

        public static void Dots(int duration)
        {
            Console.Write(".");
            Thread.Sleep(duration);
            Console.Write(".");
            Thread.Sleep(duration);
            Console.Write(".");
            Thread.Sleep(duration);
        }
    }


    public static class Game_old
    {
        public static string[][] mapCurrent, mapCurrentUnpopulated;
        public static bool gameOn = true, roundOn = true;
        public static int mapCurrentId, opponentIndex;
        public static string menuCurrent = "exploration";

        public static Character_old[] character = new Character_old[7];
        public static int[][] characterLocation = new int[7][];

        public static string[] specialName = { "Strength", "Perception", "Endurance", "Charisma", "Intelligence", "Agility", "Luck" };
        public static char[] specialChar = { 'S', 'P', 'E', 'C', 'I', 'A', 'L' };

        public static string[] color = { "White", "DarkBlue", "DarkGreen", "DarkCyan", "DarkRed", 
                                        "DarkMagenta", "DarkYellow", "Gray", "DarkGrey", "Blue", 
                                        "Green", "Cyan", "Red", "Magenta", "Yellow", 
                                        "Pride" };
        public static string[] colorToLower = { "white", "darkblue", "darkgreen", "darkcyan", "darkred",
                                        "darkmagenta", "darkyellow", "gray", "darkgrey", "blue",
                                        "green", "cyan", "red", "magenta", "yellow",
                                        "pride" };
    }


    public static class FightCurrent
    {
        public static bool ongoing, won, playerTurn, playerBlocking, enemyBlocking, enemyCharging;
        public static int enemyChargeCD;
    }


    public static class Map_old
    {
        // --- Different walls: ░░░ ▒▒▒ ▓▓▓
        //public static string[] walls = { " ║ ", "══", "██", " ╔", "╗ ", " ║", "║ ", " ╚", "╝ " }; // --- Old walls
        public static string[] walls = { "▒▒" };


        public static string[][] map1 = { ["▒▒", "▒▒", "▒▒", "▒▒", "▒▒", "▒▒", "▒▒"],
                                          ["▒▒", "  ", "▒▒", "  ", "  ", "  ", "▒▒"],
                                          ["▒▒", "  ", "▒▒", "▒▒", "▒▒", "  ", "▒▒"],
                                          ["▒▒", "  ", "▒▒", "  ", "  ", "  ", "▒▒"],
                                          ["▒▒", "  ", "  ", "  ", "▒▒", "  ", "▒▒"],
                                          ["▒▒", "▒▒", "▒▒", "  ", "▒▒", "  ", "▒▒"],
                                          ["▒▒", "▒▒", "▒▒", "▒▒", "▒▒", "▒▒", "▒▒"] };
        
                                              /* 0     1     2     3     4     5     6     7     8     9     10    11*/
        public static string[][] map2 = { /*0*/["▒▒", "▒▒", "▒▒", "▒▒", "▒▒", "  ", "  ", "▒▒", "▒▒", "▒▒", "▒▒", "▒▒"],
                                               ["▒▒", "  ", "  ", "  ", "▒▒", "  ", "  ", "▒▒", "  ", "  ", "  ", "▒▒"],
                                               ["▒▒", "  ", "  ", "  ", "▒▒", "  ", "  ", "▒▒", "  ", "  ", "  ", "▒▒"],
                                               ["▒▒", "▒▒", "  ", "▒▒", "▒▒", "▒▒", "▒▒", "▒▒", "  ", "  ", "  ", "▒▒"],
                                               ["▒▒", "  ", "  ", "  ", "  ", "  ", "  ", "  ", "  ", "  ", "  ", "▒▒"],
                                          /*5*/["▒▒", "▒▒", "▒▒", "  ", "▒▒", "▒▒", "▒▒", "▒▒", "▒▒", "▒▒", "  ", "▒▒"],
                                               ["▒▒", "  ", "▒▒", "  ", "▒▒", "  ", "  ", "▒▒", "  ", "▒▒", "  ", "▒▒"],
                                               ["▒▒", "  ", "  ", "  ", "▒▒", "  ", "  ", "▒▒", "  ", "  ", "  ", "▒▒"],
                                               ["▒▒", "▒▒", "▒▒", "▒▒", "▒▒", "  ", "  ", "▒▒", "▒▒", "▒▒", "▒▒", "▒▒"] };


        public static int[][][] mapSpawns = { [ [5, 5], [1, 1], [5, 3], [1, 3] ],
                                              [ [6, 1], [1, 2], [6, 8], [1, 10], [4, 5] ]};
        public static string[][][] mapList = { map1, map2 };
    }



    public static class CharInfo 
    {
        public static string[] name =      { "Player",  "Small Rat",  "Rat",    "Big Rat"};
        public static string[] nameMap =   { "Pl",      "rt",         "Rt",     "RT" };
        public static string[] colorName = { "White",   "Gray",       "Gray",   "DarkRed" };
        public static int[] level =        { 1,     1,     3,     5 };
        public static int[] strength =     { 3,     2,     3,     4 };
        public static int[] perception =   { 3,     1,     2,     2 };
        public static int[] endurance =    { 3,     1,     2,     3 };
        public static int[] charisma =     { 3,     0,     0,     0 };
        public static int[] intelligence = { 3,     1,     1,     2 };
        public static int[] agility =      { 3,     5,     2,     0 };
        public static int[] luck =         { 3,     4,     4,     5 };
        public static float[] attackMp =   { 1,     1,  1.2f,  1.5f };
        public static float[] hpMp =       { 1,  0.5f,  0.6f,  0.7f };

        public static string spriteSmallRatAlive = 
            """     _______     """ + "\n" +
            """   ./_o?____\_/  """ + "\n" +
            """      //  \\     """;

        public static string spriteSmallRatDead =
            """                  """ + "\n" +
            """   .__\\__//__    """ + "\n" +
            """    \_*b____/ \   """;

        public const string spriteBigRatAlive =
            """           _     __,..-----.._               ':-.     """ + "\n" +
            """         _/_)_.-`             '-.              `\\    """ + "\n" +
            """   \|.-'`   /o)                  '.              ||   """ + "\n" +
            """   /.   _o  ,                      \            .'/   """ + "\n" +
            """   \,_|\__,/                _..-    \       _.-'.'    """ + "\n" +
            """      | \ \      \         /         `----'`_.-'      """ + "\n" +
            """         _/;--._\ )        |   _\.__/`-----'          """ + "\n" +
            """       (((-'  __//`'-..__..-\       )                 """ + "\n" +
            """            (((-'       __// ''--. /                  """ + "\n" +
            """                      (((-'    __//                   """ + "\n" +
            """                             (((-'                    """;

        public const string spriteBigRatDead =
            """                                                               """ + "\n" +
            """                                                               """ + "\n" +
            """                  _,..-----.._                                 """ + "\n" +
            """          _    .-'            '-.                              """ + "\n" +
            """        _/_)_./                  '.                            """ + "\n" +
            """  \|.-'`   /o)                     \                           """ + "\n" +
            """  /.   _x  ,                _..-    \                          """ + "\n" +
            """  \,_|\__,/      \         /         `----..__                 """ + "\n" +
            """     | \ \/;--._\ )        |   _\.__/`----..__''--._           """ + "\n" +
            """       (((-' (((-'`'-..___.-\..__   )         ''--. \          """ + "\n" +
            """                      (((-'/  (((-'/               \|          """;

        public static string[][] sprite = { ["", ""], [spriteSmallRatAlive, spriteSmallRatDead], ["", ""], [spriteBigRatAlive, spriteBigRatDead] };
    }



    public class Character_old 
    {
        public string name, nameShort, colorName;
        public int id, level, specialLeft, xpCurrent = 0, xpMax, money = 0, xpBpCurrent = 0, levelBp = 0, armor, 
            strength, perception, endurance, charisma, intelligence, agility, luck;
        public float attackMultiplier, hpMultiplier, health, healthMax, attack;
        public bool[] colorNameAvailable = [true, false, false, false, false, // Game_old.color
                                            false, false, false, false, false, 
                                            false, false, false, false, false, 
                                            false];

        // --- Inventory
        public int potionHealth = 0;
        public int key = 0;

        public Character_old(string Name) 
        {
            id = Array.IndexOf(CharInfo.name, Name);
            name = CharInfo.name[id];
            nameShort = CharInfo.nameMap[id];
            colorName = CharInfo.colorName[id];
            level = CharInfo.level[id];

            strength = CharInfo.strength[id];
            perception = CharInfo.perception[id];
            endurance = CharInfo.endurance[id];
            charisma = CharInfo.charisma[id];
            intelligence = CharInfo.intelligence[id];
            agility = CharInfo.agility[id];
            luck = CharInfo.luck[id];

            attackMultiplier = CharInfo.attackMp[id];
            hpMultiplier = CharInfo.hpMp[id];

            CalculateStats();
            Heal();
        }

        public void CalculateStats()
        {
            healthMax = MathF.Round((50 + endurance * 10 + level * 10) * hpMultiplier);
            attack = MathF.Round((strength * 4 + agility * 2) * attackMultiplier);
            armor = endurance;
            xpMax = 20 + 30 * level;
        }

        public void Heal()
        {
            health = healthMax;
        }

        public bool CalculateLevel()
        {
            bool lvlup = false;
            while (xpCurrent >= xpMax)
            {
                lvlup = true;
                level++;
                xpCurrent -= xpMax;
                specialLeft += 3;
                CalculateStats();
                Console.Write(" LEVEL UP! 3 new Special points available!");
            }
            while (xpBpCurrent >= 1000)
            {
                lvlup = true;
                Console.Write(" BATTLE PASS LEVEL UP!");
                levelBp++;
                xpBpCurrent -= 1000;
                switch (levelBp)
                {
                    case 1:
                        colorNameAvailable[3] = true;
                        Console.Write($" {Game_old.color[3]} name color available!");
                        break;
                    case 2:
                        colorNameAvailable[14] = true;
                        Console.Write($" {Game_old.color[14]} name color available!");
                        break;
                    case 3:
                        colorNameAvailable[13] = true;
                        Console.Write($" {Game_old.color[13]} name color available!");
                        break;
                    case 4:
                        colorNameAvailable[4] = true;
                        Console.Write($" {Game_old.color[4]} name color available!");
                        break;
                    case 5:
                        colorNameAvailable[15] = true;
                        Console.Write($" {Game_old.color[15]} name color available!");
                        break;
                    default:
                        Console.Write(" No reward here yet!");
                        break;
                }
            }
            return lvlup;
        }

        public void WriteAttributes()
        {
            Console.Write(" --<");
            Tool.WriteColored(name, colorName);
            Console.WriteLine(">--");
            if (id == 0)
            {
                Console.WriteLine($" Level: {level}, experience: {xpCurrent}/{xpMax}");
                Console.WriteLine($" Health Points: {health}/{healthMax}");
                Console.WriteLine($" Money: {money}");
            }
            else { 
                Console.WriteLine($" Level: {level}");
                Console.WriteLine($" Health Points: {health}/{healthMax}");
            }
        }

        public void WriteSpecial()
        {
            Console.WriteLine(" --<S.P.E.C.I.A.L.>--");
            Console.WriteLine($" Strength: {strength}");
            Console.WriteLine($" Perception: {perception}");
            Console.WriteLine($" Endurance: {endurance}");
            Console.WriteLine($" Charisma: {charisma}");
            Console.WriteLine($" Intelligence: {intelligence}");
            Console.WriteLine($" Agility: {agility}");
            Console.WriteLine($" Luck: {luck}");
            if (specialLeft > 0)
            {
                Console.WriteLine($" You have {specialLeft} spare points.");
            }
        }

        public void WriteInventory()
        {
            bool empty = true;
            Console.WriteLine(" --<Inventory>--");
            if (key > 0) { Console.WriteLine($" Key x {key}"); empty = false; }
            if (potionHealth > 0) { Console.WriteLine($" Health potion x {potionHealth}"); empty = false; }
            if (empty) { Console.WriteLine(" Quite empty in here for now..."); }
        }

        public void WriteBP()
        {
            Console.WriteLine($" Your current level: {levelBp}");
            Console.WriteLine($" XP till next level: {xpBpCurrent}/1000");
            Console.WriteLine(" ╔══════════╦══════════╦══════════╦══════════╦══════════╗");
            WriteBPSection(1, 5);
            Console.WriteLine(" ╠══════════╬══════════╬══════════╬══════════╬══════════╣");

            Console.Write(" ║");
            Tool.WriteColored("Name Color", Game_old.color[3]);
            Console.Write("║");
            Tool.WriteColored("Name Color", Game_old.color[14]);
            Console.Write("║");
            Tool.WriteColored("Name Color", Game_old.color[13]);
            Console.Write("║");
            Tool.WriteColored("Name Color", Game_old.color[4]);
            Console.Write("║");
            Tool.WriteColored("Name Color", Game_old.color[15]);
            Console.WriteLine("║");

            Console.WriteLine(" ╚══════════╩══════════╩══════════╩══════════╩══════════╝");
        }

        public void WriteBPSection(int startLevel, int endLevel)
        {
            Console.Write(" ");
            while (startLevel <= endLevel)
            {
                Console.Write("║");
                if (startLevel <= levelBp)
                {
                    Tool.WriteColored("▓▓▓▓▓▓▓▓▓▓", "green");
                }
                else if (startLevel == levelBp + 1)
                {
                    int i = xpBpCurrent / 100;
                    int j = 10;
                    while (i > 0 && j > 0)
                    {
                        Console.Write("▓");
                        i--;
                        j--;
                    }
                    while (j > 0)
                    {
                        Console.Write("░");
                        j--;
                    }
                }
                else
                {
                    Console.Write("░░░░░░░░░░");
                }
                startLevel++;
            }
            Console.WriteLine("║");
        }
    }
}