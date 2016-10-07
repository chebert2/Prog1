// Set -- Parse tree node strategy for printing the special form set!

using System;

namespace Tree
{
    public class Set : Special
    {
        // TODO: Add any fields needed.
 
        // TODO: Add an appropriate constructor.
	public Set() { }
	
        public override void print(Node t, int n, bool p)
        {
            // TODO: Implement this function.
	    if (p == true)
	    	Console.Writeline("(");
	    t.getCar().print(n);
	    if (t.getCar().isPair())
	    {
	    	t.getCar().print(n, true)
		Console.Writeline(")");
	    }
	    else
	    {
	    	t.getCdr().print(n, true);
		Console.Writeline(")");
	    }
        }
    }
}

