using System.Linq.Expressions;
using Punica.Linq.Dynamic.Tokens.abstractions;

namespace Punica.Linq.Dynamic.Tokens
{
    public class MethodToken : IExpressionToken
    {
        //private readonly int _depth;
        public string MethodName { get; }
        public IExpression MemberExpression { get; }
        private IExpression? Parameter { get; }
        public List<Argument> Arguments { get; }
        public bool IsLeftAssociative => false;
        public short Precedence => 14;
        public TokenType TokenType => TokenType.Operator;
        public ExpressionType ExpressionType => ExpressionType.Call;


        public MethodToken(string methodName, IExpression memberExpression)
        {
            // _depth = depth;
            MethodName = methodName;
            MemberExpression = memberExpression;
            Arguments = new List<Argument>();
            //Parameter = parameter;
            // Parameter = new ParameterToken(memberExpression, "arg" + _depth);
        }

        public void AddToken(Argument token)
        {
            Arguments.Add(token);
        }

        //IEnumerable<TResult> SelectMany<TSource, TCollection, TResult>(this IEnumerable<TSource> source, Func<TSource, IEnumerable<TCollection>> collectionSelector, Func<TSource, TCollection, TResult> resultSelector)
        //                     SelectMany<TSource, TCollection, TResult>(MemberExpression                , Tokens[0]                                                 , Tokens[1])
        public Expression Evaluate()
        {
            var memberExpression = MemberExpression.Evaluate();
            var expressions = new Expression[Arguments.Count + 1];
            var finalExpressions = new Expression[Arguments.Count + 1];
            expressions[0] = memberExpression;
            finalExpressions[0] = memberExpression;

            //var argData = new ArgumentData[Arguments.Count];


            //for (var i = 0; i < Arguments.Count; i++)
            //{
            //    var argument = Arguments[i];
            //    argData[i] = argument.GetArgumentData();
            //}


            var methodInfo = MethodFinder.Instance.GetMethod(memberExpression.Type, MethodName, Arguments);
            var resolver = MethodFinder.Instance.GetArgData(methodInfo);

            int funcIndex = 0;
            //TODO handle for non extension types
            //TODO handle for indexing in resolver since .IsFunc(i) and resolver.LambdasTypes(expressions, index) different
            for (var i = 1; i < Arguments.Count + 1; i++)
            {
                var index = i - 1;
                var token = Arguments[index];
                var paras = Array.Empty<ParameterExpression>();

                if (resolver.IsFunc(i))
                {
                    var types = resolver.LambdasTypes(expressions, funcIndex); //Might be incorrect, might need function position instead of parameter position
                    paras = new ParameterExpression[types.Length];

                    for (int j = 0; j < types.Length; j++)
                    {
                        var type = types[j];
                        paras[j] = token.SetParameterExpressionBody(type, j);
                    }

                    funcIndex++;
                }
                expressions[i] = token.Evaluate();
                finalExpressions[i] = resolver.GetArguments(expressions, paras, i);
            }

            return resolver.Resolve(expressions, finalExpressions);
        }
    }
}
