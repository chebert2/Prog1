// Cond -- Parse tree node strategy for printing the special form cond

using System;

namespace Tree
{
    public class Cond : Special
    {
        // TODO: Add any fields needed.
        public int IndentationInt;
        public string IndentationString;

        // TODO: Add an appropriate constructor.
        public Cond()
        {
           
        }

        public override void print(Node t, int n, bool p)
        { 
            // TODO: Implement this function.
            Node car = t.getCar();
            Node cadr = t.getCdr().getCar();
            for(int i = 0; i < n; i++)
                Console.Writeline(" ");
            if  (p == true)
            {
                Console.Writeline("(");
            }
            car.print(n);
            if (t.getCdr().isPair())
            { 
                cadr.print(n, false);
                Console.Writeline();
            }
            else 
                t.getCdr().print(n,true):
            Console.Writeline(")");
            
        }
    }
}
