using System.Linq.Expressions;
using Punica.Linq.Dynamic.Tokens.abstractions;

namespace Punica.Linq.Dynamic.Tokens
{
    public class NullCoalescingToken : Operation
    {
        public override short Precedence => 2;
        public override ExpressionType ExpressionType => ExpressionType.Coalesce; 
        public override Expression Evaluate(Stack<Expression> stack)
        {
            var right = stack.Pop();
            var left = stack.Pop();
            return Expression.Coalesce(left, right);
        }
    }
}
