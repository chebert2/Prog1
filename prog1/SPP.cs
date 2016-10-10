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

        Parser.pushedBack_extraToken_fromQuoteMark_ = null;
    Parser.special_type_Has_to_have_non_nil_cdr = false;
        Parser.last_itertion_Identifier = null;
        Parser.lastRunHad_Special_id = 0;

        Parser.quote_mark_misc_to_placed_cursor__is_not_new_data = false;
        Parser.quote_mark__misc_LPAREN__0___2 =  'A';
    Parser.flip_Past_RightParenthesis_after_tail = false;
        Parser.start_addit_Ident_needing_End_expression_tail = false;

        Parser.need_to_insert_right_parenthesis = false;
        Parser.still_need_to_put_in_RPAREN__next_iteration = false;
        Parser.quote_extension_going = false;
        Parser.runningStarted_ForSomething = false;
        Parser.emptyTerm = false;
        // Parse and pretty-print each input expression

        bool firstTimePrinting = false; 
        root = parser.parseExp();
        if (root != null)
            root.print(0);
        while (root != null) 
        {
            if (!firstTimePrinting)
                firstTimePrinting = true;
            else
                root.print(0);


            //an error if Console.WriteLine(Parser.number_of_Left_parentheses_extra_over_zero); 
            Parser.firstRun = true;
            Parser.stillReadFirstParenthesis = false;
            // start new parse line
            Console.WriteLine();
            scanner.numberParentheses_L = 0;
            scanner.numberParentheses_R = 0;
            scanner.makeANew_Peek = false;
            scanner.ifPast_lookWasPeek = false;
            Parser.pushedBack_extraToken_fromQuoteMark_ = null;
            Parser.quote_mark_misc_to_placed_cursor__is_not_new_data = false;
            Parser.quote_mark__misc_LPAREN__0___2 = 'A';
            Parser.special_type_Has_to_have_non_nil_cdr = false;
            Parser.last_itertion_Identifier = null;
            Parser.lastRunHad_Special_id = 0;

            Parser.flip_Past_RightParenthesis_after_tail = false;
            Parser.tail_item_A_list = false;
            Parser.start_addit_Ident_needing_End_expression_tail = false;
            Special.localExpression_ended_case = false;
            Regular.boolean_first_run = true;
            Regular.lastCall_was_to_cdr = false;

            Parser.need_to_insert_right_parenthesis = false;
            Parser.runningStarted_ForSomething = false;
            Parser.still_need_to_put_in_RPAREN__next_iteration = false;
            Parser.quote_extension_going = false;
            Parser.emptyTerm = false;

            root = parser.parseExp();
            
        }
        

        Console.ReadKey();
        return 0;
    }
}
