using System.Linq.Expressions;
using Punica.Linq.Dynamic.Abstractions;

namespace Punica.Linq.Dynamic.Abstractions
{
    public abstract class Operation : IOperation
    {
        public virtual bool IsLeftAssociative => true;
        public abstract short Precedence { get; }
        public TokenType TokenType => TokenType.Operator;
        public abstract ExpressionType ExpressionType { get; }

        public abstract Expression Evaluate(Stack<Expression> stack);

        protected static (Expression left, Expression right) ConvertExpressions(Expression left, Expression right)
        {
            if (left.Type == right.Type)
            {
                return (left, right);
            }

            if (left.Type == typeof(double) || right.Type == typeof(double))
            {
                return (Convert(left, typeof(double)), Convert(right, typeof(double)));
            }

            if (left.Type == typeof(float) || right.Type == typeof(float))
            {
                return (Convert(left, typeof(float)), Convert(right, typeof(float)));
            }

            if (left.Type == typeof(long) || right.Type == typeof(long))
            {
                return (Convert(left, typeof(long)), Convert(right, typeof(long)));
            }

            if (left.Type == typeof(int) || right.Type == typeof(int))
            {
                return (Convert(left, typeof(int)), Convert(right, typeof(int)));
            }

            if (left.Type == typeof(short) || right.Type == typeof(short))
            {
                return (Convert(left, typeof(short)), Convert(right, typeof(short)));
            }

            if (left.Type == typeof(byte) || right.Type == typeof(byte))
            {
                return (Convert(left, typeof(byte)), Convert(right, typeof(byte)));
            }

            throw new InvalidOperationException("Cannot add types " + left.Type + " and " + right.Type);

        }

        protected static Expression Convert(Expression expression, Type type)
        {
            if (expression.Type == type)
            {
                return expression;
            }

            return Expression.Convert(expression, type);
        }
    }
}
