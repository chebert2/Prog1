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

        
        Special.localExpression_ended_case = false;
        Regular.boolean_first_run = true;
        Regular.lastCall_was_to_cdr = false;

        // Parse and pretty-print each input expression

        root = parser.parseExp();
        while (root != null)
        {
            root.print(0);
            
                 //an error if Console.WriteLine(Parser.number_of_Left_parentheses_extra_over_zero); 
            Parser.firstRun = true;
            Parser.stillReadFirstParenthesis = true;
            // start new parse line
            Console.WriteLine();
            scanner.numberParentheses_L = 0;
            scanner.numberParentheses_R = 0;
            Parser.tail_item_A_list = false;
            Special.localExpression_ended_case = false;
            Regular.boolean_first_run = true;
            Regular.lastCall_was_to_cdr = false;

            root = parser.parseExp();
        }

        return 0;
    }
}
