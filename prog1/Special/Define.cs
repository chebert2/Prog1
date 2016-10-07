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
         Node cddr = t.getCdr().getCdr();
            if (p != false)
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
                Console.Writeline();
                
                //should this be outside of current if statement & does it need changing
                if(cddr != null)
                {
                    cddr.print(n,false); 
                    // Am I supposed to be incrementting n?
                }
            }
            else
            {
                for(int i = 0; i < n; i++)
                    Console.Writeline(" ");
                t.getCar().print(n);
                Console.Writeline();
             }
            Console.Writeline(")");
            
        }
    }
}


