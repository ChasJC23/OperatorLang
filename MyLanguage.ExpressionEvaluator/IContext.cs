using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyLanguage.ExpressionEvaluator_POC
{
    internal interface IContext
    {
        dynamic ResolveVariable(string variableName);
        double CallFunction(string functionName, double[] arguments);
    }
}
