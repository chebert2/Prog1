// Cons -- Parse tree node class for representing a Cons node

using System;

namespace Tree
{
    public class Cons : Node
    {
        private Node car;
        private Node cdr;
        private Special form;

        public Cons(Node a, Node d)
        {
            car = a;
            cdr = d;
            parseList();
        }

        // parseList() `parses' special forms, constructs an appropriate
        // object of a subclass of Special, and stores a pointer to that
        // object in variable form.  It would be possible to fully parse
        // special forms at this point.  Since this causes complications
        // when using (incorrect) programs as data, it is easiest to let
        // parseList only look at the car for selecting the appropriate
        // object from the Special hierarchy and to leave the rest of
        // parsing up to the interpreter.
        void parseList()
        {
            
            if (car.isSymbol())
            {
                Ident a = (Ident)car;

                string type = a.getSymbol();
                if (type.Equals("quote"))
                    form = new Quote();
                //else if (type.Equals("if"))
                //    form = new If();
                //else if (type.Equals("begin"))
                //    form = new Begin();
                //else if (type.Equals("cond"))
                //    form = new Cond();
                //else if (type.Equals("define"))
                //    form = new Define();
                //else if (type.Equals("lambda"))
                //    form = new Lambda();
                //else if (type.Equals("let"))
                //    form = new Let();
                //else if (type.Equals("set!"))
                //    form = new Set();
                else
                    form = new Regular();
            }
            else
                form = new Regular();
        }

        public override void print(int n)
        {
            form.print(this, n, false);
        }

        public override void print(int n, bool p)
        {
            form.print(this, n, p);
        }
        public override bool isPair() { return true; }

        // TODO: Report an error in these default methods and implement them
        // in class Cons.  After setCar, a Cons cell needs to be `parsed' again
        // using parseList.
        public override Node getCar()
        {
            return car;
        }

        public override Node getCdr()
        {
            return cdr;
        }

        public override void setCar(Node a)
        {
            car = a;
        }
        public override void setCdr(Node d)
        {
            cdr = d;

        }
    }
}
