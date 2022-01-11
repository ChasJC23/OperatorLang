using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyLanguage.ExpressionEvaluator.Operators
{
    /* Binary operator priority for less ordinary operators which imply some other operation when repeated.
     * For example, 2 < 5 < 8 implies 2 < 5 ∧ 5 < 8.
     * Can support multiple different operators,
     * which can lead to some unusual behaviour.
     * Should probably be used for transitive relations.
     */
    internal class ImpliedAssociativeBinaryHashtable<I, O> : Hashtable
    {
        public readonly bool rightAssociative;
        public ImpliedAssociativeBinaryHashtable(Func<O, O, O> impliedOperation, bool rightAssociative) : base()
        {
            this.ImpliedOperation = (object a, object b) => impliedOperation((O)a, (O)b) as object;
            this.rightAssociative = rightAssociative;
        }
        public Func<object, object, object> ImpliedOperation { get; private set; }
        public void SetImpliedOperation(Func<O, O, O> impliedOperation)
        {
            ImpliedOperation = (object a, object b) => impliedOperation((O)a, (O)b) as object;
        }
        public void Add(int key, Func<I, I, O> value)
        {
            base.Add(key, (object a, object b) => value((I)a, (I)b) as object);
        }
    }
}
