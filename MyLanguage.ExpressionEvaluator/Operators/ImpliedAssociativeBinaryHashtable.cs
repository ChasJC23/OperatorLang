using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyLanguage.ExpressionEvaluator.Operators
{
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
