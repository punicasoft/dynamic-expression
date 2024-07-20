using System.Linq.Expressions;

namespace Punica.Linq.Dynamic.Abstractions
{
    public interface IOperation : IToken
    {
        public bool IsLeftAssociative { get; }
        public short Precedence { get; }
        Expression Evaluate(Stack<Expression> stack);
    }
}
