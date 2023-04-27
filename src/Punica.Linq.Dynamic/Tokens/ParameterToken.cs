using System.Linq.Expressions;
using Punica.Extensions;
using Punica.Linq.Dynamic.Tokens.abstractions;

namespace Punica.Linq.Dynamic.Tokens
{
    public class ParameterToken : IExpression
    {
        private Expression? _value;
        private bool _evaluated;
        private readonly IExpression? _expression;
        private readonly string? _name;

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

        public ParameterToken(IExpression expression, string name)
        {
            _expression = expression;
            _name = name;
            _evaluated = false;
        }

        public Expression Evaluate()
        {
            if (!_evaluated)
            {
                var memberExpression = _expression!.Evaluate();

                if (memberExpression.Type.IsCollection(out var type))
                {
                    _value = Expression.Parameter(type, _name);
                }
                else
                {
                    _value = Expression.Parameter(memberExpression.Type, _name);
                }
                _evaluated = true;
            }

            return _value;
        }
    }
}
