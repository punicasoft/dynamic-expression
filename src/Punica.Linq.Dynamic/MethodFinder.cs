using System;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using Punica.Extensions;
using Punica.Linq.Dynamic.Tokens;

namespace Punica.Linq.Dynamic
{
    public class MethodFinder
    {
        private static readonly Dictionary<string, List<MethodInfo>> _methods = new Dictionary<string, List<MethodInfo>>();

        public static MethodFinder Instance { get; } = new MethodFinder();

        private MethodFinder()
        {
            InitializeMethodInfo(typeof(Enumerable));
        }


        public void InitializeMethodInfo(Type type)
        {
            var methods = type.GetMethods(BindingFlags.Public | BindingFlags.Static);
            foreach (var method in methods)
            {
                var isExtensionMethod = method.IsDefined(typeof(ExtensionAttribute));

                var argCount = method.GetParameters().Length;

                if (isExtensionMethod)
                {
                    argCount--;
                }

                var key = $"{type.FullName}.{method.Name}.{argCount}";
                if (!_methods.ContainsKey(key))
                {
                    _methods.Add(key, new List<MethodInfo>() { method });
                }
                else
                {
                    _methods[key].Add(method);
                }
            }

        }

        public void Print()
        {
            foreach (var key in _methods.Keys)
            {
                Console.WriteLine(key + " " + _methods[key].Count);
            }
        }

        public MethodInfo GetMethod(Type type, string methodName, List<Argument> args)
        {
            var key = $"{typeof(Enumerable).FullName}.{methodName}.{args.Count}";
            if (type.IsCollection())
            {
                if (_methods.ContainsKey(key))
                {
                    var methodInfos = _methods[key];

                    if (methodInfos.Count == 1)
                    {
                        return methodInfos[0];
                    }
                    else
                    {
                        var methodInfo = FindBestMethod(methodInfos, args, type);

                        if (methodInfo != null)
                        {
                            return methodInfo;
                        }
                        else
                        {
                            throw new ArgumentException($"Method {methodName} with {args.Count} arguments not found in {type.FullName}");
                        }
                    }
                }
            }

            throw new ArgumentException($"Method {methodName} with {args.Count} arguments not found in {type.FullName} 2");
        }

        public MethodInfo? FindBestMethod(List<MethodInfo> methods, List<Argument> args, Type type)
        {
            MethodInfo? bestMatch = null;
            int bestMatchCount = 0;
            List<MethodInfo> matches = new List<MethodInfo>();
            foreach (var method in methods)
            {
                var isExtensionMethod = method.IsDefined(typeof(ExtensionAttribute));
                var parameters = method.GetParameters();
                var argMeta = GetArgData(method);

                int j = 0;

                bool[] bestMatches = new bool[parameters.Length];

                if (isExtensionMethod)
                {

                    if (!IsPassableForGenericType2(parameters[0].ParameterType, type))
                    {
                        continue;
                    }
                    //if (!IsPassableForGenericType(parameters[0].ParameterType.GetGenericTypeDefinition(), type))
                    //{
                    //    continue;
                    //}
                    //if (parameters[0].ParameterType == type)
                    //{
                    //    bestMatches[0] = true;
                    //}
                    //else if (!parameters[0].ParameterType.IsAssignableFrom(type))
                    //{
                    //    continue;
                    //}

                    j++;
                }

                bool match = true;


                for (int i = 0; i < args.Count; i++, j++)
                {
                    var arg = args[i].GetArgumentData();
                    if (arg.IsFunction)
                    {
                        if (parameters[j].ParameterType.GetGenericTypeDefinition() != arg.FuncType)
                        {
                            match = false;
                            break;
                        }

                        //hack not a good one. Try to evaluate first function if it is input is source
                        if (argMeta.EvalOrder[1].Contains(j) && argMeta.EvalOrder[0].Count == 1 && argMeta.EvalOrder[0].Contains(0))
                        {
                            var types = argMeta.LambdasTypes(new Expression[] { Expression.Parameter(type) }, 0);
                            args[i].SetParameterExpressionBody(types[0], 0);

                            arg = args[i].GetArgumentData();

                            if (!parameters[j].ParameterType.GetGenericArguments().Last().IsGenericParameter)
                            {
                                if (parameters[j].ParameterType.GetGenericArguments().Last() == arg.Type)
                                {
                                    bestMatches[j] = true;
                                }
                                else
                                {
                                    match = false;
                                    break;
                                }
                            }

                        }

                    }
                    else
                    {
                        if (parameters[j].ParameterType == arg.Type)
                        {
                            bestMatches[j] = true;
                            continue;
                        }

                        if (!parameters[j].ParameterType.IsAssignableFrom(arg.Type))
                        {
                            match = false;
                            break;
                        }
                    }
                }

                if (match)
                {
                    matches.Add(method);
                    var count = bestMatches.Count(b => b);

                    if (count > bestMatchCount)
                    {
                        bestMatchCount = count;
                        bestMatch = method;
                    }
                }
            }

            if (bestMatch != null)
            {
                return bestMatch;
            }

            return matches.FirstOrDefault();
        }

