// Lambda -- Parse tree node strategy for printing the special form lambda

using System;

namespace Tree
{
    public class Lambda : Special
    {
        // TODO: Add any fields needed.

        // TODO: Add an appropriate constructor.
	public Lambda() { }

        public override void print(Node t, int n, bool p)
        {
            // TODO: Implement this function.
	    for (int i = 0; i<n; i++)
	    {
	    	Console.Writeline(" ");
	    }
	    if(p == true)
	    	Console.Writeline("(");
    									
	    
  	}
    }
}
