using System.Linq.Expressions;
using Punica.Linq.Dynamic.Reflection;
using Punica.Linq.Dynamic.Tokens.abstractions;

namespace Punica.Linq.Dynamic.Tokens
{
    public class AddToken : Operation
    {
        public override short Precedence => 11;
        public override ExpressionType ExpressionType => ExpressionType.Add;

        public override Expression Evaluate(Stack<Expression> stack)
        {
            var right = stack.Pop();
            var left = stack.Pop();

            if (left.Type == typeof(string))
            {
                return Expression.Call(CachedMethodInfo.Concat, left, right);
            }

            var tuple = ConvertExpressions(left, right);
            return Expression.Add(tuple.left, tuple.right);
        }

      
    }
}
