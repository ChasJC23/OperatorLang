using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyLanguage.ExpressionEvaluator_POC.Operators
{
    /* Binary operator priority level which only conerns a single type,
     * and therefore can support some form of associativity.
     */
    internal class AssociativeBinaryHashtable<T> : BinaryHashtable
    {
        public readonly bool rightAssociative;
        public AssociativeBinaryHashtable(bool rightAssociative) : base()
        {
            this.rightAssociative = rightAssociative;
        }
        public Func<object, object, object>? this[int key] { get => (Func<object, object, object>?)base[key]; set => base[key] = value; }
        public void Add(int key, Func<T, T, T> value)
        {
            base.Add(key, (object a, object b) => value((T)a, (T)b) as object);
        }
    }
}
