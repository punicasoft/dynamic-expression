using System.Linq.Expressions;

namespace Punica.Linq.Dynamic.Expressions
{
    // TODO : revisit the code as moved from another project
    public static class ExpressionExtension
    {
        public static Expression<Func<TSource, TOut>> Bind<TSource, TProperty, TOut>(
            this Expression<Func<TSource, TProperty>> propertySelector, Expression<Func<TProperty, TOut>> propertyPredicate)
        {
            var expr = Expression.Lambda<Func<TSource, TOut>>(propertyPredicate.Body, propertySelector.Parameters);
            var binder = new ParameterToMemberExpressionBinder(propertyPredicate.Parameters[0], propertySelector.Body);
            expr = (Expression<Func<TSource, TOut>>)binder.Visit(expr);
            return expr;
        }

        public static Expression<Func<T, bool>> OrElse<T>(
            this Expression<Func<T, bool>> expr1,
            Expression<Func<T, bool>> expr2)
        {
            var parameter = Expression.Parameter(typeof(T));

            var leftVisitor = new ReplaceExpressionVisitor(expr1.Parameters[0], parameter);
            var left = leftVisitor.Visit(expr1.Body);

            var rightVisitor = new ReplaceExpressionVisitor(expr2.Parameters[0], parameter);
            var right = rightVisitor.Visit(expr2.Body);

            return Expression.Lambda<Func<T, bool>>(
                Expression.OrElse(left, right), parameter);
        }

        public static Expression<Func<T, bool>> AndAlso<T>(
            this Expression<Func<T, bool>> expr1,
            Expression<Func<T, bool>> expr2)
        {
            var parameter = Expression.Parameter(typeof(T));

            var leftVisitor = new ReplaceExpressionVisitor(expr1.Parameters[0], parameter);
            var left = leftVisitor.Visit(expr1.Body);

            var rightVisitor = new ReplaceExpressionVisitor(expr2.Parameters[0], parameter);
            var right = rightVisitor.Visit(expr2.Body);

            return Expression.Lambda<Func<T, bool>>(
                Expression.AndAlso(left, right), parameter);
        }

        public static Expression<Func<T, TResult>> ReplaceParameter<T, TResult>(
            this Expression<Func<T, TResult>> expr1,
            ParameterExpression parameter)
        {
            var leftVisitor = new ReplaceExpressionVisitor(expr1.Parameters[0], parameter);

            return Expression.Lambda<Func<T, TResult>>(leftVisitor.Visit(expr1.Body), parameter);
        }

        private sealed class ReplaceExpressionVisitor
            : ExpressionVisitor
        {
            private readonly Expression _oldValue;
            private readonly Expression _newValue;

            public ReplaceExpressionVisitor(Expression oldValue, Expression newValue)
            {
                _oldValue = oldValue;
                _newValue = newValue;
            }

            public override Expression Visit(Expression node)
            {
                if (node == _oldValue)
                    return _newValue;
                return base.Visit(node);
            }
        }

    }
}
