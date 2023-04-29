using System.Linq.Expressions;
using Punica.Extensions;

namespace Punica.Linq.Dynamic.Reflection
{
    public class EnumerableMethodHandler : IMethodHandler, ILinqMethods
    {
        public MethodCallExpression CallMethod(string methodName, Expression member, ParameterExpression parameter, Expression[] expressions)
        {
            switch (methodName)
            {
                case "All":
                    return All(member, parameter, expressions);
                case "Any":
                    return Any(member, parameter, expressions);
                case "Average":
                    return Average(member, parameter, expressions);
                case "Cast":
                    return Cast(member, parameter, expressions);
                case "Concat":
                    return Concat(member, parameter, expressions);
                case "Contains":
                    return Contains(member, parameter, expressions);
                case "Count":
                    return Count(member, parameter, expressions);
                case "DefaultIfEmpty":
                    return DefaultIfEmpty(member, parameter, expressions);
                case "Distinct":
                    return Distinct(member, parameter, expressions);
                case "ElementAt":
                    return ElementAt(member, parameter, expressions);
                case "ElementAtOrDefault":
                    return ElementAtOrDefault(member, parameter, expressions);
                case "Except":
                    return Except(member, parameter, expressions);
                case "First":
                    return First(member, parameter, expressions);
                case "FirstOrDefault":
                    return FirstOrDefault(member, parameter, expressions);
                case "GroupBy":
                    return GroupBy(member, parameter, expressions);
                case "GroupJoin":
                    return GroupJoin(member, parameter, expressions);
                case "Intersect":
                    return Intersect(member, parameter, expressions);
                case "Join":
                    return Join(member, parameter, expressions);
                case "Last":
                    return Last(member, parameter, expressions);
                case "LastOrDefault":
                    return LastOrDefault(member, parameter, expressions);
                case "LongCount":
                    return LongCount(member, parameter, expressions);
                case "Max":
                    return Max(member, parameter, expressions);
                case "Min":
                    return Min(member, parameter, expressions);
                case "OrderBy":
                    return OrderBy(member, parameter, expressions);
                case "OrderByDescending":
                    return OrderByDescending(member, parameter, expressions);
                case "Reverse":
                    return Reverse(member, parameter, expressions);
                case "Select":
                    return Select(member, parameter, expressions);
                case "SelectMany":
                    return SelectMany(member, parameter, expressions);
                case "SequenceEqual":
                    return SequenceEqual(member, parameter, expressions);
                case "Single":
                    return Single(member, parameter, expressions);
                case "SingleOrDefault":
                    return SingleOrDefault(member, parameter, expressions);
                case "Skip":
                    return Skip(member, parameter, expressions);
                case "SkipWhile":
                    return SkipWhile(member, parameter, expressions);
                case "Sum":
                    return Sum(member, parameter, expressions);
                case "Take":
                    return Take(member, parameter, expressions);
                case "TakeWhile":
                    return TakeWhile(member, parameter, expressions);
                case "ThenBy":
                    return ThenBy(member, parameter, expressions);
                case "ThenByDescending":
                    return ThenByDescending(member, parameter, expressions);
                case "ToArray":
                    return ToArray(member, parameter, expressions);
                case "ToList":
                    return ToList(member, parameter, expressions);
                case "Union":
                    return Union(member, parameter, expressions);
                case "Where":
                    return Where(member, parameter, expressions);
                default:
                    throw new ArgumentException($"{methodName} is not supported or not available method in {nameof(Enumerable)}");
            }
        }

        public MethodCallExpression All(Expression member, ParameterExpression parameter, Expression[] expressions)
        {
            var type = typeof(Func<,>).MakeGenericType(parameter.Type, typeof(bool));
            var arg1 = Expression.Lambda(type, expressions[0], parameter);
            return Expression.Call(EnumerableCachedMethodInfo.All(parameter.Type), member, arg1);
        }

