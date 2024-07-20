using System.Linq.Expressions;

namespace Punica.Linq.Dynamic.Tests.Utils
{
    public abstract class ExpressionTestsBase
    {
        protected Expression<Func<TResult>> GetExpression<TResult>(string expression)
        {
            var eval = new Evaluator();
            return eval.Parse<Expression<Func<TResult>>>(expression);
        }

        protected Expression<Func<T1, TResult>> GetExpression<T1, TResult>(string expression)
        {
            var eval = new Evaluator()
                .AddStartParameter(typeof(T1));
            return eval.Parse<Expression<Func<T1, TResult>>>(expression);
        }

        protected LambdaExpression GetGeneralExpression<T1>(string expression)
        {
            var eval = new Evaluator()
                .AddStartParameter(typeof(T1));
            return eval.Parse(expression);
        }

        protected LambdaExpression GetGeneralExpression<T1>(string expression, Type type)
        {
            var eval = new Evaluator()
                .AddStartParameter(type);

            return eval.Parse(expression);
        }

        protected LambdaExpression GetGeneralExpression(string expression, Expression? variables = null, params ParameterExpression[] parameters)
        {

            var eval = new Evaluator()
                .SetVariableInstance(variables);

            foreach (var parameter in parameters)
            {
                eval.AddParameter(parameter);
            }

            return eval.Parse(expression);

        }

    }
}
