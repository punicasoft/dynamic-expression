using System.Linq.Expressions;

namespace Punica.Linq.Dynamic.Tests.Utils
{
    public abstract class ExpressionTestsBase
    {
        protected Expression<Func<TResult>> GetExpression<TResult>(string expression)
        {
            var rootToken = Tokenizer.Evaluate(new TokenContext(expression));

            var resultExpression = rootToken.Evaluate();
            return (Expression<Func<TResult>>)resultExpression;
        }

        protected Expression<Func<T1, TResult>> GetExpression<T1, TResult>(string expression)
        {
            var context = new TokenContext(expression);
            context.AddStartParameter(typeof(T1));
            var rootToken = Tokenizer.Evaluate(context);

            var resultExpression = rootToken.Evaluate();
            return (Expression<Func<T1, TResult>>)resultExpression;
        }

        protected LambdaExpression GetGeneralExpression<T1>(string expression)
        {
            var context = new TokenContext(expression);
            context.AddStartParameter(typeof(T1));
            var rootToken = Tokenizer.Evaluate(context);

            var resultExpression = rootToken.Evaluate();
            return (LambdaExpression)resultExpression;
        }

        protected LambdaExpression GetGeneralExpression<T1>(string expression, Type type)
        {
            var context = new TokenContext(expression);
            context.AddStartParameter(type);
            var rootToken = Tokenizer.Evaluate(context);

            var resultExpression = rootToken.Evaluate();
            return (LambdaExpression)resultExpression;
        }

        protected LambdaExpression GetGeneralExpression(string expression, Expression? variables = null, params ParameterExpression[] parameters)
        {
            var context = new TokenContext(expression, variables);

            foreach (var parameter in parameters)
            {
                context.AddParameter(parameter);
            }

            var rootToken = Tokenizer.Evaluate(context);

            var resultExpression = rootToken.Evaluate();
            return (LambdaExpression)resultExpression;
        }

    }
}