        public MethodCallExpression Any(Expression member, ParameterExpression parameter, Expression[] expressions)
        {
            switch (expressions.Length)
            {
                case 0:
                    return Expression.Call(EnumerableCachedMethodInfo.Any(parameter.Type), member);
                case 1:
                    var type = typeof(Func<,>).MakeGenericType(parameter.Type, typeof(bool));
                    var arg1 = Expression.Lambda(type, expressions[0], parameter);
                    return Expression.Call(EnumerableCachedMethodInfo.Any_Predicate(parameter.Type), member, arg1);
                default:
                    throw new ArgumentException("Invalid number of parameters");
            }
        }

        public MethodCallExpression Average(Expression member, ParameterExpression parameter, Expression[] expressions)
        {
            if (expressions.Length != 1)
            {
                throw new ArgumentException("Invalid number of parameters");
            }

            var type = typeof(Func<,>).MakeGenericType(parameter.Type, expressions[0].Type);
            var arg1 = Expression.Lambda(type, expressions[0], parameter);

            if (expressions[0].Type == typeof(int))
            {
                return Expression.Call(EnumerableCachedMethodInfo.Average_Int_TSource(parameter.Type), member, arg1);
            }
            else if (expressions[0].Type == typeof(long))
            {
                return Expression.Call(EnumerableCachedMethodInfo.Average_Long_TSource(parameter.Type), member, arg1);
            }
            else if (expressions[0].Type == typeof(float))
            {
                return Expression.Call(EnumerableCachedMethodInfo.Average_Float_TSource(parameter.Type), member, arg1);
            }
            else if (expressions[0].Type == typeof(double))
            {
                return Expression.Call(EnumerableCachedMethodInfo.Average_Double_TSource(parameter.Type), member, arg1);
            }
            else if (expressions[0].Type == typeof(decimal))
            {
                return Expression.Call(EnumerableCachedMethodInfo.Average_Decimal_TSource(parameter.Type), member, arg1);
            }
            else if (expressions[0].Type == typeof(int?))
            {
                return  Expression.Call(EnumerableCachedMethodInfo.Average_NullableInt_TSource(parameter.Type), member, arg1);
            }
            else if (expressions[0].Type == typeof(long?))
            {
                return Expression.Call(EnumerableCachedMethodInfo.Average_NullableLong_TSource(parameter.Type), member, arg1);
            }
            else if (expressions[0].Type == typeof(float?))
            {
                return Expression.Call(EnumerableCachedMethodInfo.Average_NullableFloat_TSource(parameter.Type), member, arg1);
            }
            else if (expressions[0].Type == typeof(double?))
            {
                return Expression.Call(EnumerableCachedMethodInfo.Average_NullableDouble_TSource(parameter.Type), member, arg1);
            }
            else if (expressions[0].Type == typeof(decimal?))
            {
                return Expression.Call(EnumerableCachedMethodInfo.Average_NullableDecimal_TSource(parameter.Type), member, arg1);
            }
            else
            {
                throw new ArgumentException("Invalid parameter type");
            }
        }

        public MethodCallExpression Cast(Expression member, ParameterExpression parameter, Expression[] expressions)
        {
            throw new NotImplementedException();
        }

        public MethodCallExpression Concat(Expression member, ParameterExpression parameter, Expression[] expressions)
        {
            if (expressions.Length != 1)
            {
                throw new ArgumentException("Invalid number of parameters");
            }

            return Expression.Call(EnumerableCachedMethodInfo.Concat(parameter.Type), member, expressions[0]);
        }

        public MethodCallExpression Contains(Expression member, ParameterExpression parameter, Expression[] expressions)
        {
            if (expressions.Length != 1)
            {
                throw new ArgumentException("Invalid number of parameters");
            }

            if (expressions[0].Type == parameter.Type)
            {
                return Expression.Call(EnumerableCachedMethodInfo.Contains(parameter.Type), member, expressions[0]);
            }

            throw new ArgumentException("Invalid parameter type");

        }

