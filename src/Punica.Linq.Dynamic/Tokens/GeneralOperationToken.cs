using System.Linq.Expressions;
using Punica.Linq.Dynamic.Abstractions;

namespace Punica.Linq.Dynamic.Tokens
{
    public class GeneralOperationToken : IOperation
    {
        public bool IsLeftAssociative => true;
        public short Precedence => -2;
        public Expression Evaluate(Stack<Expression> stack)
        {
            throw new NotImplementedException();
        }

        public TokenType TokenType { get; }
        public ExpressionType ExpressionType { get; }

        public GeneralOperationToken(TokenType tokenType)
        {
            TokenType = tokenType;
            ExpressionType = ExpressionType.Extension;
        }
    }
}
