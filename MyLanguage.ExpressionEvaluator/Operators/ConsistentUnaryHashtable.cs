using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyLanguage.ExpressionEvaluator_POC.Operators
{
    /* Unary operator priority level which only concerns a single type,
     * and therefore can be applied multiple times to the same input.
     */
    internal class ConsistentUnaryHashtable<T> : Hashtable
    {
        public readonly bool postfix;
        public ConsistentUnaryHashtable(bool postfix = false) : base()
        {
            this.postfix = postfix;
        }
        public Func<object, object>? this[int key] { get => (Func<object, object>?)base[key]; set => base[key] = value; }
        public void Add(int key, Func<T, T> value)
        {
            base.Add(key, (object a) => value((T)a) as object);
        }
    }
}
