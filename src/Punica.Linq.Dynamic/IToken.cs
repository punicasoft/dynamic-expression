using System.Linq.Expressions;

namespace Punica.Linq.Dynamic
{
    public interface IToken
    {
        public bool IsLeftAssociative { get; }
        public short Precedence { get; }
        public TokenType TokenType { get; }

        public ExpressionType ExpressionType { get; }
    }
}
