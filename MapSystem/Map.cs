using Fallin.Characters;

namespace Fallin.MapSystem
{
    public class Map
    {
        public Cell[,] Layout { get; protected set; }
        private int id;
    
    
        public Map(int overrideId = -1)
        {
            id = overrideId;
            if (id == -1)
            {
                id = MapLibrary.GetRandomMapID();
            }
            string[][] walls = MapLibrary.GetMap(id);
            int x = walls[0].Length;
            int y = walls.Length;
            Layout = new Cell[y,x];

            for (int i = 0; i < y; i++)
            {
                for (int j = 0; j < x; j++)
                {
                    if (walls[i][j] == "  ")
                    {
                        Layout[i, j] = new Cell(false);
                    }
                    else
                    {
                        Layout[i, j] = new Cell(true);
                    }
                }
            }
        }

        /// <summary>
        /// Used to move enemy, returns true if movement was successful
        /// </summary>
        public bool MoveEnemy(Enemy enemy, string direction)
        {
            (int y, int x) position = enemy.Position;
            switch (direction)
            {
                case "up":
                    position.y -= 1;
                    break;

                case "down":
                    position.y += 1;
                    break;

                case "left":
                    position.x -= 1;
                    break;

                case "right":
                    position.x += 1;
                    break;

                default:
                    return false;
            }
            Cell cellTarget = Layout[position.y, position.x];

            if (cellTarget.IsWall || cellTarget.HasEnemy) 
            { 
                return false; 
            }
            else if (cellTarget.HasHero)
            {
                // ADD fight

                return true;
            }
            else
            {
                Layout[enemy.Position.y, enemy.Position.x].RemoveCharacter();
                cellTarget.AddCharacter(enemy);
                enemy.Position = position;

                return true;
            }
        }

        /// <summary>
        /// Used to move hero, returns true if movement was successful
        /// </summary>
        public bool MoveHero(Hero player, string direction)
        {
            bool bananaSlip = false;
            (int y, int x) position = player.Position;
            switch (direction)
            {
                case "north":
                case "up":
                    position.y -= 1;
                    break;

                case "south":
                case "down":
                    position.y += 1;
                    break;

                case "west":
                case "left":
                    position.x -= 1;
                    break;

                case "east":
                case "right":
                    position.x += 1;
                    break;

                default:
                    bananaSlip = true;
                    break;
            }
            Cell cellTarget = Layout[position.y, position.x];

            if (bananaSlip)
            {
                player.Health -= 5;
                Console.Write(" You slip on a banana peel. HP -5");
                Utilities.Dots();

                return false;
            }
            else if (cellTarget.IsWall)
            {
                player.Health -= 5;
                Console.Write(" You bonk into a wall. HP -5");
                Utilities.Dots();

                return false;
            }
            else if (cellTarget.HasEnemy)
            {
                // ADD FIGHT

                return true;
            }
            else
            {
                Layout[player.Position.y, player.Position.x].RemoveCharacter();
                cellTarget.AddCharacter(player);
                player.Position = position;

                Console.Write($" You move {direction}");
                Utilities.Dots(400);

                return true;
            }
        }

        public void DrawMap()
        {
            for (int y = 0; y < Layout.GetLength(0); y++)
            {
                Console.Write("  ");
                for (int x = 0; x < Layout.GetLength(1); x++)
                {
                    Layout[y, x].DrawCell();
                }
                Console.WriteLine();
            }
        }

        protected bool Spawn(Character character)
        {
            int[][] spawns = MapLibrary.GetSpawns(id);
            Utilities.ShuffleArray(spawns);
            for (int i = 0; i < spawns.Length; i++)
            {
                Cell target = Layout[spawns[i][0], spawns[i][1]];
                if (!target.IsWall && !target.HasEnemy)
                {
                    target.AddCharacter(character);
                    character.Position = (spawns[i][0], spawns[i][1]);
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Used to spawn player at the start of the round at random point, assumes there is a free spawn point
        /// </summary>
        /// <param name="player"></param>
        public void SpawnHeroAtRandomPosition(Hero player)
        {
            Spawn(player);
        }

        /// <summary>
        /// Used to spawn enemies at random points, deletes the enemy and prints the name of it if spawn fails
        /// </summary>
        /// <param name="enemy"></param>
        public void TrySpawnEnemyAtRandomPosition(Enemy enemy)
        {
            bool success = Spawn(enemy);
            if (!success)
            {
                enemy.Remove();
                Console.WriteLine($" Failed to spawn {enemy.Name}!");
                Utilities.Dots();
            }
        }
    }
}