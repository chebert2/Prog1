// If -- Parse tree node strategy for printing the special form if

using System;

namespace Tree
{
    public class If : Special
    {
        // TODO: Add any fields needed.
        public int IndentationInt;
        public string IndentationString;

        // TODO: Add an appropriate constructor.
        public If()
        {
            
        }

        public override void print(Node t, int n, bool p)
        {
            // TODO: Implement this function
            
            Node cddr = t.getCdr().getCdr();
            Node cadr = t.getCdr().Car();
            if(p != null)
                Console.Writeline("(");
            t.getCar().print(n);
            cadr.print(0, false);
            
            while(cddr != null)
            {
                cddr.getCar().print(n);
                Console.Writeline();
                cddr = cddr.getCdr(); // may be incorrect, trying to progress through the remaining cddr
            }
            cddr.print(n);
            Console.Writeline(")");
    }
}
