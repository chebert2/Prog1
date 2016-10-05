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

        public Nil nil_object = new Nil();

        private static Token currentToken1;
        private static Token currentToken2;

        //private static Node currentNode_ofToken1;

        private static bool firstRun;

        public Parser(Scanner s)
        {
            currentToken1 = null;
            currentToken2 = null;
            //currentNode_ofToken1 = null;
            scanner = s;

            firstRun = true;
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

            currentToken2 = this.scanner.getNextToken();

            if (currentToken1 == null)
                return null;

            if (currentToken1.getType() == TokenType.LPAREN)
            {
                returnNode = parseRest();

                Console.WriteLine(" parseRest Needed...");

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

                return returnNode;
            }
            else if (currentToken1.getType() == TokenType.TRUE)
            {
                returnNode = new BoolLit(true);

                // define node in static variable for future reference !
                //currentNode_ofToken1 = returnNode;

                return returnNode;
            }
            else if (currentToken1.getType() == TokenType.INT)
            {
                returnNode = new IntLit(currentToken1.getIntVal());

                // define node in static variable for future reference !
                //currentNode_ofToken1 = returnNode;

                return returnNode;
            }
            else if (currentToken1.getType() == TokenType.STRING)
            {
                returnNode = new StringLit(currentToken1.getStringVal());

                // define node in static variable for future reference !
                //currentNode_ofToken1 = returnNode;

                return returnNode;
            }


            else if (currentToken1.getType() == TokenType.QUOTE)
            {
                returnNode = new Cons(new Ident("Quote"), parseExp());

                // define node in static variable for future reference !
                //currentNode_ofToken1 = returnNode;

                return returnNode;
            }
            else if (currentToken1.getType() == TokenType.IDENT)
            {
                returnNode = new Ident(currentToken1.getName());

                // define node in static variable for future reference !
                //currentNode_ofToken1 = returnNode;

                return returnNode;
            }

            else
                throw new Exception("Error: Token is null");


        }

        protected Node parseRest()
        {
            Node endReturnNode = null;

            // TODO: write code for parsing a rest
            if (currentToken2 == null)
                throw new Exception("stops at start of expression_ Or while exceeded balance of LeftParentheses. Not a complete statement.");


            if (currentToken2.getType() == TokenType.RPAREN)
            {

                endReturnNode = nil_object;
                // prevent Rest from repeating a nil
                currentToken1 = currentToken2;

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
                    Console.WriteLine("error: Dot has no car __  preceeded by LParen.");
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
                        endReturnNode = new IntLit(currentToken2.getIntVal());
                    else if (currentToken2.getType() == TokenType.STRING)
                        endReturnNode = new StringLit(currentToken2.getStringVal());
                    else if (currentToken2.getType() == TokenType.TRUE)
                        endReturnNode = new BoolLit(true);
                    else if (currentToken2.getType() == TokenType.FALSE)
                        endReturnNode = new BoolLit(false);
                    else if (currentToken2.getType() == TokenType.IDENT)
                        endReturnNode = new Ident(currentToken2.getName());
                    else
                        return parseExp();


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
                                throw new Exception("ends abruptly afterr dot_ . in list with tail items.");
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
                                    Console.WriteLine("error : cannot have any more than one literal for dot non-list tail.");
                                    continue;
                                }
                                else
                                {
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
                    throw new Exception("Ends early with Dot [cons/] element");

                endReturnNode = null;
                return endReturnNode;
            }
            // this does any further expansion of the parse tree  in any Cons created
            else
            {
                endReturnNode = new Cons(parseExp(), parseRest());
            }



            return endReturnNode;
        }

        // TODO: Add any additional methods you might need.
    }
}

