using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyLanguage.ExpressionEvaluator_POC.Nodes
{
    internal class BinaryNode : Node
    {
        public readonly Node left;
        public readonly Node right;
        public readonly Func<object, object, object> op;
        public BinaryNode(Node left, Node right, Func<object, object, object> op)
        {
            this.left = left;
            this.right = right;
            this.op = op;
        }
        public override object Eval(IContext ctx)
        {
            var lhs = left.Eval(ctx);
            var rhs = right.Eval(ctx);
            return op(lhs, rhs);
        }
        //public static implicit operator BinaryNode<object, object, object>(BinaryNode<I1, I2, O> node)
        //{
        //    return new BinaryNode<object, object, object>(Conv(node._left), Conv(node._right), (a, b) => node.op);
        //}
        //public static implicit operator BinaryNode<I1, I2, O>(BinaryNode<object, object, object> node)
        //{
        //    return new BinaryNode<I1, I2, O>(Conv<I1>(node._left), Conv<I2>(node._right), (a, b) => (O)node.op(a, b));
        //}
    }
}
