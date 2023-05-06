using System.Linq.Expressions;
using Punica.Dynamic;
using Punica.Linq.Dynamic.Abstractions;
using Punica.Linq.Dynamic.Expressions;

namespace Punica.Linq.Dynamic.Tokens
{
    public class NewToken : IExpression
    {
        public List<Argument> Tokens { get; }
        public TokenType TokenType => TokenType.Member;
        public ExpressionType ExpressionType => ExpressionType.New;

        public NewToken()
        {
            Tokens = new List<Argument>();
        }

        public void AddToken(Argument token)
        {
            Tokens.Add(token);
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
            foreach (var token in Tokens)
            {
                var expression = ExpressionEvaluator.Evaluate(token.Tokens);

                expressions.Add(expression);
            }

            var properties = new List<AnonymousProperty>();
            var bindkeys = new Dictionary<string, Expression>();

            foreach (var expression in expressions)
            {
                var name = GetName(expression);
                bindkeys[name] = expression;
                properties.Add(new AnonymousProperty(name, expression.Type));
            }

            var type = AnonymousTypeFactory.CreateType(properties);

            var bindings = new List<MemberBinding>();
            var members = type.GetProperties();

            foreach (var member in members)
            {
                bindings.Add(Expression.Bind(member, bindkeys[member.Name]));
            }

            return Expression.MemberInit(Expression.New(type), bindings);
        }
    }
}
