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


        public void MoveHero(Hero hero, string direction)
        {
            (int x, int y) position = hero.Position;
            switch (direction)
            {
                case "up":
                    position.x += 1;
                    break;
                case "down":
                    position.x -= 1;
                    break;
                case "left":
                    position.y -= 1;
                    break;
                case "right":
                    position.y += 1;
                    break;
            }
            Cell cellTarget = MapCurrent[position.x, position.y];
            if (cellTarget.IsWall)
            {
                // ADD WALL BONK
            }
            else if (cellTarget.HasEnemy)
            {
                // ADD FIGHT
            }
            else
            {
                MapCurrent[hero.Position.x, hero.Position.y].RemoveCharacter();
                cellTarget.AddCharacter(hero);
                hero.Position = position;
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