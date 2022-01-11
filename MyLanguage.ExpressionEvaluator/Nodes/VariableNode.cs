using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyLanguage.ExpressionEvaluator.Nodes
{
    internal class VariableNode : Node
    {
        readonly string name;
        public VariableNode(string variableName)
        {
            this.name = variableName;
        }
        public override object Eval(IContext ctx)
        {
            return ctx.ResolveVariable(name);
        }
        //public static implicit operator VariableNode<object>(VariableNode<T> node)
        //{
        //    return new VariableNode<object>(node.name);
        //}
        //public static implicit operator VariableNode<T>(VariableNode<object> node)
        //{
        //    return new VariableNode<T>(node.name);
        //}
    }
}
