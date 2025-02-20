using Fallin.Characters;

namespace Fallin.MapSystem
{
    public class Map
    {
        public Cell[,] MapCurrent { get; protected set; }
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
            MapCurrent = new Cell[x,y];

            for (int i = 0; i < y; i++)
            {
                for (int j = 0; j < x; j++)
                {
                    if (walls[j][i] == "  ")
                    {
                        MapCurrent[j, i] = new Cell(false);
                    }
                    else
                    {
                        MapCurrent[j, i] = new Cell(true);
                    }
                }
            }
        }


        public void MoveHero(Hero player, string direction)
        {
            bool bananaSlip = false;
            (int x, int y) position = player.Position;
            switch (direction)
            {
                case "north":
                case "up":
                    position.x += 1;
                    break;

                case "south":
                case "down":
                    position.x -= 1;
                    break;

                case "west":
                case "left":
                    position.y -= 1;
                    break;

                case "east":
                case "right":
                    position.y += 1;
                    break;

                default:
                    bananaSlip = true;
                    break;
            }
            Cell cellTarget = MapCurrent[position.x, position.y];

            if (bananaSlip)
            {
                player.Health -= 5;
                Console.Write(" You slip on a banana peel. HP -5");
                Utilities.Dots();
            }
            else if (cellTarget.IsWall)
            {
                player.Health -= 5;
                Console.Write(" You bonk into a wall. HP -5");
                Utilities.Dots();
            }
            else if (cellTarget.HasEnemy)
            {
                // ADD FIGHT
            }
            else
            {
                MapCurrent[player.Position.x, player.Position.y].RemoveCharacter();
                cellTarget.AddCharacter(player);
                player.Position = position;

                Console.Write($" You move {direction}");
                Utilities.Dots();
            }
        }

        public void DrawMap()
        {
            for (int x = 0; x < MapCurrent.GetLength(0); x++)
            {
                Console.Write("  ");
                for (int y = 0; y < MapCurrent.GetLength(1); y++)
                {
                    MapCurrent[x, y].DrawCell();
                }
                Console.WriteLine();
            }
        }

        public void Spawn(Character character)
        {
            int[] position = MapLibrary.GetRandomSpawn(id);
            character.Position = (position[0], position[1]);
            MapCurrent[position[0], position[1]].AddCharacter(character);
        }
    }
}