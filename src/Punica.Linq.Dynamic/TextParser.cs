
using System.Linq.Expressions;

namespace Punica.Linq.Dynamic
{
    public static class TextParser
    {
        private static readonly Token[] Keys = new[]
        {
            new Token("as", TokenType.Operator, Operator.As),
            new Token("true", TokenType.Boolean, Operator.Unknown),
            new Token("false", TokenType.Boolean, Operator.Unknown),
            new Token("new", TokenType.Operator, Operator.New)
        };

        // TODO: add support for custom function call like MyMethod() since currently it only support Instance.MyMethod(), maybe use this.MyMethod()
        public static Expression[] Evaluate(string expression, IEvaluator evaluator)
        {
            // Convert the expression into a list of tokens
            List<Token> tokens = Tokenize(expression);

            //If there is no tokens return empty array;
            if (tokens.Count == 0)
            {
                return Array.Empty<Expression>();
            }

            // Evaluate the expression using the shunting-yard algorithm
            Stack<Token> outputQueue = new Stack<Token>();
            Stack<Token> operatorStack = new Stack<Token>();
            List<Expression> expressions = new List<Expression>();

            foreach (var token in tokens)
            {
                switch (token.Type)
                {
                    case TokenType.Operator:
                        // Pop operators from the stack until a lower-precedence or left-associative operator is found
                        while (operatorStack.Count > 0 &&
                               (GetPrecedence(token.Operator) < GetPrecedence(operatorStack.Peek().Operator) ||
                                GetPrecedence(token.Operator) == GetPrecedence(operatorStack.Peek().Operator) && IsLeftAssociative(token.Value)))
                        {
                            outputQueue.Push(operatorStack.Pop());
                        }

                        // Push the new operator onto the stack
                        operatorStack.Push(token);
                        break;

                    case TokenType.OpenParen:
                        // Push left parentheses onto the stack
                        operatorStack.Push(token);
                        break;

                    case TokenType.CloseParen:
                        // Pop operators from the stack and add them to the output queue until a left parenthesis is found
                        while (operatorStack.Count > 0 && operatorStack.Peek().Value != "(")
                        {
                            outputQueue.Push(operatorStack.Pop());
                        }

                        // If a left parenthesis was not found, the expression is invalid
                        if (operatorStack.Count == 0)
                        {
                            throw new ArgumentException("Mismatched parentheses");
                        }

                        // Pop the left parenthesis from the stack
                        operatorStack.Pop();
                        break;
                    case TokenType.OpenCurlyParen:
                        // Push left parentheses onto the stack
                        operatorStack.Push(token);
                        outputQueue.Push(token);
                        break;

                    case TokenType.CloseCurlyParen:
                        // Pop operators from the stack and add them to the output queue until a left parenthesis is found
                        while (operatorStack.Count > 0 && operatorStack.Peek().Value != "{")
                        {
                            outputQueue.Push(operatorStack.Pop());
                        }

                        // If a left parenthesis was not found, the expression is invalid
                        if (operatorStack.Count == 0)
                        {
                            throw new ArgumentException("Mismatched curly brackets");
                        }

                        // Pop the left parenthesis from the stack
                        operatorStack.Pop();
                        outputQueue.Push(token);
                        break;

                    case TokenType.Unknown:
                        // Do nothing 
                        break;

                    case TokenType.Sequence:
                        // Pop any remaining operators from the stack and add them to the output queue
                        while (operatorStack.Count > 0)
                        {
                            if (operatorStack.Peek().Value == "(")
                            {
                                throw new ArgumentException("Mismatched parentheses");
                            }

                            outputQueue.Push(operatorStack.Pop());
                        }

                        expressions.Add(Pop(outputQueue, evaluator));
                        break;
                    default:
                        // Push operands onto the output queue
                        outputQueue.Push(token);
                        break;
                }
            }

            // Pop any remaining operators from the stack and add them to the output queue
            while (operatorStack.Count > 0)
            {
                if (operatorStack.Peek().Value == "(")
                {
                    throw new ArgumentException("Mismatched parentheses");
                }

                outputQueue.Push(operatorStack.Pop());
            }

            // Evaluate the postfix expression
            expressions.Add(Pop(outputQueue, evaluator));

            return expressions.ToArray();
        }

