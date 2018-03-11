using System.Collections.Generic;

namespace CalculatorRPNConsole
{
    class StackOperator
    {
        private List<Operator> listOperator = new List<Operator>();
        public int Count
        {
            get { return listOperator.Count; }
        }

        public void Push(string sym, int prior)
        {
            Operator op = new Operator(sym, prior);
            listOperator.Insert(0, op);
        }

        public string Pop()
        {
            if (IsEmpty() == false)
            {
                string s = listOperator[0].Symbol;
                listOperator.RemoveAt(0);
                return s;
            }
            else
                return "";
        }

        public string SymbolAt(int i)
        {
            return listOperator[i].Symbol;
        }

        public int PriorityAt(int i)
        {
            return listOperator[i].Priority;
        }

        public void Remove(string s)
        {
            for (int i = 0; i < listOperator.Count; i++)
            {
                if (listOperator[i].Symbol == s)
                {
                    listOperator.RemoveAt(i);
                    break;
                }
            }
        }

        private bool IsEmpty()
        {
            if (listOperator.Count == 0)
                return true;
            else
                return false;
        }
    }
}
