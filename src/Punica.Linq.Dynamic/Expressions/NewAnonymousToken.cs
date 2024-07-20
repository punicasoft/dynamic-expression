using Punica.Dynamic;
using Punica.Linq.Dynamic.Abstractions;
using System.Linq.Expressions;

namespace Punica.Linq.Dynamic.Expressions
{
    public class NewAnonymousToken : INewExpression
    {
        public List<Argument> Arguments { get; }
        public TokenType TokenType => TokenType.Member;//TODO check this
        public ExpressionType ExpressionType => ExpressionType.New;
        public Type? Type { get; private set; }

        public NewAnonymousToken()
        {
            Arguments = new List<Argument>();

        }

        public void AddArgument(Argument token)
        {
            Arguments.Add(token);
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


        public Expression Evaluate()
        {
            List<Expression> expressions = new List<Expression>();
            foreach (var argument in Arguments)
            {
                var expression = argument.Evaluate();

                expressions.Add(expression);
            }


            var properties = new List<AnonymousProperty>();
            var bindKeys = new Dictionary<string, Expression>();

            foreach (var expression in expressions)
            {
                var name = GetName(expression);
                bindKeys[name] = expression;
                properties.Add(new AnonymousProperty(name, expression.Type));
            }

            Type = AnonymousTypeFactory.CreateType(properties);

            var bindings = new List<MemberBinding>();
            var members = Type.GetProperties();

            foreach (var member in members)
            {
                bindings.Add(Expression.Bind(member, bindKeys[member.Name]));
            }

            return Expression.MemberInit(Expression.New(Type), bindings);

        }


    }
}
