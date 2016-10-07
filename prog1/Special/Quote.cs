// Quote -- Parse tree node strategy for printing the special form quote

using System;

namespace Tree
{
    public class Quote : Special
    {
        // TODO: Add any fields needed.
  
        // TODO: Add an appropriate constructor.
	public Quote() { }

        public override int print(Node t, int n, bool p)
        {
            // working implementation

            for(int i = 0; i<n; i++)
            {
                Console.Write(" ");
                n++;
            }
            
            Console.Write("'");

            if (t.getCdr().isPair())
                if (t.getCdr().getCdr().isNull())
                    return 1 + t.getCdr().getCar().print(0, false);
                else
                    return 1 + t.getCdr().print(0, false);
            else
            {
                Console.Error.WriteLine("quote has no parameter.");
                return 0;
            }
                
        }
    }
}

