using System.Linq.Expressions;
using Punica.Linq.Dynamic.Tokens.abstractions;

namespace Punica.Linq.Dynamic
{
    public class ValueToken : IExpressionToken
    {
        public bool IsLeftAssociative => false;
        private Expression? _value;
        private bool _evaluated;
        private readonly IExpression? _expression;
        public short Precedence => 0;

        //public Expression Value
        //{
        //    get
        //    {
        //        if (!_evaluated)
        //        {
        //            _value = _expression.Evaluate();
        //            _evaluated = true;
        //        }

        //        return _value;
        //    }
        //}

       // public Expression Value => _value;

        public TokenType TokenType { get; }
        public ExpressionType ExpressionType => _value?.NodeType ?? ExpressionType.Constant;

        public ValueToken(Expression value)
        {
            _value = value;
            _evaluated = true;
            TokenType = TokenType.Value;
        }

        public ValueToken(IExpression value)
        {
            _expression = value;
            _evaluated = false;
            TokenType = TokenType.Value;
        }

        public Expression Evaluate()
        {
            if (!_evaluated)
            {
                _value = _expression?.Evaluate();
                _evaluated = true;
            }

            return _value!;
        }
    }
}
