using System.Linq.Expressions;
using Punica.Linq.Dynamic.Abstractions;

namespace Punica.Linq.Dynamic.Tokens
{
    public class BitwiseAndToken : Operation
    {
        public override short Precedence => 7;
        public override ExpressionType ExpressionType => ExpressionType.And;
        public override Expression Evaluate(Stack<Expression> stack)
        {
            throw new NotImplementedException();
        }
    }
}
