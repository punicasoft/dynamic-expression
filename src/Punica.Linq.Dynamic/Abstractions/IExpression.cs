using System.Linq.Expressions;

namespace Punica.Linq.Dynamic.abstractions
{
    public interface IExpression : IToken
    {
        Expression Evaluate();
    }
}
