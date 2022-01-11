using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyLanguage.ExpressionEvaluator.Nodes;
using MyLanguage.ExpressionEvaluator.Operators;

namespace MyLanguage.ExpressionEvaluator
{
    internal class Parser
    {
        private readonly Tokeniser _tokeniser;
        public Parser(Tokeniser tokeniser)
        {
            _tokeniser = tokeniser;
        }
        public Node ParseExpression()
        {
            var expression = ParseOperatorCategory(0);

            if (_tokeniser.Token != (int)DefaultToken.EOF)
                throw new Exception("Unexpected characters at end of expression");

            return expression;
        }
        private Node Helper<T>(ConsistentUnaryHashtable<T> unaryHashtable, int depth)
        {
            if (unaryHashtable.postfix)
                return ParseConsistentPostfixUnary(depth);
            else
                return ParseConsistentPrefixUnary(depth);
        }
        private Node Helper<T>(AssociativeBinaryHashtable<T> binaryHashtable, int depth)
        {
            if (binaryHashtable.rightAssociative)
                return ParseRightAssociativeBinary(depth);
            else
                return ParseLeftAssociativeBinary(depth);
        }
        private Node Helper<I, O>(ImpliedAssociativeBinaryHashtable<I, O> binaryHashtable, int depth)
        {
            if (binaryHashtable.rightAssociative)
                return ParseImpliedRightAssociativeBinary<I, O>(depth);
            else
                return ParseImpliedLeftAssociativeBinary<I, O>(depth);
        }
        private Node ParseOperatorCategory(int depth)
        {
            if (depth == _tokeniser.operatorTable.Count) return ParseLeaf();

            Hashtable operatorCategory = _tokeniser.operatorTable[depth];
            switch (operatorCategory)
            {
                case ConsistentUnaryHashtable<float> unaryHashtable:
                    return Helper(unaryHashtable, depth);
                case ConsistentUnaryHashtable<int> unaryHashtable:
                    return Helper(unaryHashtable, depth);
                case ConsistentUnaryHashtable<bool> unaryHashtable:
                    return Helper(unaryHashtable, depth);

                case InconsistentUnaryHashtable unaryHashtable:
                    if (unaryHashtable.postfix)
                        return ParseInconsistentPostfixUnary(depth);
                    else
                        return ParseInconsistentPrefixUnary(depth);
                    
                case AssociativeBinaryHashtable<float> binaryHashtable:
                    return Helper(binaryHashtable, depth);
                case AssociativeBinaryHashtable<int> binaryHashtable:
                    return Helper(binaryHashtable, depth);
                case AssociativeBinaryHashtable<bool> binaryHashtable:
                    return Helper(binaryHashtable, depth);

                case ImpliedAssociativeBinaryHashtable<float, bool> binaryHashtable:
                    return Helper(binaryHashtable, depth);

                case InconsistentBinaryHashtable _:
                    return ParseNonAssociativeBinary(depth);

                default:
                    throw new ArgumentException("operator category either does not exist or is of incorrect type.");
            }
        }
        private Node ParseImpliedLeftAssociativeBinary<I, O>(int depth)
        {
            Node lhs = ParseOperatorCategory(depth + 1);
            var op = (Func<object, object, object>?)_tokeniser.operatorTable[depth][_tokeniser.Token];
            if (op == null)
            {
                return lhs;
            }
            _tokeniser.NextToken();

            Node rhs = ParseOperatorCategory(depth + 1);

            Node expr = new BinaryNode(lhs, rhs, op);

            var impliedOperation = ((ImpliedAssociativeBinaryHashtable<I, O>)_tokeniser.operatorTable[depth]).ImpliedOperation;

            while (true)
            {
                op = (Func<object, object, object>?)_tokeniser.operatorTable[depth][_tokeniser.Token];
                if (op == null)
                {
                    return expr;
                }
                _tokeniser.NextToken();

                lhs = rhs;
                rhs = ParseOperatorCategory(depth + 1);
                expr = new BinaryNode(expr, new BinaryNode(lhs, rhs, op), impliedOperation);
            }
        }
        private Node ParseImpliedRightAssociativeBinary<I, O>(int depth)
        {
            Node lhs = ParseOperatorCategory(depth + 1);

            var op = (Func<object, object, object>?)_tokeniser.operatorTable[depth][_tokeniser.Token];
            if (op == null)
            {
                return lhs;
            }
            _tokeniser.NextToken();

            Node rhs = ParseImpliedRightAssociativeBinary<I, O>(depth);

            if (rhs is BinaryNode binaryRight && (((ImpliedAssociativeBinaryHashtable<I, O>)_tokeniser.operatorTable[depth]).ImpliedOperation == binaryRight.op || _tokeniser.operatorTable[depth].Contains(binaryRight.op)))
            {
                var impliedOperation = ((ImpliedAssociativeBinaryHashtable<I, O>)_tokeniser.operatorTable[depth]).ImpliedOperation;
                /*
                 *       &
                 *      / \
                 *     #   &
                 *    /|  / \
                 *   / | #  ...
                 *  /  |/ \ / \
                 * a   b  ... ...
                 */
                if (binaryRight.left is BinaryNode binaryRightLeft)
                    return new BinaryNode(new BinaryNode(lhs, binaryRightLeft.left, op), binaryRight, impliedOperation);
                /*     &
                 *    / \
                 *   #   #
                 *  / \ / \
                 * a   b   c
                 */
                else return new BinaryNode(new BinaryNode(lhs, binaryRight.left, op), binaryRight, impliedOperation);
            }
            /*   #
             *  / \
             * a   b
             */
            else return new BinaryNode(lhs, rhs, op);
        }
        private Node ParseLeftAssociativeBinary(int depth)
        {
            Node lhs = ParseOperatorCategory(depth + 1);

            while (true)
            {
                var op = (Func<object, object, object>?)_tokeniser.operatorTable[depth][_tokeniser.Token];
                if (op == null)
                {
                    if ((_tokeniser.operatorTable[depth] as BinaryHashtable).NullOperator == null || _tokeniser.Token >= (int)DefaultToken.CloseParens || _tokeniser.Token <= (int)DefaultToken.OpenParens)
                        return lhs;
                    else
                        op = (_tokeniser.operatorTable[depth] as BinaryHashtable).NullOperator;
                }
                else _tokeniser.NextToken();

                Node rhs = ParseOperatorCategory(depth + 1);

                lhs = new BinaryNode(lhs, rhs, op);
            }
        }
        private Node ParseRightAssociativeBinary(int depth)
        {
            Node lhs = ParseOperatorCategory(depth + 1);

            var op = (Func<object, object, object>?)_tokeniser.operatorTable[depth][_tokeniser.Token];
            if (op == null)
            {
                if ((_tokeniser.operatorTable[depth] as BinaryHashtable).NullOperator == null || _tokeniser.Token >= (int)DefaultToken.CloseParens || _tokeniser.Token <= (int)DefaultToken.OpenParens)
                    return lhs;
                else
                    op = (_tokeniser.operatorTable[depth] as BinaryHashtable).NullOperator;
            }
            else _tokeniser.NextToken();

            Node rhs = ParseRightAssociativeBinary(depth);

            return new BinaryNode(lhs, rhs, op);
        }
        private Node ParseNonAssociativeBinary(int depth)
        {
            Node lhs = ParseOperatorCategory(depth + 1);

            var op = (Func<object, object, object>?)_tokeniser.operatorTable[depth][_tokeniser.Token];
            if (op == null)
            {
                if ((_tokeniser.operatorTable[depth] as BinaryHashtable).NullOperator == null || _tokeniser.Token >= (int)DefaultToken.CloseParens || _tokeniser.Token <= (int)DefaultToken.OpenParens)
                    return lhs;
                else
                    op = (_tokeniser.operatorTable[depth] as BinaryHashtable).NullOperator;
            }
            else _tokeniser.NextToken();

            Node rhs = ParseOperatorCategory(depth + 1);

            return new BinaryNode(lhs, rhs, op);
        }
        private Node ParseConsistentPrefixUnary(int depth)
        {
            var op = (Func<object, object>?)_tokeniser.operatorTable[depth][_tokeniser.Token];

            if (op == null)
                return ParseOperatorCategory(depth + 1);

            _tokeniser.NextToken();

            var arg = ParseConsistentPrefixUnary(depth);

            return new UnaryNode(arg, op);
        }
        private Node ParseConsistentPostfixUnary(int depth)
        {

            Node arg = ParseOperatorCategory(depth + 1);

            while (true)
            {
                var op = (Func<object, object>?)_tokeniser.operatorTable[depth][_tokeniser.Token];

                if (op == null)
                    return arg;

                _tokeniser.NextToken();

                arg = new UnaryNode(arg, op);
            }
        } 
        private Node ParseInconsistentPrefixUnary(int depth)
        {
            var op = (Func<object, object>?)_tokeniser.operatorTable[depth][_tokeniser.Token];

            if (op == null)
                return ParseOperatorCategory(depth + 1);

            _tokeniser.NextToken();

            var arg = ParseOperatorCategory(depth + 1);

            return new UnaryNode(arg, op);
        }
        private Node ParseInconsistentPostfixUnary(int depth)
        {
            var arg = ParseOperatorCategory(depth + 1);

            var op = (Func<object, object>?)_tokeniser.operatorTable[depth][_tokeniser.Token];

            if (op == null)
                return arg;

            _tokeniser.NextToken();

            return new UnaryNode(arg, op);
        }
        private Node ParseLeaf()
        {
            Node node;
            switch (_tokeniser.Token)
            {
                case (int)DefaultToken.IntLiteral:
                    node = new LiteralNode(_tokeniser.IntLiteral);
                    _tokeniser.NextToken();
                    return node;
                case (int)DefaultToken.FloatLiteral:
                    node = new LiteralNode(_tokeniser.FloatLiteral);
                    _tokeniser.NextToken();
                    return node;
                case (int)DefaultToken.OpenParens:
                    _tokeniser.NextToken();
                    node = ParseOperatorCategory(0);
                    if (_tokeniser.Token != (int)DefaultToken.CloseParens)
                        throw new Exception("Missing close parenthesis");
                    _tokeniser.NextToken();
                    return node;
                case (int)DefaultToken.Identifier:
                    node = new VariableNode(_tokeniser.Identifier);
                    _tokeniser.NextToken();
                    return node;
                default:
                    throw new Exception($"Unexpected token: {_tokeniser.Token}");
            }
        }

        public static Node Parse(string str, OperatorTable operatorTable)
        {
            return Parse(new Tokeniser(new StringReader(str), operatorTable));
        }

        public static Node Parse(Tokeniser tokeniser)
        {
            Parser parser = new(tokeniser);
            return parser.ParseExpression();
        }
    }
}
