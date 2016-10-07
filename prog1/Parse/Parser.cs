// Parser -- the parser for the Scheme printer and interpreter
//
// Defines
//
//   class Parser;
//
// Parses the language
//
//   exp  ->  ( rest
//         |  #f
//         |  #t
//         |  ' exp
//         |  integer_constant
//         |  string_constant
//         |  identifier
//    rest -> )
//         |  exp+ [. exp] )
//
// and builds a parse tree.  Lists of the form (rest) are further
// `parsed' into regular lists and special forms in the constructor
// for the parse tree node class Cons.  See Cons.parseList() for
// more information.
//
// The parser is implemented as an LL(0) recursive descent parser.
// I.e., parseExp() expects that the first token of an exp has not
// been read yet.  If parseRest() reads the first token of an exp
// before calling parseExp(), that token must be put back so that
// it can be reread by parseExp() or an alternative version of
// parseExp() must be called.
//
// If EOF is reached (i.e., if the scanner returns a NULL) token,
// the parser returns a NULL tree.  In case of a parse error, the
// parser discards the offending token (which probably was a DOT
// or an RPAREN) and attempts to continue parsing with the next token.

using System;
using Tokens;
using Tree;

namespace Parse
{
    public class Parser
    {
        
        private Scanner scanner;

        public static Nil nil_object = new Nil();

        private static Token currentToken1;
        private static Token currentToken2;

        public static int number_of_Left_parentheses_extra_over_zero = 0;

        public static bool firstRun;

        public static bool stillReadFirstParenthesis;
        // this to keep track of parenthesis around a dotted   (expression . () )
        public static bool tail_item_A_list = false;

        public Parser(Scanner s)
        {
            currentToken1 = null;
            currentToken2 = null;
            //currentNode_ofToken1 = null;
            scanner = s;

            firstRun = true;
            stillReadFirstParenthesis = true;
        }

        public Node parseExp()
        {
            // TODO: write code for parsing an exp

            

            Node returnNode = null;

            //bool keepGettingData = true;

            if (firstRun)
            {
                currentToken1 = this.scanner.getNextToken();
                firstRun = false;
            }
            else
                currentToken1 = currentToken2;
            // special case of single quote
            // construct expression (quote rest) given current first token was ' char 
            // and we redirected a Lparenthesis instead for preceeding currentToken1 instead of a ' char
            // in order to start (quote
            if (scanner.quoteMark_engaged == true)
            {
                currentToken2 = new Token(TokenType.QUOTE);

                // remove char '    advance of QUOTE
                scanner.quoteMark_engaged = false;
            }

            // ignore above if test! 
            //for
            // Conventional normal run
            else
            {
                if (Parser.number_of_Left_parentheses_extra_over_zero != 0 || stillReadFirstParenthesis)
                {

                    currentToken2 = this.scanner.getNextToken();
                    stillReadFirstParenthesis = false;
                }
                // do not read another token because input end is now there.  
                else
                {
                    // a pointless declaration to currentToken2
                    // _ for end of total parsed expression...
                       // we just don't want currentToken2 to be null
                    currentToken2 = new Token(TokenType.LPAREN);
                    if (Scanner.flag_debugger)
                        Console.Write("expression complete: Not reading additional tokens in this line.");
                }
            }


            if (currentToken1 == null)
                return null;

            if (currentToken1.getType() == TokenType.LPAREN)
            {
                number_of_Left_parentheses_extra_over_zero++;

                
                if (Scanner.flag_debugger)
                    Console.WriteLine("LPAREN");

                returnNode = parseRest();

                // define node in static variable for future reference !... after a unit has parsed a leftSide list !
                //currentNode_ofToken1 = returnNode;

                return returnNode;
            }
            // also need to convert '(list)
            else if (currentToken1.getType() == TokenType.FALSE)
            {
                returnNode = new BoolLit(false);
                // define node in static variable for future reference !
                //currentNode_ofToken1 = returnNode;

                if (Scanner.flag_debugger)
                    Console.WriteLine("FALSE");

                return returnNode;
            }
            else if (currentToken1.getType() == TokenType.TRUE)
            {
                returnNode = new BoolLit(true);

                // define node in static variable for future reference !
                //currentNode_ofToken1 = returnNode;

                if (Scanner.flag_debugger)
                    Console.WriteLine("TRUE");

                return returnNode;
            }
            else if (currentToken1.getType() == TokenType.INT)
            {
                returnNode = new IntLit(currentToken1.getIntVal());

                // define node in static variable for future reference !
                //currentNode_ofToken1 = returnNode;

                if (Scanner.flag_debugger)
                    Console.WriteLine("INT, intVAL =" + currentToken1.getIntVal());

                return returnNode;
            }
            else if (currentToken1.getType() == TokenType.STRING)
            {
                returnNode = new StringLit(currentToken1.getStringVal());

                // define node in static variable for future reference !
                //currentNode_ofToken1 = returnNode;

                if (Scanner.flag_debugger)
                    Console.WriteLine("STRING, stringVal =" + currentToken1.getStringVal());

                return returnNode;
            }
            else if (currentToken1.getType() == TokenType.QUOTE)
            {
                if (Scanner.flag_debugger)
                    Console.WriteLine("IDENT, name = quote");

                return new Ident("quote");

                // define node in static variable for future reference !
                //currentNode_ofToken1 = returnNode;

            }
            else if (currentToken1.getType() == TokenType.IDENT)
            {
                returnNode = new Ident(currentToken1.getName());

                // define node in static variable for future reference !
                //currentNode_ofToken1 = returnNode;

                if (Scanner.flag_debugger)
                    Console.WriteLine("IDENT, name = " + currentToken1.getName());

                return returnNode;
            }

            else
            {
                Console.Error.WriteLine("Error: Token is unidentifiable in our grammar.");
                return null;
            }


        }

