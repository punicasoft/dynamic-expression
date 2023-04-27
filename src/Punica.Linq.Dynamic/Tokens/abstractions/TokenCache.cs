using System.Linq.Expressions;

namespace Punica.Linq.Dynamic.Tokens.abstractions
{
    public static class TokenCache
    {
        public static readonly IToken Add = new AddToken();
        public static readonly IToken Subtract = new SubtractToken();
        public static readonly IToken Multiply = new MultiplyToken();
        public static readonly IToken Divide = new DivideToken();
        public static readonly IToken Modulo = new ModuloToken();
        public static readonly IToken Equal = new EqualToken();
        public static readonly IToken NotEqual = new NotEqualToken();
        public static readonly IToken GreaterThan = new GreaterThanToken();
        public static readonly IToken GreaterThanOrEqual = new GreaterThanOrEqualToken();
        public static readonly IToken LessThan = new LessThanToken();
        public static readonly IToken LessThanOrEqual = new LessThanOrEqualToken();
        public static readonly IToken AndAlso = new AndToken();
        public static readonly IToken OrElse = new OrToken();
        public static readonly IToken Not = new BitwiseNotToken();
        public static readonly IToken LeftParenthesis = new GeneralOperationToken(TokenType.OpenParen);
        public static readonly IToken RightParenthesis = new GeneralOperationToken(TokenType.CloseParen);
        public static readonly IToken Comma = new AddToken();
        public static readonly IToken Dot = new AddToken();
        public static readonly IToken QuestionMark = new AddToken();
        public static readonly IToken Colon = new ColonToken();
        public static readonly IToken True = new ValueToken(Expression.Constant(true));
        public static readonly IToken False = new ValueToken(Expression.Constant(false));
        public static readonly IToken Null = new AddToken();
        public static readonly IToken Identifier = new AddToken();
        public static readonly IToken BitwiseAnd = new BitwiseAndToken();
        public static readonly IToken BitwiseOr = new BitwiseOrToken();
        public static readonly IToken BitwiseXor = new BitwiseXorToken();
        public static readonly IToken BitwiseNot = new BitwiseNotToken();
        public static readonly IToken LeftShift = new AddToken();
        public static readonly IToken RightShift = new AddToken();
        public static readonly IToken Assign = new AddToken();
        public static readonly IToken NullCoalescing = new NullCoalescingToken();
        public static readonly IToken Conditional = new ConditionalToken();
        public static readonly IToken In = new InToken();
        public static readonly IToken As = new AsToken();
    }

  
}
