using System.Linq.Expressions;
using Punica.Dynamic;
using Punica.Linq.Dynamic.Expressions;
using Punica.Linq.Dynamic.Tokens.abstractions;

namespace Punica.Linq.Dynamic.Tokens
{
    public class NewToken : IExpressionToken
    {
       // public Expression MemberExpression { get; }
       // public IExpression? Parameter { get; }
        public List<Argument> Tokens { get; }
        public bool IsLeftAssociative => true;
        public short Precedence => 14;
        public TokenType TokenType => TokenType.Operator;
        public ExpressionType ExpressionType => ExpressionType.New;

        public NewToken()
        {
            Tokens = new List<Argument>();
           // Parameter = parameter;
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
                    return GetName(memberExpression.Expression) + memberExpression.Member.Name;
                    break;
                case ExpressionType.Parameter:
                    return "";
                    break;
                case ExpressionType.Extension:
                    if (expression is AliasExpression e)
                    {
                        return e.Alias;
                    }
                    throw new ArgumentException("Invalid Expression");
                    break;
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
