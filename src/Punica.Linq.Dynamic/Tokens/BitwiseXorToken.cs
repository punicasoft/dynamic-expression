using System.Linq.Expressions;
using Punica.Linq.Dynamic.Abstractions;

namespace Punica.Linq.Dynamic.Tokens
{
    public class BitwiseXorToken : Operation
    {
        public override short Precedence => 6;
        public override ExpressionType ExpressionType => ExpressionType.ExclusiveOr;

        public override Expression Evaluate(Stack<Expression> stack)
        {
            throw new NotImplementedException();
        }
    }
}
