// StringLit -- Parse tree node class for representing string literals

using System;

namespace Tree
{
    public class StringLit : Node
    {
        private string stringVal;

        public StringLit(string s)
        {
            stringVal = s;
        }

        public override int print(int n)
        {
            // There got to be a more efficient way to print n spaces.
            for (int i = 0; i < n; i++)
                Console.Write(" ");

            Console.Write("\"" + stringVal + "\"");
            return n + stringVal.Length;
        }

        public override bool isString() { return true; }

        public string getString() { return stringVal; }
    }
}
