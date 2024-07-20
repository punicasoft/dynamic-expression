using System;
using System.Globalization;
using System.Linq.Expressions;
using System.Reflection;
using Punica.Extensions;
using Punica.Linq.Dynamic.Abstractions;
using Punica.Linq.Dynamic.Expressions;

namespace Punica.Linq.Dynamic
{
    public class Evaluator
    {
        public Dictionary<string, Identifier> Identifiers { get; } = new Dictionary<string, Identifier>();
        private HashSet<Type> _types = new HashSet<Type>();

        //TODO try remove this.
        public MethodContext MethodContext { get; }
        public Expression? VariablesInstance { get; private set; }


        public Evaluator()
        {
            MethodContext = new MethodContext();
        }

        public Evaluator AddStartParameter(Type type)
        {
            return AddParameter(Expression.Parameter(type, "_arg"));
        }

        public Evaluator AddParameter(Type type, string name)
        {
            return AddParameter(Expression.Parameter(type, name));
        }

        public Evaluator AddParameter<T>(string? name)
        {
            return AddParameter(Expression.Parameter(typeof(T), name));
        }

        public Evaluator AddParameter(ParameterExpression expression)
        {
            AddAllTypes(expression.Type);
            MethodContext.AddParameter(new ParameterToken(expression));
            return this;
        }

        public Evaluator AddVariable(string name, object? value)
        {
            Identifiers.Add(name, new Identifier(name, Expression.Constant(value)));
            return this;
        }

        public Evaluator AddType<T>()
        {
           _types.Add(typeof(T));
            return this;
        }

        /// <summary>
        /// Get variable through instance properties
        /// </summary>
        /// <param name="variablesInstance"></param>
        /// <returns></returns>
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
            var rootToken = Tokenizer.Evaluate(new Parser(text, MethodContext, VariablesInstance, _types, Identifiers));
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


        private void AddAllTypes(Type type)
        {
            if (type.IsCollection(out var elementType))
            {
                var add = _types.Add(elementType!);

                if (!add)
                {
                    return;
                }

                var propertyInfos = type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                    .Where(p => !p.PropertyType.IsPrimitive && !p.PropertyType.IsInterface );

                foreach (var propertyInfo in propertyInfos)
                {
                    AddAllTypes(propertyInfo.PropertyType);
                }
            }
            else
            {
                var add = _types.Add(type);

                if (!add)
                {
                    return;
                }


                var propertyInfos = type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                    .Where(p => !p.PropertyType.IsPrimitive && !p.PropertyType.IsInterface );

                foreach (var propertyInfo in propertyInfos)
                {
                    AddAllTypes(propertyInfo.PropertyType);
                }
            }
        }
    }
}
