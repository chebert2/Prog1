// Special -- Parse tree node strategy for printing special forms

using System;

namespace Tree
{
    // There are several different approaches for how to implement the Special
    // hierarchy.  We'll discuss some of them in class.  The easiest solution
    // is to not add any fields and to use empty constructors.

    abstract public class Special
    {
        //public static bool quote_noParenthesis;

        public static int Indentation_cumulative;

        public static bool printing_quote_Contents;
        // last recent cons inspected  was itself a cdr
        public static bool last_cons_A_cdr = false;
        
        
        // whenever some clause left indenting item is encountered
        //
        // we will store the previous node tree indentation in 
        // at index i
        // in a reserve array for
        // reverting back to it when the new clause block is finished

        public abstract void print(Node t, int n, bool p);
    }
}

