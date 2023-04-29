using System.Linq.Expressions;
using Punica.Linq.Dynamic.Tokens.abstractions;

namespace Punica.Linq.Dynamic.Tokens
{
    public class ParameterToken : IExpressionToken
    {
        private Expression? _value;
        private bool _evaluated;
        private IExpression? _expression;
        private readonly string? _name;

        private Type? _type;

        /// <summary>
        /// The Name of the parameter or variable.
        /// </summary>
        public string? Name => _name;

        public ParameterToken(ParameterExpression parameterExpression)
        {
            _value = parameterExpression;
            _name = parameterExpression.Name;
            _evaluated = true;

        }

        public ParameterToken(string name)
        {
            _name =  name;
            _evaluated = false;
        }

        public ParameterToken(IExpression expression, string name)
        {
            _expression = expression;
            _name = name;
            _evaluated = false;
        }

        //internal void SetExpression(IExpression expression)
        //{
        //    _expression = expression; //TODO handle invalid scenarios
        //}

        //Remove SetExpression if this works
        internal void SetType(Type type)
        {
            if (!_evaluated) 
            {
                _type = type;//TODO handle invalid scenarios
            }
        }

        internal bool IsInitialized()
        {
            return _type != null;
        }

        public Expression Evaluate()
        {
            if (!_evaluated)
            {
                //var memberExpression = _expression!.Evaluate();

                //if (memberExpression.Type.IsCollection(out var type))
                //{
                //    _value = Expression.Parameter(type, _name);
                //}
                //else
                //{
                //    _value = Expression.Parameter(memberExpression.Type, _name);
                //}
                _value = Expression.Parameter(_type!, _name);
                _evaluated = true;
            }

            return _value;
        }

        //TODO remove?
        public bool IsLeftAssociative => true;
        public short Precedence => 0;
        public TokenType TokenType => TokenType.Value;
        public ExpressionType ExpressionType => ExpressionType.Parameter;
    }
}
