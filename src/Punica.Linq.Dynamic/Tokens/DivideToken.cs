using System.Linq.Expressions;
using Punica.Linq.Dynamic.abstractions;
using Punica.Linq.Dynamic.Abstractions;

namespace Punica.Linq.Dynamic.Tokens
{
    public class DivideToken : Operation
    {
        public override short Precedence => 12;
        public override ExpressionType ExpressionType => ExpressionType.Divide;
        public override Expression Evaluate(Stack<Expression> stack)
        {
            var right = stack.Pop();
            var left = stack.Pop();

            var tuple = ConvertExpressions(left, right);
            return Expression.Divide(tuple.left, tuple.right);
        }
    }
}
