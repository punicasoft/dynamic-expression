using System.Linq.Expressions;
using Punica.Linq.Dynamic.Tokens.abstractions;

namespace Punica.Linq.Dynamic.Tokens;

public class PropertyToken : IExpression
{
    private readonly IExpression _expression;
    private readonly string _propertyName;
    private Expression _value;
    private bool _evaluated;

    public PropertyToken(IExpression expression, string propertyName)
    {
        _expression = expression;
        _propertyName = propertyName;
        _evaluated = false;
    }

    public Expression Evaluate()
    {
        if (!_evaluated)
        {
            _value = Expression.PropertyOrField(_expression.Evaluate(), _propertyName);
            _evaluated = true;
        }

        return _value;
    }
}