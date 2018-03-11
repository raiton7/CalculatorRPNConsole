using System;

namespace CalculatorRPNConsole
{
    class VerifyingCorrectness
    {
        public static bool CheckAndDeleteWhiteChars(ref string s)
        {
            for (int i = 0; i < s.Length; i++)
            {
                if (s[i] == ' ' || s[i] == '\t' || s[i] == '\n')
                {
                    if ((s[i - 1] >= '0' && s[i - 1] <= '9') || s[i - 1] == '.' || s[i - 1] == ',')
                    { //if digit or . or ,
                        while (s[i] == ' ' || s[i] == '\t' || s[i] == '\n')
                        {
                            i++;
                            if ((s[i] >= '0' && s[i] <= '9') || s[i] == '.' || s[i] == ',')
                                return false;
                            //so if it's like: "20  . 5" <- that's not correct
                        }
                    }
                    if (s[i - 1] >= 'a' && s[i - 1] <= 'z')
                    { //if chars of function
                        while (s[i] == ' ' || s[i] == '\t' || s[i] == '\n')
                        {
                            i++;
                            if ((s[i] >= 'a' && s[i] <= 'z') || s[i] == '(' || (s[i] >= '0' && s[i] <= '9'))
                                return false;
                            // so if it's like: "c os (...)" or "log  43(...)" <- also not correct
                        }
                    }
                }
            }
            string tmp = "";
            for (int i = 0; i < s.Length; i++)
            {
                if (s[i] != ' ' && s[i] != '\t' && s[i] != '\n')
                    tmp += s[i];
            }
            s = tmp;

            return true;
        }

        public static bool CheckChars(string s)
        {
            // sin cos tg ctg log ln sqrt pi
            char[] tab = {'c', 'g', 'i', 'l', 'n', 'o', 'p', 'q', 'r', 's', 't',
                            '+', '-', '*', '/', '^', '%', '(', ')', ',', '.',
                                '0', '1', '2', '3', '4', '5', '6', '7', '8', '9'};
            int a = 0;
            for (int i = 0; i < s.Length; i++)
            {
                foreach (char key in tab)
                {
                    if (s[i] == key)
                        a++;
                }
                if (a == 0)
                    return false;
                a = 0;
            }
            return true;
        }

