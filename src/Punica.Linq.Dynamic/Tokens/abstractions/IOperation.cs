using System.Linq.Expressions;

namespace Punica.Linq.Dynamic.Tokens.abstractions
{
    public interface IOperation : IToken
    {
        Expression Evaluate(Stack<Expression> stack);
    }
}
