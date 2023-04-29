using System.Linq.Expressions;

namespace Punica.Linq.Dynamic.Tokens.abstractions
{
    public interface IExpression
    {
        Expression Evaluate();
    }
}
