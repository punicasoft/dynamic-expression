using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Punica.Linq.Dynamic.Expressions
{
    // TODO : revisit the code as moved from another project
    public class ParameterToMemberExpressionBinder : ExpressionVisitor
    {
        readonly ParameterExpression _paramExpr;
        readonly Expression _memberExpr;

        public ParameterToMemberExpressionBinder(ParameterExpression paramExpr, Expression memberExpr)
        {
            _paramExpr = paramExpr;
            _memberExpr = memberExpr;
        }

        public override Expression Visit(Expression node)
        {
            return base.Visit(node == _paramExpr ? _memberExpr : node);
        }
    }
}
