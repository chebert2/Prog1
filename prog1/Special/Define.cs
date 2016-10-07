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
         Node cadr = t.getCdr().getCar();
            if (p == false)
                Console.WriteLine("(");
            t.getCar().print(n);
            if (t.getCdr() == null)
            {
                Console.WriteLine(")");
            }

            if (cadr.isPair())
            {
                Console.WriteLine(" ");
                cadr.print(n, false);
            }
            
        }
    }
}


