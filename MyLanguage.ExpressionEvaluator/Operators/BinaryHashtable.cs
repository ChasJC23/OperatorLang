using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyLanguage.ExpressionEvaluator_POC.Operators
{
    /* Base class which represents priority level for vast majority of binary operators.
     * Includes a null operator which is used when two arguments are left adjacent in an expression.
     * eg. 3x = 3 * x.
     */
    internal class BinaryHashtable : Hashtable
    {
        public BinaryHashtable() : base()
        {
        }
        public Func<object, object, object>? NullOperator { get; private set; }
        public void SetNullOperator<I1, I2, O>(Func<I1, I2, O> op)
        {
            this.NullOperator = (object a, object b) => op((I1)a, (I2)b) as object;
        }
    }
}
