using System;
using System.Linq.Expressions;
using System.Reflection;
using Punica.Linq.Dynamic.Abstractions;

namespace Punica.Linq.Dynamic.Expressions
{
    public class NewConstructorToken : INewExpression
    {
        public List<Argument> Arguments { get; }
        public List<Argument> ConstructorArguments { get; }
        public TokenType TokenType => TokenType.Member;//TODO check this
        public ExpressionType ExpressionType => ExpressionType.New;
        public Type? Type { get; private set; }

        public NewConstructorToken(Type type, List<Argument> constructorArguments)
        {
            ConstructorArguments = constructorArguments;
            Type = type;
            Arguments = new List<Argument>();
        }

        public void AddArgument(Argument token)
        {
            Arguments.Add(token);
        }

        public Expression Evaluate()
        {
            List<Expression> constructorExpressions = new List<Expression>();

            foreach (var ctArgument in ConstructorArguments)
            {
                var expression = ctArgument.Evaluate();

                constructorExpressions.Add(expression);
            }

            List<Expression> expressions = new List<Expression>();
            foreach (var argument in Arguments)
            {
                var expression = argument.Evaluate();

                expressions.Add(expression);
            }

            var bindKeys = new Dictionary<string, Expression>();

            foreach (var expression in expressions)
            {
                var name = GetName(expression);
                bindKeys[name] = expression;
            }

            var bindings = new List<MemberBinding>();
            var members = Type!.GetProperties();

            foreach (var member in members)
            {
                if (bindKeys.ContainsKey(member.Name))
                {
                    bindings.Add(Expression.Bind(member, bindKeys[member.Name]));
                }
            }

            var types = constructorExpressions.Select(c => c.Type).ToArray();
            var constructor = Type.GetConstructor(types);

            if (constructor == null)
            {
                throw new ArgumentException($"Could not find the constructor for given arguments types: {string.Join(',', types.Select(t => t.Name))}");
            }

            return Expression.MemberInit(Expression.New(constructor, constructorExpressions), bindings);
        }

        private string GetName(Expression expression)
        {
            switch (expression.NodeType)
            {
                case ExpressionType.MemberAccess:
                    var memberExpression = (MemberExpression)expression;

                    if (memberExpression.Expression.NodeType == ExpressionType.MemberAccess)
                    {
                        return GetName(memberExpression.Expression) + memberExpression.Member.Name;
                    }

                    return memberExpression.Member.Name;
                case ExpressionType.Parameter:
                    return ((ParameterExpression)expression).Name;
                case ExpressionType.Constant:
                    return "";
                case ExpressionType.Extension:
                    if (expression is AliasExpression e)
                    {
                        return e.Alias;
                    }
                    throw new ArgumentException("Invalid Expression");
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

    }
}
