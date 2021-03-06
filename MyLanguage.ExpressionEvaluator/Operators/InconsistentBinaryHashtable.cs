using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyLanguage.ExpressionEvaluator_POC.Operators
{
    /* Binary operator priority for differing types.
     * Will be more useful for derived types such as matrices.
     * Multiple operators of differing types can be on the same level,
     * yet lack of associativity means only one operation on this level
     * will be successfully parsed on a given branch.
     */
    internal class InconsistentBinaryHashtable : BinaryHashtable
    {
        public InconsistentBinaryHashtable() : base()
        {
        }
        public Func<object, object, object>? this[int key] { get => (Func<object, object, object>?)base[key]; set => base[key] = value; }
        public void Add<I1, I2, O>(int key, Func<I1, I2, O> value)
        {
            base.Add(key, (object a, object b) => value((I1)a, (I2)b) as object);
        }
    }
}
