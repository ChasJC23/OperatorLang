using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyLanguage.ExpressionEvaluator
{
    internal class BaseContext : IContext
    {
        public dynamic ResolveVariable(string identifier)
        {
            if (identifier == "x") return 5.0f;
            throw new NotImplementedException();
        }
        public double CallFunction(string identifier, double[] arguments)
        {
            throw new NotImplementedException();
        }
    }
}
