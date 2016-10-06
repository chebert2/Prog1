// Quote -- Parse tree node strategy for printing the special form quote

using System;

namespace Tree
{
    public class Quote : Special
    {
        // TODO: Add any fields needed.
  
        // TODO: Add an appropriate constructor.
	public Quote() { }

        public override void print(Node t, int n, bool p)
        {
            // working implementation

            for(int i = 0; i<n; i++)
            {
                Console.Write(" ");
                n++;
            }
            
            Console.Write("'");
            n++;
            t.getCdr().print(n);
        }
    }
}

