using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyLanguage.ExpressionEvaluator_POC.Operators
{
    internal class OperatorTable
    {
        private readonly List<Hashtable> _operatorCategories = new();
        private readonly HashSet<string> _operatorNames = new(new string[]{"(", ")", "true", "false"});
        public OperatorTable() { }
        public int Count { get { return _operatorCategories.Count;} }
        public Hashtable this[int priority] { get => _operatorCategories[priority]; set => _operatorCategories[priority] = value; }
        #region Add Category
        public void AddInconsistentUnaryCategory(bool postFix = false)
        {
            InconsistentUnaryHashtable operatorCategory = new(postFix);
            _operatorCategories.Add(operatorCategory);
        }
        public void AddConsistentUnaryCategory<T>(bool postfix = false)
        {
            ConsistentUnaryHashtable<T> operatorCategory = new(postfix);
            _operatorCategories.Add(operatorCategory);
        }
        public void AddAssociativeBinaryCategory<T>(bool rightAssociative = false)
        {
            AssociativeBinaryHashtable<T> operatorCategory = new(rightAssociative);
            _operatorCategories.Add(operatorCategory);
        }
        public void AddNonAssociativeBinaryCategory()
        {
            InconsistentBinaryHashtable operatorCategory = new();
            _operatorCategories.Add(operatorCategory);
        }
        public void AddImpliedAssociativeBinaryCategory<I, O>(Func<O, O, O> impliedOperation, bool rightAssociative = false)
        {
            ImpliedAssociativeBinaryHashtable<I, O> operatorCategory = new(impliedOperation, rightAssociative);
            _operatorCategories.Add(operatorCategory);
        }
        #endregion
        #region Insert Category
        public void InsertInconsistentUnaryCategory(bool postFix, int priority)
        {
            InconsistentUnaryHashtable operatorCategory = new(postFix);
            _operatorCategories.Insert(priority, operatorCategory);
        }
        public void InsertConsistentUnaryCategory<T>(bool postFix, int priority)
        {
            ConsistentUnaryHashtable<T> operatorCategory = new(postFix);
            _operatorCategories.Insert(priority, operatorCategory);
        }
        public void InsertAssociativeBinaryCategory<T>(bool rightAssociative, int priority)
        {
            AssociativeBinaryHashtable<T> operatorCategory = new(rightAssociative);
            _operatorCategories.Insert(priority, operatorCategory);
        }
        public void InsertNonAssociativeBinaryCategory(int priority)
        {
            InconsistentBinaryHashtable operatorCategory = new();
            _operatorCategories.Insert(priority, operatorCategory);
        }
        public void InsertImpliedAssociativeBinaryCategory<I, O>(Func<O, O, O> impliedOperation, bool rightAssociative, int priority)
        {
            ImpliedAssociativeBinaryHashtable<I, O> operatorCategory = new(impliedOperation, rightAssociative);
            _operatorCategories.Insert(priority, operatorCategory);
        }
        #endregion
        #region Add Operator
        public void AddInconsistentUnaryOperator<I, O>(string symbol, Func<I, O> op, int priority = -1)
        {
            if (priority == -1) priority = _operatorCategories.Count - 1;
            Hashtable operatorCategory = _operatorCategories[priority];
            Console.WriteLine($"{symbol.GetHashCode():D10}\t: Symbol {symbol} setup");
            if (operatorCategory is InconsistentUnaryHashtable inconsistentUnaryCategory)
                inconsistentUnaryCategory.Add(symbol.GetHashCode(), op);
            else
                throw new ArgumentException("operator category at that priority is not compatible with the supplied operator.");
            _operatorNames.Add(symbol);
        }
        public void AddConsistentUnaryOperator<T>(string symbol, Func<T, T> op, int priority = -1)
        {
            if (priority == -1) priority = _operatorCategories.Count - 1;
            Hashtable operatorCategory = _operatorCategories[priority];
            Console.WriteLine($"{symbol.GetHashCode():D10}\t: Symbol {symbol} setup");
            if (operatorCategory is ConsistentUnaryHashtable<T> consistentUnaryCategory)
                consistentUnaryCategory.Add(symbol.GetHashCode(), op);
            else
                throw new ArgumentException("operator category at that priority is not compatible with the supplied operator.");
            _operatorNames.Add(symbol);
        }
        public void AddAssociativeBinaryOperator<T>(string symbol, Func<T, T, T> op, int priority = -1)
        {
            if (priority == -1) priority = _operatorCategories.Count - 1;
            Hashtable operatorCategory = _operatorCategories[priority];
            Console.WriteLine($"{symbol.GetHashCode():D10}\t: Symbol {symbol} setup");
            if (operatorCategory is AssociativeBinaryHashtable<T> associativeBinaryCategory)
                associativeBinaryCategory.Add(symbol.GetHashCode(), op);
            else
                throw new ArgumentException("operator category at that priority is not compatible with the supplied operator.");
            _operatorNames.Add(symbol);
        }
        public void AddNonAssociativeBinaryOperator<I1, I2, O>(string symbol, Func<I1, I2, O> op, int priority = -1)
        {
            if (priority == -1) priority = _operatorCategories.Count - 1;
            Hashtable operatorCategory = _operatorCategories[priority];
            Console.WriteLine($"{symbol.GetHashCode():D10}\t: Symbol {symbol} setup");
            if (operatorCategory is InconsistentBinaryHashtable nonAssociativeBinaryCategory)
                nonAssociativeBinaryCategory.Add(symbol.GetHashCode(), op);
            else
                throw new ArgumentException("operator category at that priority is not compatible with the supplied operator.");
            _operatorNames.Add(symbol);
        }
        public void AddImplicativeAssociativeBinaryOperator<I, O>(string symbol, Func<I, I, O> op, int priority = -1)
        {
            if (priority == -1) priority = _operatorCategories.Count - 1;
            Hashtable operatorCategory = _operatorCategories[priority];
            Console.WriteLine($"{symbol.GetHashCode():D10}\t: Symbol {symbol} setup");
            if (operatorCategory is ImpliedAssociativeBinaryHashtable<I, O> implicitAssociativeBinaryCategory)
                implicitAssociativeBinaryCategory.Add(symbol.GetHashCode(), op);
            else
                throw new ArgumentException("operator category at that priority is not compatible with the supplied operator.");
            _operatorNames.Add(symbol);
        }
        public void SetAssociativeNullBinaryOperator<T>(Func<T, T, T> op, int priority = -1)
        {
            if (priority == -1) priority = _operatorCategories.Count - 1;
            Hashtable operatorCategory = _operatorCategories[priority];
            if (operatorCategory is AssociativeBinaryHashtable<T> associativeBinaryCategory)
                associativeBinaryCategory.SetNullOperator(op);
            else
                throw new ArgumentException("operator category at that priority is not compatible with the supplied operator.");
        }
        public void SetNonAssociativeNullBinaryOperator<I1, I2, O>(Func<I1, I2, O> op, int priority = -1)
        {
            if (priority == -1) priority = _operatorCategories.Count - 1;
            Hashtable operatorCategory = _operatorCategories[priority];
            if (operatorCategory is InconsistentBinaryHashtable nonAssociativeBinaryCategory)
                nonAssociativeBinaryCategory.SetNullOperator(op);
            else
                throw new ArgumentException("operator category at that priority is not compatible with the supplied operator.");
        }
        #endregion
        public int PossibleCount(string symbol)
        {
            int count = 0;
            foreach (string name in _operatorNames)
            {
                if (name.StartsWith(symbol))
                    count++;
            }
            return count;
        }
        public bool OperatorExists(string symbol)
        {
            return _operatorNames.Contains(symbol);
        }
    }
}
