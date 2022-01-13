using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyLanguage.ExpressionEvaluator_POC.Operators;

namespace MyLanguage.ExpressionEvaluator_POC
{
    internal class Tokeniser
    {
        private readonly TextReader reader;
        private char currentChar;
        public readonly OperatorTable operatorTable;
        public Tokeniser(TextReader reader, OperatorTable operatorTable)
        {
            this.reader = reader;
            this.operatorTable = operatorTable;
            this.Identifier = "";
            NextChar();
            NextToken();
        }
        public int Token { get; private set; }
        public int IntLiteral { get; private set; }
        public float FloatLiteral { get; private set; }
        public char CharLiteral { get; private set; }
        public bool BoolLiteral { get; private set; }
        public string Identifier { get; private set; }

        void NextChar()
        {
            int ch = reader.Read();
            // replaces EOF with \0 character
            if (ch < 0) currentChar = '\0';
            else currentChar = (char)ch;
        }

        public void NextToken()
        {
            while (char.IsWhiteSpace(currentChar))
            {
                NextChar();
            }

            if (currentChar == '\0')
            {
                NextChar();
                Token = (int)DefaultToken.EOF;
                return;
            }

            // Numeric literals
            if (char.IsDigit(currentChar))
            {
                StringBuilder sb = new();
                bool haveDecimalPoint = false;
                while (char.IsDigit(currentChar) || (!haveDecimalPoint && currentChar == '.'))
                {
                    sb.Append(currentChar);
                    haveDecimalPoint |= currentChar == '.';
                    NextChar();
                }

                // parse the number
                if (!haveDecimalPoint)
                {
                    IntLiteral = int.Parse(sb.ToString());
                    Token = (int)DefaultToken.IntLiteral;
                    Console.WriteLine($"{Token:D10}\t: Int literal {IntLiteral}");
                    return;
                }
                else
                {
                    FloatLiteral = float.Parse(sb.ToString());
                    Token = (int)DefaultToken.FloatLiteral;
                    Console.WriteLine($"{Token:D10}\t: Float literal {FloatLiteral}");
                    return;
                }
            }

            // character literals
            if (currentChar == '\'')
            {
                NextChar();
                CharLiteral = currentChar;
                Token = (int)DefaultToken.CharLiteral;
                Console.WriteLine($"{Token:D10}\t: Char literal '{CharLiteral}'");
                NextChar();
                if (currentChar != '\'')
                    throw new Exception("Character identifier was not correctly closed.");
                NextChar();
                return;
            }

            // custom operators
            string customSymbol = currentChar.ToString();
            if (operatorTable.PossibleCount(customSymbol) > 0)
            {
                NextChar();
                while (operatorTable.PossibleCount(customSymbol + currentChar) > 0)
                {
                    customSymbol += currentChar;
                    NextChar();
                }
                if (!operatorTable.OperatorExists(customSymbol))
                {
                    throw new InvalidDataException($"Unknown operator: {customSymbol} is invalid");
                }
                switch (customSymbol)
                {
                    case "(":
                        Token = (int)DefaultToken.OpenParens;
                        break;
                    case ")":
                        Token = (int)DefaultToken.CloseParens;
                        break;
                    case "true":
                        Token = (int)DefaultToken.BoolLiteral;
                        BoolLiteral = true;
                        break;
                    case "false":
                        Token = (int)DefaultToken.BoolLiteral;
                        BoolLiteral = false;
                        break;
                    default:
                        Token = customSymbol.GetHashCode();
                        break;
                }
                Console.WriteLine($"{Token:D10}\t: Symbol {customSymbol}");
                return;
            }

            // Identifier
            if (char.IsLetter(currentChar))
            {
                StringBuilder sb = new();
                while (char.IsLetterOrDigit(currentChar))
                {
                    sb.Append(currentChar);
                    NextChar();
                }
                Token = (int)DefaultToken.Identifier;
                Identifier = sb.ToString();
                Console.WriteLine($"{Token:D10}\t: Identifier {Identifier}");
                return;
            }

            throw new InvalidDataException($"Unexpected character: {currentChar} is invalid.");
        }
    }
}
