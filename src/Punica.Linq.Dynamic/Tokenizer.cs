using System.Linq.Expressions;
using Punica.Linq.Dynamic.Tokens;
using Punica.Linq.Dynamic.Tokens.abstractions;

namespace Punica.Linq.Dynamic
{
    public static class Tokenizer
    {
        private static readonly Dictionary<string, IToken> Tokens = new Dictionary<string, IToken>()
        {
            {"in", TokenCache.In},
            {"as", TokenCache.As},
            {"true", TokenCache.True},
            {"false", TokenCache.False},
        };

        public static RootToken Evaluate(TokenContext context)
        {
            return new RootToken(context.MethodContext.GetParameters(), Tokenize(context));
        }

        public static List<IToken> Tokenize(TokenContext context)
        {
            List<IToken> tokens = new List<IToken>();

            while (context.CanRead)
            {
                GetToken(context, out var token);

                if (token != null)
                {
                    tokens.Add(token);
                }
            }

            return tokens;
        }

        private static void GetToken(TokenContext context, out IToken? token)
        {
            token = null;
            char c = context.Current; 

            switch (c)
            {
                case '!':
                    context.NextToken();
                    if (context.Current == '=')
                    {
                        context.NextToken();
                        token = TokenCache.NotEqual;
                    }
                    else
                    {
                        token = TokenCache.Not;
                    }

                    break;
                case '"':
                    AddString(context, c, out token);
                    break;
                case '#':
                case '$':
                    throw new NotImplementedException($"Character '{c}' not supported");
                case '%':
                    context.NextToken();
                    token = TokenCache.Modulo;
                    break;
                case '&':
                    context.NextToken();
                    if (context.Current == '&')
                    {
                        context.NextToken();
                        token = TokenCache.AndAlso;
                    }
                    else
                    {
                        token = TokenCache.BitwiseAnd;
                    }

                    break;
                case '\'':
                    AddString(context, c, out token);
                    context.NextToken();
                    break;
                case '(':
                    context.NextToken();
                    token = TokenCache.LeftParenthesis;
                    break;
                case ')':
                    context.NextToken();
                    token = TokenCache.RightParenthesis;
                    break;
                case '*':
                    context.NextToken();
                    token = TokenCache.Multiply;
                    break;
                case '+':
                    context.NextToken();
                    token = TokenCache.Add;
                    break;
                case ',':
                    context.NextToken();
                    break;
                case '-':
                    context.NextToken();
                    token = TokenCache.Subtract;
                    break;
                case '.':
                    throw new NotImplementedException($"Character '{c}' not supported at this level");
                    break;
                case '/':
                    context.NextToken();
                    token = TokenCache.Divide;
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
                    AddNumber(context, out token);
                    context.NextToken(); // To Skip spaces
                    break;
                case ':':
                    context.NextToken();
                    break;
                case ';':
                    throw new NotImplementedException($"Character ';' not supported");
                case '<':
                    context.NextToken();
                    if (context.Current == '=')
                    {
                        context.NextToken();
                        token = TokenCache.LessThanOrEqual;
                    }
                    else
                    {
                        token = TokenCache.LessThan;
                    }

                    break;
                case '=':
                    context.NextToken();
                    if (context.Current == '=')
                    {
                        context.NextToken();
                        token = TokenCache.Equal;
                    }
                    else
                    {
                        token = TokenCache.Assign;
                    }

                    break;
                case '>':
                    context.NextToken();
                    if (context.Current == '=')
                    {
                        context.NextToken();
                        token = TokenCache.GreaterThanOrEqual;
                    }
                    else
                    {
                        token = TokenCache.GreaterThan;
                    }

                    break;
                case '?':
                    context.NextToken();
                    if (context.Current == '?')
                    {
                        context.NextToken();
                        token = TokenCache.NullCoalescing;
                    }
                    else
                    {
                        token = TokenCache.Conditional;
                    }

                    break;
                case '@':
                    AddParameter(context, out token);
                    break;
                case '[':
                    throw new NotImplementedException($"Character '{c}' not supported");
                    break;
                case ']':
                    throw new NotImplementedException($"Character '{c}' not supported");
                    break;
                case '^':
                    throw new NotImplementedException($"Character '^' not supported");
                case '{':
                    throw new NotImplementedException($"Character '{c}' not supported");
                    break;
                case '|':
                    context.NextToken();
                    if (context.Current == '|')
                    {
                        context.NextToken();
                        token = TokenCache.OrElse;
                    }
                    else
                    {
                        token = TokenCache.BitwiseOr;
                    }

                    break;
                default:
                    if (Match(context, out var key))
                    {
                        context.SetPosition(context.CurrentPosition + key.Length);
                        token = Tokens[key];
                        context.NextToken();
                    }
                    else
                    {
                        Parse3(context, out token);
                    }

                    break;
            }


        }

