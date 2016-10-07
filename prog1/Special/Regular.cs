// Regular -- Parse tree node strategy for printing regular lists

using System;
using Parse;


namespace Tree
{
    public class Regular : Special
    {
        // TODO: Add any fields needed.
        
        public int local_indent;
        public static bool current_Expression_NotEnded;
        public static bool boolean_first_run;
        public static bool lastCall_was_to_cdr;
        // TODO: Add an appropriate constructor.
        public Regular()
        {
            local_indent = 0;
            
        }

        public override int print(Node t, int n, bool p)
        {
            
            // TODO: Implement this function.
            // working implementation

            if(boolean_first_run)
            {
                current_Expression_NotEnded = true;
                boolean_first_run = false;
            }

            local_indent = n;

            if (!t.getCdr().isNull() && Regular.lastCall_was_to_cdr)
                current_Expression_NotEnded = true;
            else
                current_Expression_NotEnded = false;


            if (p == false)
            {
                
                if(current_Expression_NotEnded || Regular.lastCall_was_to_cdr)
                    Special.localExpression_ended_case = false;
                else
                    Console.Write("(");
                p = true;
                local_indent = n + 1;
            }

            // print single car item // __ if it is not itself regular expression
            if (!t.getCar().isPair())
            {
                if (t.getCar().isBool())
                {
                    BoolLit boolNode = (BoolLit) t.getCar();
                    local_indent += 2;
                }
                else if(t.getCar().isSymbol())
                {
                    Ident identNode = (Ident) t.getCar();
                    local_indent += identNode.print(0);
                }
                else if (t.getCar().isNumber())
                {
                    IntLit intNode = (IntLit) t.getCar();
                    local_indent += intNode.print(0);
                }
                else if (t.getCar().isNull())
                {
                    Parser.nil_object.print(0);
                    local_indent += 2;
                }
                else if (t.getCar().isString())
                {
                    StringLit stringNode = (StringLit) t.getCar();
                    local_indent += stringNode.print(0);
                }
            }
            else
            {
                Regular.lastCall_was_to_cdr = false;
                local_indent += t.getCar().print(0);

             }

            // check if cdr is a nil.
            if (t.getCdr().isNull()) {
                Console.Write(")");


                // terminate if node is nil.
                Special.localExpression_ended_case = true;

                local_indent += 1;

            }
            // check if cdr is a single tail non-nil constant
            else if (t.getCdr().isBool() )
            {
                Console.Write(" . ");
                local_indent += 3;
                t.getCdr().print(0);
                //needs fixing
                local_indent += 2;
                Console.Write(")");
                local_indent++;
                p = false;
            }
            else if (t.getCdr().isNumber() )
            {
                Console.Write(" . ");
                local_indent += 3;
                IntLit intNodeHere = (IntLit) t.getCdr();
               
                local_indent += intNodeHere.print(0);
                Console.Write(")");
                local_indent++;
                p = false;
            }
            else if (t.getCdr().isSymbol() )
            {
                Console.Write(" . ");
                local_indent += 3;
                Ident identNodeHere = (Ident) t.getCdr();

                local_indent += identNodeHere.print(0);
                Console.Write(")");
                local_indent++;
                p = false;
            }
            else if (t.getCdr().isString())
            {
                Console.Write(" . ");
                local_indent += 3;
                StringLit stringLitNodeHere = (StringLit)t.getCdr();

                local_indent += stringLitNodeHere.print(0);
                Console.Write(")");
                local_indent++;
                p = false;
            }
            
            else
            {
                Console.Write(" ");
                local_indent++;
                // the only possible thing here could be a extended cons node
                if (t.getCdr().isPair())
                {
                    Regular.lastCall_was_to_cdr = true;


                    current_Expression_NotEnded = true;
                    if (!t.getCdr().getCar().isPair())
                        local_indent += t.getCdr().print(0, true);
                    else
                        local_indent += t.getCdr().print(0, false);
                }
                else
                    throw new Exception("unknown Node for tail. Cannot print.");
                
            }
            return local_indent;
        }
    }
}


