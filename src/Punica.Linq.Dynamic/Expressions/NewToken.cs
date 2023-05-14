using System.Linq.Expressions;
using Punica.Dynamic;
using Punica.Linq.Dynamic.Abstractions;

namespace Punica.Linq.Dynamic.Expressions
{
    public class NewToken : IExpression
    {
        public List<Argument> Arguments { get; }
        public TokenType TokenType => TokenType.Member;//TODO check this
        public ExpressionType ExpressionType => ExpressionType.New;
        public Type? Type { get; private set; }
        public bool IsAnonymous { get; private set; } = true;

        public NewToken()
        {
            Arguments = new List<Argument>();

        }

        public void AddToken(Argument token)
        {
            Arguments.Add(token);
        }

        internal void SetType(Type type)
        {
            Type = type;
            IsAnonymous = false;
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
                var expression = ExpressionEvaluator.Evaluate(argument.Tokens);

                expressions.Add(expression);
            }

            if (IsAnonymous)
            {

                var properties = new List<AnonymousProperty>();
                var bindKeys = new Dictionary<string, Expression>();

                foreach (var expression in expressions)
                {
                    var name = GetName(expression);
                    bindKeys[name] = expression;
                    properties.Add(new AnonymousProperty(name, expression.Type));
                }

                var type = AnonymousTypeFactory.CreateType(properties);

                var bindings = new List<MemberBinding>();
                var members = type.GetProperties();

                foreach (var member in members)
                {
                    bindings.Add(Expression.Bind(member, bindKeys[member.Name]));
                }

                return Expression.MemberInit(Expression.New(type), bindings);
            }
            else
            {
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

                return Expression.MemberInit(Expression.New(Type!), bindings);
            }

        }
    }
}