        static Expression Pop(Stack<Token> outputQueue, IEvaluator evaluator)
        {
            Stack<object> evaluationStack = new Stack<object>();

            if (outputQueue.Count == 1)
            {
                var exp = evaluator.Single(outputQueue.Pop());
                return exp;
            }

            var reverse = outputQueue.Reverse();


            foreach (var token in reverse)
            {
                switch (token.Operator)
                {
                    case Operator.And:
                        var rightOperand1 = evaluationStack.Pop();
                        var leftOperand1 = evaluationStack.Pop();
                        evaluationStack.Push(evaluator.And(leftOperand1, rightOperand1));
                        break;

                    case Operator.Or:
                        var rightOperand2 = evaluationStack.Pop();
                        var leftOperand2 = evaluationStack.Pop();
                        evaluationStack.Push(evaluator.Or(leftOperand2, rightOperand2));
                        break;

                    case Operator.Not:
                        var operand3 = evaluationStack.Pop();
                        evaluationStack.Push(evaluator.Not(operand3));
                        break;

                    case Operator.LessThan:
                        var rightOperand4 = evaluationStack.Pop();
                        var leftOperand4 = evaluationStack.Pop();
                        evaluationStack.Push(evaluator.LessThan(leftOperand4, rightOperand4));
                        break;

                    case Operator.GreaterThanEqual:
                        var rightOperand5 = evaluationStack.Pop();
                        var leftOperand5 = evaluationStack.Pop();
                        evaluationStack.Push(evaluator.GreaterOrEqual(leftOperand5, rightOperand5));
                        break;

                    case Operator.GreaterThan:
                        var rightOperand6 = evaluationStack.Pop();
                        var leftOperand6 = evaluationStack.Pop();
                        evaluationStack.Push(evaluator.GreaterThan(leftOperand6, rightOperand6));
                        break;

                    case Operator.LessThanEqual:
                        var rightOperand7 = evaluationStack.Pop();
                        var leftOperand7 = evaluationStack.Pop();
                        evaluationStack.Push(evaluator.LessOrEqual(leftOperand7, rightOperand7));
                        break;

                    case Operator.Equal:
                        var rightOperand8 = evaluationStack.Pop();
                        var leftOperand8 = evaluationStack.Pop();
                        evaluationStack.Push(evaluator.Equal(leftOperand8, rightOperand8));
                        break;

                    case Operator.NotEqual:
                        var rightOperand9 = evaluationStack.Pop();
                        var leftOperand9 = evaluationStack.Pop();
                        evaluationStack.Push(evaluator.NotEqual(leftOperand9, rightOperand9));
                        break;

                    case Operator.Plus:
                        var rightOperand10 = evaluationStack.Pop();
                        var leftOperand10 = evaluationStack.Pop();
                        evaluationStack.Push(evaluator.Add(leftOperand10, rightOperand10));
                        break;

                    case Operator.Minus:
                        var rightOperand11 = evaluationStack.Pop();
                        var leftOperand11 = evaluationStack.Pop();
                        evaluationStack.Push(evaluator.Subtract(leftOperand11, rightOperand11));
                        break;

                    case Operator.Multiply:
                        var rightOperand12 = evaluationStack.Pop();
                        var leftOperand12 = evaluationStack.Pop();
                        evaluationStack.Push(evaluator.Multiply(leftOperand12, rightOperand12));
                        break;

                    case Operator.Divide:
                        var rightOperand13 = evaluationStack.Pop();
                        var leftOperand13 = evaluationStack.Pop();
                        evaluationStack.Push(evaluator.Divide(leftOperand13, rightOperand13));
                        break;

                    case Operator.Modulo:
                        var rightOperand14 = evaluationStack.Pop();
                        var leftOperand14 = evaluationStack.Pop();
                        evaluationStack.Push(evaluator.Modulo(leftOperand14, rightOperand14));
                        break;
                    case Operator.Question:
                        var ifFalse = evaluationStack.Pop();
                        var ifTrue = evaluationStack.Pop();
                        var condition = evaluationStack.Pop();
                        evaluationStack.Push(evaluator.Condition(condition, ifTrue, ifFalse));
                        break;
                    case Operator.NullCoalescing:
                        var rightOperand15 = evaluationStack.Pop();
                        var leftOperand15 = evaluationStack.Pop();
                        evaluationStack.Push(evaluator.Coalesce(leftOperand15, rightOperand15));
                        break;
                    case Operator.Colon:
                        break;
                    case Operator.In:
                        var rightOperand16 = evaluationStack.Pop();
                        var leftOperand16 = evaluationStack.Pop();
                        evaluationStack.Push(evaluator.Contains(leftOperand16, rightOperand16));
                        break;
                    //case Operator.Method:
                    //    var rightOperand17 = evaluationStack.Pop();
                    //    var leftOperand17 = evaluationStack.Pop();
                    //    evaluationStack.Push(evaluator.Any(leftOperand17, rightOperand17));
                    //    break;
                    case Operator.New:
                        var rightOperand18 = evaluationStack.Pop();
                        evaluationStack.Push(evaluator.New(rightOperand18));
                        break;
                    case Operator.Dot:

                        var rightOperand19 = evaluationStack.Pop();
                        var leftOperand19 = evaluationStack.Pop();

                        if (evaluationStack.Count > 0 && (evaluationStack.Peek() is Token t1 && t1.Type == TokenType.Member || evaluationStack.Peek() is Expression) )
                        {
                            var member = evaluationStack.Pop();
                            evaluationStack.Push(evaluator.Call(member, leftOperand19, rightOperand19));
                        }
                        else
                        {
                            evaluationStack.Push(evaluator.Dot(leftOperand19, rightOperand19));
                        }

                        break;
                    //case Operator.Sequence:
                    //    break;
                    case Operator.As:
                        var rightOperand20 = evaluationStack.Pop();
                        var leftOperand20 = evaluationStack.Pop();
                        evaluationStack.Push(evaluator.As(leftOperand20, rightOperand20));
                        break;
                    default:
                        evaluationStack.Push(token);
                        break;
                }
            }

            //// The final value on the stack is the result of the expression

            //List<Expression> result = new List<Expression>();

            //while (evaluationStack.Count > 0)
            //{
            //    result.Add((Expression)evaluationStack.Pop());
            //}
            //return result.ToArray();
            outputQueue.Clear();

            return (Expression)evaluationStack.Pop();
        }

