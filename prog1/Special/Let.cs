// Let -- Parse tree node strategy for printing the special form let

using System;

namespace Tree
{
    public class Let : Special
    {
        // TODO: Add any fields needed.
 
        // TODO: Add an appropriate constructor.
	public Let() { }

        public override void print(Node t, int n, bool p)
        {
            // TODO: Implement this function.
	    Node cadr = t.getCdr().getCar();
	    Node cddr = t.getCdr().getCdr();
	    Console.Writeline("(");
	    t.getCar().print(n, true);
	    Console.Writeline();
	    n++;
	    
	    // i want to change form to regular but don't know how
	    
	    while(t.getCdr() != null)
	    {
	    	cadr.print(n, false);
		Console.Writeline();
		t.getCdr() = cddr;
            }
	    t.getCdr().print(n, true);
	    Console.Writeline();
        }
    }
}


