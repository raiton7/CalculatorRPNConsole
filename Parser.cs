using System;
using System.Collections.Generic;

namespace CalculatorRPNConsole
{
    class Parser
    {
        // fields and properties
        private string input;
        private bool flag;
        private string errorText;
        private List<string> listInput;
        private List<string> listRpn;
        public string Input
        {
            set { input = value; }
        }
        public bool Flag
        {
            get { return flag; }
        }
        public string ErrorText
        {
            get { return errorText; }
        }

        // methods
        public Parser()
        {
            flag = false;
            input = "";
            errorText = "";
            listInput = new List<string>();
            listRpn = new List<string>();
        }

        public static bool IsOperator(char c)
        {
            if (c == '+' || c == '-' || c == '*' || c == '/' || c == '^' || c == '%')
                return true;
            return false;
        }

        private bool IsFunction(string s)
        {
            if (s == "sin" || s == "cos" || s == "tg" || s == "ctg" || s == "sqrt" ||
                s == "ln" || s == "log")
            {
                return true;
            }
            else if (s[0] == 'l' && s[1] == 'o' && s[2] == 'g' && (s[3] >= '0' && s[3] <= '9'))
            {
                return true;
            }
            else if (s[0] == 'r' && s[1] == 't' && (s[2] >= '0' && s[2] <= '9'))
            {
                return true;
            }
            else
                return false;
        }

        private void MakeParts()
        {
            string s = "";
            for (int i = 0; i < input.Length; i++)
            {
                if (input[i] >= '0' && input[i] <= '9') //digit
                {
                    while (input[i] >= '0' && input[i] <= '9' || input[i] == ',')
                    {
                        s += input[i];
                        i++;
                    }
                    listInput.Add(s);
                    s = "";
                }
                if (input[i] >= 'a' && input[i] <= 'z') //function
                {
                    while (input[i] != '(')
                    {
                        s += input[i];
                        i++;
                    }
                    listInput.Add(s);
                    s = "";
                }
                if (IsOperator(input[i]) || input[i] == '(' || input[i] == ')') //operator
                {
                    listInput.Add(input[i].ToString());
                }
            }
        }

        private void AddBrackets()
        {
            input = input.Insert(0, "(");
            input = input.Insert(input.Length, ")");
        }

        private void ToRPN()
        {
            StackOperator stackOper = new StackOperator();

            for (int i = 0; i < listInput.Count; i++)
            {
                //Pushing numbers to listRpn and operators to Stack
                if ((listInput[i])[0] >= '0' && (listInput[i])[0] <= '9') //number
                {
                    listRpn.Add(listInput[i]);
                }
                if (listInput[i] == "(")
                {
                    //priority of operator 0
                    stackOper.Push(listInput[i], 0);
                }
                if (listInput[i] == ")")
                {
                    for (int j = 0; j < stackOper.Count; j++)
                    {
                        if (stackOper.SymbolAt(j) == "(")
                        {
                            stackOper.Remove("(");
                            break;
                        }
                        else
                        {
                            listRpn.Add(stackOper.Pop());
                            j--; //decrement index j because Pop() removes element
                        }
                    }
                }

                if (IsOperator(listInput[i][0]) || IsFunction(listInput[i]))
                {
                    Operator op = new Operator();
                    if (listInput[i] == "+" || listInput[i] == "-")
                    { //priority 1
                        op.Priority = 1;
                        op.Symbol = listInput[i];
                    }
                    if (listInput[i] == "*" || listInput[i] == "/" || listInput[i] == "%")
                    { //priority 2
                        op.Priority = 2;
                        op.Symbol = listInput[i];
                    }
                    if (listInput[i] == "^")
                    { //priority 3
                        op.Priority = 3;
                        op.Symbol = listInput[i];
                    }
                    if (listInput[i] == "!")
                    { //priority 4
                        op.Priority = 4;
                        op.Symbol = listInput[i];
                    }
                    if (IsFunction(listInput[i]))
                    { //priority 5
                        op.Priority = 5;
                        op.Symbol = listInput[i];
                    }

                    //now working as priority of operator
                    if (stackOper.Count == 0 || op.Priority > stackOper.PriorityAt(0))
                    {
                        stackOper.Push(op.Symbol, op.Priority);
                    }
                    else
                    {
                        for (int j = 0; j < stackOper.Count; j++)
                        {
                            if (stackOper.SymbolAt(j) == "(")
                            {
                                break;
                            }
                            if (stackOper.PriorityAt(j) >= op.Priority)
                            {
                                listRpn.Add(stackOper.Pop()); //operator from stack to listRpn
                                j--;
                            }
                        }
                        stackOper.Push(op.Symbol, op.Priority); //read operator to listRpn
                    }
                }
            }
            for (int j = 0; j < stackOper.Count; j++)
            {
                listRpn.Add(stackOper.Pop());
                j--;
                //rest of operators to listRpn
            }
        }

