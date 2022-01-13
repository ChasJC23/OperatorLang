using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyLanguage.ExpressionEvaluator_POC
{
    internal enum DefaultToken
    {
        // this ordering *is* intentional, think of it as (...)\0
        OpenParens,
        IntLiteral,
        FloatLiteral,
        BoolLiteral,
        CharLiteral,
        Identifier,
        CloseParens,
        EOF,
    }
}
