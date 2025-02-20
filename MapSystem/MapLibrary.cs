using System.Data.Common;

namespace Fallin.MapSystem
{
    public static class MapLibrary
    {
        // --- Different walls: ░░░ ▒▒▒ ▓▓▓
        //public static string[] walls = { " ║ ", "══", "██", " ╔", "╗ ", " ║", "║ ", " ╚", "╝ " }; // --- Old walls
        //public static string[] walls = { "▒▒" };

        private static readonly string[][] Map1 = [ ["▒▒", "▒▒", "▒▒", "▒▒", "▒▒", "▒▒", "▒▒"],
                                                    ["▒▒", "  ", "▒▒", "  ", "  ", "  ", "▒▒"],
                                                    ["▒▒", "  ", "▒▒", "▒▒", "▒▒", "  ", "▒▒"],
                                                    ["▒▒", "  ", "▒▒", "  ", "  ", "  ", "▒▒"],
                                                    ["▒▒", "  ", "  ", "  ", "▒▒", "  ", "▒▒"],
                                                    ["▒▒", "▒▒", "▒▒", "  ", "▒▒", "  ", "▒▒"],
                                                    ["▒▒", "▒▒", "▒▒", "▒▒", "▒▒", "▒▒", "▒▒"] ];
        
                                                        /* 0     1     2     3     4     5     6     7     8     9     10    11*/
        private static readonly string[][] Map2 = [ /*0*/["▒▒", "▒▒", "▒▒", "▒▒", "▒▒", "  ", "  ", "▒▒", "▒▒", "▒▒", "▒▒", "▒▒"],
                                                         ["▒▒", "  ", "  ", "  ", "▒▒", "  ", "  ", "▒▒", "  ", "  ", "  ", "▒▒"],
                                                         ["▒▒", "  ", "  ", "  ", "▒▒", "  ", "  ", "▒▒", "  ", "  ", "  ", "▒▒"],
                                                         ["▒▒", "▒▒", "  ", "▒▒", "▒▒", "▒▒", "▒▒", "▒▒", "  ", "  ", "  ", "▒▒"],
                                                         ["▒▒", "  ", "  ", "  ", "  ", "  ", "  ", "  ", "  ", "  ", "  ", "▒▒"],
                                                    /*5*/["▒▒", "▒▒", "▒▒", "  ", "▒▒", "▒▒", "▒▒", "▒▒", "▒▒", "▒▒", "  ", "▒▒"],
                                                         ["▒▒", "  ", "▒▒", "  ", "▒▒", "  ", "  ", "▒▒", "  ", "▒▒", "  ", "▒▒"],
                                                         ["▒▒", "  ", "  ", "  ", "▒▒", "  ", "  ", "▒▒", "  ", "  ", "  ", "▒▒"],
                                                         ["▒▒", "▒▒", "▒▒", "▒▒", "▒▒", "  ", "  ", "▒▒", "▒▒", "▒▒", "▒▒", "▒▒"] ];


        private static readonly int[][][] MapSpawns = [[ [5, 5], [1, 1], [5, 3], [1, 3] ],
                                             [ [6, 1], [1, 2], [6, 8], [1, 10], [4, 5] ]];
        private static readonly string[][][] MapList = [Map1, Map2];


        public static int GetRandomMapID()
        {
            Random rnd = new();
            return rnd.Next(0, MapList.Length);
        }

        public static int[] GetRandomSpawn(int id)
        {
            Random rnd = new();
            return MapSpawns[id][rnd.Next(0, MapSpawns[id].Length)];
        }

        public static string[][] GetMap(int id)
        {
            return MapList[id];
        }

        public static int[][] GetMapSpawns(int id)
        {
            return MapSpawns[id];
        }
    }
}