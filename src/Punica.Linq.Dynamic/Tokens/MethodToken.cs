using System.Linq.Expressions;
using Punica.Extensions;
using Punica.Linq.Dynamic.Reflection;
using Punica.Linq.Dynamic.Tokens.abstractions;

namespace Punica.Linq.Dynamic.Tokens
{
    /// <summary>
    /// So method token you might need lambada expressions for each parameter if the parameter is function
    /// </summary>
    public class MethodToken : Operation, ITokenList, IExpression
    {
        public string MethodName { get; }
        public IExpression MemberExpression { get; } // TODO support chaining of methods
        private IExpression? Parameter { get; }
        public List<IToken> Tokens { get; }
        public override bool IsLeftAssociative => false;
        public override short Precedence => 14;
        public override ExpressionType ExpressionType => ExpressionType.Call;
        

        public MethodToken(string methodName, IExpression memberExpression, IExpression parameter)
        {
            MethodName = methodName;
            MemberExpression = memberExpression;
            Tokens = new List<IToken>();
            Parameter = parameter;
        }


        public void AddToken(IToken token)
        {
            Tokens.Add(token);
        }

        public override Expression Evaluate(Stack<Expression> stack)
        {
            return Evaluate();
        }

        public Expression Evaluate()
        {
            var memberExpression = MemberExpression.Evaluate();
            var parameter = (ParameterExpression)Parameter.Evaluate();

            if (memberExpression.Type.IsCollection())
            {


                List<Expression> expressions = new List<Expression>();
                foreach (var token in Tokens)
                {
                    var list = token as ITokenList;

                    var expression = Process(list.Tokens);

                    expressions.Add(expression);
                }

                var methodHandler = MethodHandler.Instance.GetHandler(memberExpression.Type);

                return methodHandler.CallMethod(MethodName, memberExpression, parameter, expressions.ToArray());

            }
            else
            {
                switch (MethodName)
                {
                    default:
                        throw new ArgumentException($"Invalid method {MethodName}");
                }
            }
        }
    }


}
