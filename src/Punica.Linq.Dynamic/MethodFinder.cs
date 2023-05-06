using System.Collections.Concurrent;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using Punica.Extensions;
using Punica.Linq.Dynamic.Expressions;

namespace Punica.Linq.Dynamic
{
    public class MethodFinder
    {
        private static readonly Dictionary<string, List<MethodMetaInfo>> Methods = new Dictionary<string, List<MethodMetaInfo>>();
        private static readonly HashSet<string> Refrences = new HashSet<string>();
        private static readonly HashSet<string> Types = new HashSet<string>();
        private static readonly ConcurrentDictionary<string, IReadOnlyList<string>> TypeMapCache = new ConcurrentDictionary<string, IReadOnlyList<string>>();

        public static MethodFinder Instance { get; } = new MethodFinder();

        private MethodFinder()
        {
            var list = new List<string>()
            {
                "System.Collections.Generic.IEnumerable`1{0}",
                "System.Collections.Generic.IEnumerable`1",
                "System.Collections.IEnumerable"
            };

            var list2 = new List<string>()
            {
                "System.Collections.IEnumerable"
            };

            var list3 = new List<string>()
            {
                "System.Linq.IQueryable`1{0}",
                "System.Linq.IQueryable`1",
                "System.Linq.IQueryable",
                "System.Collections.Generic.IEnumerable`1{0}",
                "System.Collections.Generic.IEnumerable`1",
                "System.Collections.IEnumerable"
            };

            TypeMapCache.TryAdd("System.Array", list);
            TypeMapCache.TryAdd("System.Collections.Generic.List`1", list);
            TypeMapCache.TryAdd("System.Collections.Generic.IList`1", list);
            TypeMapCache.TryAdd("System.Collections.Generic.ICollection`1", list);
            TypeMapCache.TryAdd("System.Collections.Generic.IEnumerable`1", list);
            TypeMapCache.TryAdd("System.Collections.Generic.IOrderedEnumerable`1", list);
            TypeMapCache.TryAdd("System.Collections.Generic.IReadOnlyList`1", list);
            TypeMapCache.TryAdd("System.Collections.Generic.IReadOnlyCollection`1", list);
            TypeMapCache.TryAdd("System.Linq.IQueryable`1", list3);
            TypeMapCache.TryAdd("System.Linq.EnumerableQuery`1", list3);
            TypeMapCache.TryAdd("System.Linq.IOrderedQueryable`1", list3);
            TypeMapCache.TryAdd("Microsoft.EntityFrameworkCore.DbSet`1", list3);

            TypeMapCache.TryAdd("System.String", new List<string>(1){ "System.String" });


            TypeMapCache.TryAdd("System.Collections.IList", list2);
            TypeMapCache.TryAdd("System.Collections.ICollection", list2);
            TypeMapCache.TryAdd("System.Collections.IEnumerable", list2);

            InitializeMethodInfo(typeof(Enumerable), BindingFlags.Public | BindingFlags.Static);
            InitializeMethodInfo(typeof(Queryable), BindingFlags.Public | BindingFlags.Static);
            InitializeMethodInfo(typeof(string), BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static);
        }


