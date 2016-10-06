// Scanner -- The lexical analyzer for the Scheme printer and interpreter

using System;
using System.IO;
using Tokens;

namespace Parse
{   

    public class Scanner
    {
        // a variable for if the mode is in debugger or not
        public static bool flag_debugger;

        private BufferedStream bs;

        private StreamReader reader;

        public String inputBufferLine = null;

        public int charIndexOfReadLine = 0;

        // maximum length of strings and identifier
        private const int BUFSIZE = 1000;
        private char[] buf = new char[BUFSIZE];

        // this flag is false to make sure that a once starting line is read for getToken() 
        private bool readFirstLine = false;

        private bool noMoreLinesOfInput = false;


        private int numberParentheses_L = 0;

        private int numberParentheses_R = 0;


        public Scanner()
        {
            
            //bs = new BufferedStream(Console.OpenStandardInput(8192));
            //reader = new StreamReader(bs);
            FileStream file2 = File.OpenRead("C:/Users/Colin/Desktop/file1.txt");
            //bs = new BufferedStream(Console.OpenStandardInput(8192));
            reader = new StreamReader(file2);

        }
        /** reads another line...
         *  returns true if a newLine with code is read,
         *   and 
         *   false if there is no more content that has code
         * 
         * **/

        public bool readAnotherLine()
        {
            try
            {
                // bool value that says if another line is on the way
                bool needNewLine = true;

                // bool value that represents if the line starts with a comment, with ";"
                bool commentNotEncountered = true;

                // read lines until we get a code filled line
                while (needNewLine)
                {
                    String lineRead1 = reader.ReadLine();

                    // shutdown since last readline feed was end-of-file
                    if (lineRead1 == null)
                    {
                        needNewLine = false;
                        return false;
                    }


                    // set read character index to 0 for getting tokens
                    this.charIndexOfReadLine = 0;
                    char[] chr1 = lineRead1.ToCharArray();

                    // length of current line in chars
                    int chrArrayLength = chr1.Length;

                    // values to flip through line and check if there is a char with ";"
                    int charIndex = 0;

                    commentNotEncountered = true;

                    while (commentNotEncountered && charIndex <= chrArrayLength - 1)
                    {
                        // check for whitespace or tab character first on each line read
                        if (chr1[charIndex] == 32 || chr1[charIndex] == 9)
                        {
                            charIndex++;
                            continue;
                        }
                        // char value 59 ... A semicolon found ... IGNORE the commented-line
                        else if (chr1[charIndex] == 59)
                        {
                            // mark that a comment was encountered
                            commentNotEncountered = false;
                            break;
                        }
                        else
                        {
                            this.inputBufferLine = lineRead1;
                            this.buf = lineRead1.ToCharArray();

                            needNewLine = false;
                            break;
                        }

                    }



                }
                // return that a code line was retrieved!
                return true;
            }
            catch (IOException e)
            {
                Console.Error.WriteLine("IOException: " + e.Message);
                return false;
            }
        }

        // get token element from current line feed

