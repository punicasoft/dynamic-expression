using System.Linq.Expressions;
using Punica.Linq.Dynamic.Abstractions;

namespace Punica.Linq.Dynamic.Tokens
{
    public class ConditionalToken : Operation
    {
        public override short Precedence  => 1;
        public override ExpressionType ExpressionType => ExpressionType.Conditional;
        public override Expression Evaluate(Stack<Expression> stack)
        {
            var ifFalse = stack.Pop();
            var ifTrue = stack.Pop();
            var condition = stack.Pop();

            return Expression.Condition(condition, ifTrue, ifFalse);
        }
    }
}
