using System.Linq.Expressions;
using Punica.Linq.Dynamic.Abstractions;

namespace Punica.Linq.Dynamic.Tokens
{
    public class UnaryPlusToken:Operation
    {
        public override short Precedence => 14;
        public override ExpressionType ExpressionType => ExpressionType.UnaryPlus;

        public override Expression Evaluate(Stack<Expression> stack)
        {
            var right = stack.Pop();
            return Expression.UnaryPlus(right);
        }
    }
}
