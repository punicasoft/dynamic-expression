using System;
using System.Globalization;
using System.Linq.Expressions;
using Punica.Linq.Dynamic.Abstractions;
using Punica.Linq.Dynamic.Expressions;

namespace Punica.Linq.Dynamic
{
    public class Evaluator
    {
        public Dictionary<string, Identifier> Identifiers { get; } = new Dictionary<string, Identifier>();

        //TODO remove this. currently used for backward compatibility
        public MethodContext MethodContext { get; }
        public Expression? VariablesInstance { get; private set; }


        public Evaluator()
        {
            MethodContext = new MethodContext();
        }

        public Evaluator AddStartParameter(Type type)
        {
            MethodContext.AddParameter(new ParameterToken(Expression.Parameter(type, "_arg")));
            return this;
        }

        public Evaluator AddLambda(Type type, string name)
        {
            MethodContext.AddParameter(new ParameterToken(Expression.Parameter(type, name)));
            return this;
        }

        public Evaluator AddParameter(ParameterExpression parameter)
        {
            MethodContext.AddParameter(new ParameterToken(parameter));
            return this;
        }

        public Evaluator AddIdentifier(string name, Expression expression)
        {
            Identifiers.Add(name, new Identifier(name, expression));
            return this;
        }

        public Evaluator SetVariableInstance(Expression? variablesInstance)
        {
            VariablesInstance = variablesInstance;
            return this;
        }

        public Identifier? GetIdentifier(string name)
        {
            return Identifiers.TryGetValue(name, out var identifier) ? identifier : null;
        }

        public LambdaExpression Parse(string text)
        {
            return Parse<LambdaExpression>(text);
        }

        public T Parse<T>(string text) where T : Expression
        {
            var rootToken = Tokenizer.Evaluate(new Parser(text, MethodContext, VariablesInstance, Identifiers));
            return (T)rootToken.Evaluate();
        }

        public object? Evaluate(string text, params object?[]? args)
        {
            var expression = Parse<LambdaExpression>(text);
            return expression.Compile().DynamicInvoke(args);
        }

        public T Evaluate<T>(string text)
        {
            var expression = Parse<Expression<T>>(text);
            return expression.Compile();
        }

    }
}