        public MethodCallExpression Count(Expression member, ParameterExpression parameter, Expression[] expressions)
        {
            switch (expressions.Length)
            {
                case 0:
                    return Expression.Call(EnumerableCachedMethodInfo.Count(parameter.Type), member);
                case 1:
                    var type = typeof(Func<,>).MakeGenericType(parameter.Type, typeof(bool));
                    var arg1 = Expression.Lambda(type, expressions[0], parameter);
                    return Expression.Call(EnumerableCachedMethodInfo.Count_Predicate(parameter.Type), member, arg1);
                default:
                    throw new ArgumentException("Invalid number of parameters");
            }
        }

        public MethodCallExpression DefaultIfEmpty(Expression member, ParameterExpression parameter, Expression[] expressions)
        {
            switch (expressions.Length)
            {
                case 0:
                    return Expression.Call(EnumerableCachedMethodInfo.DefaultIfEmpty(parameter.Type), member);
                case 1:
                    var arg1 = expressions[0];
                    return Expression.Call(EnumerableCachedMethodInfo.DefaultIfEmpty_DefaultValue(parameter.Type), member, arg1);
                default:
                    throw new ArgumentException("Invalid number of parameters");
            }
        }

        public MethodCallExpression Distinct(Expression member, ParameterExpression parameter, Expression[] expressions)
        {
            switch (expressions.Length)
            {
                case 0:
                    return Expression.Call(EnumerableCachedMethodInfo.Distinct(parameter.Type), member);
                default:
                    throw new ArgumentException("Invalid number of parameters");
            }
        }

        public MethodCallExpression ElementAt(Expression member, ParameterExpression parameter, Expression[] expressions)
        {
            if (expressions.Length != 1)
            {
                throw new ArgumentException("Invalid number of parameters");
            }

            var arg1 = expressions[0];
            return Expression.Call(EnumerableCachedMethodInfo.ElementAt(parameter.Type), member, arg1);
        }

        public MethodCallExpression ElementAtOrDefault(Expression member, ParameterExpression parameter, Expression[] expressions)
        {
            if (expressions.Length != 1)
            {
                throw new ArgumentException("Invalid number of parameters");
            }

            var arg1 = expressions[0];
            return Expression.Call(EnumerableCachedMethodInfo.ElementAtOrDefault(parameter.Type), member, arg1);
        }

        public MethodCallExpression Except(Expression member, ParameterExpression parameter, Expression[] expressions)
        {
            if (expressions.Length != 1)
            {
                throw new ArgumentException("Invalid number of parameters");
            }

            var arg1 = expressions[0];
            return Expression.Call(EnumerableCachedMethodInfo.Except(parameter.Type), member, arg1);
        }

        public MethodCallExpression First(Expression member, ParameterExpression parameter, Expression[] expressions)
        {
            switch (expressions.Length)
            {
                case 0:
                    return Expression.Call(EnumerableCachedMethodInfo.First(parameter.Type), member);
                case 1:
                    var type = typeof(Func<,>).MakeGenericType(parameter.Type, typeof(bool));
                    var arg1 = Expression.Lambda(type, expressions[0], parameter);
                    return Expression.Call(EnumerableCachedMethodInfo.First_Predicate(parameter.Type), member, arg1);
                default:
                    throw new ArgumentException("Invalid number of parameters");
            }
        }

        public MethodCallExpression FirstOrDefault(Expression member, ParameterExpression parameter, Expression[] expressions)
        {
            switch (expressions.Length)
            {
                case 0:
                    return Expression.Call(EnumerableCachedMethodInfo.FirstOrDefault(parameter.Type), member);
                case 1:
                    var type = typeof(Func<,>).MakeGenericType(parameter.Type, typeof(bool));
                    var arg1 = Expression.Lambda(type, expressions[0], parameter);
                    return Expression.Call(EnumerableCachedMethodInfo.FirstOrDefault_Predicate(parameter.Type), member, arg1);
                default:
                    throw new ArgumentException("Invalid number of parameters");
            }
        }

