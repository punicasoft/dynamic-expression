using System.Linq.Expressions;
using Punica.Linq.Dynamic.abstractions;
using Punica.Linq.Dynamic.Abstractions;

namespace Punica.Linq.Dynamic.Tokens
{
    public class SubtractToken : Operation
    {
        public override short Precedence => 11;
        public override ExpressionType ExpressionType => ExpressionType.Subtract;

        public override Expression Evaluate(Stack<Expression> stack)
        {
            var right = stack.Pop();
            var left = stack.Pop();

            var tuple = ConvertExpressions(left, right);
            return Expression.Subtract(tuple.left, tuple.right);
        }
    }
}
