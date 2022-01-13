using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyLanguage.ExpressionEvaluator_POC.Nodes
{
    internal class LiteralNode : Node
    {
        private readonly object _value;
        public LiteralNode(object value)
        {
            _value = value;
        }
        public override object Eval(IContext ctx)
        {
            return _value;
        }
        //public static implicit operator LiteralNode<object>(LiteralNode<T> node)
        //{
        //    return new LiteralNode<object>(node._value);
        //}
        //public static implicit operator LiteralNode<T>(LiteralNode<object> node)
        //{
        //    return new LiteralNode<T>((T)node._value);
        //}
    }
}
