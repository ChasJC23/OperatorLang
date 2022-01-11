using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyLanguage.ExpressionEvaluator
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
