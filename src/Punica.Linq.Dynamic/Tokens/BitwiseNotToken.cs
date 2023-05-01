using System.Linq.Expressions;
using Punica.Linq.Dynamic.Abstractions;

namespace Punica.Linq.Dynamic.Tokens
{
    public class BitwiseNotToken : Operation
    {
        public override bool IsLeftAssociative => false;
        public override short Precedence => 13;
        public override ExpressionType ExpressionType => ExpressionType.Not;
        public override Expression Evaluate(Stack<Expression> stack)
        {
            return Expression.Not(stack.Pop());
        }
    }
}
