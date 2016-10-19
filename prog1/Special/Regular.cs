// Regular -- Parse tree node strategy for printing regular lists

using System;
using Parse;


namespace Tree
{
    public class Regular : Special
    {
        
        // TODO: Add any fields needed.

        //public int local_indent;
        //public static bool current_Expression_NotEnded;
        //public static bool boolean_first_run;
        //public static bool lastCall_was_to_cdr;
        // TODO: Add an appropriate constructor.
        public Regular()

        {
       
            //local_indent = 0;

        }

        public override void print(Node t, int n, bool p)
        {
            //int Indent__just_direct_deal = 0;

            for(int i = 0; i<n; i++)
            {
                Console.WriteLine(" ");
            }

            bool carIs_null = false;
            bool cdrIs_null = false;

            // first we will check if the car of this item has is even a non-null node.
            if (t.getCar() != null)
                carIs_null = false;
            else
            {
                carIs_null = true;
                if (Scanner.flag_debugger)
                    Console.Error.WriteLine("car is null... not correct.");
                return;
            }
       // this will accomplish   printing only quote expression   and not the list quote form per se.
            bool do_something__if_on_topmost_1st_cdr_side_of_mainLine;

            if (Special.printing_quote_Contents == true)
            {
                if (Special.last_cons_A_cdr == false)
                    do_something__if_on_topmost_1st_cdr_side_of_mainLine = false;

                else
                    do_something__if_on_topmost_1st_cdr_side_of_mainLine = true;
                
            }
            else
                do_something__if_on_topmost_1st_cdr_side_of_mainLine = false;
            





                // write our left parenthesis.
            if (carIs_null == false && !p)
            {
                Console.Write("(");

                //Indent__just_direct_deal += 1;
                //Special.Indentation_cumulative = n + Indent__just_direct_deal;
                Special.Indentation_cumulative = n + 1;
            }
            else if(carIs_null == false)
                Special.Indentation_cumulative = n;

            bool carIs_consNode = false;

            // check if car is a pair now.
            if (carIs_null == false && t.getCar().isPair())
                carIs_consNode = true;

            // print car item     below code:
            // car is a cons ...
            if(carIs_null == false && carIs_consNode)
            {
                if (Special.printing_quote_Contents == true)
                    Special.last_cons_A_cdr = false;

                t.getCar().print(0, false);
            }
            // car is a primitive node
            else if (carIs_null == false)
            {
                t.getCar().print(0, !p);
            }
            // else
            // error


           
            // work on printing cdr of cons of t

            if (t.getCdr() != null)
                cdrIs_null = false;
            else
                cdrIs_null = true;

            if (cdrIs_null == false && t.getCdr().isPair())
            {
                Console.Write(" ");
                Special.Indentation_cumulative += 1;

                // reset this as case   with the upper most diagonal case  of cdr .. being followed
              // this is a flag of whether the last cons node passed and inspected was a cdr.
              // set to true here
              // this flag is of a bit odd  and particular application  only in quote printing
                if    (Special.printing_quote_Contents    && 
                    
                    do_something__if_on_topmost_1st_cdr_side_of_mainLine == true  )

                    Special.last_cons_A_cdr = true;



                t.getCdr().print(0, true);
                

            }
            // check if cdr is not undefined
            else if (cdrIs_null == false)
            {
                // check if cdr is empty list.
                if (t.getCdr().isNull() && do_something__if_on_topmost_1st_cdr_side_of_mainLine == false)
                {                    
                    // add one  only for right paren that closes the list
                    Special.Indentation_cumulative += 1;
                }
                // check if cdr ends with not a typical  nil () value.
                else if(do_something__if_on_topmost_1st_cdr_side_of_mainLine == false)
                {
                    t.getCdr().print(0);
                    // add one  only for right paren  closing list
                    Special.Indentation_cumulative += 1;
                }

                if( do_something__if_on_topmost_1st_cdr_side_of_mainLine == false)
                   Console.Write(")");
                
            }

        }
    }
}

