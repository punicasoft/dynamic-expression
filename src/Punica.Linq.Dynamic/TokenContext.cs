using System.Globalization;
using System.Linq.Expressions;
using Punica.Linq.Dynamic.Tokens;
using Punica.Linq.Dynamic.Tokens.abstractions;

namespace Punica.Linq.Dynamic
{
    public class TokenContext
    {
        private readonly Dictionary<string, Identifier> _identifiers = new Dictionary<string, Identifier>();
        private static readonly Dictionary<string, Alias> Aliases = new()
        {
            { "eq", new Alias() { Id = TokenId.Equal, Token = TokenCache.Equal} },
            { "ne", new Alias() { Id = TokenId.NotEqual, Token = TokenCache.NotEqual} },
            { "lt", new Alias() { Id = TokenId.LessThan, Token = TokenCache.LessThan} },
            { "le", new Alias() { Id =  TokenId.LessThanOrEqual, Token = TokenCache.LessThanOrEqual} },
            { "gt", new Alias() { Id =  TokenId.GreaterThan, Token = TokenCache.GreaterThan}},
            { "ge", new Alias() { Id =  TokenId.GreaterThanOrEqual, Token = TokenCache.GreaterThanOrEqual} },
            { "in", new Alias() { Id =  TokenId.In, Token = TokenCache.In} },
            { "as", new Alias() { Id =  TokenId.As, Token = TokenCache.As}},
            { "or", new Alias() { Id =  TokenId.OrElse, Token = TokenCache.OrElse} },
            { "and", new Alias() { Id =  TokenId.AndAlso, Token = TokenCache.AndAlso} },
            { "mod", new Alias() { Id =  TokenId.Modulo, Token = TokenCache.Modulo}},
            { "not", new Alias() { Id =  TokenId.Not, Token = TokenCache.Not} },

            { "add", new Alias() { Id =  TokenId.Add, Token = TokenCache.Add} },
            { "sub", new Alias() { Id =  TokenId.Subtract, Token = TokenCache.Subtract} },
            { "mul", new Alias() { Id =  TokenId.Multiply, Token = TokenCache.Multiply} },
            { "div", new Alias() { Id =  TokenId.Divide, Token = TokenCache.Divide} },

            { "true",new Alias() { Id =  TokenId.BooleanLiteral, Token = TokenCache.True} },
            { "false", new Alias() { Id =  TokenId.BooleanLiteral, Token = TokenCache.False} },
        };

        private readonly string _txt;
        private int _pos;

        public bool CanRead => _pos < _txt.Length;
        public char Current { get; private set; }

        public int CurrentPosition => _pos;

        public Token CurrentToken;
        /// <summary>
        /// Used to pass parameters for expressions @param where param is a property/field of VariablesInstance
        /// </summary>
        public Expression? VariablesInstance { get; }

        //TODO remove this. currently used for backward compatibility
        public MethodContext MethodContext { get; }


        public TokenContext(string txt, Expression? variablesInstance = null)
        {
            _txt = txt;
            VariablesInstance = variablesInstance;
            SetPosition(0);
            MethodContext = new MethodContext();
            NextToken();
        }

        public void AddStartParameter(Type type)
        {
            MethodContext.AddParameter(new ParameterToken(Expression.Parameter(type, "_arg")));
        }

        public void AddIdentifier(string name, Expression expression)
        {
            _identifiers.Add(name, new Identifier(name, expression));
        }

        public Identifier? GetIdentifier(string name)
        {
            return _identifiers.TryGetValue(name, out var identifier) ? identifier : null;
        }


        public void SetPosition(int pos)
        {
            _pos = pos;
            Current = CanRead ? _txt[_pos] : '\0';
        }

        public char PeekNext()
        {
            return _pos + 1 < _txt.Length ? _txt[_pos + 1] : '\0';
        }

        private void NextChar()
        {
            if (CanRead)
            {
                _pos++;
            }

            Current = CanRead ? _txt[_pos] : '\0';
        }