        protected Node parseRest()
        {
            
            Node endReturnNode = null;

            // TODO: write code for parsing a rest
            if (currentToken2 == null)
            {
                Console.Error.WriteLine("stops at start of expression_ Or while exceeded balance of LeftParentheses. Not a complete statement.");
                return null;
            }
            if (currentToken2.getType() == TokenType.RPAREN)
            {
                if (Scanner.flag_debugger)
                    Console.WriteLine("RPAREN");
                endReturnNode = nil_object;
                // prevent Rest from repeating a nil
                currentToken1 = currentToken2;

                

                Parser.number_of_Left_parentheses_extra_over_zero--;

                if(Parser.number_of_Left_parentheses_extra_over_zero != 0)
                  currentToken2 = this.scanner.getNextToken();

            }
            // work on  in parseRest() Like handling single following right ')'
            else if (currentToken2.getType() == TokenType.DOT)
            {
                // declare return value of proper S-expression with DOT
                //Cons consReturn = null;

                // find parse error of expression:  ( . ) = ( error .    so on ....
                if (currentToken1.getType() == TokenType.LPAREN)
                {
                    // print that there was parser error
                    
                    Console.Error.WriteLine("error: Dot has no car __  preceeded by LParen.");
                    currentToken1 = currentToken2;

                    currentToken2 = this.scanner.getNextToken();
                    
                    return parseRest();
                    //ends if that happens
                }

                currentToken1 = currentToken2;

                currentToken2 = this.scanner.getNextToken();

                if (currentToken2 != null)
                {
                    if (currentToken2.getType() == TokenType.INT)
                    {
                        endReturnNode = new IntLit(currentToken2.getIntVal());
                        if (Scanner.flag_debugger)
                            Console.WriteLine("INT, intVAL =" + currentToken2.getIntVal());
                    }
                    else if (currentToken2.getType() == TokenType.STRING)
                    {
                        endReturnNode = new StringLit(currentToken2.getStringVal());
                        if (Scanner.flag_debugger)
                            Console.WriteLine("STRING, stringVal =" + currentToken2.getStringVal());
                    }
                    else if (currentToken2.getType() == TokenType.TRUE)
                    {
                        endReturnNode = new BoolLit(true);
                        if (Scanner.flag_debugger)
                            Console.WriteLine("TRUE");
                    }
                    else if (currentToken2.getType() == TokenType.FALSE)
                    {
                        endReturnNode = new BoolLit(false);
                        if (Scanner.flag_debugger)
                            Console.WriteLine("FALSE");
                    }
                    else if (currentToken2.getType() == TokenType.IDENT)
                    {
                        endReturnNode = new Ident(currentToken2.getName());
                        if (Scanner.flag_debugger)
                            Console.WriteLine("Ident, name =" + currentToken2.getName());
                    }
                    else
                    {
                        Parser.tail_item_A_list = true;
                        return parseExp();


                    }


                    // check if there is correct grammar for a tail node with it's sole_literal value
                    //   thus: make sure there are not multiple values after tail.
                    bool additional_ReadMore = true;
                    // this is turned to true when a proper following Rparenth comes like . 2')'
                    bool flip_Past_RightParenthesis_after_tail = false;
                    // as closing touch... this will try to get 
                    // token beyond the next RightParenthesis

                    //  continues iterating on ... if Parsing ERRORS.
                    while (additional_ReadMore)
                    {


                        if (currentToken2 != null)
                        {

                            currentToken1 = currentToken2;

                            currentToken2 = this.scanner.getNextToken();

                            // exit criteria   for proper tail and right parenthesis being correct;
                            if (flip_Past_RightParenthesis_after_tail)
                            {
                                additional_ReadMore = false;
                                return endReturnNode;
                            }
                            if (currentToken2 == null)
                            {
                                additional_ReadMore = false;
                                Console.Error.WriteLine("ends abruptly after dot_ . in list with tail items.");
                                return null;
                                //break;
                            }
                            else if (currentToken2.getType() == TokenType.RPAREN)
                            {
                                flip_Past_RightParenthesis_after_tail = true;
                                continue;
                            }
                            // examine if dot tail item ends appropriately.
                            else
                            {
                                if (currentToken2.getType() == TokenType.INT ||
                                  currentToken2.getType() == TokenType.STRING ||
                                  currentToken2.getType() == TokenType.TRUE ||
                                  currentToken2.getType() == TokenType.FALSE)
                                {
                                    if (Scanner.flag_debugger)
                                        Console.WriteLine("error : cannot have any more than one literal for dot non-list tail.");
                                    continue;
                                }
                                else
                                {
                                    if (Scanner.flag_debugger)
                                        Console.WriteLine("error : cannot have any identifiers or lists in non-list tail.");
                                    continue;
                                }

                            }

                        }
                        else
                            additional_ReadMore = false;

                    }

                }
                else
                {
                    Console.Error.WriteLine("Ends early with Dot [cons/] element");
                    return null;
                }
                    

                endReturnNode = null;
                return endReturnNode;
            }
            // this does any further expansion of the parse tree  in any Cons created
            else
            {
                Node oneExpression = parseExp();
                Node rest = parseRest();
                if (oneExpression == null)
                    return null;
                else if (rest == null)
                    return null;
                endReturnNode = new Cons(oneExpression , rest);
                // print out the last parenthesis   in a cons with a dot
                if (Parser.tail_item_A_list)
                {
                    if (Scanner.flag_debugger)
                        Console.WriteLine("RPAREN");
                    // turn it back off now.
                    Parser.tail_item_A_list = false;
                }

            }



            return endReturnNode;
        }

        // TODO: Add any additional methods you might need.
    }
}

