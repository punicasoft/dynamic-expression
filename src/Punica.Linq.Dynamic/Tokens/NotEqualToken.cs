using System.Linq.Expressions;
using Punica.Linq.Dynamic.abstractions;
using Punica.Linq.Dynamic.Abstractions;

namespace Punica.Linq.Dynamic.Tokens
{
    public class NotEqualToken :Operation
    {
        public override short Precedence => 8;
        public override ExpressionType ExpressionType => ExpressionType.NotEqual;
        public override Expression Evaluate(Stack<Expression> stack)
        {
            var right = stack.Pop();
            var left = stack.Pop();
            var tuple = ConvertExpressions(left, right);

            return Expression.NotEqual(tuple.left, tuple.right);
        }
    }
}
