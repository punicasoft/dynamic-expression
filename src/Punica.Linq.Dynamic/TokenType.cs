namespace Punica.Linq.Dynamic
{
    public enum TokenType
    {
        Unknown,
        Operator,
        OpenParen,
        CloseParen,
        OpenCurlyParen,
        CloseCurlyParen,
        OpenBracket,
        CloseBracket,
        Member,
        String,
        Number,
        RealNumber,
        Boolean,
        Parameter,
        Sequence,
    }
}
