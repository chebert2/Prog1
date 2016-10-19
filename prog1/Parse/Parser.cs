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

       // public static bool quote_mark__needs_another_right_parenthesis_later_on = false;
       // public static bool wait_till_quote_element_read__for_flag_RPAREN = false;

        public static bool special_type_Has_to_have_non_nil_cdr;
        public static String last_itertion_Identifier;
        public static int lastRunHad_Special_id;
        public static bool start_addit_Ident_needing_End_expression_tail;

        public static bool firstRun;

        public static bool stillReadFirstParenthesis;

        // public static bool need_to_insert_right_parenthesis;

        public static bool flip_Past_RightParenthesis_after_tail;

        public static bool quote_mark_misc_to_placed_cursor__is_not_new_data;
        // if the quote mark that is placed as left parenthesis
        //   is C , it means it is in currentToken2  , B currentToken1, or A not in any anymore.
        public static char quote_mark__misc_LPAREN__0___2;

        // this to keep track of parenthesis around a dotted   (expression . () )
        public static bool tail_item_A_list = false;
        public static Token pushedBack_extraToken_fromQuoteMark_;
       // public static bool no_RPAREN__next_iteration_after_tail_with_dot;
        public static bool quote_extension_going = false;
        public static bool emptyTerm;

        public static bool runningStarted_ForSomething;
        // something that will flip as an accessory to a tri chain of of printing right parenthesis mark
        public static bool condition_in_dotted_exp;

        public static int cancel_balance_number_Left_Paren__DottedExp;
        
        

        public Parser(Scanner s)
        {
            currentToken1 = null;
            currentToken2 = null;
            //currentNode_ofToken1 = null;
            scanner = s;

            firstRun = true;
            stillReadFirstParenthesis = false;
        }

        public Node parseExp()
        {
            // TODO: write code for parsing an exp


            Node returnNode = null;

            Token TokenPeekedAt = null;
            bool quote_mark_succeeded_incrementing_tokens = false;

           
            bool toggleIf_inputVaries = false;
            // this is assigned later
            // so declaration does not matter
            bool newTrouble = true;
            bool trouble;

            /**
            if (currentToken1 != null && currentToken1.getType() == TokenType.IDENT)
                if (currentToken1.getName() == "cards")
                    //Console.WriteLine("right");
            **/
            if ( !stillReadFirstParenthesis    && 
                
                pushedBack_extraToken_fromQuoteMark_ == null  )

                quote_extension_going = false;

            


            if (firstRun)
            {
                currentToken1 = this.scanner.getNextToken();

                //if (Parser.quote_mark_misc_to_placed_cursor__is_not_new_data == true)
                // assigned to index 1  like 0 1 2  A B C  for currenttoken
                //Parser.quote_mark__misc_LPAREN__0___2 = 'B';

                firstRun = false;
                runningStarted_ForSomething = true;

                // start delving into a quote mark extension from '   to   (quote    so on later.... expres
                if (scanner.quoteMark_engaged)
                {

                    
                    // set up our accessing current read token feed to delegate onward
                    currentToken1 = new Token(TokenType.LPAREN);
                    currentToken2 = new Token(TokenType.QUOTE);


                    if (Parser.quote_mark_misc_to_placed_cursor__is_not_new_data)
                    {
                        // assigned to index 1  like 0 1 2  A B C  for currenttoken
                        // 'A' means that the quote mark no longer is in current tokens
                        Parser.quote_mark__misc_LPAREN__0___2 = 'A';
                        Parser.quote_mark_misc_to_placed_cursor__is_not_new_data = false;
                    }

                    scanner.quoteMark_engaged = false;
                    Parser.quote_extension_going = true;

                }

            }
            
            // dealing with extension of a quote mark to (quote expre)
            else if (stillReadFirstParenthesis)
            {


                if (pushedBack_extraToken_fromQuoteMark_ == null)
                {
                    // for assignments of tokens ... skip all below steps  
                    // out further beyond of this block section

                    // normal transfer of token 2 to token 1  after the following



                    // normal transfer of token 2 to token 1 

                    pushedBack_extraToken_fromQuoteMark_ = scanner.getNextToken();
                    
                }





                if (pushedBack_extraToken_fromQuoteMark_ == null)
                {
                    Console.Error.WriteLine("ran out of input to complete expression.");
                    return null;
                }

                else if (pushedBack_extraToken_fromQuoteMark_.getType() == TokenType.LPAREN)
                {
                    Token quickLookPeek = null;

                    if (scanner.ifPast_lookWasPeek)
                        quickLookPeek = scanner.getLast_item_peeked_at();
                    else
                        quickLookPeek = scanner.peekAtNextToken();

                    if (quickLookPeek.getType() == TokenType.RPAREN)
                    {
                        returnNode = nil_object;
                        // only do following if more of phrase to be absorbed.
                        if(Parser.number_of_Left_parentheses_extra_over_zero != 0)
                        {
                            // increment now  to get it over with
                            scanner.getNextToken();
                            // now get the next token for the circle iteration
                            currentToken2 = scanner.getNextToken();
                        }
                        

                        // we can go on continuing quote as usual in this iteration of parseExp();
                        Parser.quote_extension_going = false;

                        // algorithm, no longer need this.  need_to_insert_right_parenthesis = true;
                        // change due to removal of current token 2 below
                        if (Parser.quote_mark_misc_to_placed_cursor__is_not_new_data &&
                            Parser.quote_mark__misc_LPAREN__0___2 == 'C')
                        {
                            // assigned to index 1  like 0 1 2  A B C  for currenttoken// assigned to index 1  like 0 1 2  A B C  for currenttoken
                            Parser.quote_mark__misc_LPAREN__0___2 = 'A';
                            Parser.quote_mark_misc_to_placed_cursor__is_not_new_data = false;

                        }
                  

                        stillReadFirstParenthesis = false;
                        return returnNode;
                    }
                    else
                    {
                        // change due to removal of current token 2 below
                        if (Parser.quote_mark_misc_to_placed_cursor__is_not_new_data &&
                            Parser.quote_mark__misc_LPAREN__0___2 == 'B')
                        {
                            // assigned to index 1  like 0 1 2  A B C  for currenttoken// assigned to index 1  like 0 1 2  A B C  for currenttoken
                            Parser.quote_mark__misc_LPAREN__0___2 = 'A';
                            Parser.quote_mark_misc_to_placed_cursor__is_not_new_data = false;
                        }
                        // reassign this to one that will be used at moment on in code following
                        //currentToken1 = new Token(TokenType.LPAREN);
                        // same as next declaration
                        currentToken1 = pushedBack_extraToken_fromQuoteMark_;
                        
                        currentToken2 = scanner.getNextToken();
                        // do not continue quote as usual in this iteration of parseExp();
                       //so  Parser.quote_extension_going    stays  true;

                        number_of_Left_parentheses_extra_over_zero++;
                        // algorithm, no longer need it. Parser.still_need_to_put_in_RPAREN__next_iteration = true;
                        stillReadFirstParenthesis = false;
                        // for assignments of tokens ... skip all below steps  
                        // out further beyond of this block section
                        emptyTerm = false;
                    }
                }

                else
                {
                    // we know in this clause, that quote mark would have remained at token 2 
                    //in more unusual conditions
                    if (Parser.quote_mark_misc_to_placed_cursor__is_not_new_data   &&
                        Parser.quote_mark__misc_LPAREN__0___2 == 'B'
                        )
                    {
                        // assigned to index 1  like 0 1 2  A B C  for currenttoken
                        // 'A' means that the quote mark no longer is in current tokens
                        Parser.quote_mark__misc_LPAREN__0___2 = 'A';
                        Parser.quote_mark_misc_to_placed_cursor__is_not_new_data = false;
                    }

                    currentToken1 = pushedBack_extraToken_fromQuoteMark_;

                    // only do following if more of phrase to be absorbed.
                    if (Parser.number_of_Left_parentheses_extra_over_zero > 1 )
                    {
                        currentToken2 = scanner.getNextToken();
                    }
                    
                    // turn off  parseExp();
                    Parser.quote_extension_going = false;
                    Parser.pushedBack_extraToken_fromQuoteMark_ = null;

                    stillReadFirstParenthesis = false;
                    // algorithm, no longer need this.  need_to_insert_right_parenthesis = true;
                    // for assignments of tokens ... skip all below steps  
                    // out further beyond of this block section
                    emptyTerm = true;
                }
            }
            //  normal transfer of token 2 to token 1   at next  :  else {  currentToken1 = currentToken2;} below
            else if (!firstRun && !emptyTerm)
            {
                // only read if new token2 is not erroneous quote  LPAREN symbol
                if(Parser.quote_mark__misc_LPAREN__0___2 != 'C')
                    currentToken1 = currentToken2;

                // wipe out remains of currentToken 1 if it was a quote erroneous  LPAREN for ' mark
                if (Parser.quote_mark__misc_LPAREN__0___2 == 'B'
                                  && Parser.quote_mark_misc_to_placed_cursor__is_not_new_data)
                {
                    Parser.quote_mark__misc_LPAREN__0___2 = 'A';
                    Parser.quote_mark_misc_to_placed_cursor__is_not_new_data = false;

                }
               



            }







            // ignore above if test! 
            //when given following (not first)
            // Conventional normal run 

            if ( (Parser.number_of_Left_parentheses_extra_over_zero != 0 || 
                runningStarted_ForSomething == true)  && !emptyTerm)
            {


                // special case of single quote
                // construct expression (quote rest) given current first token was ' char 
                // and we redirected a Lparenthesis instead for preceeding currentToken1 instead of a ' char
                // in order to start (quote
                if (scanner.quoteMark_engaged == true && runningStarted_ForSomething == true)
                {
                    // reset this  to new value, given where things occured
                    // algorithm, no longer need this.  need_to_insert_right_parenthesis = false;

                    if (currentToken2 != null && Parser.quote_mark__misc_LPAREN__0___2 != 'C')
                    {
                        newTrouble = false;
                    }
                        
                    if (!newTrouble && currentToken2.getType() != TokenType.RPAREN)
                    {
                        quote_mark_succeeded_incrementing_tokens = true;
                        // look at troubled case  of   whether currentToken2   is a residue 
                        // of erroneous quote symbol LPAREN
                        if (Parser.quote_mark_misc_to_placed_cursor__is_not_new_data &&
                            Parser.quote_mark__misc_LPAREN__0___2 == 'C')
                       // the value here does not matter
                      // the only outcome though here, is that pushedBack_extraToken_fromQuoteMark == null now.
                            trouble = true;
                        // a good proceeding of currentToken2
                        else
                            pushedBack_extraToken_fromQuoteMark_ = currentToken2;
                    }
                    else
                    {
                        TokenPeekedAt = scanner.peekAtNextToken();

                        if (TokenPeekedAt != null)
                        {
                            if (TokenPeekedAt.getType() != TokenType.RPAREN)
                            {
                                quote_mark_succeeded_incrementing_tokens = true;

                                pushedBack_extraToken_fromQuoteMark_ = scanner.getNextToken();
                            }
                            else
                                do
                                {
                                    Console.Error.WriteLine(" ) _right parenthesis cannot appear right after quote");
                                    TokenPeekedAt = scanner.peekAtNextToken();
                                    if (TokenPeekedAt != null)
                                    {
                                        break;
                                    }
                                    if (TokenPeekedAt.getType() != TokenType.RPAREN)
                                    {
                                        quote_mark_succeeded_incrementing_tokens = true;
                                        pushedBack_extraToken_fromQuoteMark_ = scanner.getNextToken();
                                    }
                                }
                                while (!quote_mark_succeeded_incrementing_tokens);
                            if (TokenPeekedAt == null)
                            {
                                quote_mark_succeeded_incrementing_tokens = false;
                                Console.Error.WriteLine("Current token a ' err) and EOF reached: not a complete quotation expression.");
                                return null;
                            }
                            else
                                quote_mark_succeeded_incrementing_tokens = true;

                        }
                        else
                        {
                            Console.Error.WriteLine("EOF cannot go Further checking faulty incomplete expression (quote rightpar)");
                            return null;
                        }
                    }
                    // this is done to wipe out left over erroneous quote token
               // done because of code below... after this if block
                    if (Parser.quote_mark_misc_to_placed_cursor__is_not_new_data)
                    {
                        // assigned to index 1  like 0 1 2  A B C  for currenttoken
                        // 'A' means that the quote mark no longer is in current tokens
                        Parser.quote_mark__misc_LPAREN__0___2 = 'A';
                        Parser.quote_mark_misc_to_placed_cursor__is_not_new_data = false;
                    }

                    currentToken1 = new Token(TokenType.LPAREN);
                    currentToken2 = new Token(TokenType.QUOTE);


                    // remove char '    advance of QUOTE
                    scanner.quoteMark_engaged = false;
                    Parser.quote_extension_going = true;
                    // we may need an additional right parenthesis a bit on later...
                    // for the left we added above.
                    //quote_mark__needs_another_right_parenthesis_later_on = true;

                    // later a check condition will be set to make 
                    //sure quote has a tail to it.  
                    //And not a left parenthesis error.
                    //
                    // at token check QUOTE

                }

                // as long as it is not a quote in current newest token,
                // read another
                          // the pushed back token is one delayed from quote form extension where a token had to be put aside last run
                else if(!scanner.quoteMark_engaged && !Parser.quote_extension_going && Parser.pushedBack_extraToken_fromQuoteMark_ == null)
                {
                    
                    // important  Reads next token for the standard input flow
                    //  //ignoree this part
                    // this inspects to mark that next token taken in will or will not turn out to be quote mark
                    if (Parser.quote_mark_misc_to_placed_cursor__is_not_new_data == true)
                        toggleIf_inputVaries = false;
                    else
                        toggleIf_inputVaries = true;
                    // read next token
                    currentToken2 = this.scanner.getNextToken();

                    // this is run if the next token turns out to be a quote mark
                    if (Parser.quote_mark_misc_to_placed_cursor__is_not_new_data == true && toggleIf_inputVaries)
                        // assigned to index 1  like 0 1 2  A B C  for currenttoken// assigned to index 1  like 0 1 2  A B C  for currenttoken
                        Parser.quote_mark__misc_LPAREN__0___2 = 'C';


                }


                // assign token reserved from pushed back element in quotation mark delay extension
                // start wiping it out because quote extension going is false
                else if (!scanner.quoteMark_engaged && !Parser.quote_extension_going && Parser.pushedBack_extraToken_fromQuoteMark_ != null)
                {
                    //  dont need this line currentToken2 = pushedBack_extraToken_fromQuoteMark_;

                    pushedBack_extraToken_fromQuoteMark_ = null;
                }

            }
            // do not read another token because input end is now there.  
                // emptyTerm is only  for excluding quote extending to not go into this
                 // skipping blocks with this flag_ otherwise _ has nothing to do with this block.
            else if (!emptyTerm)
            {
                // a pointless declaration to currentToken2
                // _ for end of total parsed expression...
                // we just don't want currentToken2 to be null
                currentToken2 = new Token(TokenType.LPAREN);
                if (Scanner.flag_debugger)
                    Console.Write("expression complete: Not reading additional tokens in this line.");

                // things will terminate with the currentToken1 hopefully  and token 2 will not be needed.
            }
            


            if (currentToken1 == null && !emptyTerm)
                return null;

            

            // increment if last was a special... 
            if (Parser.special_type_Has_to_have_non_nil_cdr && !emptyTerm)
            {
                // if less than or equal to zero , ...
                if (!(Parser.lastRunHad_Special_id >= 1))
                {
                    Parser.lastRunHad_Special_id = ++Parser.lastRunHad_Special_id;
                    // this is to be false for Ident constructor clause
                    // currentToken1.getType() == TokenType.IDENT
                    //to have a tail of special car item
                    // needing a tail.
                    start_addit_Ident_needing_End_expression_tail = false;
                }

            }
            // in a future case... in loop next_ following iteration of above previous 
            // make this blank again.  *
            // usually the reflex goes to first if above
            else if (Parser.last_itertion_Identifier != null && !emptyTerm)
                Parser.last_itertion_Identifier = null;
            // if it gets above zero , set back to 0;   // and cant do both at once
            else if (Parser.lastRunHad_Special_id >= 1 && !emptyTerm)
                Parser.lastRunHad_Special_id = 0;

            // shut off the skipping  by setting empty term to false
            Parser.emptyTerm = false;


            if (currentToken1.getType() == TokenType.LPAREN)
            {

                number_of_Left_parentheses_extra_over_zero++;

                if (Parser.condition_in_dotted_exp)
                    Parser.cancel_balance_number_Left_Paren__DottedExp++;

                // a new expression will deal with the requirement that special types 
                //need a tail non nil element
                if (Parser.special_type_Has_to_have_non_nil_cdr && Parser.lastRunHad_Special_id >= 1)
                {
                    Parser.lastRunHad_Special_id = 0;
                    Parser.special_type_Has_to_have_non_nil_cdr = false;
                }


                if (Scanner.flag_debugger)
                    Console.WriteLine("LPAREN");
                // note _ check for null _ only in interest of avoiding null point for the test
                //   currenttoken2.getTYpe() ...only making sure
                if (currentToken2 != null && (currentToken2.getType() != TokenType.QUOTE))
                    Parser.quote_extension_going = false;

                // a quote is a matched inmate   wedged in here to the clause   LPAREN quote   as in  (quote ...
                // define node in static variable for future reference !... after a unit has parsed a leftSide list !


                if (currentToken2.getType() == TokenType.QUOTE)
                {
                    // start doing quote cons expression
                    // a special type now has to have a tail non-nill element!
                    Parser.special_type_Has_to_have_non_nil_cdr = true;

                    Ident firstIdent = new Ident("quote");

                    // set flag to get next expression, 
                    //whether it is pushedBack_extraToken_fromQuoteMark_ or more ( expression)
                    stillReadFirstParenthesis = true;

                    Node newParseNode = this.parseExp();

                    Cons inBetweenSection;
                    // check if parseExp worked      an error if it is null
                    if (newParseNode == null)
                        // error
                        return null;
                    else
                        inBetweenSection = new Cons(newParseNode, nil_object);




                    Cons secondSection = new Cons(firstIdent, inBetweenSection);
                    // lower this... since we added a closing nil   for a Right PAREN
                    Parser.number_of_Left_parentheses_extra_over_zero--;


                    return secondSection;


                }

                else
                {
                    returnNode = parseRest();

                    

                    //currentNode_ofToken1 = returnNode;
                    return returnNode;
                }

            }
            // also need to convert '(list)
            else if (currentToken1.getType() == TokenType.FALSE)
                {
                    // a new expression will deal with the requirement that special types 
                    //need a tail non nil element
                    if (Parser.special_type_Has_to_have_non_nil_cdr && Parser.lastRunHad_Special_id >= 1)
                    {
                        Parser.lastRunHad_Special_id = 0;
                        Parser.special_type_Has_to_have_non_nil_cdr = false;
                    }

                    returnNode = new BoolLit(false);
                    // define node in static variable for future reference !
                    //currentNode_ofToken1 = returnNode;

                    if (Scanner.flag_debugger)
                        Console.WriteLine("FALSE");

                    return returnNode;
                }
                else if (currentToken1.getType() == TokenType.TRUE)
                {
                    // a new expression will deal with the requirement that special types 
                    //need a tail non nil element
                    if (Parser.special_type_Has_to_have_non_nil_cdr && Parser.lastRunHad_Special_id >= 1)
                    {
                        Parser.lastRunHad_Special_id = 0;
                        Parser.special_type_Has_to_have_non_nil_cdr = false;
                    }

                    returnNode = new BoolLit(true);

                    // define node in static variable for future reference !
                    //currentNode_ofToken1 = returnNode;

                    if (Scanner.flag_debugger)
                        Console.WriteLine("TRUE");

                    return returnNode;
                }
                else if (currentToken1.getType() == TokenType.INT)
                {

                    // a new expression will deal with the requirement that special types 
                    //need a tail non nil element
                    if (Parser.special_type_Has_to_have_non_nil_cdr && Parser.lastRunHad_Special_id >= 1)
                    {
                        Parser.lastRunHad_Special_id = 0;
                        Parser.special_type_Has_to_have_non_nil_cdr = false;
                    }


                    returnNode = new IntLit(currentToken1.getIntVal());

                    // define node in static variable for future reference !
                    //currentNode_ofToken1 = returnNode;

                    if (Scanner.flag_debugger)
                        Console.WriteLine("INT, intVAL =" + currentToken1.getIntVal());

                    return returnNode;
                }
                else if (currentToken1.getType() == TokenType.STRING)
                {
                    // a new expression will deal with the requirement that special types 
                    //need a tail non nil element
                    if (Parser.special_type_Has_to_have_non_nil_cdr && Parser.lastRunHad_Special_id >= 1)
                    {
                        Parser.lastRunHad_Special_id = 0;
                        Parser.special_type_Has_to_have_non_nil_cdr = false;
                    }


                    returnNode = new StringLit(currentToken1.getStringVal());

                    // define node in static variable for future reference !
                    //currentNode_ofToken1 = returnNode;

                    if (Scanner.flag_debugger)
                        Console.WriteLine("STRING, stringVal =" + currentToken1.getStringVal());

                    return returnNode;
                }
                // this never gets run ... since it is squished inside of  if(currentToken1 == TokenType.LPAREN)
                   // instead
                   /**
                else if (currentToken1.getType() == TokenType.QUOTE)
                {
                    if (Scanner.flag_debugger)
                        Console.WriteLine("IDENT, name = quote");




                    // a special type now has to have a tail non-nill element!
                    Parser.special_type_Has_to_have_non_nil_cdr = true;

                    Ident firstIdent = new Ident("quote");

                    // set flag to get next expression, 
                    //whether it is pushedBack_extraToken_fromQuoteMark_ or more ( expression)
                    stillReadFirstParenthesis = true;

                   Node newParseNode = this.parseExp();

                    Cons inBetweenSection;
                // check if parseExp worked
                    if (newParseNode == null)
                        return null;
                    else
                        inBetweenSection = new Cons(newParseNode, nil_object);

                // cap everything off with RPAREN  
                //  
                //  if  not done automatically in some path cases
                if (Parser.still_need_to_put_in_RPAREN__next_iteration)
                {
                    // store it for next  iteration of parseRest  a couple lines below the following


                    // assigned to index 1  like 0 1 2  A B C  for currenttoken// assigned to index 1  like 0 1 2  A B C  for currenttoken
                   

                    // delete the quote mark LPAREN   from prevoius ' mark
                    if (Parser.quote_mark_misc_to_placed_cursor__is_not_new_data)
                    {
                        if (Parser.quote_mark__misc_LPAREN__0___2 == 'C')
                            // assigned to index 1  like 0 1 2  A B C  for currenttoken// assigned to index 1  like 0 1 2  A B C  for currenttoken
                            Parser.quote_mark__misc_LPAREN__0___2 = 'A';

                        Parser.quote_mark_misc_to_placed_cursor__is_not_new_data = false;
                    }

                    Parser.still_need_to_put_in_RPAREN__next_iteration = false;
                }


                    Cons secondSection = new Cons(firstIdent, inBetweenSection);




                    return secondSection;


                }
    **/
                else if (currentToken1.getType() == TokenType.IDENT)
                {
                    String tokenTypeIdent = currentToken1.getName();


                    // checking if a second consecutive identifier will require
                    //   a new general term tail....  the listed identifier are not tail material
                    //
                    // pretest of past identifier        since sequential two may have happened
                    if (
                        (last_itertion_Identifier == "begin" ||
                        last_itertion_Identifier == "cond" ||
                        last_itertion_Identifier == "define" ||
                        last_itertion_Identifier == "if" ||
                        last_itertion_Identifier == "lambda" ||
                        last_itertion_Identifier == "let" ||
                        last_itertion_Identifier == "quote" ||
                        last_itertion_Identifier == "set!") &&

                       (tokenTypeIdent == "begin" ||
                        tokenTypeIdent == "cond" ||
                        tokenTypeIdent == "define" ||
                        tokenTypeIdent == "if" ||
                        tokenTypeIdent == "lambda" ||
                        tokenTypeIdent == "let" ||
                        tokenTypeIdent == "quote" ||
                        tokenTypeIdent == "set!")
                        )
                    {
                        start_addit_Ident_needing_End_expression_tail = true;
                        Parser.lastRunHad_Special_id = 0;
                    }




                    // check if IDENT is a special type...
                    // NOTE:
                    // some of these are  going to need  to activate Special_type_has_to_have_non
                    // -nil_cdr flag

                    if (tokenTypeIdent == "begin")
                    {
                        last_itertion_Identifier = "begin";
                        //Parser.special_type_Has_to_have_non_nil_cdr = true;
                    }
                    else if (tokenTypeIdent == "cond")
                    {
                        last_itertion_Identifier = "cond";
                        //Parser.special_type_Has_to_have_non_nil_cdr = true;
                    }
                    else if (tokenTypeIdent == "define")
                    {
                        last_itertion_Identifier = "define";
                        //Parser.special_type_Has_to_have_non_nil_cdr = true;
                    }
                    else if (tokenTypeIdent == "if")
                    {
                        last_itertion_Identifier = "if";
                        //Parser.special_type_Has_to_have_non_nil_cdr = true;
                    }
                    else if (tokenTypeIdent == "lambda")
                    {
                        last_itertion_Identifier = "lambda";
                        //Parser.special_type_Has_to_have_non_nil_cdr = true;

                    }
                    else if (tokenTypeIdent == "let")
                    {
                        last_itertion_Identifier = "let";
                        //Parser.special_type_Has_to_have_non_nil_cdr = true;
                    }
                    else if (tokenTypeIdent == "quote")
                    {
                        last_itertion_Identifier = "quote";
                        Parser.special_type_Has_to_have_non_nil_cdr = true;
                    }
                    else if (tokenTypeIdent == "set!")
                    {
                        last_itertion_Identifier = "set!";
                        //Parser.special_type_Has_to_have_non_nil_cdr = true;
                    }

                    // a new expression will deal with the requirement that special types 
                    //need a tail non nil element
                    // also note: it won't execute if the past start was (Ident _currentIdent...)
                    // flag start_addit_Ident_needing_End_expression_tail gets toggled then
                    if
                    (Parser.special_type_Has_to_have_non_nil_cdr && (Parser.lastRunHad_Special_id > 0) && (Parser.lastRunHad_Special_id < 2)
                     && !start_addit_Ident_needing_End_expression_tail)
                    {
                        // tail item reported to be found
                        Parser.special_type_Has_to_have_non_nil_cdr = false;
                        lastRunHad_Special_id = 0;
                    }

                    returnNode = new Ident(currentToken1.getName());

                    // define node in static variable for future reference !
                    //currentNode_ofToken1 = returnNode;

                    if (Scanner.flag_debugger)
                        Console.WriteLine("IDENT, name = " + currentToken1.getName());

                    return returnNode;
                }

                else
                {
                    // this follows after last run of ident checks ...

                    // follows a previous token being quote.
                    // make sure quote extension has argument as tail.  and not a )   error
                    if (Parser.special_type_Has_to_have_non_nil_cdr)
                    {

                    // lets turn things so can go on continuing quote as usual in this iteration of parseExp();
                    Parser.quote_extension_going = false;
                    

                    // iterate peek until we get to non null non Right Parenthesis token.
                    TokenPeekedAt = scanner.peekAtNextToken();

                        if (TokenPeekedAt == null)
                        {
                            Console.Error.WriteLine("EOF cannot go Further checking faulty incomplete expression (quote rightpar)");

                        }
                        else
                        {

                            if (TokenPeekedAt.getType() != TokenType.RPAREN)
                                quote_mark_succeeded_incrementing_tokens = true;
                            else
                                do
                                {
                                    Console.Error.WriteLine(" ) _right parenthesis cannot appear right after quote");
                                    TokenPeekedAt = scanner.peekAtNextToken();
                                    if (TokenPeekedAt == null)
                                        break;

                                    if (TokenPeekedAt.getType() != TokenType.RPAREN)
                                        quote_mark_succeeded_incrementing_tokens = true;
                                }
                                while (!quote_mark_succeeded_incrementing_tokens);

                            if (TokenPeekedAt == null)
                            {
                                quote_mark_succeeded_incrementing_tokens = false;
                                Console.Error.WriteLine("Current token a ' err) and EOF reached: not a complete quotation expression.");
                                return null;
                            }
                            else { 
                                quote_mark_succeeded_incrementing_tokens = true;

                            Parser.special_type_Has_to_have_non_nil_cdr = false;
                            return this.parseExp();
                        }

                     }
                  }

                    else
                        Console.Error.WriteLine("Error: Token is unidentifiable in our grammar.");
                // lets turn things so can go on continuing quote as usual in this iteration of parseExp();
                Parser.quote_extension_going = false;
                

                return null;
                }
                
            }
            


        

        protected Node parseRest()
        {
            Token TokenPeekedAt = null;
            bool quote_mark_succeeded_incrementing_tokens = false;
            Token extraPeekedAtToken = null;
            
            Node endReturnNode = null;

            bool toggleIf_inputVaries;
            


            // TODO: write code for parsing a rest
            if (currentToken2 == null)
            {
                Console.Error.WriteLine("stops at start of expression_ Or while exceeded balance of LeftParentheses. Not a complete statement.");
                return null;
            }
            if (Parser.flip_Past_RightParenthesis_after_tail )
            {

                // prevent Rest from repeating a nil
                currentToken1 = currentToken2;

                 if (Parser.number_of_Left_parentheses_extra_over_zero != 0)
                {
                    if (Parser.quote_mark_misc_to_placed_cursor__is_not_new_data == true)
                        toggleIf_inputVaries = false;
                    else
                        toggleIf_inputVaries = true;
                    currentToken2 = this.scanner.getNextToken();

                    if (Parser.quote_mark_misc_to_placed_cursor__is_not_new_data == true && toggleIf_inputVaries)
                        // assigned to index 1  like 0 1 2  A B C  for currenttoken// assigned to index 1  like 0 1 2  A B C  for currenttoken
                        Parser.quote_mark__misc_LPAREN__0___2 = 'C';
                    

                }
                Parser.flip_Past_RightParenthesis_after_tail = false;
                
                return parseRest();
            }
            // will print rparenthesis when appropriate
            if (currentToken2.getType() == TokenType.RPAREN )
            {
                if (Scanner.flag_debugger)
                    Console.WriteLine("RPAREN");
                endReturnNode = nil_object;

                // this is the core measure of parenthesis balance in the parser
                 Parser.number_of_Left_parentheses_extra_over_zero--;
                
                // do this for tail balanced L and R parentheses , heuristics
                if (Parser.condition_in_dotted_exp)
                   Parser.cancel_balance_number_Left_Paren__DottedExp--;

                // prevent Rest from repeating a nil
                   // nextItemRPAREN basically says to not jump to closing tail boundary
                     // in a special dot tail case
                if ( !Parser.condition_in_dotted_exp || Parser.cancel_balance_number_Left_Paren__DottedExp != 0 
                    && Parser.number_of_Left_parentheses_extra_over_zero != 0)
                {
                    currentToken1 = currentToken2;
                    

                    if (Parser.number_of_Left_parentheses_extra_over_zero != 0)
                    {
                        if (Parser.quote_mark_misc_to_placed_cursor__is_not_new_data == true)
                            toggleIf_inputVaries = false;
                        else
                            toggleIf_inputVaries = true;
                        currentToken2 = this.scanner.getNextToken();

                        if (Parser.quote_mark_misc_to_placed_cursor__is_not_new_data == true && toggleIf_inputVaries)
                            // assigned to index 1  like 0 1 2  A B C  for currenttoken// assigned to index 1  like 0 1 2  A B C  for currenttoken
                            Parser.quote_mark__misc_LPAREN__0___2 = 'C';


                    }
                }

                
                
                
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
                    
                    Node parsedRest =  parseRest();
                    
                    return parsedRest;
                    //ends if that happens
                }
                Parser.condition_in_dotted_exp = true;

                currentToken1 = currentToken2;

                currentToken2 = this.scanner.getNextToken();

                if (currentToken2 != null)
                {
                    if (currentToken2.getType() == TokenType.LPAREN)
                    {
                        Parser.tail_item_A_list = true;
                        endReturnNode = parseExp();
                        
                    }

                    else if (currentToken2.getType() == TokenType.INT)
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
                    else if(currentToken2.getType() == TokenType.RPAREN)
                    {
                        
                            Console.Error.WriteLine(" ) _right parenthesis cannot appear right after quote");

                       
                         TokenPeekedAt = scanner.peekAtNextToken();


                        if (TokenPeekedAt == null)
                        {
                            Console.Error.WriteLine("Current token a ' err) and EOF reached: not a complete quotation expression.");
                            return null;
                        }

                            if (TokenPeekedAt.getType() != TokenType.RPAREN)

                                quote_mark_succeeded_incrementing_tokens = true;
                            
                            else
                                do
                                {
                                    Console.Error.WriteLine(" ) _right parenthesis cannot appear right after quote");

                                  TokenPeekedAt = scanner.peekAtNextToken();

                                if (TokenPeekedAt == null)
                                   {
                                       Console.Error.WriteLine("Current token a ' err) and EOF reached: not a complete quotation expression.");
                                        return null;
                                    }

                                if (TokenPeekedAt.getType() != TokenType.RPAREN)
                                    
                                        quote_mark_succeeded_incrementing_tokens = true;
                                    
                                    
                                }
                                while (!quote_mark_succeeded_incrementing_tokens);
                        if (TokenPeekedAt == null)
                        {
                            quote_mark_succeeded_incrementing_tokens = false;
                            Console.Error.WriteLine("Current token a ' err) and EOF reached: not a complete quotation expression.");
                            return null;
                        }
                        else
                        {
                            quote_mark_succeeded_incrementing_tokens = true;
                            currentToken2 = TokenPeekedAt;
                        }
                      // assign right dot item.
                        if (TokenPeekedAt.getType() == TokenType.INT)
                        {
                            endReturnNode = new IntLit(TokenPeekedAt.getIntVal());

                            if (Scanner.flag_debugger)
                                Console.WriteLine("INT, intVAL =" + TokenPeekedAt.getIntVal());
                        }
                        else if (TokenPeekedAt.getType() == TokenType.STRING)
                        {
                            endReturnNode = new StringLit(TokenPeekedAt.getStringVal());
                            if (Scanner.flag_debugger)
                                Console.WriteLine("STRING, stringVal =" + TokenPeekedAt.getStringVal());
                        }
                        else if (TokenPeekedAt.getType() == TokenType.TRUE)
                        {
                            endReturnNode = new BoolLit(true);
                            if (Scanner.flag_debugger)
                                Console.WriteLine("TRUE");
                        }
                        else if (TokenPeekedAt.getType() == TokenType.FALSE)
                        {
                            endReturnNode = new BoolLit(false);
                            if (Scanner.flag_debugger)
                                Console.WriteLine("FALSE");
                        }
                        else if (TokenPeekedAt.getType() == TokenType.IDENT)
                        {
                            endReturnNode = new Ident(TokenPeekedAt.getName());
                            if (Scanner.flag_debugger)
                                Console.WriteLine("Ident, name =" + TokenPeekedAt.getName());
                        }
                        else
                        {
                            // tail will terminate with some expression   since a dot's right side is tail
                            Parser.tail_item_A_list = true;

                            
                            endReturnNode = parseExp();
                            
                        }
                        currentToken2 = TokenPeekedAt;
                    }
                    


                    // check if there is correct grammar for a tail node with it's sole_literal value
                    //   thus: make sure there are not multiple values after tail.
                    bool additional_ReadMore = true;
                    // this is turned to true when a proper following Rparenth comes like . 2')'
                    flip_Past_RightParenthesis_after_tail = false;
                    // as closing touch... this will try to get 
                    // token beyond the next RightParenthesis

                    //  continues iterating on ... if Parsing ERRORS.



                    extraPeekedAtToken = scanner.peekAtNextToken();
                    // check if next token is a right Parenthesis
                    if (extraPeekedAtToken != null &&
                        extraPeekedAtToken.getType() == TokenType.RPAREN)
                    {
                        
                        currentToken2 = scanner.getNextToken();

                        Parser.condition_in_dotted_exp = false;

                        Parser.flip_Past_RightParenthesis_after_tail = true;

                        number_of_Left_parentheses_extra_over_zero--;

                        return endReturnNode;
                    }
                    else if (extraPeekedAtToken == null)
                    {
                        Console.Error.WriteLine("end of input at dotted S-Expression before closing parenthesis.");
                        return null;
                    }
                    else if (extraPeekedAtToken.getType() == TokenType.INT ||
                                  extraPeekedAtToken.getType() == TokenType.STRING ||
                                  extraPeekedAtToken.getType() == TokenType.TRUE ||
                                  extraPeekedAtToken.getType() == TokenType.FALSE ||
                                  extraPeekedAtToken.getType() == TokenType.IDENT)
                    {
                        if (Scanner.flag_debugger)
                            Console.WriteLine("error : cannot have any more than one literal for dot non-list tail.");
                        
                    }

                    while (additional_ReadMore)
                    {


                        if (extraPeekedAtToken != null)
                        {

                            currentToken1 = extraPeekedAtToken;


                            // exit criteria   for proper tail and right parenthesis being correct;
                            if (flip_Past_RightParenthesis_after_tail)
                            {
                                additional_ReadMore = false;

                                currentToken2 = scanner.getNextToken();

                                Parser.condition_in_dotted_exp = false;

                                return endReturnNode;
                            }

                            extraPeekedAtToken = scanner.peekAtNextToken();


                            if (extraPeekedAtToken == null)
                            {
                                additional_ReadMore = false;
                                Console.Error.WriteLine("ends abruptly after dot_ . in list with tail items.");
                                return null;
                                //break;
                            }
                            else if (extraPeekedAtToken.getType() == TokenType.RPAREN)
                            {
                                number_of_Left_parentheses_extra_over_zero--;

                                flip_Past_RightParenthesis_after_tail = true;
                                continue;
                            }
                            // examine if dot tail item ends appropriately.
                            else
                            {
                                if (extraPeekedAtToken.getType() == TokenType.INT ||
                                  extraPeekedAtToken.getType() == TokenType.STRING ||
                                  extraPeekedAtToken.getType() == TokenType.TRUE ||
                                  extraPeekedAtToken.getType() == TokenType.FALSE)
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
                

                Node oneExpression = parseExp(); ;
           
               
            
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

