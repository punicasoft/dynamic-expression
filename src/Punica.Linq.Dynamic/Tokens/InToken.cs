using System.ComponentModel;
using System.Linq.Expressions;
using Punica.Extensions;
using Punica.Linq.Dynamic.Reflection;
using Punica.Linq.Dynamic.Tokens.abstractions;

namespace Punica.Linq.Dynamic.Tokens
{
    public class InToken : Operation
    {
        public override short Precedence => 9;
        public override ExpressionType ExpressionType => ExpressionType.Call;
        public override Expression Evaluate(Stack<Expression> stack)
        {
            var right = stack.Pop();
            var left = stack.Pop();

            if (right.Type == typeof(string))
            {
                return Expression.Call(right, CachedMethodInfo.Contains, left);
            }
            else
            {
                var type = right.Type;
                if (type.IsCollection())
                {
                    type = type.GetElementOrGenericArgType();
                }

                if (left.Type != type)
                {
                    var converter = TypeDescriptor.GetConverter(type);
                    var val = converter.ConvertFrom(((ConstantExpression)left).Value);
                    left = Expression.Constant(val);
                }

                return Expression.Call(EnumerableCachedMethodInfo.Contains(type), right, left);
            }

        }
    }
}
