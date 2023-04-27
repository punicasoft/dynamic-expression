using System.Linq.Expressions;
using Punica.Linq.Dynamic.Tokens.abstractions;

namespace Punica.Linq.Dynamic.Tokens
{
    public class AndToken : Operation
    {
        public override short Precedence => 4;
        public override ExpressionType ExpressionType => ExpressionType.AndAlso;
        public override Expression Evaluate(Stack<Expression> stack)
        {
            var right = stack.Pop();
            var left = stack.Pop();

            return Expression.AndAlso(left, right);
        }
    }
}
