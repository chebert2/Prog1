// Regular -- Parse tree node strategy for printing regular lists

using System;
using Parse;

namespace Tree
{
    public class Regular : Special
    {
        // TODO: Add any fields needed.
        //bool LParenthesis_absent;

        // TODO: Add an appropriate constructor.
        public Regular() { }

        public override void print(Node t, int n, bool p)
        {
            // TODO: Implement this function.
            // working implementation
            
            
            if (p == false)
            {
                Console.Write("(");
                p = true;
                n++;
            }
            // print single car item // __ if it is not itself regular expression
            if (!t.getCar().isPair())
            {
                if (t.getCar().isBool())
                {
                    BoolLit boolNode = (BoolLit) t.getCar();
                    boolNode.print(0);
                    n += 2;
                }
                else if(t.getCar().isSymbol())
                {
                    Ident identNode = (Ident) t.getCar();
                    identNode.print(0);
                    n += identNode.getSymbol().Length;
                }
                else if (t.getCar().isNumber())
                {
                    IntLit intNode = (IntLit) t.getCar();
                    intNode.print(0);
                    n += intNode.getInt().ToString().Length;
                }
                else if (t.getCar().isNull())
                {
                    Parser.nil_object.print(0);
                    n += 2;
                }
                else if (t.getCar().isString())
                {
                    StringLit stringNode = (StringLit) t.getCar();
                    stringNode.print(0);
                    n += stringNode.getString().Length;
                }
            }
            else
            {
                t.getCar().print(n);
            }

            // check if cdr is a nil.
            if (t.getCdr().isNull()) {
                Console.Write(")");
                // terminate if node is nil.
                p = false;
                n++;

            }
            // check if cdr is a single tail non-nil constant
            else if (t.getCdr().isBool() )
            {
                Console.Write(" . ");
                n += 3;
                t.getCdr().print(0);
                //needs fixing
                n += 2;
                Console.Write(")");
                n++;
                p = false;
            }
            else if (t.getCdr().isNumber() )
            {
                Console.Write(" . ");
                n += 3;
                IntLit intNodeHere = (IntLit) t.getCdr();
                intNodeHere.print(0);
                //needs fixing
                n += intNodeHere.ToString().Length;
                Console.Write(")");
                n++;
                p = false;
            }
            else if (t.getCdr().isSymbol() )
            {
                Console.Write(" . ");
                n += 3;
                Ident identNodeHere = (Ident) t.getCdr();
                identNodeHere.print(0);
                //needs fixing
                n += identNodeHere.getSymbol().Length;
                Console.Write(")");
                n++;
                p = false;
            }
            else if (t.getCdr().isString())
            {
                Console.Write(" . ");
                n += 3;
                StringLit stringLitNodeHere = (StringLit)t.getCdr();
                stringLitNodeHere.print(0);
                //needs fixing
                n += stringLitNodeHere.ToString().Length;
                Console.Write(")");
                n++;
                p = false;
            }
            
            else
            {
                Console.Write(" ");
                n++;
                t.getCdr().print(n, p);
            }
           
        }
    }
}


