using System.Diagnostics;

namespace Punica.Linq.Dynamic
{
    [DebuggerDisplay("{Value}")]
    public struct Token
    {
        public string Value { get; }
        public TokenType Type { get; }
        public Operator Operator { get; }

        public Token(string value, TokenType type, Operator @operator)
        {
            Value = value;
            Type = type;
            Operator = @operator;
        }
    }
}
