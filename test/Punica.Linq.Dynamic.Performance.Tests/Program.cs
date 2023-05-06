// See https://aka.ms/new-console-template for more information

using BenchmarkDotNet.Running;
using Punica.Linq.Dynamic;
using Punica.Linq.Dynamic.Performance.Tests;
using System.Reflection;
using System.Runtime.CompilerServices;
using Punica.Extensions;
using System.Text;
using System;
using System.Linq;
using System.Xml.Linq;
using System.Collections.ObjectModel;
using System.Linq.Expressions;

Console.WriteLine("Hello, World!");


//BenchmarkRunner.Run<DynamicTests>();

//var test = new DynamicTests();
//test.Average_Int_With_Predicate();


Console.WriteLine(typeof(Enumerable).FullName);

var targetType = typeof(List<string>);

Console.WriteLine(targetType.FullName);


var methodInfo = typeof(Enumerable).GetMethod(nameof(Enumerable.All));

var parameterType = methodInfo.GetParameters()[0].ParameterType;


Dictionary<string, List<MethodMetaInfo>> _methods = new Dictionary<string, List<MethodMetaInfo>>();
HashSet<string> _types = new HashSet<string>();
Dictionary<string, List<string>> typeMapCache = new Dictionary<string, List<string>>();



InitializeMethodInfo(typeof(String), BindingFlags.Public | BindingFlags.DeclaredOnly | BindingFlags.Static | BindingFlags.Instance);

Console.WriteLine();
Console.WriteLine(_methods.Keys.Count);
Console.WriteLine("Finding a matching method");
Match<IQueryable<long?>>("Average", 0);

Console.WriteLine();
foreach (var key in typeMapCache.Keys)
{
    Console.WriteLine(key);
    List<string> list = typeMapCache[key];
    Console.WriteLine();
    foreach (var val in list)
    {
        Console.WriteLine(val);
    }
}



string GetName(Type type, bool generic = false)
{
    string? fullName;

    if (type.IsGenericType)
    {

        if (!generic)
        {
            fullName = type.GetGenericTypeDefinition().FullName + "[";
            var arguments = type.GenericTypeArguments;

            foreach (var argument in arguments)
            {
                var underlyingType = Nullable.GetUnderlyingType(argument);
                if (underlyingType != null)
                {
                    fullName += underlyingType.FullName + "?,";
                }
                else
                {
                    fullName += argument.FullName + ",";
                }

            }

            fullName = fullName.TrimEnd(',') + "]";
        }
        else
        {
            fullName = type.GetGenericTypeDefinition().FullName;
        }

    }
    else
    {
        fullName = type.FullName;
    }

    return fullName!;
}


(string, string) GetFullName(Type type)
{
    string? genericName;
    string typeName = "";

    if (type.IsGenericType)
    {
        genericName = type.GetGenericTypeDefinition().FullName;

        typeName += "[";

        var arguments = type.GenericTypeArguments;

        foreach (var argument in arguments)
        {
            var underlyingType = Nullable.GetUnderlyingType(argument);
            if (underlyingType != null)
            {
                typeName += underlyingType.FullName + "?,";
            }
            else
            {
                typeName += argument.FullName + ",";
            }
        }

        typeName = typeName.TrimEnd(',') + "]";

    }
    else
    {
        genericName = type.FullName;
    }

    return (genericName!, typeName);
}

IEnumerable<Type> GetImplementedTypes(Type? type)
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

void AddInterfaces(List<Type> types, Type type)
{
    foreach (var iType in type.GetInterfaces())
    {
        types.Add(iType);
        AddInterfaces(types, iType);
    }
}

//List<string> GetPossibleMatchingNames(Type type)
//{
//    List<string> generalList = new List<string>();

//    var distinct = GetImplementedTypes(type).Distinct();

//    foreach (var implementedType in distinct)
//    {
//        var (baseName, specificName) = GetFullName(implementedType);

//        if (!string.IsNullOrEmpty(specificName))
//        {
//            var name = $"{baseName}{specificName}";

//            if (_types.Contains(baseName))
//            {
//                generalList.Add(baseName + "{0}");
//                generalList.Add(baseName);
//            }
//        }
//        else if (_types.Contains(baseName))
//        {
//            generalList.Add(baseName);
//        }
//    }