        public static bool CheckExpression(ref string s)
        {
            bool flag = false;

            if (s.Length == 2)//input has only added brackets
                return false;

            string tmp = "";
            for (int i = 0; i < s.Length; i++)
            {
                if (s[i] != '.')
                    tmp += s[i];
                else
                    tmp += ',';
            }
            s = tmp;
            //replacing dots to commas
            int openBrackets = 0, closeBrackets = 0;

            for (int i = 0; i < s.Length; i++)
            {
                if (s[i] == '(')
                    openBrackets++;
                else if (s[i] == ')')
                    closeBrackets++;
                if (s[i] == ',')
                {
                    int j = i + 1;
                    if ((s[j] >= '0' && s[j] <= '9') == false)
                        return false;
                    //so there isn't digit after comma
                    while (s[j] >= '0' && s[j] <= '9')
                    {
                        if (s[j + 1] == ',')
                            return false;
                        //so if there are two or more commas in number
                        j++;
                    }
                }
                if (Parser.IsOperator(s[i]) && Parser.IsOperator(s[i + 1]))
                    return false;
                //operator after operator, like "2+*4"
                if (i > 0 && flag == false)
                {
                    if (s[i] >= 'a' && s[i] <= 'z')
                    {
                        int k = i - 1;
                        if ((Parser.IsOperator(s[k]) || s[k] == '(') == false)
                            return false;
                        //there is something different than operator or '(' before function name
                        flag = true;
                    }
                }
                if (s[i] == '(')
                    flag = false;
            }
            if (openBrackets != closeBrackets)
                return false;
            //####################################
            flag = false;
            for (int i = 0; i < s.Length; i++)
            {
                if (s[i] == 's')//sin( or sqrt(
                {
                    if (i > 0)
                    {
                        if (s[i - 1] >= 'a' && s[i - 1] <= 'z')
                            return false;
                        //there is a letter before function name
                    }
                    if (s[i + 1] != 'i' && s[i + 1] != 'q')
                        return false;
                    if (s[i + 1] == 'i' && s[i + 2] != 'n')
                        return false;
                    if (s[i + 1] == 'i' && s[i + 2] == 'n' && s[i + 3] != '(')//sin(
                        return false;
                    //#######################
                    if (s[i + 1] == 'q' && s[i + 2] != 'r')
                        return false;
                    if (s[i + 1] == 'q' && s[i + 2] == 'r' && s[i + 3] != 't')
                        return false;
                    if (s[i + 1] == 'q' && s[i + 2] == 'r' && s[i + 3] == 't' && s[i + 4] != '(')//sqrt(
                        return false;
                    if (s[i + 1] == 'q' && s[i + 2] == 'r' && s[i + 3] == 't' && s[i + 4] == '(')//sqrt(
                        flag = true;
                    if (flag == false)
                        i += 3;
                    else
                    {
                        i += 4;
                        flag = false;
                    }
                    //to prevent checking letters which aren't starting function's name
                    //iterator jump to '('
                }
                if (s[i] == 'c')//cos( or ctg(
                {
                    if (i > 0)
                    {
                        if (s[i - 1] >= 'a' && s[i - 1] <= 'z')
                            return false;
                    }
                    if (s[i + 1] != 'o' && s[i + 1] != 't')
                        return false;
                    if (s[i + 1] == 'o' && s[i + 2] != 's')
                        return false;
                    if (s[i + 1] == 'o' && s[i + 2] == 's' && s[i + 3] != '(')//cos(
                        return false;
                    //#######################
                    if (s[i + 1] == 't' && s[i + 2] != 'g')
                        return false;
                    if (s[i + 1] == 't' && s[i + 2] == 'g' && s[i + 3] != '(')//ctg(
                        return false;

                    i += 3;
                    //both cos and ctg have same length
                }
                if (s[i] == 'l')//ln( or logx(
                {
                    if (i > 0)
                    {
                        if (s[i - 1] >= 'a' && s[i - 1] <= 'z')
                            return false;
                    }
                    if (s[i + 1] != 'n' && s[i + 1] != 'o')
                        return false;
                    if (s[i + 1] == 'n' && s[i + 2] != '(')//ln(
                        return false;
                    //######################
                    if (s[i + 1] == 'o' && s[i + 2] != 'g')
                        return false;
                    if (s[i + 1] == 'o' && s[i + 2] == 'g')//log...
                    {
                        int j = i + 3;
                        if (((s[j] >= '0' && s[j] <= '9') || s[j] == '(') == false)
                            return false;
                        //base of logarithm is not digit or '('
                        else if (s[j] != '(')
                        {
                            while (s[j] >= '0' && s[j] <= '9' || s[j] == ',')
                                j++;
                            if (s[j] != '(')
                                return false;
                            //there is no '(' after logarithm's base
                        }
                        i = j;
                        flag = true;
                    }
                    if (flag == false)
                        i += 2;
                    else
                        flag = false;
                }
                if (s[i] == 't')//tg(
                {
                    if (i > 0)
                    {
                        if (s[i - 1] >= 'a' && s[i - 1] <= 'z')
                            return false;
                    }
                    if (s[i + 1] != 'g')
                        return false;
                    if (s[i + 1] == 'g' && s[i + 2] != '(')//tg(
                        return false;
                    i += 2;
                }
                if (s[i] == 'r')//rtx(
                {
                    if (i > 0)
                    {
                        if (s[i - 1] >= 'a' && s[i - 1] <= 'z')
                            return false;
                    }
                    if (s[i + 1] != 't')
                        return false;
                    if (s[i+1] == 't')
                    {
                        int j = i + 2;
                        if ((s[j] >= '0' && s[j] <= '9') == false)
                            return false;
                        //grade of root is not digit
                        else
                        {
                            while (s[j] >= '0' && s[j] <= '9' || s[j] == ',')
                                j++;
                            if (s[j] != '(')
                                return false;
                            //there is no '(' after root's grade
                        }
                        i = j;
                    }
                }
                if (s[i] == 'p')//pi
                {
                    if (i > 0)
                    {
                        if (s[i - 1] >= 'a' && s[i - 1] <= 'z')
                            return false;
                    }
                    if (s[i + 1] != 'i')
                        return false;
                    if (Parser.IsOperator(s[i + 2]) == false && s[i + 2] != ')')
                        return false;
                    i += 2;
                }
            }
            //####################
            for (int i = 0; i < s.Length - 1; i++)
            {
                if (s[i] == '(')
                {
                    if (s[i + 1] == '-')
                    {
                        s = s.Insert(i, '0'.ToString());
                    }
                    else if (Parser.IsOperator(s[i + 1]))
                        return false;
                }
                if (s[i] == ')' && (s[i + 1] >= 'a' && s[i + 1] <= 'z'))
                    return false;

                if (s[i] == 'p' && s[i + 1] == 'i')
                    s = s.Replace("pi", Math.PI.ToString());

                if (s[i] == '(' && s[i + 1] == ')')
                    return false;
            }
            for (int i = s.Length - 2; i >= 0; i--)
            {
                if (Parser.IsOperator(s[i]))
                    if (s[i + 1] == ')')
                        return false;
            }
            
            //everything is ok so return true
            return true;
        }
    }
}
