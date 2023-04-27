namespace Punica.Linq.Dynamic
{
    public enum Operator
    {
        Unknown,
        Sequence, // ,

        // Assignment Operators
        Assignment, // =
        AdditionAssignment, // +=
        SubtractionAssignment, // -=
        MultiplicationAssignment,
        DivisionAssignment,
        ModuloAssignment,
        BitwiseAndAssignment,
        BitwiseOrAssignment,
        BitwiseXorAssignment,
        LeftShiftAssignment,
        RightShiftAssignment,

        // Conditional Operators
        Question, //?
        Colon, // :

        // Null-coalescing operator
        NullCoalescing, // ??


        // Logical Operators
        Or, // ||
        And, // &&

        // Bitwise Operators
        BitwiseOr, // |
        BitwiseAnd, // &
        BitwiseXor,

        // Equality Operators
        Equal,
        NotEqual,

        // Relational Operators
        GreaterThanEqual,
        LessThan,
        GreaterThan,
        LessThanEqual,

        // Type Operators
        Is,
        As,

        // Collection
        In,

        // Shift Operators
        LeftShift,
        RightShift,

        // Arithmetic Operators
        Plus,
        Minus,
        Multiply,
        Divide,
        Modulo,

        // Unary Operators
        Tide,
        Not,
        Increment,
        Decrement,

        // Grouping Operators
        OpenParen,
        CloseParen,

        OpenCurlyParen,
        CloseCurlyParen,

        OpenBracket,
        CloseBracket,

        // New Operator
        New,

        // Other Operator
        Dot,

        Method,
    }
}
