// Define -- Parse tree node strategy for printing the special form define

using System;

namespace Tree
{
    public class Define : Special
    {
        // TODO: Add any fields needed.


        // TODO: Add an appropriate constructor.
        public Define()
        {

        }

        public override void print(Node t, int n, bool p)
        {
           
            if (p == false)
            {
                Special.Indentation_cumulative++;
                Console.Write("(");
            }

            t.getCar().print(n);

            if (t.getCdr() != null)
            {
                Console.Write(" ");
                Special.Indentation_cumulative++;
                t.getCdr().print(n, true);
            }

         

        }
    }
}