        public MethodCallExpression GroupBy(Expression member, ParameterExpression parameter, Expression[] expressions)
        {
            if (expressions.Length is < 1 or > 2)
            {
                throw new ArgumentException("Invalid number of parameters");
            }

            var type = typeof(Func<,>).MakeGenericType(parameter.Type, expressions[0].Type);
            var arg1 = Expression.Lambda(type, expressions[0], parameter);

            switch (expressions.Length)
            {
                case 1:
                    return Expression.Call(EnumerableCachedMethodInfo.GroupBy_KeySelector(parameter.Type, expressions[0].Type), member, arg1);
                default:
                    var type2 = typeof(Func<,>).MakeGenericType(parameter.Type, typeof(bool));
                    var arg2 = Expression.Lambda(type2, expressions[0], parameter);
                    return Expression.Call(EnumerableCachedMethodInfo.GroupBy_ValueSelector(parameter.Type, expressions[0].Type, expressions[1].Type), member, arg1, arg2);
            }
        }

        public MethodCallExpression GroupJoin(Expression member, ParameterExpression parameter, Expression[] expressions)
        {
            throw new NotImplementedException();
        }

        public MethodCallExpression Intersect(Expression member, ParameterExpression parameter, Expression[] expressions)
        {
            if (expressions.Length != 1)
            {
                throw new ArgumentException("Invalid number of parameters");
            }

            var arg1 = expressions[0];
            return Expression.Call(EnumerableCachedMethodInfo.Intersect(parameter.Type), member, arg1);
        }

        public MethodCallExpression Join(Expression member, ParameterExpression parameter, Expression[] expressions)
        {
            throw new NotImplementedException();
        }

        public MethodCallExpression Last(Expression member, ParameterExpression parameter, Expression[] expressions)
        {
            switch (expressions.Length)
            {
                case 0:
                    return Expression.Call(EnumerableCachedMethodInfo.Last(parameter.Type), member);
                case 1:
                    var type = typeof(Func<,>).MakeGenericType(parameter.Type, typeof(bool));
                    var arg1 = Expression.Lambda(type, expressions[0], parameter);
                    return Expression.Call(EnumerableCachedMethodInfo.Last_Predicate(parameter.Type), member, arg1);
                default:
                    throw new ArgumentException("Invalid number of parameters");
            }
        }

        public MethodCallExpression LastOrDefault(Expression member, ParameterExpression parameter, Expression[] expressions)
        {
            switch (expressions.Length)
            {
                case 0:
                    return Expression.Call(EnumerableCachedMethodInfo.LastOrDefault(parameter.Type), member);
                case 1:
                    var type = typeof(Func<,>).MakeGenericType(parameter.Type, typeof(bool));
                    var arg1 = Expression.Lambda(type, expressions[0], parameter);
                    return Expression.Call(EnumerableCachedMethodInfo.LastOrDefault_Predicate(parameter.Type), member, arg1);
                default:
                    throw new ArgumentException("Invalid number of parameters");
            }
        }

        public MethodCallExpression LongCount(Expression member, ParameterExpression parameter, Expression[] expressions)
        {
            switch (expressions.Length)
            {
                case 0:
                    return Expression.Call(EnumerableCachedMethodInfo.LongCount(parameter.Type), member);
                case 1:
                    var type = typeof(Func<,>).MakeGenericType(parameter.Type, typeof(bool));
                    var arg1 = Expression.Lambda(type, expressions[0], parameter);
                    return Expression.Call(EnumerableCachedMethodInfo.LongCount_Predicate(parameter.Type), member, arg1);
                default:
                    throw new ArgumentException("Invalid number of parameters");
            }
        }

