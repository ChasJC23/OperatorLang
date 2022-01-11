using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyLanguage.ExpressionEvaluator.Nodes
{
    internal class UnaryNode : Node
    {
        public readonly Node arg;
        private readonly Func<object, object> op;
        public UnaryNode(Node arg, Func<object, object> op)
        {
            this.arg = arg;
            this.op = op;
        }
        public override object Eval(IContext ctx)
        {
            var inpVal = arg.Eval(ctx);
            return op(inpVal);
        }
        //public static implicit operator UnaryNode<object, object>(UnaryNode<I, O> node)
        //{
        //    return new UnaryNode<object, object>(Conv(node.arg), (a) => node.op);
        //}
        //public static implicit operator UnaryNode<I, O>(UnaryNode<object, object> node)
        //{
        //    return new UnaryNode<I, O>(Conv<I>(node.arg), (a) => (O)node.op(a));
        //}
    }
}
