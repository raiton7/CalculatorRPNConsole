namespace CalculatorRPNConsole
{
    class Operator
    {
       public int Priority { set; get; }
       public string Symbol { set; get; }

        public Operator(string symbol = "", int priority = 0)
        {
            Symbol = symbol;
            Priority = priority;
        }
    }
}