        public MethodCallExpression Max(Expression member, ParameterExpression parameter, Expression[] expressions)
        {
            switch (expressions.Length)
            {
                case 0:
                    return Expression.Call(EnumerableCachedMethodInfo.Max(parameter.Type), member);
                case 1:
                    var type = typeof(Func<,>).MakeGenericType(parameter.Type, expressions[0].Type);
                    var arg1 = Expression.Lambda(type, expressions[0], parameter);
                    return Expression.Call(EnumerableCachedMethodInfo.Max_Selector(parameter.Type, expressions[0].Type), member, arg1);
                default:
                    throw new ArgumentException("Invalid number of parameters");
            }
        }

        public MethodCallExpression Min(Expression member, ParameterExpression parameter, Expression[] expressions)
        {
            switch (expressions.Length)
            {
                case 0:
                    return Expression.Call(EnumerableCachedMethodInfo.Min(parameter.Type), member);
                case 1:
                    var type = typeof(Func<,>).MakeGenericType(parameter.Type, expressions[0].Type);
                    var arg1 = Expression.Lambda(type, expressions[0], parameter);
                    return Expression.Call(EnumerableCachedMethodInfo.Min_Selector(parameter.Type, expressions[0].Type), member, arg1);
                default:
                    throw new ArgumentException("Invalid number of parameters");
            }
        }

        public MethodCallExpression OrderBy(Expression member, ParameterExpression parameter, Expression[] expressions)
        {
            if (expressions.Length != 1)
            {
                throw new ArgumentException("Invalid number of parameters");
            }

            var type = typeof(Func<,>).MakeGenericType(parameter.Type, expressions[0].Type);
            var arg1 = Expression.Lambda(type, expressions[0], parameter);
            return Expression.Call(EnumerableCachedMethodInfo.OrderBy_KeySelector(parameter.Type, expressions[0].Type), member, arg1);

        }

        public MethodCallExpression OrderByDescending(Expression member, ParameterExpression parameter, Expression[] expressions)
        {
            if (expressions.Length != 1)
            {
                throw new ArgumentException("Invalid number of parameters");
            }

            var type = typeof(Func<,>).MakeGenericType(parameter.Type, expressions[0].Type);
            var arg1 = Expression.Lambda(type, expressions[0], parameter);
            return Expression.Call(EnumerableCachedMethodInfo.OrderByDescending_KeySelector(parameter.Type, expressions[0].Type), member, arg1);
        }

        public MethodCallExpression Reverse(Expression member, ParameterExpression parameter, Expression[] expressions)
        {
            if (expressions.Length != 0)
            {
                throw new ArgumentException("Invalid number of parameters");
            }
            
            return Expression.Call(EnumerableCachedMethodInfo.Reverse(parameter.Type), member);
        }

        public MethodCallExpression Select(Expression member, ParameterExpression parameter, Expression[] expressions)
        {
            switch (expressions.Length)
            {
                case 1:
                    var type = typeof(Func<,>).MakeGenericType(parameter.Type, expressions[0].Type);
                    var arg1 = Expression.Lambda(type, expressions[0], parameter);
                    return Expression.Call(EnumerableCachedMethodInfo.Select(parameter.Type, expressions[0].Type), member, arg1);
                default:
                    throw new ArgumentException("Invalid number of parameters");
            }
        }

        public MethodCallExpression SelectMany(Expression member, ParameterExpression parameter, Expression[] expressions)
        {
            throw new NotImplementedException();
        }

        public MethodCallExpression SequenceEqual(Expression member, ParameterExpression parameter, Expression[] expressions)
        {
            switch (expressions.Length)
            {
                case 1:
                    var arg1 = expressions[0];
                    return Expression.Call(EnumerableCachedMethodInfo.SequenceEqual(parameter.Type), member, arg1);
                default:
                    throw new ArgumentException("Invalid number of parameters");
            }
        }

        public MethodCallExpression Single(Expression member, ParameterExpression parameter, Expression[] expressions)
        {
            switch (expressions.Length)
            {
                case 0:
                    return Expression.Call(EnumerableCachedMethodInfo.Single(parameter.Type), member);
                case 1:
                    var type = typeof(Func<,>).MakeGenericType(parameter.Type, typeof(bool));
                    var arg1 = Expression.Lambda(type, expressions[0], parameter);
                    return Expression.Call(EnumerableCachedMethodInfo.Single_Predicate(parameter.Type), member, arg1);
                default:
                    throw new ArgumentException("Invalid number of parameters");
            }
        }

