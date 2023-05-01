using System.Linq.Expressions;

namespace Punica.Linq.Dynamic.abstractions
{
    public interface IToken
    {
        public TokenType TokenType { get; }

        public ExpressionType ExpressionType { get; }
    }
}