        /// Person.Name or Person() or Person.Select() or  Person.Select().ToList() 
        public static void Parse3(TokenContext context, out IToken token)
        {
            IExpression exp = context.MethodContext.GetParameter();
            token = null;
            var identifier = GetIdentifier(context);

            var literal = NextToken(context);
            while (!string.IsNullOrEmpty(literal))
            {
                if (literal == ".")
                {
                    context.NextToken(false);
                    if (!string.IsNullOrEmpty(identifier))
                    {
                        exp = new PropertyToken(exp, identifier);
                    }
                }
                else if (literal == "(")
                {
                    context.MethodContext.NextDepth();
                    context.MethodContext.AddParameter(exp);
                    var methodToken = new MethodToken(identifier, exp, context.MethodContext.GetParameter());
                    identifier = string.Empty;

                    var parameter = new TokenList();
                    context.NextToken();
                    int depth = 1;

                    while (context.CanRead && depth > 0)
                    {
                        switch (context.Current)
                        {
                            case '(':
                                throw new ArgumentException("Invalid Case Check algorithm"); // possibly not a option
                            case ')':
                                depth--;
                                if (parameter.Tokens.Count > 0)
                                {
                                    methodToken.AddToken(parameter); // only add if it is on same level as there could be mix due to incorrect number of end curly brackets
                                }
                                context.NextToken();
                                break;
                            case ',':
                                methodToken.AddToken(parameter);
                                parameter = new TokenList();
                                context.MethodContext.MoveToNextArgument();
                                context.NextToken();
                                break;
                            default:
                                {
                                    GetToken(context, out var token2);

                                    if (token2 != null)
                                    {
                                        parameter.AddToken(token2);
                                    }

                                    break;
                                }
                        }
                    }

                    if (depth != 0)
                    {
                        throw new ArgumentException("Input contains mismatched parentheses.");
                    }

                    context.MethodContext.PreviousDepth();
                    token = methodToken;
                    exp = methodToken;

                }
                else if (literal == "{")
                {
                    if (identifier != "new")
                    {
                        throw new NotSupportedException($"Not able to handle {{ with this identifier {identifier}");
                    }

                    var newToken = new NewToken(context.MethodContext.GetParameter());
                    identifier = string.Empty;

                    var parameter = new TokenList();

                    context.NextToken();
                    int depth = 1;

                    while (context.CanRead && depth > 0)
                    {
                        switch (context.Current)
                        {
                            case '{':
                                throw new ArgumentException("Invalid Case Check algorithm"); // possibly not a option
                            case '}':
                                depth--;
                                if (parameter.Tokens.Count > 0)
                                {
                                    newToken.AddToken(parameter); // only add if it is on same level as there could be mix due to incorrect number of end curly brackets
                                }
                                context.NextToken();
                                break;
                            case ',':
                                newToken.AddToken(parameter);
                                context.NextToken();
                                parameter = new TokenList();
                                break;
                            default:
                                {
                                    GetToken(context, out var token2);

                                    if (token2 != null)
                                    {
                                        parameter.AddToken(token2);
                                    }

                                    break;
                                }
                        }
                    }

                    if (depth != 0)
                    {
                        throw new ArgumentException("Input contains mismatched parentheses.");
                    }

                    token = newToken;
                    exp = newToken;

                }
                else
                {
                    identifier = literal;
                }

                literal = NextToken(context);
            }

            if (!string.IsNullOrEmpty(identifier))
            {
                exp = new PropertyToken(exp, identifier);
                token = new ValueToken(exp);
            }

        }