        static List<Token> Tokenize(string expression)
        {
            List<Token> tokens = new List<Token>();

            for (int i = 0; i < expression.Length; i++)
            {
                char c = expression[i];

                // Skip whitespace
                if (char.IsWhiteSpace(c))
                {
                    continue;
                }


                // Handle operators
                if (c == ',')
                {
                    tokens.Add(new Token(",", TokenType.Sequence, Operator.Unknown));
                }
                else if (c == '.')
                {
                    tokens.Add(new Token(".", TokenType.Operator, Operator.Dot));
                }
                else if (c == '&' && i + 1 < expression.Length && expression[i + 1] == '&')
                {
                    tokens.Add(new Token("&&", TokenType.Operator, Operator.And));
                    i++;
                }
                else if (c == '|' && i + 1 < expression.Length && expression[i + 1] == '|')
                {
                    tokens.Add(new Token("||", TokenType.Operator, Operator.Or));
                    i++;
                }
                else if (c == '!' && i + 1 < expression.Length && expression[i + 1] == '=')
                {
                    tokens.Add(new Token("!=", TokenType.Operator, Operator.NotEqual));
                    i++;
                }
                else if (c == '!')
                {
                    tokens.Add(new Token("!", TokenType.Operator, Operator.Not));
                }
                else if (c == '<' && i + 1 < expression.Length && expression[i + 1] == '=')
                {
                    tokens.Add(new Token("<=", TokenType.Operator, Operator.LessThanEqual));
                    i++;
                }
                else if (c == '>' && i + 1 < expression.Length && expression[i + 1] == '=')
                {
                    tokens.Add(new Token(">=", TokenType.Operator, Operator.GreaterThanEqual));
                    i++;
                }
                else if (c == '<')
                {
                    tokens.Add(new Token("<", TokenType.Operator, Operator.LessThan));
                }
                else if (c == '>')
                {
                    tokens.Add(new Token(">", TokenType.Operator, Operator.GreaterThan));
                }
                else if (c == '=' && i + 1 < expression.Length && expression[i + 1] == '=')
                {
                    tokens.Add(new Token("==", TokenType.Operator, Operator.Equal));
                    i++;
                }
                else if (c == '=')
                {
                    tokens.Add(new Token("=", TokenType.Operator, Operator.Assignment));
                    i++;
                }
                //else if (c == '(')
                //{
                //    tokens.Add(new Token("(", TokenType.OpenParen, Operator.OpenParen));
                //}
                //else if (c == ')')
                //{
                //    tokens.Add(new Token(")", TokenType.CloseParen, Operator.CloseParen));
                //}
                //else if (c == '{')
                //{
                //    tokens.Add(new Token("{", TokenType.OpenCurlyParen, Operator.OpenCurlyParen));
                //}
                //else if (c == '}')
                //{
                //    tokens.Add(new Token("}", TokenType.CloseCurlyParen, Operator.CloseCurlyParen));
                //}


                else if (c == '+')
                {
                    tokens.Add(new Token("+", TokenType.Operator, Operator.Plus));
                }
                else if (c == '-')
                {
                    tokens.Add(new Token("-", TokenType.Operator, Operator.Minus));
                }
                else if (c == '/')
                {
                    tokens.Add(new Token("/", TokenType.Operator, Operator.Divide));
                }
                else if (c == '*')
                {
                    tokens.Add(new Token("*", TokenType.Operator, Operator.Multiply));
                }
                else if (c == '%')
                {
                    tokens.Add(new Token("%", TokenType.Operator, Operator.Modulo));
                }
                else if (c == '?' && i + 1 < expression.Length && expression[i + 1] == '?')
                {
                    tokens.Add(new Token("??", TokenType.Operator, Operator.NullCoalescing));
                    i++;
                }
                else if (c == '?')
                {
                    tokens.Add(new Token("?", TokenType.Operator, Operator.Question));
                }
                else if (c == ':')
                {
                    tokens.Add(new Token(":", TokenType.Operator, Operator.Colon));
                }
                else if (c == 'i' && i + 1 < expression.Length && expression[i + 1] == 'n')
                {
                    tokens.Add(new Token("in", TokenType.Operator, Operator.In));
                    i++;
                }
                else if (c == '\'' || c == '"')
                {
                    // Handle operands
                    int j = i + 1;

                    while (j < expression.Length && expression[j] != c)
                    {
                        j++;
                    }

                    if (expression[j] != c)
                    {
                        throw new ArgumentException("Input contains invalid string literal.");
                    }

                    var stringVal = expression.Substring(i + 1, j - i - 1);

                    tokens.Add(new Token(stringVal, TokenType.String, Operator.Unknown));

                    i = j; // last character since we starting with next
                }
                else if (c == '(')
                {
                    var count = tokens.Count;

                    if (count > 0)
                    {
                        if (tokens[count - 1].Type == TokenType.Member)
                        {
                            int j = i + 1;

                            int depth = 1;

                            while (j < expression.Length && depth > 0)
                            {
                                switch (expression[j])
                                {
                                    case '(':
                                        depth++;
                                        break;
                                    case ')':
                                        depth--;
                                        break;
                                    default:
                                        break;
                                }

                                j++;
                            }

                            if (depth != 0)
                            {
                                throw new ArgumentException("Input contains mismatched parentheses.");
                            }

                            var innerExp = expression.Substring(i + 1, j - i - 2);

                            //tokens[count - 1] = new Token(tokens[count - 1].Value, TokenType.Operator, Operator.Method); //set previous token as a method
                            tokens.Add(new Token(innerExp, TokenType.String, Operator.Unknown));

                            i = j - 1;

                        }
                        else
                        {
                            tokens.Add(new Token("(", TokenType.OpenParen, Operator.OpenParen));
                        }
                    }
                    else
                    {
                        tokens.Add(new Token("(", TokenType.OpenParen, Operator.OpenParen));
                    }


                }
                else if (c == ')')
                {
                    tokens.Add(new Token(")", TokenType.CloseParen, Operator.CloseParen));
                }
                else if (c == '{')
                {
                    int j = i + 1;

                    int depth = 1;

                    while (j < expression.Length && depth > 0)
                    {
                        switch (expression[j])
                        {
                            case '{':
                                depth++;
                                break;
                            case '}':
                                depth--;
                                break;
                            default:
                                break;
                        }

                        j++;
                    }

                    if (depth != 0)
                    {
                        throw new ArgumentException("Input contains mismatched parentheses.");
                    }

                    var innerExp = expression.Substring(i + 1, j - i - 2);
                    tokens.Add(new Token(innerExp, TokenType.String, Operator.Unknown));
                    i = j - 1;
                }
                else if (Match(i, expression, out var index))
                {
                    var token = Keys[index];
                    tokens.Add(token);
                    i = i + token.Value.Length - 1;
                }
                else
                {
                    bool number = true;
                    bool real = false;

                    // Handle operands
                    int j = i + 1;

                    if (!char.IsDigit(c))
                    {
                        number = false;
                    }

                    int dotIndex = -1;
                    int methodIndex = -1;

                    // TODO : have custom punctuation
                    while (j < expression.Length && !(char.IsWhiteSpace(expression[j]) || (char.IsPunctuation(expression[j]) && !number)))
                    {
                        if (number)
                        {
                            if (!char.IsDigit(expression[j]) && expression[j] != '.')
                            {
                                number = false;
                            }

                            if (expression[j] == '.')
                            {
                                if (real)
                                {
                                    throw new ArgumentException(
                                        $"Invalid number detected {expression.Substring(i, j)}");
                                }

                                real = true;
                            }
                        }
                        //else
                        //{
                        //    //Childrens.any('Male' in Gender && 5 > (1+2) ).any()
                        //    if (expression[j] == '.')
                        //    {
                        //        dotIndex = j;
                        //    }
                        //    else if (expression[j] == '(')
                        //    {
                        //        if (dotIndex < 0 || dotIndex + 1 == j)
                        //        {
                        //            throw new ArgumentException($"Invalid method syntax detected {expression.Substring(i, j)}");
                        //        }

                        //        methodIndex = j;

                        //        int k = j + 1;
                        //        int depth = 1;

                        //        while (k < expression.Length && depth > 0)
                        //        {
                        //            switch (expression[k])
                        //            {
                        //                case '(':
                        //                    depth++;
                        //                    break;
                        //                case ')':
                        //                    depth--;
                        //                    break;
                        //                default:
                        //                    break;
                        //            }

                        //            k++;
                        //        }

                        //        if (depth != 0)
                        //        {
                        //            throw new ArgumentException("Input contains mismatched parentheses.");
                        //        }

                        //        var member = expression.Substring(i, dotIndex - i);
                        //        var method = expression.Substring(dotIndex + 1, methodIndex - dotIndex - 1);
                        //        var innerExp = expression.Substring(methodIndex + 1, k - methodIndex - 2);

                        //        tokens.Add(new Token(member, TokenType.Member, Operator.Unknown));
                        //        tokens.Add(new Token(method, TokenType.Operator, Operator.Method));
                        //        tokens.Add(new Token(innerExp, TokenType.String, Operator.Unknown));
                        //        i = k;
                        //        j = i - 1; //j++ will make j = i since we want to start where we left off instead original like j= i+1;
                        //    }

                        //}

                        j++;
                    }

                    if (i >= expression.Length || i == j)
                    {
                        continue;
                    }

                    var stringVal = expression.Substring(i, j - i);
                    var type = TokenType.Unknown;
                    var op = Operator.Unknown;

                    if (number)
                    {
                        type = real ? TokenType.RealNumber : TokenType.Number;
                    }
                    else if (stringVal.StartsWith('\'') && stringVal.EndsWith('\''))
                    {
                        type = TokenType.String;
                    }
                    else if (stringVal.Equals("true", StringComparison.Ordinal) || stringVal.Equals("false", StringComparison.Ordinal))
                    {
                        type = TokenType.Boolean;
                    }
                    else if (stringVal.Equals("any", StringComparison.Ordinal))
                    {
                        type = TokenType.Operator; // remove this may be??
                        op = Operator.Method;
                    }
                    else if (stringVal.Equals("new", StringComparison.Ordinal))
                    {
                        type = TokenType.Operator; // remove this may be??
                        op = Operator.New;
                    }
                    else if (stringVal.StartsWith('@'))
                    {
                        type = TokenType.Parameter;
                    }
                    //else if (expression[j] == '(')
                    //{
                    //    type = TokenType.Operator;
                    //    op = Operator.Method;
                    //}
                    else
                    {
                        type = TokenType.Member;
                    }


                    tokens.Add(new Token(stringVal.Trim('\'').TrimStart('@'), type, op));
                    i = j - 1;
                }
            }

            return tokens;
        }

