using System;

namespace CalculatorRPNConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Calculator\nCalculate by parsing string and using RPN algorithm");
            string option = "";
            while (true)
            {
                bool exit = false;
                Console.WriteLine("Choose option:");
                Console.WriteLine("1) Calculate and show result\n2) Show possible operations\n0) Quit");
                option = Console.ReadLine();
                switch (option)
                {
                    case "1":
                        Console.Clear();
                        Parser ob = new Parser();
                        string input = "";
                        Console.WriteLine("Enter expression to calculate:");
                        input = Console.ReadLine();
                        ob.Input = input;

                        double result = ob.Parse();
                        if (ob.Flag == false)
                        {
                            Console.WriteLine("\nExpression = {0}\n", result);
                            Console.WriteLine("Expression in postfix:");
                            ob.ShowPostfix();
                            Console.WriteLine();
                        }
                        else
                        {
                            switch (ob.ErrorText)
                            {
                                case "notCorrect":
                                    Console.WriteLine("Expression is not correct");
                                    break;
                                case "wrongChars":
                                    Console.WriteLine("Expression has incorrect characters");
                                    break;
                                case "div":
                                    Console.WriteLine("Division by 0");
                                    break;
                                case "sqrt":
                                    Console.WriteLine("Argument of square root is less than 0");
                                    break;
                                case "ln":
                                    Console.WriteLine("Argument of natural logarithm is less than or equal 0");
                                    break;
                                case "log":
                                    Console.WriteLine("Argument of logarithm base 10 is less than or equal 0");
                                    break;
                                case "logx":
                                    Console.WriteLine("Argument of logarithm base x is less than or equal 0");
                                    break;
                                case "rtx":
                                    Console.WriteLine("Argument of grade x root is less than 0");
                                    break;
                                case "rt0":
                                    Console.WriteLine("Grade x of root is 0");
                                    break;
                                default:
                                    Console.WriteLine("Something went wrong");
                                    break;
                            }
                        }
                        Console.WriteLine("Press any key to continue");
                        Console.ReadKey();
                        break;
                    case "2":
                        Console.Clear();
                        ShowOperations();
                        Console.WriteLine("\nPress any key to continue");
                        Console.ReadKey();
                        break;
                    case "0":
                        Console.Clear();
                        Console.WriteLine("Press any key to quit");
                        Console.ReadKey();
                        exit = true;
                        break;
                    default:
                        Console.WriteLine("There is no such option");
                        Console.WriteLine("Press any key to continue");
                        Console.ReadKey();
                        break;
                }
                Console.Clear();
                if (exit)
                    break;             
            }
        }

        private static void ShowOperations()
        {
            Console.WriteLine("Calculator can do following:");
            Console.WriteLine("+  -  *  /");
            Console.WriteLine("x^y <- x power to y");
            Console.WriteLine("x%y <- x modulo y");
            Console.WriteLine("write 'pi' so it will be replaced to number");
            Console.WriteLine("sin()  cos()  tg()  ctg()");
            Console.WriteLine("log() <- logarithm base 10\nlogx() <- logarithm base x\nln() <- natural logarithm");
            Console.WriteLine("sqrt() <- square root\nrtx() <- root grade x");
        }
    }
}
