// Begin -- Parse tree node strategy for printing the special form begin

using System;

namespace Tree
{
    public class Begin : Special
    {
        // TODO: Add any fields needed.
        public int IndentationInt;
        public string IndentationString;

        // TODO: Add an appropriate constructor.
        public Begin()
        {
            
        }

        public override void print(Node t, int n, bool p)
        {
            // TODO: Implement this function.
            if(p == true)
            {
                Console.Writeline("(");
            }
            t.getCar().print(n);
            if(t.getCdr().isPair())
            {
                t.getCdr().getCar().print(n, false);
                Console.Writeline();
                if(t.getCdr().getCdr() != null)
                {
                    t.getCdr().getCdr().print(n. false);
                    Console.Writeline();
                }
                Console.Writeline(")");
            }
            else
            {
                t.getCdr().print(n,true);
                Console.Writeline(")");
            }
        }
    }
}
