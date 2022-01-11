using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyLanguage.ExpressionEvaluator.Operators
{
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
