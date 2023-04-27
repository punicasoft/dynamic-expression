using System.Linq.Expressions;
using Punica.Linq.Dynamic.Tokens.abstractions;

namespace Punica.Linq.Dynamic.Tokens
{
    public class TokenList: ITokenList
    {
        public bool IsLeftAssociative => true;
        public IExpression? Parameter { get; } = null;
        public List<IToken> Tokens { get; }
        public short Precedence => 0;
        public TokenType TokenType => TokenType.List;
        public ExpressionType ExpressionType => ExpressionType.Extension;

        public TokenList()
        {
            Tokens = new List<IToken>();

        }

        public void AddToken(IToken token)
        {
            Tokens.Add(token);
        }

    }
}