        public void NextToken()
        {
            while (char.IsWhiteSpace(Current))
            {
                NextChar();
            }

            // ReSharper disable once RedundantAssignment
            TokenId tokenId = TokenId.Unknown;
            IToken? token = null;
            string stringVal = string.Empty;

            int tokenPos = _pos;

            switch (Current)
            {
                case '!':
                    NextChar();
                    if (Current == '=')
                    {
                        NextChar();
                        token = TokenCache.NotEqual;
                        tokenId = TokenId.NotEqual;
                    }
                    else
                    {
                        token = TokenCache.Not;
                        tokenId = TokenId.Not;
                    }

                    break;
                case '"':
                    token = AddString(Current);
                    tokenId = TokenId.StringLiteral;
                    NextChar();
                    break;
                case '#':
                case '$':
                    throw new NotImplementedException($"Character '{Current}' not supported");
                case '%':
                    NextChar();
                    token = TokenCache.Modulo;
                    tokenId = TokenId.Modulo;
                    break;
                case '&':
                    NextChar();
                    if (Current == '&')
                    {
                        NextChar();
                        token = TokenCache.AndAlso;
                        tokenId = TokenId.AndAlso;
                    }
                    else
                    {
                        token = TokenCache.BitwiseAnd;
                        tokenId = TokenId.BitwiseAnd;
                    }

                    break;
                case '\'':
                    token = AddString(Current);
                    tokenId = TokenId.StringLiteral;
                    NextChar();
                    break;
                case '(':
                    NextChar();
                    token = TokenCache.LeftParenthesis;
                    tokenId = TokenId.LeftParenthesis;
                    break;
                case ')':
                    NextChar();
                    token = TokenCache.RightParenthesis;
                    tokenId = TokenId.RightParenthesis;
                    break;
                case '*':
                    NextChar();
                    token = TokenCache.Multiply;
                    tokenId = TokenId.Multiply;
                    break;
                case '+':
                    NextChar();
                    token = TokenCache.Add;
                    tokenId = TokenId.Add;
                    break;
                case ',':
                    NextChar();
                    tokenId = TokenId.Comma;
                    token = TokenCache.Comma;
                    break;
                case '-':
                    NextChar();
                    token = TokenCache.Subtract;
                    tokenId = TokenId.Subtract;
                    break;
                case '.':
                    NextChar();
                    tokenId = TokenId.Dot;
                    break;
                case '/':
                    NextChar();
                    token = TokenCache.Divide;
                    tokenId = TokenId.Divide;
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

                    tokenId = TokenId.IntegerLiteral;

                    do
                    {
                        NextChar();
                    } while (char.IsDigit(Current));

                    if (Current == 'L')
                    {
                        NextChar();

                        if (char.IsLetterOrDigit(Current)) // cannot be followed by a letter or digit but can be operator or space
                        {
                            throw new ArgumentException($"Invalid number detected {_txt.Substring(tokenPos, CurrentPosition - tokenPos)}");
                        }

                        stringVal = _txt.Substring(tokenPos, CurrentPosition - tokenPos);
                        token = new ValueToken(Expression.Constant(long.Parse(stringVal)));
                        break;
                    }

                    if (Current == '.' || Current == ',')
                    {
                        tokenId = TokenId.RealLiteral;
                        NextChar();

                        if (!char.IsDigit(Current))
                        {
                            throw new ArgumentException($"Invalid number detected {_txt.Substring(tokenPos, CurrentPosition - tokenPos)}");
                        }

                        do
                        {
                            NextChar();
                        } while (char.IsDigit(Current));

                        if (Current == 'F' || Current == 'f')
                        {
                            NextChar();
                            if (char.IsLetterOrDigit(Current)) // cannot be followed by a letter or digit but can be operator or space
                            {
                                throw new ArgumentException($"Invalid number detected {_txt.Substring(tokenPos, CurrentPosition - tokenPos)}");
                            }

                            stringVal = _txt.Substring(tokenPos, CurrentPosition - tokenPos);
                            token = new ValueToken(Expression.Constant(float.Parse(stringVal)));
                            break;
                        }

                        if (Current == 'D' || Current == 'd')
                        {
                            NextChar();
                            if (char.IsLetterOrDigit(
                                    Current)) // cannot be followed by a letter or digit but can be operator or space
                            {
                                throw new ArgumentException($"Invalid number detected {_txt.Substring(tokenPos, CurrentPosition - tokenPos)}");
                            }
                            stringVal = _txt.Substring(tokenPos, CurrentPosition - tokenPos);
                            token = new ValueToken(Expression.Constant(double.Parse(stringVal)));
                            break;
                        }

                        if (Current == 'M' || Current == 'm')
                        {
                            NextChar();
                            if (char.IsLetterOrDigit(Current)) // cannot be followed by a letter or digit but can be operator or space
                            {
                                throw new ArgumentException($"Invalid number detected {_txt.Substring(tokenPos, CurrentPosition - tokenPos)}");
                            }
                            stringVal = _txt.Substring(tokenPos, CurrentPosition - tokenPos);
                            token = new ValueToken(Expression.Constant(decimal.Parse(stringVal)));
                            break;
                        }

                        if (Current == 'E' || Current == 'e')
                        {
                            NextChar();
                            if (Current == '+' || Current == '-')
                            {
                                NextChar();
                            }

                            if (!char.IsDigit(Current))
                            {
                                throw new ArgumentException($"Invalid number detected {_txt.Substring(tokenPos, CurrentPosition - tokenPos)}");
                            }

                            do
                            {
                                NextChar();
                            } while (char.IsDigit(Current));

                            if (Current == 'F' || Current == 'f')
                            {
                                NextChar();
                                if (char.IsLetterOrDigit(Current)) // cannot be followed by a letter or digit but can be operator or space
                                {
                                    throw new ArgumentException($"Invalid number detected {_txt.Substring(tokenPos, CurrentPosition - tokenPos)}");
                                }

                                stringVal = _txt.Substring(tokenPos, CurrentPosition - tokenPos);
                                token = new ValueToken(Expression.Constant(float.Parse(stringVal, NumberStyles.Float, CultureInfo.InvariantCulture)));
                                break;
                            }

                            if (Current == 'M' || Current == 'm')
                            {
                                NextChar();
                                if (char.IsLetterOrDigit(
                                        Current)) // cannot be followed by a letter or digit but can be operator or space
                                {
                                    throw new ArgumentException($"Invalid number detected {_txt.Substring(tokenPos, CurrentPosition - tokenPos)}");
                                }
                                stringVal = _txt.Substring(tokenPos, CurrentPosition - tokenPos);
                                token = new ValueToken(Expression.Constant(decimal.Parse(stringVal, NumberStyles.Float, CultureInfo.InvariantCulture)));
                                break;
                            }

                            stringVal = _txt.Substring(tokenPos, CurrentPosition - tokenPos);
                            token = new ValueToken(Expression.Constant(double.Parse(stringVal, NumberStyles.Float, CultureInfo.InvariantCulture)));
                            break;
                        }

                        stringVal = _txt.Substring(tokenPos, CurrentPosition - tokenPos);
                        token = new ValueToken(Expression.Constant(double.Parse(stringVal, NumberStyles.Float, CultureInfo.InvariantCulture)));
                        break;
                    }

                    stringVal = _txt.Substring(tokenPos, CurrentPosition - tokenPos);
                    token = new ValueToken(Expression.Constant(int.Parse(stringVal)));
                    break;
                case ':':
                    NextChar();
                    tokenId = TokenId.Colon;
                    break;
                case ';':
                    throw new NotImplementedException($"Character ';' not supported");
                case '<':
                    NextChar();
                    if (Current == '=')
                    {
                        NextChar();
                        token = TokenCache.LessThanOrEqual;
                        tokenId = TokenId.LessThanOrEqual;
                    }
                    else
                    {
                        token = TokenCache.LessThan;
                        tokenId = TokenId.LessThan;
                    }
                    break;
                case '=':
                    NextChar();
                    if (Current == '=')
                    {
                        NextChar();
                        token = TokenCache.Equal;
                        tokenId = TokenId.Equal;
                    }
                    else if (Current == '>')
                    {
                        NextChar();
                        tokenId = TokenId.Lambda;
                    }
                    else
                    {
                        token = TokenCache.Assign;
                        tokenId = TokenId.Assign;
                    }

                    break;
                case '>':
                    NextChar();
                    if (Current == '=')
                    {
                        NextChar();
                        token = TokenCache.GreaterThanOrEqual;
                        tokenId = TokenId.GreaterThanOrEqual;
                    }
                    else
                    {
                        token = TokenCache.GreaterThan;
                        tokenId = TokenId.GreaterThan;
                    }

                    break;
                case '?':
                    NextChar();
                    if (Current == '?')
                    {
                        NextChar();
                        token = TokenCache.NullCoalescing;
                        tokenId = TokenId.NullCoalescing;
                    }
                    else
                    {
                        token = TokenCache.Conditional;
                        tokenId = TokenId.Conditional;
                    }

                    break;
                case '@':
                    do
                    {
                        NextChar();
                    } while (char.IsLetterOrDigit(Current) || Current == '_');

                    stringVal = _txt.Substring(tokenPos + 1, CurrentPosition - tokenPos - 1);

                    if (!_identifiers.ContainsKey(stringVal))
                    {
                        _identifiers.Add(stringVal, new Identifier(stringVal, Expression.PropertyOrField(VariablesInstance, stringVal)));
                    }
                    token = new ValueToken(_identifiers[stringVal].Expression);
                    tokenId = TokenId.Variable;
                    break;
                case '[':
                    throw new NotImplementedException($"Character '{Current}' not supported");
                    break;
                case ']':
                    throw new NotImplementedException($"Character '{Current}' not supported");
                    break;
                case '^':
                    throw new NotImplementedException($"Character '^' not supported");
                case '{':
                    NextChar();
                    tokenId = TokenId.LeftCurlyParen;
                    break;
                case '|':
                    NextChar();
                    if (Current == '|')
                    {
                        NextChar();
                        token = TokenCache.OrElse;
                        tokenId = TokenId.OrElse;
                    }
                    else
                    {
                        token = TokenCache.BitwiseOr;
                        tokenId = TokenId.BitwiseOr;
                    }
                    break;
                case '}':
                    NextChar();
                    tokenId = TokenId.RightCurlyParen;
                    break;
                default:
                    if (char.IsLetter(Current) || Current == '_')
                    {
                        do
                        {
                            NextChar();
                        } while (char.IsLetterOrDigit(Current) || Current == '_');
                        tokenId = TokenId.Identifier;
                        stringVal = _txt.Substring(tokenPos, CurrentPosition - tokenPos);
                        break;
                    }

                    if (_pos == _txt.Length)
                    {
                        tokenId = TokenId.End;
                        break;
                    }

                    throw new ArgumentException($"Not supported text detected {_txt.Substring(tokenPos, CurrentPosition - tokenPos)}");
                    break;
            }

            CurrentToken.Id = tokenId;
            CurrentToken.Text = stringVal;
            CurrentToken.ParsedToken = token;
            var hasAlias = Aliases.TryGetValue(stringVal, out Alias alias);

            if (hasAlias)
            {
                CurrentToken.Id = alias.Id;
                CurrentToken.ParsedToken = alias.Token;
            }
        }

        public string Substring(int startIndex, int length)
        {
            return _txt.Substring(startIndex, length);
        }

        public bool Match(string text)
        {
            int j;
            int k = _pos;
            for (j = 0; j < text.Length; j++)
            {
                k = _pos + j;

                if (k >= _txt.Length || _txt[k] != text[j])
                {
                    return false;
                }
            }
            return true;
        }

        private IToken AddString(char c)
        {
            // Handle operands
            NextChar();
            int startIndex = CurrentPosition;

            while (CanRead && Current != c)
            {
                NextChar();
            }

            if (Current != c)
            {
                throw new ArgumentException("Input contains invalid string literal.");
            }

            var stringVal = _txt.Substring(startIndex, CurrentPosition - startIndex);

            return new ValueToken(Expression.Constant(stringVal));
        }
    }
}
