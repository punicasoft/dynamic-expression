using System.Linq.Expressions;
using Punica.Linq.Dynamic.Abstractions;

namespace Punica.Linq.Dynamic.Expressions
{
    public class MethodToken : IExpression
    {
        public string MethodName { get; }
        public IExpression MemberExpression { get; }
        public List<Argument> Arguments { get; }
        public TokenType TokenType => TokenType.Member;
        public ExpressionType ExpressionType => ExpressionType.Call;


        public MethodToken(string methodName, IExpression memberExpression)
        {
            MethodName = methodName;
            MemberExpression = memberExpression;
            Arguments = new List<Argument>();
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

            var metaInfo = MethodFinder.Instance.GetMethod(memberExpression.Type, MethodName, Arguments);
            var resolver = metaInfo.Resolver;

            if (metaInfo.IsExtension)
            {
                var expressions = new Expression[Arguments.Count + 1];
                var finalExpressions = new Expression[Arguments.Count + 1];
                expressions[0] = memberExpression;
                finalExpressions[0] = memberExpression;

                int funcIndex = 0;
                
                for (var i = 1; i < Arguments.Count + 1; i++)
                {
                    var index = i - 1;
                    var token = Arguments[index];
                    var paras = Array.Empty<ParameterExpression>();

                    if (resolver.IsFunc(i))
                    {
                        var types = resolver.LambdasTypes(expressions, funcIndex);
                        paras = new ParameterExpression[types.Length];

                        for (int j = 0; j < types.Length; j++)
                        {
                            var type = types[j];
                            paras[j] = token.SetParameterType(type, j);
                        }

                        funcIndex++;
                    }
                    expressions[i] = token.Evaluate();
                    finalExpressions[i] = resolver.GetArguments(expressions, paras, i);
                }

                return resolver.Resolve(expressions, finalExpressions);
            }
            else
            {
                var expressions = new Expression[Arguments.Count];
                var finalExpressions = new Expression[Arguments.Count];

                int funcIndex = 0;

                for (var i = 0; i < Arguments.Count; i++)
                {
                    var token = Arguments[i];
                    var paras = Array.Empty<ParameterExpression>();

                    if (resolver.IsFunc(i))
                    {
                        var types = resolver.LambdasTypes(expressions, funcIndex);
                        paras = new ParameterExpression[types.Length];

                        for (int j = 0; j < types.Length; j++)
                        {
                            var type = types[j];
                            paras[j] = token.SetParameterType(type, j);
                        }

                        funcIndex++;
                    }
                    expressions[i] = token.Evaluate();
                    finalExpressions[i] = resolver.GetArguments(expressions, paras, i);
                }

                return resolver.Resolve(memberExpression, expressions, finalExpressions);
            }
            //TODO add static method invocation

        }
    }
}