        public static string NextToken(TokenContext context)
        {
            var c = context.Current;

            while (char.IsWhiteSpace(c))
            {
                context.NextToken(false);
                c = context.Current;
            }

            if (c == '.')
            {
                return ".";
            }
            else if (c == '(')
            {
                return "(";
            }
            else if (c == '[')
            {
                return "[";
            }
            else if (c == '{')
            {
                return "{";
            }
            if (Match(context, out var key))
            {
                return "";
            }
            else if (char.IsLetter(c))
            {
                return GetIdentifier(context);
            }
            else
            {
                return "";
            }

            return null;
        }

        public static string GetIdentifier(TokenContext context)
        {
            // Handle operands
            int i = context.CurrentPosition;
            context.NextToken(false);

            // TODO : have custom punctuation
            while (context.CanRead && !(char.IsWhiteSpace(context.Current) || (char.IsPunctuation(context.Current))))
            {
                context.NextToken(false);
            }


            var stringVal = context.Substring(i, context.CurrentPosition - i);
            return stringVal;
        }


        private static int Parse(int i, string expression, out IToken token)
        {
            token = null;
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
                return i;
            }

            var stringVal = expression.Substring(i, j - i);

            token = new ValueToken(Expression.Constant(stringVal));
            i = j - 1;
            return i;
        }



        private static bool Match(TokenContext context, out string index)
        {
            index = "";

            foreach (var key in Tokens.Keys)
            {
                index = key;
                var value = key;

                if (context.Match(value))
                {
                    return true;
                }
            }

            return false;
        }

        private static void AddParameter(TokenContext context, out IToken token)
        {
            if (context.VariablesInstance == null)
            {
                throw new ArgumentException("Parameters not supplied");
            }

            context.NextToken(false);
            int i = context.CurrentPosition;
            while (context.CanRead && !(char.IsPunctuation(context.Current)))
            {
                context.NextToken(false);
            }

            var stringVal = context.Substring(i, context.CurrentPosition - i);



            token = new ValueToken(Expression.PropertyOrField(context.VariablesInstance, stringVal));
        }

        private static void AddNumber(TokenContext context, out IToken token)
        {
            int i = context.CurrentPosition;
            context.NextToken(false);
            var real = false;

            while (context.CanRead && !(char.IsWhiteSpace(context.Current)))
            {
                if (!char.IsDigit(context.Current) && context.Current != '.')
                {
                    throw new ArgumentException($"Invalid number detected {context.Substring(i, context.CurrentPosition)}");
                }

                if (context.Current == '.')
                {
                    if (real)
                    {
                        throw new ArgumentException($"Invalid number detected {context.Substring(i, context.CurrentPosition)}");
                    }

                    real = true;
                }

                context.NextToken(false);
            }

            var stringVal = context.Substring(i, context.CurrentPosition - i);

            token = real ? new ValueToken(Expression.Constant(double.Parse(stringVal))) : new ValueToken(Expression.Constant(int.Parse(stringVal)));
        }



        private static void AddString(TokenContext context, char c, out IToken token)
        {
            // Handle operands
            context.NextToken();
            int startIndex = context.CurrentPosition;

            while (context.CanRead && context.Current != c)
            {
                context.NextToken();
            }

            if (context.Current != c)
            {
                throw new ArgumentException("Input contains invalid string literal.");
            }

            var stringVal = context.Substring(startIndex, context.CurrentPosition - startIndex);

            token = new ValueToken(Expression.Constant(stringVal));
        }
    }
}
