namespace Fallin
{
    public static class Utilities
    {
        public static void WriteColored(string text, string color) // --- For coloring text
        {
            switch (color.ToLower().Replace(" ", ""))
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

        public static void Dots(int dotDuration=300, int dotsAmount=3)
        {
            for (int i = 0; i < dotsAmount - 1; i++)
            {
                Console.Write(".");
                Thread.Sleep(dotDuration); 
            }
            Console.WriteLine(".");
            Thread.Sleep(dotDuration);
        }
    }
}