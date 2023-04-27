using System.Linq.Expressions;
using Punica.Linq.Dynamic.Tokens.abstractions;

namespace Punica.Linq.Dynamic.Tokens
{
    public class EqualToken :Operation
    {
        public override short Precedence => 8;
        public override ExpressionType ExpressionType => ExpressionType.Equal;
        public override Expression Evaluate(Stack<Expression> stack)
        {
            var right = stack.Pop();
            var left = stack.Pop();

            var tuple = ConvertExpressions(left, right);
            return Expression.Equal(tuple.left, tuple.right);
        }
    }
}
