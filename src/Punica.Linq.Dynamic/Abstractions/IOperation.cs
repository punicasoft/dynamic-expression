using System.Linq.Expressions;

namespace Punica.Linq.Dynamic.abstractions
{
    public interface IOperation : IToken
    {
        public bool IsLeftAssociative { get; }
        public short Precedence { get; }
        Expression Evaluate(Stack<Expression> stack);
    }
}