        public static bool IsPassableForGenericType(Type targetType, Type givenType)
        {
            if (targetType.IsGenericTypeDefinition)
            {
                // Check if the given type itself matches the target generic type definition
                if (givenType.IsGenericType && givenType.GetGenericTypeDefinition() == targetType)
                {
                    return true;
                }

                // Check if any of the given type's implemented interfaces match the target generic type definition
                return givenType.GetInterfaces()
                    .Any(interfaceType => interfaceType.IsGenericType && interfaceType.GetGenericTypeDefinition() == targetType);
            }

            // If targetType is not a generic type definition, use the IsAssignableFrom method
            return targetType.IsAssignableFrom(givenType);
        }

        public static bool IsPassableForGenericType2(Type targetType, Type givenType)
        {
            if (targetType.IsOpenGeneric())
            {
                return IsPassableForGenericType(targetType.GetGenericTypeDefinition(), givenType);
            }
            else
            {
                return targetType.IsAssignableFrom(givenType);
            }
        }



        public SignatureResolver GetArgData(MethodInfo methodInfo)
        {
            var map = new Dictionary<string, Func<Expression[], Type>>();
            var parameters = methodInfo.GetParameters();

            var order = new Dictionary<int, List<int>>();
            order[0] = new List<int>();
            order[1] = new List<int>();
            order[2] = new List<int>();

            var inputs = new Dictionary<int, Func<Expression[], ParameterExpression[], Expression>>();
            var lambdas = new Dictionary<int, Func<Expression[], Type[]>>();

            var genericParametersNames = methodInfo.GetGenericArguments().Select(g => g.Name).ToList();

            //foreach (var parameter in genericParameters)
            //{
            //    Console.WriteLine($"Generic:{parameter.IsGenericType}, Name:{parameter.Name} , {parameter}");
            //    //map[parameter.Name] = null;
            //}

            for (var index = 0; index < parameters.Length; index++)
            {
                var arg = parameters[index];
                if (arg.ParameterType.IsGenericType)
                {
                    if (arg.ParameterType.Name.StartsWith("Func"))
                    {
                        var typeArguments = arg.ParameterType.GetGenericArguments();
                        var last = typeArguments.Last();

                        if (genericParametersNames.Contains(last.Name) && !map.ContainsKey(last.Name))
                        {
                            var index1 = index;
                            map[last.Name] = args => args[index1].Type;  // $"Func_[{index}].Type";
                            order[2].Add(index);
                        }

                        var parTypes = new List<Func<Expression[], Type>>();

                        //set input types and skip last output types
                        for (var i = 0; i < typeArguments.Length - 1; i++)
                        {
                            if (typeArguments[i].IsOpenGeneric())
                            {
                                if (map.ContainsKey(typeArguments[i].Name)) // Func<Tsource>
                                {
                                    parTypes.Add(map[typeArguments[i].Name]);
                                    order[1].Add(index);
                                }
                                else
                                {
                                    var genericArguments = typeArguments[i].GetGenericArguments();

                                    foreach (var genericArgument in genericArguments)
                                    {
                                        if (map.ContainsKey(genericArgument.Name))
                                        {
                                            var i1 = i;
                                            parTypes.Add(args => typeArguments[i1].GetGenericTypeDefinition().MakeGenericType(map[genericArgument.Name](args)));

                                            if (!order[1].Contains(index))
                                            {
                                                order[1].Add(index);
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                var i1 = i;
                                parTypes.Add(args => typeArguments[i1]);
                            }
                           
                        }

                        lambdas[index] = args =>
                        {
                            var outputArray = new Type[parTypes.Count];

                            for (var i = 0; i < parTypes.Count; i++)
                            {
                                outputArray[i] = parTypes[i](args);
                            }

                            return outputArray;
                        };

                        var index2 = index;
                        inputs[index] = (args, paras) =>
                        {
                            var ins = lambdas[index2](args);
                            var typeArgs = new Type[ins.Length + 1];
                            Array.Copy(ins, typeArgs, ins.Length);
                            typeArgs[ins.Length] = args[index2].Type;

                            return Expression.Lambda(
                                    arg.ParameterType.GetGenericTypeDefinition().MakeGenericType(typeArgs),
                                    args[index2],
                                    paras);
                        };
                    }
                    else
                    {
                        if (genericParametersNames.Contains(arg.ParameterType.Name) && !map.ContainsKey(arg.ParameterType.Name))
                        {
                            var index1 = index;
                            map[arg.ParameterType.Name] = args => args[index1].Type;
                            order[0].Add(index);
                        }
                        else
                        {
                            var genericArguments = arg.ParameterType.GetGenericArguments();

                            for (var i = 0; i < genericArguments.Length; i++)
                            {
                                var argument = genericArguments[i];
                                if (genericParametersNames.Contains(argument.Name) && !map.ContainsKey(argument.Name))
                                {
                                    var index1 = index;
                                    var i1 = i;

                                    if (i1 == 0)
                                    {
                                        map[argument.Name] =
                                            args =>
                                            {
                                                if (args[index1].Type.IsArray)
                                                {
                                                    return args[index1].Type.GetElementType()!;
                                                }

                                                return args[index1].Type.GetGenericArguments()[i1];
                                            };
                                    }
                                    else
                                    {
                                        map[argument.Name] =
                                            args => args[index1].Type.GetGenericArguments()[i1];
                                    }


                                    if (!order[0].Contains(index))
                                    {
                                        order[0].Add(index);
                                    }
                                }
                            }
                        }

                        var index2 = index;
                        inputs[index] = (args, paras) => args[index2];
                    }
                }
                else
                {
                    var index1 = index;
                    inputs[index] = (args, paras) => args[index1]; //No mapping returns the same
                }
            }

            return new SignatureResolver(methodInfo, map.Values.ToArray(), lambdas.Values.ToArray(), inputs.Values.ToArray(), lambdas.Keys.ToArray(), order);
        }

    }

    public class SignatureResolver
    {
        private readonly MethodInfo _methodInfo;
        private readonly Func<Expression[], Type>[] _genericArgumentsResolvers;
        private readonly Func<Expression[], Type[]>[] _lambdasInputTypesResolvers;
        private readonly Func<Expression[], ParameterExpression[], Expression>[] _methodArgumentResolvers;
        private readonly int[] _funcIndex;
        public Dictionary<int, List<int>> EvalOrder { get; private set; }

        public int FuncCount => _lambdasInputTypesResolvers.Length;

        public bool IsFunc(int index)
        {
            return _funcIndex.Contains(index);
        }

        public Type[] LambdasTypes(Expression[] args, int index)
        {
            return _lambdasInputTypesResolvers[index](args);
        }

        public SignatureResolver(MethodInfo methodInfo,
            Func<Expression[], Type>[] genericArgumentsResolvers,
            Func<Expression[], Type[]>[] lambdasInputTypesResolvers,
            Func<Expression[], ParameterExpression[], Expression>[] methodArgumentResolvers, int[] funcIndex, Dictionary<int, List<int>> evalOrder)
        {
            _methodInfo = methodInfo;
            _genericArgumentsResolvers = genericArgumentsResolvers;
            _lambdasInputTypesResolvers = lambdasInputTypesResolvers;
            _methodArgumentResolvers = methodArgumentResolvers;
            _funcIndex = funcIndex;
            EvalOrder = evalOrder;
        }

        public MethodCallExpression Resolve(Expression[] args, Expression[] finalArgs)
        {
            if (_methodInfo.IsGenericMethodDefinition)
            { 
               var methodInfo = _methodInfo.MakeGenericMethod(GetGenericTypeArguments(args));

               return Expression.Call(methodInfo, finalArgs);
            }

            return Expression.Call(_methodInfo, finalArgs);
        }

        public Type[] GetGenericTypeArguments(Expression[] args)
        {
            Type[] outputArray = new Type[_genericArgumentsResolvers.Length];

            for (var i = 0; i < _genericArgumentsResolvers.Length; i++)
            {
                var func = _genericArgumentsResolvers[i];
                outputArray[i] = func(args);
            }

            return outputArray;
        }

        //public Expression[] GetArguments(Expression[] args, ParameterExpression[] paras)
        //{
        //    Expression[] outputArray = new Expression[_methodArgumentResolvers.Length];

        //    for (var i = 0; i < _methodArgumentResolvers.Length; i++)
        //    {
        //        var func = _methodArgumentResolvers[i];
        //        outputArray[i] = func(args, paras);
        //    }

        //    return outputArray;
        //}

        //Can't use above as for each lambda we need to create new lambda with their own parameters
        public Expression GetArguments(Expression[] args, ParameterExpression[] paras, int index)
        {
            var func = _methodArgumentResolvers[index];
            return func(args, paras);
        }
    }
}
