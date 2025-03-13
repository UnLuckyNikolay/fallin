namespace Fallin
{
    /// <summary>
    /// Used for Utilities.WriteColored()
    /// </summary>
    public enum Color
    {
        // Console colors
        DarkGray, DarkBlue, DarkGreen, DarkCyan, DarkRed, DarkMagenta, DarkYellow,
        White, Gray, Blue, Green, Cyan, Red, Magenta, Yellow,
        // Custom colors
        Pride
    }

    public static class Utilities
    {
        public static void WriteColored(string text, Color color)
        {
            switch (color)
            {
                case Color.Pride:
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
                    if (Enum.TryParse<ConsoleColor>(color.ToString(), out ConsoleColor colorConsole))
                    {
                        Console.ForegroundColor = colorConsole;
                        Console.Write(text);
                    }
                    else { Console.Write(text); }
                    break;
            }
            Console.ForegroundColor = ConsoleColor.White;
        }

        public static void Dots(int dotDuration=800, int dotsAmount=3)
        {
            for (int i = 0; i < dotsAmount - 1; i++)
            {
                Console.Write(".");
                Thread.Sleep(dotDuration); 
            }
            Console.WriteLine(".");
            Thread.Sleep(dotDuration);
        }

        public static void ShuffleArray<T>(T[] array)
        {
            Random rng = new();
            int n = array.Length;
            while (n > 1)
            {
                int k = rng.Next(n--);
                T temp = array[n];
                array[n] = array[k];
                array[k] = temp;
            }
        }
    }
}