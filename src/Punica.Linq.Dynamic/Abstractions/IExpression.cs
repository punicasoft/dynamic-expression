using System.Linq.Expressions;

namespace Punica.Linq.Dynamic.Abstractions
{
    public interface IExpression : IToken
    {
        Expression Evaluate();
    }
}
