namespace Punica.Linq.Dynamic
{
    public class Tokenizer
    {
        private static readonly Token[] Keys = new[]
        {
            new Token("in", TokenType.Operator, Operator.In),
            new Token("as", TokenType.Operator, Operator.As),
            new Token("true", TokenType.Boolean, Operator.Unknown),
            new Token("false", TokenType.Boolean, Operator.Unknown),
            new Token("new", TokenType.Operator, Operator.New)
        };

        public static List<Token> Tokenize(string expression)
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

                switch (c)
                {
                    case '!':
                        if (i + 1 < expression.Length && expression[i + 1] == '=')
                        {
                            tokens.Add(new Token("!=", TokenType.Operator, Operator.NotEqual));
                            i++;
                        }
                        else
                        {
                            tokens.Add(new Token("!", TokenType.Operator, Operator.Not));
                        }

                        break;
                    case '"':
                        i = AddString(expression, i, c, tokens);
                        break;
                    case '#':
                    case '$':
                        throw new NotImplementedException($"Character '{c}' not supported");
                    case '%':
                        tokens.Add(new Token("%", TokenType.Operator, Operator.Modulo));
                        break;
                    case '&':
                        if (i + 1 < expression.Length && expression[i + 1] == '&')
                        {
                            tokens.Add(new Token("&&", TokenType.Operator, Operator.And));
                            i++;
                        }
                        else
                        {
                            tokens.Add(new Token("&", TokenType.Operator, Operator.BitwiseAnd));
                        }

                        break;
                    case '\'':
                        i = AddString(expression, i, c, tokens);
                        break;
                    case '(':
                        i = AddOpenParenthsesOrMethodContent(expression, tokens, i);
                        break;
                    case ')':
                        tokens.Add(new Token(")", TokenType.CloseParen, Operator.CloseParen));
                        break;
                    case '*':
                        tokens.Add(new Token("*", TokenType.Operator, Operator.Multiply));
                        break;
                    case '+':
                        tokens.Add(new Token("+", TokenType.Operator, Operator.Plus));
                        break;
                    case ',':
                        tokens.Add(new Token(",", TokenType.Sequence, Operator.Unknown));
                        break;
                    case '-':
                        tokens.Add(new Token("-", TokenType.Operator, Operator.Minus));
                        break;
                    case '.':
                        tokens.Add(new Token(".", TokenType.Operator, Operator.Dot));
                        break;
                    case '/':
                        tokens.Add(new Token("/", TokenType.Operator, Operator.Divide));
                        break;
                    case '0':
                    case '1':
                    case '2':
                    case '3':
                    case '4':
                    case '5':
                    case '6':
                    case '7':
                    case '8':
                    case '9':
                        i = AddNumber(expression, i, tokens);
                        break;
                    case ':':
                        tokens.Add(new Token(":", TokenType.Operator, Operator.Colon));
                        break;
                    case ';':
                        throw new NotImplementedException($"Character ';' not supported");
                    case '<':
                        if (i + 1 < expression.Length && expression[i + 1] == '=')
                        {
                            tokens.Add(new Token("<=", TokenType.Operator, Operator.LessThanEqual));
                            i++;
                        }
                        else
                        {
                            tokens.Add(new Token("<", TokenType.Operator, Operator.LessThan));
                        }

                        break;
                    case '=':
                        if (i + 1 < expression.Length && expression[i + 1] == '=')
                        {
                            tokens.Add(new Token("==", TokenType.Operator, Operator.Equal));
                            i++;
                        }
                        else
                        {
                            tokens.Add(new Token("=", TokenType.Operator, Operator.Assignment));
                        }

                        break;
                    case '>':
                        if (i + 1 < expression.Length && expression[i + 1] == '=')
                        {
                            tokens.Add(new Token(">=", TokenType.Operator, Operator.GreaterThanEqual));
                            i++;
                        }
                        else
                        {
                            tokens.Add(new Token(">", TokenType.Operator, Operator.GreaterThan));
                        }

                        break;
                    case '?':
                        if (i + 1 < expression.Length && expression[i + 1] == '?')
                        {
                            tokens.Add(new Token("??", TokenType.Operator, Operator.NullCoalescing));
                            i++;
                        }
                        else
                        {
                            tokens.Add(new Token("?", TokenType.Operator, Operator.Question));
                        }

                        break;
                    case '@':
                        i = AddParameter(expression, i, tokens);
                        break;
                    case '[':
                        tokens.Add(new Token("[", TokenType.OpenBracket, Operator.Unknown));
                        break;
                    case ']':
                        tokens.Add(new Token("[", TokenType.CloseBracket, Operator.Unknown));
                        break;
                    case '^':
                        throw new NotImplementedException($"Character '^' not supported");
                    case '{':
                        i = AddCurlyParenthesesContent(expression, i, tokens);
                        break;
                    case '|':
                        if (i + 1 < expression.Length && expression[i + 1] == '|')
                        {
                            tokens.Add(new Token("||", TokenType.Operator, Operator.Or));
                            i++;
                        }
                        else
                        {
                            tokens.Add(new Token(">", TokenType.Operator, Operator.BitwiseOr));
                        }

                        break;
                    default:
                        if (Match(i, expression, out var index))
                        {
                            var token = Keys[index];
                            tokens.Add(token);
                            i = i + token.Value.Length - 1;
                        }
                        else
                        {
                            // Handle operands
                            int j = i + 1;


                            // TODO : have custom punctuation
                            while (j < expression.Length &&
                                   !(char.IsWhiteSpace(expression[j]) || (char.IsPunctuation(expression[j]))))
                            {
                                j++;
                            }

                            if (i >= expression.Length || i == j)
                            {
                                continue;
                            }

                            var stringVal = expression.Substring(i, j - i);

                            tokens.Add(new Token(stringVal, TokenType.Member, Operator.Unknown));
                            i = j - 1;
                        }

                        break;

                }

            }

            return tokens;
        }

        private static bool Match(int i, string expression, out int index)
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

        private static int AddCurlyParenthesesContent(string expression, int i, List<Token> tokens)
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
            return i;
        }

        private static int AddParameter(string expression, int i, List<Token> tokens)
        {
            int j = i + 1;
            while (j < expression.Length && !(char.IsWhiteSpace(expression[j])))
            {
                j++;
            }

            var stringVal = expression.Substring(i, j - i);

            tokens.Add(new Token(stringVal, TokenType.Parameter, Operator.Unknown));
            i = j - 1;

            return i;
        }

        private static int AddNumber(string expression, int i, List<Token> tokens)
        {
            int j = i + 1;
            var real = false;

            while (j < expression.Length && !(char.IsWhiteSpace(expression[j])))
            {
                if (!char.IsDigit(expression[j]) && expression[j] != '.')
                {
                    throw new ArgumentException($"Invalid number detected {expression.Substring(i, j)}");
                }

                if (expression[j] == '.')
                {
                    if (real)
                    {
                        throw new ArgumentException($"Invalid number detected {expression.Substring(i, j)}");
                    }

                    real = true;
                }

                j++;
            }

            var stringVal = expression.Substring(i, j - i);

            var type = TokenType.Number;
            if (real)
            {
                type = TokenType.RealNumber;
            }

            tokens.Add(new Token(stringVal, type, Operator.Unknown));
            i = j - 1;

            return i;
        }

        private static int AddOpenParenthsesOrMethodContent(string expression, List<Token> tokens, int i)
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

            return i;
        }

        private static int AddString(string expression, int i, char c, List<Token> tokens)
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
            return i;
        }
    }
}
