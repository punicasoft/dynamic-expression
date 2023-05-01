using System.Linq.Expressions;
using Punica.Linq.Dynamic.Abstractions;

namespace Punica.Linq.Dynamic.Tokens
{
    public class LessThanToken : Operation
    {
        public override short Precedence => 9;
        public override ExpressionType ExpressionType => ExpressionType.LessThan;
        public override Expression Evaluate(Stack<Expression> stack)
        {
            var right = stack.Pop();
            var left = stack.Pop();

            var tuple = ConvertExpressions(left, right);
            return Expression.LessThan(tuple.left, tuple.right);
        }
    }
}