        public MethodCallExpression SingleOrDefault(Expression member, ParameterExpression parameter, Expression[] expressions)
        {
            switch (expressions.Length)
            {
                case 0:
                    return Expression.Call(EnumerableCachedMethodInfo.SingleOrDefault(parameter.Type), member);
                case 1:
                    var type = typeof(Func<,>).MakeGenericType(parameter.Type, typeof(bool));
                    var arg1 = Expression.Lambda(type, expressions[0], parameter);
                    return Expression.Call(EnumerableCachedMethodInfo.SingleOrDefault_Predicate(parameter.Type), member, arg1);
                default:
                    throw new ArgumentException("Invalid number of parameters");
            }
        }

        public MethodCallExpression Skip(Expression member, ParameterExpression parameter, Expression[] expressions)
        {
            switch (expressions.Length)
            {
                case 1:
                    var arg1 = expressions[0];
                    return Expression.Call(EnumerableCachedMethodInfo.Skip(parameter.Type), member, arg1);
                default:
                    throw new ArgumentException("Invalid number of parameters");
            }
        }

        public MethodCallExpression SkipWhile(Expression member, ParameterExpression parameter, Expression[] expressions)
        {
            switch (expressions.Length)
            {
                case 1:
                    var type = typeof(Func<,>).MakeGenericType(parameter.Type, typeof(bool));
                    var arg1 = Expression.Lambda(type, expressions[0], parameter);
                    return Expression.Call(EnumerableCachedMethodInfo.SkipWhile(parameter.Type), member, arg1);
                default:
                    throw new ArgumentException("Invalid number of parameters");
            }
        }

        public MethodCallExpression Sum(Expression member, ParameterExpression parameter, Expression[] expressions)
        {
            if (expressions.Length == 0)
            {
                return Expression.Call(EnumerableCachedMethodInfo.Sum(parameter.Type), member);
            }

            if (expressions.Length != 1)
            {
                throw new ArgumentException("Invalid number of parameters");
            }

            var type = typeof(Func<,>).MakeGenericType(parameter.Type, expressions[0].Type);
            var arg1 = Expression.Lambda(type, expressions[0], parameter);

            if (expressions[0].Type == typeof(int))
            {
                return Expression.Call(EnumerableCachedMethodInfo.Sum(parameter.Type), member, arg1);
            }
            else if (expressions[0].Type == typeof(long))
            {
                return Expression.Call(EnumerableCachedMethodInfo.Sum(parameter.Type), member, arg1);
            }
            else if (expressions[0].Type == typeof(float))
            {
                return Expression.Call(EnumerableCachedMethodInfo.Sum(parameter.Type), member, arg1);
            }
            else if (expressions[0].Type == typeof(double))
            {
                return Expression.Call(EnumerableCachedMethodInfo.Sum(parameter.Type), member, arg1);
            }
            else if (expressions[0].Type == typeof(decimal))
            {
                return Expression.Call(EnumerableCachedMethodInfo.Sum(parameter.Type), member, arg1);
            }
            else if (expressions[0].Type == typeof(int?))
            {
                return Expression.Call(EnumerableCachedMethodInfo.Sum(parameter.Type), member, arg1);
            }
            else if (expressions[0].Type == typeof(long?))
            {
                return Expression.Call(EnumerableCachedMethodInfo.Sum(parameter.Type), member, arg1);
            }
            else if (expressions[0].Type == typeof(float?))
            {
                return Expression.Call(EnumerableCachedMethodInfo.Sum(parameter.Type), member, arg1);
            }
            else if (expressions[0].Type == typeof(double?))
            {
                return Expression.Call(EnumerableCachedMethodInfo.Sum(parameter.Type), member, arg1);
            }
            else if (expressions[0].Type == typeof(decimal?))
            {
                return Expression.Call(EnumerableCachedMethodInfo.Sum(parameter.Type), member, arg1);
            }
            else
            {
                throw new ArgumentException("Invalid parameter type");
            }
        }

