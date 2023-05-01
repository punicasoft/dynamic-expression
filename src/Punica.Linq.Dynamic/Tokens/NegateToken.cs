using Punica.Linq.Dynamic.abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Punica.Linq.Dynamic.Abstractions;

namespace Punica.Linq.Dynamic.Tokens
{
    public class NegateToken : Operation
    {
        public override short Precedence => 14;
        public override ExpressionType ExpressionType => ExpressionType.Negate;

        public override Expression Evaluate(Stack<Expression> stack)
        {
            var right = stack.Pop();
            return Expression.Negate(right);
        }
    }
}
