using System.Diagnostics;
using System.Linq.Expressions;

namespace Punica.Linq.Dynamic.Expressions
{
    [DebuggerDisplay("{Expression}")]
    public class AliasExpression : Expression
    {
        /// <summary>
        /// Gets the containing object of the field or property.
        /// </summary>
        public Expression Expression { get; }

        public string Alias { get; }

        public override ExpressionType NodeType => ExpressionType.Extension;

        public sealed override Type Type => Expression.Type;


        // param order: factories args in order, then other args
        internal AliasExpression(Expression expression, string alias)
        {
            Expression = expression;
            Alias = alias;
        }

        public override bool CanReduce => true;

        public override Expression Reduce()
        {
            return Expression;
        }

        public override string ToString()
        {
            return Expression.ToString();
        }
    }
}
