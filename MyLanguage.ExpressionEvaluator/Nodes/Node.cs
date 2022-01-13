using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyLanguage.ExpressionEvaluator_POC.Nodes
{
    internal abstract class Node
    {
        public abstract object Eval(IContext ctx);

        //public static Node<_T> Conv<_T>(Node<object> node) => node switch
        //{
        //    LiteralNode<object> literal => (LiteralNode<_T>)literal,
        //    VariableNode<object> variable => (VariableNode<_T>)variable,
        //    BinaryNode<object, object, object> binary => (BinaryNode<_T, _T, _T>)binary,
        //    UnaryNode<object, object> unary => (UnaryNode<_T, _T>)unary,
        //    _ => throw new NotImplementedException(),
        //};
        //public static Node<object> Conv<_T>(Node<_T> node) => node switch
        //{
        //    LiteralNode<_T> literal => (LiteralNode<object>)literal,
        //    VariableNode<_T> variable => (VariableNode<object>)variable,
        //    BinaryNode<_T, _T, _T> binary => (BinaryNode<object, object, object>)binary,
        //    UnaryNode<_T, _T> unary => (UnaryNode<object, object>)unary,
        //    _ => throw new NotImplementedException(),
        //};
    }
}