        public void InitializeMethodInfo(Type type, BindingFlags flags)
        {
            var fullName = type.FullName;

            if (fullName != null && !Refrences.Contains(fullName))
            {
                Refrences.Add(fullName);

                var methods = type.GetMethods(flags);

                foreach (var method in methods)
                {
                    var isExtensionMethod = method.IsDefined(typeof(ExtensionAttribute));

                    var parameters = method.GetParameters();

                    var argCount = parameters.Length;

                    if (isExtensionMethod)
                    {
                        argCount--;
                        var parameterType1 = parameters[0].ParameterType;

                        if (parameterType1.IsGenericType)
                        {
                            fullName = parameterType1.GetGenericTypeDefinition().FullName;

                            if (fullName == null)
                            {
                                continue;
                            }

                            Types.Add(fullName);

                            if (!parameterType1.IsOpenGeneric())
                            {
                                var sb = new StringBuilder();
                                sb.Append(fullName);
                                sb.Append("[");

                                var arguments = parameterType1.GenericTypeArguments;

                                foreach (var argument in arguments)
                                {
                                    var underlyingType = Nullable.GetUnderlyingType(argument);
                                    if (underlyingType != null)
                                    {
                                        sb.Append(underlyingType.FullName);
                                        sb.Append("?,");
                                    }
                                    else
                                    {
                                        sb.Append(argument.FullName);
                                        sb.Append(",");
                                    }

                                }

                                sb.Remove(sb.Length - 1, 1);
                                sb.Append("]");
                                fullName = sb.ToString();
                            }

                        }
                        else
                        {
                            fullName = parameterType1.FullName;

                            if (fullName == null)
                            {
                                continue;
                            }

                            Types.Add(fullName);
                        }

                    }
                    else if (method.IsStatic)
                    {
                        continue; // skip static methods TODO: add support for static methods
                    }

                    var key = $"{fullName}.{method.Name}.{argCount}";

                    if (!Methods.ContainsKey(key))
                    {
                        Methods.Add(key, new List<MethodMetaInfo>() { new MethodMetaInfo(method) });
                    }
                    else
                    {
                        Methods[key].Add(new MethodMetaInfo(method));
                    }
                }
            }
        }

        public IEnumerable<Type> GetImplementedTypes(Type? type)
        {
            var types = new List<Type>();
            if (type != null && type.IsInterface)
            {
                types.Add(type);
                AddInterfaces(types, type);
            }
            else
            {
                while (type != null)
                {
                    types.Add(type);
                    AddInterfaces(types, type);
                    type = type.BaseType;
                }
            }

            return types;
        }

        public void AddInterfaces(List<Type> types, Type type)
        {
            foreach (var iType in type.GetInterfaces())
            {
                types.Add(iType);
                AddInterfaces(types, iType);
            }
        }


        private IReadOnlyList<string> GetPossibleMatchingNames(Type type)
        {
            string key;

            if (type.IsGenericType)
                key = type.GetGenericTypeDefinition().FullName!;
            else
                key = type.IsArray ? "System.Array" : type.FullName!;


            if (TypeMapCache.ContainsKey(key))
            {
                return GetNames(type, TypeMapCache[key]);
            }

            throw new Exception("test"); //TODO: remove if everything works, added to see cache miss happen or not

            List<string> generalList = new List<string>();

            var distinct = GetImplementedTypes(type).Distinct();

            foreach (var implementedType in distinct)
            {
                var baseName = implementedType.IsGenericType ? implementedType.GetGenericTypeDefinition().FullName! : implementedType.FullName!;

                if (implementedType.IsGenericType)
                {
                    if (Types.Contains(baseName))
                    {
                        generalList.Add(baseName + "{0}");
                        generalList.Add(baseName);
                    }
                }
                else if (Types.Contains(baseName))
                {
                    generalList.Add(baseName);
                }
            }

            TypeMapCache[key] = generalList;


            return GetNames(type, generalList);
        }

        IReadOnlyList<string> GetNames(Type type, IReadOnlyList<string> generalNames)
        {
            if (type.IsGenericType)
            {
                var arguments = type.GenericTypeArguments;
                var sb = new StringBuilder();
                sb.Append("[");

                foreach (var argument in arguments)
                {
                    var underlyingType = Nullable.GetUnderlyingType(argument);
                    if (underlyingType != null)
                    {
                        sb.Append(underlyingType.FullName);
                        sb.Append("?,");
                    }
                    else
                    {
                        sb.Append(argument.FullName);
                        sb.Append(",");
                    }
                }

                sb.Remove(sb.Length - 1, 1);
                sb.Append("]");
                var typeName = sb.ToString();

                var list = new List<string>();

                foreach (var name in generalNames)
                {
                    list.Add(string.Format(name, typeName));
                }

                return list;
            }

            if (type.IsArray)
            {
                var elementType = type.GetElementType();
                var typeName = "[" + elementType!.FullName + "]";

                var underlyingType = Nullable.GetUnderlyingType(elementType);

                if (underlyingType != null)
                {
                    typeName = "[" + underlyingType.FullName + "?]";
                }

                var list = new List<string>();

                foreach (var name in generalNames)
                {
                    list.Add(string.Format(name, typeName));
                }

                return list;
            }


            return generalNames;
        }


