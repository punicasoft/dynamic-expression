using System.Linq.Expressions;
using Punica.Linq.Dynamic.abstractions;

namespace Punica.Linq.Dynamic.Tokens
{
    public class Argument
    {
        private readonly List<string> _lambdas = new List<string>();
        private bool _evaluated = false;
        private Expression _expression;
        private ParameterToken[] _parameter;
        // public IReadOnlyList<string> Lambdas => _lambdas;
        public bool IsLeftAssociative => true;
        public IExpression? Parameter { get; } = null;
        public List<IToken> Tokens { get; }
        public short Precedence => 0;
        public TokenType TokenType => TokenType.List;
        public ExpressionType ExpressionType => ExpressionType.Extension;

        public Argument()
        {
            Tokens = new List<IToken>();

        }

        public void AddToken(IToken token)
        {
            Tokens.Add(token);
        }

        public void AddParameters(ParameterToken[] paras)
        {
            _parameter = paras;
        }

        public IReadOnlyList<string> ProcessLambda()
        {
            short depth = 0;

            foreach (var token in Tokens)
            {
                if (token.TokenType == TokenType.OpenParen)
                {
                    if (depth == 0)
                    {
                        depth++;
                        continue;
                    }

                    throw new ArgumentException("Invalid Expression");
                }

                if (token.TokenType == TokenType.CloseParen)
                {
                    if (depth == 1 && Tokens.IndexOf(token) == Tokens.Count - 1) // must be the last token
                    {
                        depth--;
                        continue;
                    }

                    throw new ArgumentException("Invalid Expression");
                }

                if (token.TokenType == TokenType.Comma)
                {
                    continue;
                }

                if (token.TokenType == TokenType.Value)
                {
                    var propertyToken = token as PropertyToken;
                    if (propertyToken == null)
                    {
                        throw new ArgumentException("Invalid Expression");
                    }

                    _lambdas.Add(propertyToken.Name);

                }
            }

            if (depth != 0)
            {
                throw new ArgumentException("Invalid Expression");
            }

            Tokens.Clear();

            return _lambdas;
        }

        public bool IsFirstOpenParenthesis()
        {
            if (Tokens.Count > 0 && Tokens[0].TokenType == TokenType.OpenParen)
            {
                return true;
            }

            return false;
        }

        public ParameterExpression SetParameterExpressionBody(Type type, int index)
        {
            if (index >= _parameter.Length)
            {
                throw new Exception("Index exceed available parameters");
            }

            _parameter[index].SetType(type);

            return (ParameterExpression)_parameter[index].Evaluate();
        }

        internal bool IsFunction()
        {
            return _parameter.Length != 0;
        }

        internal bool CanEvaluate()
        {
            if (_parameter.Length == 0)
            {
                return true;
            }

            foreach (var para in _parameter)
            {
                if (!para.IsInitialized())
                {
                    return false;
                }
            }

            return true;
        }

        internal Expression Evaluate()
        {
            if (!_evaluated)
            {
                _expression = ExpressionEvaluator.Evaluate(Tokens);
                _evaluated = true;
                return _expression;
            }

            return _expression;
        }


        public ArgumentData GetArgumentData()
        {
            if (CanEvaluate())
            {
                var expression = Evaluate();

                return new ArgumentData(_parameter.Length != 0, _parameter.Length, expression.Type);

            }


            return new ArgumentData(_parameter.Length != 0, _parameter.Length, null);

        }

        //public ParameterExpression? SetParameterExpressionBody(IExpression memberExpression)
        //{
        //    if (_parameter.Length > 1)
        //    {
        //        throw new Exception("More than 1 arg is not handled");
        //    }

        //    if (_parameter.Length == 1)
        //    {
        //        _parameter[0].SetExpression(memberExpression);
        //        return (ParameterExpression)_parameter[0].Evaluate();
        //    }

        //    return null;
        //}
    }
}
