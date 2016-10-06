// SPP -- The main program of the Scheme pretty printer.

using System;
using Parse;
using Tokens;
using Tree;

public class SPP
{
    //public static int totalIndentation = 0;

    public static int Main(string[] args)
    {
        // Create scanner that reads from standard input
        Scanner scanner = new Scanner();

        if (args.Length > 1 ||
            (args.Length == 1 && !args[0].Equals("-d")))
        {
            Console.Error.WriteLine("Usage: mono SPP [-d]");
            return 1;
        }

        // If command line option -d is provided, debug the scanner.
        if (args.Length == 1 && args[0].Equals("-d"))
            // set to debugging_mode
            Scanner.flag_debugger = true;
        else
            Scanner.flag_debugger = false;
            
        

        // Create parser
        Parser parser = new Parser(scanner);
        Node root;

        // Parse and pretty-print each input expression
        root = parser.parseExp();
        while (root != null)
        {
            // ok
 
            root.print(0);
            root = parser.parseExp();
        }

        return 0;
    }
}