        private double Calculate()
        {
            Stack<double> SNumber = new Stack<double>();
            double tmp = 0.0;
            for (int i = 0; i < listRpn.Count; i++)
            {
                if (listRpn[i][0] >= '0' && listRpn[i][0] <= '9')
                {
                    SNumber.Push(Convert.ToDouble(listRpn[i]));
                    
                }
                else if (listRpn[i] == "+")
                {
                    SNumber.Push(SNumber.Pop() + SNumber.Pop());
                }
                else if (listRpn[i] == "-")
                {
                    SNumber.Push(-SNumber.Pop() + SNumber.Pop());
                }
                else if (listRpn[i] == "*")
                {
                    SNumber.Push(SNumber.Pop() * SNumber.Pop());
                }
                else if (listRpn[i] == "/")
                {
                    tmp = SNumber.Pop();
                    if (tmp == 0.0)
                    {
                        flag = true;
                        errorText = "div";
                        return 0.0;
                    }
                    SNumber.Push(SNumber.Pop() / tmp);
                }
                else if (listRpn[i] == "^")
                {
                    tmp = SNumber.Pop();
                    SNumber.Push(Math.Pow(SNumber.Pop(), tmp));
                }
                else if (listRpn[i] == "%")
                {
                    tmp = SNumber.Pop();
                    SNumber.Push(SNumber.Pop() % tmp);
                }
                else if (listRpn[i] == "sin")
                {
                    SNumber.Push(Math.Sin(SNumber.Pop()));
                }
                else if (listRpn[i] == "cos")
                {
                    SNumber.Push(Math.Cos(SNumber.Pop()));
                }
                else if (listRpn[i] == "tg")
                {
                    SNumber.Push(Math.Tan(SNumber.Pop()));
                }
                else if (listRpn[i] == "ctg")
                {
                    SNumber.Push(1 / Math.Tan(SNumber.Pop()));
                }
                else if (listRpn[i] == "sqrt")
                {
                    tmp = SNumber.Pop();
                    if (tmp < 0.0)
                    {
                        flag = true;
                        errorText = "sqrt";
                        return 0.0;
                    }
                    else
                        SNumber.Push(Math.Sqrt(tmp));
                }
                else if (listRpn[i] == "ln")
                {
                    tmp = SNumber.Pop();
                    if (tmp <= 0.0)
                    {
                        flag = true;
                        errorText = "ln";
                        return 0.0;
                    }
                    else
                        SNumber.Push(Math.Log(tmp));
                }
                else if (listRpn[i] == "log")
                {
                    tmp = SNumber.Pop();
                    if (tmp <= 0.0)
                    {
                        flag = true;
                        errorText = "log";
                        return 0.0;
                    }
                    else
                        SNumber.Push(Math.Log10(tmp));
                }
                else if (listRpn[i][0] == 'l' && listRpn[i][1] == 'o' && listRpn[i][2] == 'g' && 
                    (listRpn[i][3] >= '0' && listRpn[i][3] <= '9'))//log x base
                {
                    tmp = SNumber.Pop();
                    string s = "";
                    for (int j = 3; j < listRpn[i].Length; j++)
                        s += listRpn[i][j];
                    if (tmp <= 0.0)
                    {
                        flag = true;
                        errorText = "logx";
                        return 0.0;
                    }
                    else
                        SNumber.Push(Math.Log10(tmp) / Math.Log10(Convert.ToDouble(s)));
                }
                else if (listRpn[i][0] == 'r' && listRpn[i][1] == 't' &&
                    (listRpn[i][2] >= '0' && listRpn[i][2] <= '9'))//root grade x
                {
                    tmp = SNumber.Pop();
                    string s = "";
                    for (int j = 2; j < listRpn[i].Length; j++)
                        s += listRpn[i][j];
                    if (tmp < 0.0)
                    {
                        flag = true;
                        errorText = "rtx";
                        return 0.0;
                    }
                    else if (Convert.ToDouble(s) == 0.0)
                    {
                        flag = true;
                        errorText = "rt0";
                        return 0.0;
                    }
                    else
                        SNumber.Push(Math.Pow(tmp, 1/Convert.ToDouble(s)));
                }
            }

            return SNumber.Pop();
        }

        public double Parse()
        {
            AddBrackets();
            if (VerifyingCorrectness.CheckAndDeleteWhiteChars(ref input) == false)
            {
                errorText = "notCorrect";
                flag = true;
                return 0.0;
            }
            if (VerifyingCorrectness.CheckChars(input) == false)
            {
                errorText = "wrongChars";
                flag = true;
                return 0.0;
            }
            if (VerifyingCorrectness.CheckExpression(ref input) == false)
            {
                errorText = "notCorrect";
                flag = true;
                return 0.0;
            }
            MakeParts();
            ToRPN();
            return Calculate();
        }

        public void ShowPostfix()
        {
            foreach (var key in listRpn)
            {
                Console.Write("{0} ", key);
            }
            Console.WriteLine();
        }
    }
}
