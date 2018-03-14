# CalculatorRPNConsole
Calculator written in C# which works as console application, using RPN algorithm
You just put a string line, like in Matlab/Octave/Wolframalpha and program parse it to find numbers, operators and functions. 
Then it makes postfix of expression of user's input. Finally calculate it on the stack.
Classes:
- Operator - it's just a structure of operator or function, has two properties: Symbol and Priority
- Parser - it's where most of program is done
- StackOperator - it's my version of stack, made on Collections.Generic.List<T>, where T is class Operator
- VerifyingCorrectness - functions there can also be inside Parser, but I decided to seperate them, 
  it finds if something in input is wrong
- Program - it has main function which has input operations and show a result of expression or show an error
