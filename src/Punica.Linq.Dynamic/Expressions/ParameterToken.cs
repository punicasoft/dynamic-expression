using System.Linq.Expressions;
using Punica.Linq.Dynamic.Abstractions;

namespace Punica.Linq.Dynamic.Expressions
{
    public class ParameterToken : IExpression
    {
        private Expression? _value;
        private bool _evaluated;
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
            _name = name;
            _evaluated = false;
        }

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
                _value = Expression.Parameter(_type!, _name);
                _evaluated = true;
            }

            return _value!;
        }

        public TokenType TokenType => TokenType.Value;
        public ExpressionType ExpressionType => ExpressionType.Parameter;
    }
}
