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
	    Node cadr = t.getCdr().getCar();
	    Node cddr = t.getCdr().getCdr();
	    for (int i = 0; i<n; i++)
	    {
	    	Console.Writeline(" ");
	    }
	    if(p == true)
	    	Console.Writeline("(");
            t.getCar().print(n);
	    Console.Writeline(" "):
	    
	    cadr.print(n, false);
	    n++;
	    
	    for(int i = 0; i < n; i++)
	    	Console.Writeline(" ");
           cddr.t.getCar().print(n, false);
	   n--;
	   
	   caddr.print(n, true)
	   Console.Writeline(")");
	   
	        									
	    
  	}
    }
}
