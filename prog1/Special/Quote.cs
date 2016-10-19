// Quote -- Parse tree node strategy for printing the special form quote

using System;
using Parse;

namespace Tree
{
    public class Quote : Special
    {
        // TODO: Add any fields needed.
  
        // TODO: Add an appropriate constructor.
	public Quote() { }

        public override void print(Node t, int n, bool p)
        {
            // working implementation

            for(int i = 0; i<n; i++)
            {
                Console.Write(" ");
                n++;
            }
            
            Console.Write("'");

            Special.Indentation_cumulative++;

            bool isCdr_null = false;
            // check if cdr is null
            if (t.getCdr() != null)
                isCdr_null = false;
            else
                isCdr_null = true;

            // print Cdr
            // error
            if(isCdr_null)
            {
                if (Scanner.flag_debugger)
                   Console.Error.WriteLine("quote's cdr is null... not correct.");
            }
            else
            {
                
                

                Special.printing_quote_Contents = true;

                // this is a flag of whether the last cons node passed and inspected was a cdr.
                // set to true here
                 // this flag is of a bit odd  and particular application  only in quote printing
                Special.last_cons_A_cdr = true;
                // print regular expression
                t.getCdr().print(0, true);

                Special.printing_quote_Contents = false;
                // this is a flag of whether the last cons node passed and inspected was a cdr.
                // since this is the first one, the default is to be false.
                Special.last_cons_A_cdr = false;
               
            }
        }
    }
}

