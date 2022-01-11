using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyLanguage.ExpressionEvaluator.Operators
{
    /* Unary operator priority level for differing input and output types.
     * Multiple operators of differing types can be on the same level,
     * yet lack of associativity means only one operation on this level
     * will be successfully parsed on a given branch.
     */
    internal class InconsistentUnaryHashtable : Hashtable
    {
        public readonly bool postfix;
        public InconsistentUnaryHashtable(bool postfix = false) : base()
        {
            this.postfix = postfix;
        }
        public Func<object, object>? this[int key] { get => (Func<object, object>?)base[key]; set => base[key] = value; }
        public void Add<I, O>(int key, Func<I, O> value)
        {
            base.Add(key, (object a) => value((I)a) as object);
        }
    }
}