//    if (type.IsGenericType)
//    {
//        var genericName = type.GetGenericTypeDefinition().FullName;
//        if (!typeMapCache.ContainsKey(genericName))
//        {
//            typeMapCache.Add(genericName, generalList);
//        }
//    }
//    else
//    {
//        var name = type.FullName;
//        if (!typeMapCache.ContainsKey(name))
//        {
//            typeMapCache.Add(name, generalList);
//        }
//    }

//    return generalList;
//}

List<string> GetPossibleMatchingNames(Type type)
{
    string key;

    if (type.IsGenericType)
    {
        key = type.GetGenericTypeDefinition().FullName;
    }
    else
    {
        key = type.FullName;
    }

    if (typeMapCache.ContainsKey(key))
    {
        return GetNames(type, typeMapCache[key]);
    }

    List<string> generalList = new List<string>();

    var distinct = GetImplementedTypes(type).Distinct();

    foreach (var implementedType in distinct)
    {
        var (baseName, specificName) = GetFullName(implementedType);

        if (!string.IsNullOrEmpty(specificName))
        {
            var name = $"{baseName}{specificName}";

            if (_types.Contains(baseName))
            {
                generalList.Add(baseName + "{0}");
                generalList.Add(baseName);
            }
        }
        else if (_types.Contains(baseName))
        {
            generalList.Add(baseName);
        }
    }

    typeMapCache[key] = generalList;


    return GetNames(type, generalList); ;
}

List<string> GetNames(Type type, List<string> generalNames)
{
    if (type.IsGenericType)
    {
        var arguments = type.GenericTypeArguments;
        var typeName = "[";

        foreach (var argument in arguments)
        {
            var underlyingType = Nullable.GetUnderlyingType(argument);
            if (underlyingType != null)
            {
                typeName += underlyingType.FullName + "?,";
            }
            else
            {
                typeName += argument.FullName + ",";
            }
        }

        typeName = typeName.TrimEnd(',') + "]";

        var list = new List<string>();

        foreach (var name in generalNames)
        {
            list.Add(string.Format(name, typeName));
        }

        return list;
    }

    return generalNames;
}

void Match<T>(string methodName, int count)
{
    var type = typeof(T);

    var names = GetPossibleMatchingNames(type);

    foreach (var name in names)
    {
        var key = $"{name}.{methodName}.{count}";

        if (_methods.ContainsKey(key))
        {
            var methodInfos = _methods[key];

            if (methodInfos.Count == 1)
            {
                Console.WriteLine(key);
                return;
            }
            else if (methodInfos.Count > 1)
            {
                Console.WriteLine("Found many");
                Console.WriteLine(key);
                return;
            }

            throw new ArgumentException($"Method {methodName} with {count} arguments not found in {type.FullName}");
        }
    }

    throw new ArgumentException($"Method {methodName} with {count} arguments not found in {type.FullName} 2");
}

void InitializeMethodInfo(Type type, BindingFlags flags)
{
    var methods = type.GetMethods(flags);

    foreach (var method in methods)
    {
        var isExtensionMethod = method.IsDefined(typeof(ExtensionAttribute));

        var argCount = method.GetParameters().Length;

        var fullName = type.FullName;

        if (isExtensionMethod)
        {
            argCount--;
            var parameterType1 = method.GetParameters()[0].ParameterType;

            if (parameterType1.IsGenericType)
            {
                fullName = parameterType1.GetGenericTypeDefinition().FullName;

                if (fullName == null)
                {
                    continue;
                }

                _types.Add(fullName);

                if (!parameterType1.IsOpenGeneric())
                {
                    //use string builder
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

                _types.Add(fullName);
            }

        }
        else if(method.IsStatic)
        {
            continue;
        }

        var key = $"{fullName}.{method.Name}.{argCount}";

        if (!_methods.ContainsKey(key))
        {
            _methods.Add(key, new List<MethodMetaInfo>() { new MethodMetaInfo(method) });
        }
        else
        {
            _methods[key].Add(new MethodMetaInfo(method));
        }

        Console.WriteLine(key);

    }
}