        public MethodCallExpression Take(Expression member, ParameterExpression parameter, Expression[] expressions)
        {

            switch (expressions.Length)
            {
                case 1:
                    var arg1 = expressions[0];
                    return Expression.Call(EnumerableCachedMethodInfo.Take(parameter.Type), member, arg1);
                default:
                    throw new ArgumentException("Invalid number of parameters");
            }
        }

        public MethodCallExpression TakeWhile(Expression member, ParameterExpression parameter, Expression[] expressions)
        {
            switch (expressions.Length)
            {
                case 1:
                    var type = typeof(Func<,>).MakeGenericType(parameter.Type, typeof(bool));
                    var arg1 = Expression.Lambda(type, expressions[0], parameter);
                    return Expression.Call(EnumerableCachedMethodInfo.TakeWhile(parameter.Type), member, arg1);
                default:
                    throw new ArgumentException("Invalid number of parameters");
            }
        }

        public MethodCallExpression ThenBy(Expression member, ParameterExpression parameter, Expression[] expressions)
        {
            if (expressions.Length != 1)
            {
                throw new ArgumentException("Invalid number of parameters");
            }

            var type = typeof(Func<,>).MakeGenericType(parameter.Type, expressions[0].Type);
            var arg1 = Expression.Lambda(type, expressions[0], parameter);
            return Expression.Call(EnumerableCachedMethodInfo.ThenBy_Selector(parameter.Type, expressions[0].Type), member, arg1);
        }

        public MethodCallExpression ThenByDescending(Expression member, ParameterExpression parameter, Expression[] expressions)
        {
            if (expressions.Length != 1)
            {
                throw new ArgumentException("Invalid number of parameters");
            }

            var type = typeof(Func<,>).MakeGenericType(parameter.Type, expressions[0].Type);
            var arg1 = Expression.Lambda(type, expressions[0], parameter);
            return Expression.Call(EnumerableCachedMethodInfo.ThenByDescending_Selector(parameter.Type, expressions[0].Type), member, arg1);
        }

        public MethodCallExpression ToArray(Expression member, ParameterExpression parameter, Expression[] expressions)
        {
            if (expressions.Length != 0)
            {
                throw new ArgumentException("Invalid number of parameters");
            }

            return Expression.Call(EnumerableCachedMethodInfo.ToArray(parameter.Type), member);
        }

        public MethodCallExpression ToList(Expression member, ParameterExpression parameter, Expression[] expressions)
        {
            if (expressions.Length != 0)
            {
                throw new ArgumentException("Invalid number of parameters");
            }

            var type = member.Type.GetElementOrGenericArgType();
            return Expression.Call(EnumerableCachedMethodInfo.ToList(type), member);
        }

        public MethodCallExpression Union(Expression member, ParameterExpression parameter, Expression[] expressions)
        {
            switch (expressions.Length)
            {
                case 1:
                    var arg1 = expressions[0];
                    return Expression.Call(EnumerableCachedMethodInfo.Union(parameter.Type), member, arg1);
                default:
                    throw new ArgumentException("Invalid number of parameters");
            }
        }

        public MethodCallExpression Where(Expression member, ParameterExpression parameter, Expression[] expressions)
        {

            switch (expressions.Length)
            {
                case 1:
                    var type = typeof(Func<,>).MakeGenericType(parameter.Type, typeof(bool));
                    var arg1 = Expression.Lambda(type, expressions[0], parameter);
                    return Expression.Call(EnumerableCachedMethodInfo.Where(parameter.Type), member, arg1);
                default:
                    throw new ArgumentException("Invalid number of parameters");
            }
        }
    }
}
