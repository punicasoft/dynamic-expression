using System.Linq.Expressions;
using Punica.Linq.Dynamic.abstractions;
using Punica.Linq.Dynamic.Abstractions;

namespace Punica.Linq.Dynamic.Tokens
{
    public class BitwiseOrToken : Operation
    {
        public override short Precedence  => 5;
        public override ExpressionType ExpressionType => ExpressionType.Or;
        public override Expression Evaluate(Stack<Expression> stack)
        {
            throw new NotImplementedException();
        }
    }
}