        static bool Match(int i, string expression, out int index)
        {
            for (index = 0; index < Keys.Length; index++)
            {
                var token = Keys[index];
                var value = token.Value;
                int j;
                for (j = 0; j < value.Length; j++)
                {
                    int k = i + j;

                    if (k >= expression.Length || expression[k] != value[j])
                    {
                        break;
                    }
                }

                if (j == value.Length)
                {
                    return true;
                }
            }

            return false;
        }




        // Based on https://learn.microsoft.com/en-us/cpp/c-language/precedence-and-order-of-evaluation?view=msvc-170
        // and https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/operators/
        public static int GetPrecedence(Operator op)
        {
            switch (op)
            {

                // Sequential evaluation
                case Operator.Sequence:
                    return -1;

                // Assignment Operators
                case Operator.Assignment:
                    return 0;
                case Operator.AdditionAssignment:
                case Operator.SubtractionAssignment:
                    return 0;
                case Operator.MultiplicationAssignment:
                case Operator.DivisionAssignment:
                case Operator.ModuloAssignment:
                    return 0;
                case Operator.BitwiseAndAssignment:
                case Operator.BitwiseOrAssignment:
                case Operator.BitwiseXorAssignment:
                    return 0;
                case Operator.LeftShiftAssignment:
                case Operator.RightShiftAssignment:
                    return 0;


                // Conditional Operators
                case Operator.Question:
                case Operator.Colon:
                    return 1;

                // Null-coalescing operator
                case Operator.NullCoalescing:
                    return 2;


                // Logical Operators
                case Operator.Or:
                    return 3;
                case Operator.And:
                    return 4;

                // Bitwise Operators
                case Operator.BitwiseOr:
                    return 5;
                case Operator.BitwiseXor:
                    return 6;
                case Operator.BitwiseAnd:
                    return 7;

                // Equality Operators
                case Operator.Equal:
                case Operator.NotEqual:
                    return 8;

                // Relational Operators
                case Operator.GreaterThan:
                case Operator.LessThan:
                case Operator.GreaterThanEqual:
                case Operator.LessThanEqual:
                    return 9;

                // Type Operators
                case Operator.As:
                case Operator.Is:
                    return 9;

                // Collection
                case Operator.In:
                    return 9;

                // Shift
                case Operator.LeftShift:
                case Operator.RightShift:
                    return 10;

                // Arithmetic Operators
                case Operator.Plus:
                case Operator.Minus:
                    return 11;
                case Operator.Multiply:
                case Operator.Divide:
                case Operator.Modulo:
                    return 12;

                // Unary Operators
                case Operator.Tide:
                case Operator.Not:
                    return 13;

                case Operator.Increment:
                case Operator.Decrement:
                    return 14;
                case Operator.OpenParen:
                case Operator.CloseParen:
                    return -2; // while parentheses has higher precedence this handles differently

                case Operator.OpenCurlyParen:
                case Operator.CloseCurlyParen:
                    return -2; // while parentheses has higher precedence this handles differently

                case Operator.Method:
                    return 14;

                case Operator.New:
                case Operator.Dot:
                    return 14;


                default:
                    throw new ArgumentException("Invalid operator", nameof(op));
            }
        }


        static bool IsLeftAssociative(string op)
        {
            switch (op)
            {
                case "&&":
                case "||":
                case "==":
                case "!=":
                case "<":
                case "<=":
                case ">":
                case ">=":
                case "*":
                case "/":
                case "%":
                case "+":
                case "-":
                case "in":
                case "any":
                case "bind":
                case ",":
                case ".":
                    return true;
                case ":":
                case "??":
                // case "?":
                case "=":
                case "++":
                case "--":
                case "!":
                case "new":
                    return false;
                default:
                    throw new ArgumentException($"Invalid operator: {op}");
            }
        }
        
    }
}