        public MethodMetaInfo GetMethod(Type type, string methodName, List<Argument> args)
        {

            var names = GetPossibleMatchingNames(type);

            foreach (var name in names)
            {
                var key = $"{name}.{methodName}.{args.Count}";

                if (Methods.ContainsKey(key))
                {
                    var methodInfos = Methods[key];

                    if (methodInfos.Count == 1)
                    {
                        return methodInfos[0];
                    }

                    var methodInfo = FindBestMethod(methodInfos, args, type);

                    if (methodInfo != null)
                    {
                        return methodInfo;
                    }

                    throw new ArgumentException($"Method {methodName} with {args.Count} arguments not found in {type.FullName}");
                }
            }

            throw new ArgumentException($"Method {methodName} with {args.Count} arguments not found in {type.FullName} 2");
        }



        public MethodMetaInfo? FindBestMethod(List<MethodMetaInfo> metaInfos, List<Argument> args, Type type)
        {
            MethodMetaInfo? bestMatch = null;
            int bestMatchCount = 0;
            List<MethodMetaInfo> matches = new List<MethodMetaInfo>();
            foreach (var metaInfo in metaInfos)
            {
                var parameters = metaInfo.MethodInfo.GetParameters();
                var argMeta = metaInfo.Resolver;

                int j = 0;

                bool[] bestMatches = new bool[parameters.Length];

                if (metaInfo.IsExtension)
                {

                    if (!IsPassableForGenericType2(parameters[0].ParameterType, type))
                    {
                        continue;
                    }

                    j++;
                }

                bool match = true;


                for (int i = 0; i < args.Count; i++, j++)
                {
                    var argument = args[i];
                    var argumentData = argument.GetArgumentData();
                    var parameterType = parameters[j].ParameterType;

                    if (argumentData.IsFunction)
                    {
                        if (parameterType.GetGenericTypeDefinition() == typeof(Expression<>))
                        {
                            //Extract delegate inside of Expression<Func<>>
                            parameterType = parameterType.GetGenericArguments()[0];
                        }

                        if (parameterType.GetGenericTypeDefinition() != argumentData.FuncType)
                        {
                            match = false;
                            break;
                        }
                        
                        //hack not a good one. Try to evaluate first function if it is input is source
                        if (argMeta.EvalOrder[1].Contains(j) && argMeta.EvalOrder[0].Count == 1 && argMeta.EvalOrder[0].Contains(0))
                        {
                            var types = argMeta.LambdasTypes(new Expression[] { Expression.Parameter(type) }, 0);
                            argument.SetParameterType(types[0], 0);

                            argumentData = argument.GetArgumentData();

                            if (!parameterType.GetGenericArguments().Last().IsGenericParameter)
                            {
                                if (parameterType.GetGenericArguments().Last() == argumentData.Type)
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
                        if (parameterType == argumentData.Type)
                        {
                            bestMatches[j] = true;
                            continue;
                        }

                        if (!parameterType.IsAssignableFrom(argumentData.Type))
                        {
                            match = false;
                            break;
                        }
                    }
                }

                if (match)
                {
                    matches.Add(metaInfo);
                    var count = bestMatches.Count(b => b);

                    if (count > bestMatchCount)
                    {
                        bestMatchCount = count;
                        bestMatch = metaInfo;
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

            var list0 = new List<int>();
            var list1 = new List<int>();
            var list2 = new List<int>();

            var inputs = new Dictionary<int, Func<Expression[], ParameterExpression[], Expression>>();
            var lambdas = new Dictionary<int, Func<Expression[], Type[]>>();

            var genericParametersNames = methodInfo.GetGenericArguments().Select(g => g.Name).ToList();

            for (var index = 0; index < parameters.Length; index++)
            {
                var arg = parameters[index];
                if (arg.ParameterType.IsGenericType)
                {
                    var parameterType = arg.ParameterType;

                    if (parameterType.GetGenericTypeDefinition() == typeof(Expression<>))
                    {
                        parameterType = parameterType.GetGenericArguments()[0];
                    }

                    if (parameterType.Name.StartsWith("Func", StringComparison.Ordinal))
                    {
                        var typeArguments = parameterType.GetGenericArguments();
                        var last = typeArguments.Last();

                        if (genericParametersNames.Contains(last.Name) && !map.ContainsKey(last.Name))
                        {
                            var index1 = index;
                            map[last.Name] = args => args[index1].Type;
                            list2.Add(index);
                        }

                        var parTypes = new List<Func<Expression[], Type>>();

                        //set input types and skip last output types
                        for (var i = 0; i < typeArguments.Length - 1; i++)
                        {
                            var typeArgument = typeArguments[i];

                            if (typeArgument.IsOpenGeneric())
                            {
                                if (map.ContainsKey(typeArgument.Name)) // Func<Tsource>
                                {
                                    parTypes.Add(map[typeArgument.Name]);
                                    list1.Add(index);
                                }
                                else
                                {
                                    var genericArguments = typeArgument.GetGenericArguments();

                                    foreach (var genericArgument in genericArguments)
                                    {
                                        if (map.ContainsKey(genericArgument.Name))
                                        {
                                            parTypes.Add(args => typeArgument.GetGenericTypeDefinition().MakeGenericType(map[genericArgument.Name](args)));

                                            if (!list1.Contains(index))
                                            {
                                                list1.Add(index);
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                parTypes.Add(args => typeArgument);
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
                            var body = args[index2];
                            typeArgs[ins.Length] = body.Type;

                            return Expression.Lambda(
                                    parameterType.GetGenericTypeDefinition().MakeGenericType(typeArgs),
                                    body,
                                    paras);
                        };
                    }
                    else
                    {
                        if (genericParametersNames.Contains(parameterType.Name) && !map.ContainsKey(parameterType.Name))
                        {
                            var index1 = index;
                            map[parameterType.Name] = args => args[index1].Type;
                            list0.Add(index);
                        }
                        else
                        {
                            var genericArguments = parameterType.GetGenericArguments();

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
                                                var type = args[index1].Type;

                                                return type.IsArray ? type.GetElementType()! : type.GetGenericArguments()[i1];
                                            };
                                    }
                                    else
                                    {
                                        map[argument.Name] =
                                            args => args[index1].Type.GetGenericArguments()[i1];
                                    }


                                    if (!list0.Contains(index))
                                    {
                                        list0.Add(index);
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

            return new SignatureResolver(methodInfo, map.Values.ToArray(), lambdas.Values.ToArray(), inputs.Values.ToArray(), lambdas.Keys.ToArray(), new[]{ list0, list1, list2 });
        }

    }

    public class SignatureResolver
    {
        private readonly MethodInfo _methodInfo;
        private readonly Func<Expression[], Type>[] _genericArgumentsResolvers;
        private readonly Func<Expression[], Type[]>[] _lambdasInputTypesResolvers;
        private readonly Func<Expression[], ParameterExpression[], Expression>[] _methodArgumentResolvers;
        private readonly int[] _funcIndex;
        public List<int>[] EvalOrder { get; private set; }

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
            Func<Expression[], ParameterExpression[], Expression>[] methodArgumentResolvers, int[] funcIndex, List<int>[] evalOrder)
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

        public MethodCallExpression Resolve(Expression instance, Expression[] args, Expression[] finalArgs)
        {
            if (_methodInfo.IsGenericMethodDefinition)
            {
                var methodInfo = _methodInfo.MakeGenericMethod(GetGenericTypeArguments(args));

                return Expression.Call(instance, methodInfo, finalArgs);
            }

            return Expression.Call(instance, _methodInfo, finalArgs);
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


    public class MethodMetaInfo
    {
        private SignatureResolver? _resolver;
        public MethodInfo MethodInfo { get; }
        public bool IsExtension { get; }

        public SignatureResolver Resolver => _resolver ??= MethodFinder.Instance.GetArgData(MethodInfo);

        public MethodMetaInfo(MethodInfo methodInfo)
        {
            MethodInfo = methodInfo;
            IsExtension = methodInfo.IsDefined(typeof(ExtensionAttribute), false);
        }

    }
}