        // returns null if no token to get is found.
        public Token getNextToken()
        {
            //  Note from Colin:  

            // write code to take chars out of string named

            // inputBufferLine

            // IMPORTANT: before  using this method, make sure to put this getNextToken with it's

            // own loop (in SPP.cs) nested inside one that reads a good newLine from the readAnotherLine

            // so we have something to read tokens from

            Token returnToken = null;

            try

            {

                // start parsing getToken() process off
                // by reading a line from input
                //   Is run once only : in SPP.
                if (!this.readFirstLine)
                {
                    this.readFirstLine = true;
                    // readAnotherLine returns false if there is no new line to be read!
                    this.noMoreLinesOfInput = this.readAnotherLine();
                    if (this.noMoreLinesOfInput == false)
                        throw new Exception("error: no input given.");
                }

                // read another line :: because last line's characters are all used Up
                if (charIndexOfReadLine > this.inputBufferLine.Length - 1)
                    // readAnotherLine returns false if there is no new line to be read!
                    this.noMoreLinesOfInput = this.readAnotherLine();

                if (!this.noMoreLinesOfInput)
                    return returnToken;




                int numberDotsForDecimal = 0;

                //bool currentTokenIsNumber = false;

                bool number_lastCharWasDecimal = false;



                // setup reference for a Start and End Index, of a String in input expression

                int someStringLowerBound;

                int someStringLength;

                int someStringUpperBound;



                // if true, stop reading characters in constant someString

                bool endOfSomeStringReached;





                // It would be more efficient if we'd maintain our own

                // input buffer and read characters out of that

                // buffer, but reading individual characters from the

                // input stream is easier.



                //// wrong code in original zip was: ch = reader.Read();

                char ch;



                ch = buf[charIndexOfReadLine];


                while (charIndexOfReadLine <= inputBufferLine.Length - 1)
                {
                    ch = buf[charIndexOfReadLine];

                    // if whitespace or a tab ... skip and iterate to next character.

                    if (ch == 32 || ch == 9)

                    {

                        charIndexOfReadLine++;

                        continue;

                    }

                    else
                        break;

                }
                // if end of current line read  ,  return getNextToken()
                if (charIndexOfReadLine > inputBufferLine.Length - 1)
                    return getNextToken();


                // reset this boolean to null or false.

                endOfSomeStringReached = false;

                // reset all these to zero

                someStringLowerBound = 0;

                someStringLength = 0;

                someStringUpperBound = 0;



                numberDotsForDecimal = 0;

                // need to use this variable in revision number 1 that is to happen

                //currentTokenIsNumber = false;



                number_lastCharWasDecimal = false;







                // Special characters

                if (ch == '\'')

                {
                    // copy char to token


                    this.charIndexOfReadLine++;


                    returnToken = new Token(TokenType.QUOTE);
                    //return new Token (TokenType.QUOTE);

                    if(flag_debugger)
                        Console.WriteLine("value: \'");
                    return returnToken;
                }

                else if (ch == '(')

                {
                    // copy char to token

                    this.charIndexOfReadLine++;

                    //return new Token (TokenType.LPAREN);

                    numberParentheses_L++;

                    returnToken = new Token(TokenType.LPAREN);
                    if (flag_debugger)
                        Console.WriteLine("value: (");

                    return returnToken;


                }

                else if (ch == ')')

                {

                    //return new Token (TokenType.RPAREN);

                    if (numberParentheses_R > numberParentheses_L)
                    {
                        this.charIndexOfReadLine++;
                        if (flag_debugger)
                            Console.WriteLine("Too many Right Parentheses>: error! \n continued... :Last input ignored!");

                        return getNextToken();

                    }
                    else

                    {
                        if (this.numberParentheses_L > this.numberParentheses_R)
                            this.numberParentheses_L--;

                        // copy char to token


                        this.charIndexOfReadLine++;

                        returnToken = new Token(TokenType.RPAREN);
                        if (flag_debugger)
                            Console.WriteLine("value: )");

                        return returnToken;
                    }

                }

                else if (ch == '.')

                {

                    // copy char to token


                    this.charIndexOfReadLine++;

                    if (flag_debugger)
                        Console.WriteLine("value: . ");

                    // We ignore the special identifier `...'.

                    returnToken = new Token(TokenType.DOT);
                    //return new Token (TokenType.DOT);

                    return returnToken;

                }

                // Boolean constants

                else if (ch == '#')

                {

                    // note... Work on.. As said above:  

                    charIndexOfReadLine++;



                    if (charIndexOfReadLine <= inputBufferLine.Length - 1)

                    {

                        ch = buf[charIndexOfReadLine];

                        if (ch == 't')

                        {
                            // copy char to token


                            this.charIndexOfReadLine++;

                            returnToken = new Token(TokenType.TRUE);
                            if (flag_debugger)
                                Console.WriteLine("value: true");

                            //return new Token (TokenType.TRUE);
                            return returnToken;
                        }

                        else if (ch == 'f')

                        {
                            // copy char to token


                            this.charIndexOfReadLine++;

                            returnToken = new Token(TokenType.FALSE);

                            if (flag_debugger)
                                Console.WriteLine("value: false");

                            //return new Token (TokenType.FALSE);
                            return returnToken;

                        }

                        else

                        {
                            this.charIndexOfReadLine++;
                            if (flag_debugger)
                                Console.WriteLine("Illegal character '" +

                            ch + "' following #");

                            //Console.Error.WriteLine ("Illegal character '" +

                            // char)ch + "' following #");

                            return getNextToken();

                        }



                    }
                    return getNextToken();

                }



                // String constants

                else if (ch == '"')

                {

                    // TODO: scan a string into the buffer variable buf

                    //return new StringToken (new String (buf, 0, 0));



                    // colin's dealing

                    someStringLowerBound = charIndexOfReadLine;

                    // increment by 1 more character in array

                    charIndexOfReadLine++;


                    // increment someStringLength 1  for quotation mark "
                    someStringLength++;




                    while (!endOfSomeStringReached && charIndexOfReadLine <= inputBufferLine.Length - 1)

                    {



                        // set tentative someStringLength...



                        someStringLength++;

                        // tentative upper bound of someString  for future reference....

                        someStringUpperBound = charIndexOfReadLine;





                        // check if next scanned character is backslash delimiter...

                        if (buf[charIndexOfReadLine] == '\\')

                        {

                            // skip past the delimiter backslash and also the following quote Char

                            charIndexOfReadLine += 2;

                            someStringLength++;

                        }

                        else if (buf[charIndexOfReadLine] == '"')

                        {



                            endOfSomeStringReached = true;
                            
                            this.charIndexOfReadLine++;
                            someStringUpperBound = this.charIndexOfReadLine;

                        }

                        // iterate to next Char

                        else

                            charIndexOfReadLine++;



                    }

                    if (!endOfSomeStringReached)
                    {
                        if (flag_debugger)
                            Console.WriteLine("String does not finish on line,... error: ran out of characters.");
                        return getNextToken();
                    }


                    // Given that upperBound cannot have been written to be out of array bounds,

                    //    this checks if the end of quote is the last term in the current Line.

                    else if (someStringUpperBound == inputBufferLine.Length - 1)

                    {
                        returnToken = new StringToken(new String(buf, someStringLowerBound, someStringLength));

                        // return string literal ... that was just scanned
                        if (flag_debugger)
                            Console.WriteLine("value: " + new String(buf, someStringLowerBound, someStringLength));

                        return returnToken;

                        // remove comment not necessary
                        // Console.WriteLine("continuation Halted: lineTerminates Abruptedly");



                    }

                    // copy char to token


                    // return string literal ... that was just scanned

                    else if (someStringLength != 0)
                    {
                        // copy char to token

                        returnToken = new StringToken(new String(buf, someStringLowerBound, someStringLength));
                        if (flag_debugger)
                            Console.WriteLine("value: " + new String(buf, someStringLowerBound, someStringLength));

                        return returnToken;

                    }
                    // String has nothing to it  : i.e.  "[blank]
                    //else
                    // {
                    //    Console.WriteLine("value: error   Incomplete expression String literal");
                    //    return getNextToken();
                    //}

                    // dummy return null;
                    return returnToken;
                }





                // Integer constants

                else if (ch >= '0' && ch <= '9')

                {

                    //int i = ch - '0';



                    someStringLength++;







                    someStringLowerBound = charIndexOfReadLine;



                    // increment by 1 more character in array

                    charIndexOfReadLine++;



                    // someStringLength starts at 0



                    while (!endOfSomeStringReached && charIndexOfReadLine <= inputBufferLine.Length - 1)

                    {



                        ch = buf[charIndexOfReadLine];





                        // if a space appears, ... move out of number retrieval loop

                        if (ch == 32 || ch == 9)

                        {


                            if (numberDotsForDecimal == 1 && number_lastCharWasDecimal)

                            {
                                // error 
                                if (flag_debugger)
                                    Console.WriteLine("incorrect decimal form: needs a number for right side of decimal.");

                                return getNextToken();

                            }

                            else

                            {

                                endOfSomeStringReached = true;

                                break;

                            }

                        }
                        /**
                         *  DONT NEED THIS
                        else if (ch == '(')
                        {
                            
                            
                            this.charIndexOfReadLine++;

                            Console.WriteLine("illegal previous number. Use of right ')' not correct.");

                            return getNextToken();
                            
                    }
    
                         **/

                        else if (ch == ')')

                        {

                            endOfSomeStringReached = true;

                            //return new Token (TokenType.RPAREN);

                            if (numberParentheses_R > numberParentheses_L)
                            {

                                this.charIndexOfReadLine++;
                                //Console.WriteLine("Too many Right Parentheses>: error! \n continued... :Last input ignored!");
                                return getNextToken();

                            }
                            else

                            {

                                // rules out having a right Parenthesis  if number was like

                                // 10.) 


                                if (number_lastCharWasDecimal)

                                {

                                    this.charIndexOfReadLine++;
                                    if (flag_debugger)
                                        Console.WriteLine("illegal previous number. Use of right ')' not correct.");

                                    return getNextToken();
                                }

                                else

                                {


                                    // copy char to token


                                    // need to configure token to take either decimal or integer.

                                    double intermediateVal = Convert.ToDouble(new String(buf, someStringLowerBound, someStringLength) );

                                    int intVal = Convert.ToInt32(intermediateVal);
                                    returnToken = new IntToken(intVal);
                                    if (flag_debugger)
                                        Console.WriteLine("value: " + new String(buf, someStringLowerBound, someStringLength));

                                    return returnToken;

                                    //endOfSomeStringReached   dont activate   already printed 





                                    //numberParentheses_L--;

                                    // on next getToken() call... take in token of parenthesis... later..



                                }


                            }

                        }





                        // scan over first decimal for a floating p

                        if (numberDotsForDecimal == 0 && ch == '.')

                        {

                            number_lastCharWasDecimal = true;



                            numberDotsForDecimal = 1;



                            // set tentative someStringLength...



                            someStringLength++;

                            charIndexOfReadLine++;

                            continue;

                        }

                        // rules out more than one dot symbol

                        else if (numberDotsForDecimal >= 1 && ch == '.')

                        {

                            this.charIndexOfReadLine++;
                            if (flag_debugger)
                                Console.WriteLine("illegal numeric decimal form: cannot have more than one period.");

                            return getNextToken();

                            //break;

                        }

                        // rules out wrong symbols for numeric decimal digits right side of dot

                        else if (numberDotsForDecimal == 1 && number_lastCharWasDecimal && !(ch >= '0' && ch <= '9'))

                        {
                            this.charIndexOfReadLine++;
                            if (flag_debugger)
                                Console.WriteLine("incorrect decimal form: need a number for right side of decimal \n illegal start of identifier. : an identifier cannot start with numerical digits.");

                            return getNextToken();

                            //break;

                        }





                        // rules out symbols that are not numbers

                        if (!(ch >= '0' && ch <= '9'))

                        {
                            this.charIndexOfReadLine++;
                            if (flag_debugger)
                                Console.WriteLine("illegal start of number. : an number cannot start with numerical digits.");

                            return getNextToken();

                            //break;

                        }





                        // set tentative someStringLength... with our latest new character

                        someStringLength++;





                        // tentative upper bound of someString  for future reference....

                        someStringUpperBound = charIndexOfReadLine;





                        number_lastCharWasDecimal = false;





                        // proceed next increment of a loop

                        charIndexOfReadLine++;



                    }



                    // write out to console   any full decimal number



                    // Given that upperBound cannot have been written to be out of array bounds,

                    //    this checks if the end of quote is the last term in the current Line.

                    if (endOfSomeStringReached && someStringUpperBound == inputBufferLine.Length - 1)

                    {
                        // copy char to token




                        // need to configure token to take either decimal or integer.

                        double intermediateVal = Convert.ToDouble(new String(buf, someStringLowerBound, someStringLength));

                        int intVal = Convert.ToInt32( intermediateVal );
                        returnToken = new IntToken(intVal);


                        // return string literal ... that was just scanned
                        if (flag_debugger)
                            Console.WriteLine("value: " + new String(buf, someStringLowerBound, someStringLength));

                        return returnToken;



                        //Console.WriteLine("continuation Halted: lineTerminates Abruptedly");



                    }

                    else if (endOfSomeStringReached)
                    {

                        // copy char to token


                        // need to configure token to take either decimal or integer.

                        double intermediateVal = Convert.ToDouble(new String(buf, someStringLowerBound, someStringLength));


                        int intVal = Convert.ToInt32( intermediateVal );
                        returnToken = new IntToken(intVal);


                        // return string literal ... that was just scanned
                        if (flag_debugger)
                            Console.WriteLine("value: " + new String(buf, someStringLowerBound, someStringLength));

                        return returnToken;



                    }
                    // number ends at line's end  /after   the new line char delimiter
                    else if (!endOfSomeStringReached && (!number_lastCharWasDecimal && numberDotsForDecimal <= 1))

                    {
                        double intermediateVal = Convert.ToDouble(new String(buf, someStringLowerBound, someStringLength));

                        int intVal = Convert.ToInt32( intermediateVal );
                        returnToken = new IntToken(intVal);
                        if (flag_debugger)
                            Console.WriteLine("number does not finish on line,... or,  error: ran out of characters current line.");

                        return returnToken;
                    }

                    else if (!endOfSomeStringReached && (number_lastCharWasDecimal))
                    {
                        if (flag_debugger)
                            Console.WriteLine("number incomplete __ ends with decimal at end of line,... error: ran out of characters current line.");
                        return getNextToken();
                    }


                    // dummy return;
                    return returnToken;
                }

                // Identifiers

                else if ( (ch >= 'A' && ch <= 'Z') || (ch >= 'a' && ch <= 'z') ||
                
                              (ch >= '0' && ch <= '9') ||

                              ch == '!' || ch == '$' || ch == '%' || ch == '&' || ch == '*' ||

                              ch == '+' || ch == '-' || ch == '.' || ch == '/' || ch == ':' ||

                              ch == '<' || ch == '=' || ch == '>' || ch == '?' || ch == '@' ||

                              ch == '@' || ch == '^' || ch == '_' || ch == '~')

                {

                    // or ch is some other valid first character

                    // for an identifier) {

                    // TODO: scan an identifier into the buffer



                    // make sure that the character following the integer

                    // is not removed from the input stream

                    //return new IdentToken (new String (buf, 0, 0));



                    // colins dealing





                    someStringLength++;







                    someStringLowerBound = charIndexOfReadLine;



                    // increment by 1 more character in array

                    charIndexOfReadLine++;



                    // someStringLength starts at 0



                    while (!endOfSomeStringReached && charIndexOfReadLine <= inputBufferLine.Length - 1)

                    {



                        ch = buf[charIndexOfReadLine];



                        // noted as a extra evaluation of extended symbols

                        char Char1 = ch;



                        // if a space appears, ... move out of Identifier retrieval loop

                        if (ch == 32 || ch == 9)

                        {



                            endOfSomeStringReached = true;
                            this.charIndexOfReadLine++;
                            break;

                        }

                        else if (ch == ')')

                        {

                            //return new Token (TokenType.RPAREN);

                            if (numberParentheses_R > numberParentheses_L)
                            {
                                this.charIndexOfReadLine++;

                                if (flag_debugger)
                                    Console.WriteLine("Too many Right Parentheses>: error! \n continued... :Last input ignored!");
                                return getNextToken();

                            }
                            else

                            {

                                // rules out having a right Parenthesis  if number was like

                                // 10.) 



                                // note:  need to configure token to take either decimal or integer.

                                //Console.WriteLine("Identifier: " + new String(buf, someStringLowerBound, someStringLength));



                                //endOfSomeStringReached   dont activate   already printed 





                                //numberParentheses_L--;

                                //  take in token of parenthesis... later..


                                // copy char to token


                                // this.charIndexOfReadLine++;


                                //  Console.WriteLine("value: )");

                                endOfSomeStringReached = true;
                                break;



                            }

                        }

                        else if ((Char1 >= 'A' && Char1 <= 'Z') || (Char1 >= 'a' && Char1 <= 'z') ||

                              (Char1 >= '0' && Char1 <= '9') ||

                              Char1 == '!' || Char1 == '$' || Char1 == '%' || Char1 == '&' || Char1 == '*' ||

                              Char1 == '+' || Char1 == '-' || Char1 == '.' || Char1 == '/' || Char1 == ':' ||

                              Char1 == '<' || Char1 == '=' || Char1 == '>' || Char1 == '?' || Char1 == '@' ||

                              Char1 == '@' || Char1 == '^' || Char1 == '_' || Char1 == '~')

                        {

                            // set tentative someStringLength... with our latest new character

                            someStringLength++;





                            // tentative upper bound of someString  for future reference....

                            someStringUpperBound = charIndexOfReadLine;





                            // proceed next increment of a loop

                            charIndexOfReadLine++;
                            continue;

                        }

                        else

                        {
                            this.charIndexOfReadLine++;
                            if (flag_debugger)
                                Console.WriteLine("illegal character within identifier: error with accepted char' code elements.");

                            return getNextToken();
                            //break;

                        }





                    }





                    // write out to console   any full decimal number



                    // Given that upperBound cannot have been written to be out of array bounds,

                    //    this checks if the end of identifier quote is the last term in the current Line.

                    if (endOfSomeStringReached && someStringUpperBound == inputBufferLine.Length - 1)

                    {
                        // copy char to token




                        // return string literal ... that was just scanned

                        returnToken = new IdentToken(new String(buf, someStringLowerBound, someStringLength));
                        if (flag_debugger)
                            Console.WriteLine("Identifier: " + new String(buf, someStringLowerBound, someStringLength));


                        return returnToken;
                        // Console.WriteLine("continuation Halted: lineTerminates Abruptedly");



                    }

                    else if (endOfSomeStringReached)
                    {
                        // copy char to token


                        returnToken = new IdentToken(new String(buf, someStringLowerBound, someStringLength));
                        if (flag_debugger)
                            Console.WriteLine("Identifier: " + new String(buf, someStringLowerBound, someStringLength));

                        return returnToken;
                    }
                    else if (!endOfSomeStringReached)
                    {
                        // copy char to token


                        returnToken = new IdentToken(new String(buf, someStringLowerBound, someStringLength));
                        if (flag_debugger)
                            Console.WriteLine("Identifier: " + new String(buf, someStringLowerBound, someStringLength));

                        return returnToken;
                    }

                    return returnToken;



                }



                // Illegal character

                else

                {
                    this.charIndexOfReadLine++;
                    if (flag_debugger)
                         Console.WriteLine("error char");

                    return getNextToken();

                    //Console.WriteLine ("Illegal input character '"

                    //+ ch + '\'');

                    //return getNextToken ();

                    //return new Object();

                }




            }


            catch (IOException e)

            {

                Console.Error.WriteLine("IOException: " + e.Message);

                //return null;

            }

            return returnToken;

        }
    }

}

