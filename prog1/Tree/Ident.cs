// Ident -- Parse tree node class for representing identifiers

using System;

namespace Tree
{
    public class Ident : Node
    {
        private string name;

        public Ident(string n)
        {
            name = n;
        }

        public override int print(int n)
        {
            // There got to be a more efficient way to print n spaces.
            for (int i = 0; i < n; i++)
                Console.Write(" ");

            Console.Write(name);

            return n + name.Length;
        }
        public override bool isSymbol() { return true; }

        public string getSymbol()
        {
            return name;
        }
    }
}
