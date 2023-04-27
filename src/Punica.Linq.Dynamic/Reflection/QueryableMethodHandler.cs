using System.Linq.Expressions;

namespace Punica.Linq.Dynamic.Reflection
{
    public class QueryableMethodHandler : IMethodHandler, ILinqMethods
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
            return Expression.Call(QueryableCachedMethodInfo.All(parameter.Type), member, arg1);
        }

        public MethodCallExpression Any(Expression member, ParameterExpression parameter, Expression[] expressions)
        {
            switch (expressions.Length)
            {
                case 0:
                    return Expression.Call(QueryableCachedMethodInfo.Any(parameter.Type), member);
                case 1:
                    var type = typeof(Func<,>).MakeGenericType(parameter.Type, typeof(bool));
                    var arg1 = Expression.Lambda(type, expressions[0], parameter);
                    return Expression.Call(QueryableCachedMethodInfo.Any_Predicate(parameter.Type), member, arg1);
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
                return Expression.Call(QueryableCachedMethodInfo.Average_Int_TSource(parameter.Type), member, arg1);
            }
            else if (expressions[0].Type == typeof(long))
            {
                return Expression.Call(QueryableCachedMethodInfo.Average_Long_TSource(parameter.Type), member, arg1);
            }
            else if (expressions[0].Type == typeof(float))
            {
                return Expression.Call(QueryableCachedMethodInfo.Average_Float_TSource(parameter.Type), member, arg1);
            }
            else if (expressions[0].Type == typeof(double))
            {
                return Expression.Call(QueryableCachedMethodInfo.Average_Double_TSource(parameter.Type), member, arg1);
            }
            else if (expressions[0].Type == typeof(decimal))
            {
                return Expression.Call(QueryableCachedMethodInfo.Average_Decimal_TSource(parameter.Type), member, arg1);
            }
            else if (expressions[0].Type == typeof(int?))
            {
                return Expression.Call(QueryableCachedMethodInfo.Average_NullableInt_TSource(parameter.Type), member, arg1);
            }
            else if (expressions[0].Type == typeof(long?))
            {
                return Expression.Call(QueryableCachedMethodInfo.Average_NullableLong_TSource(parameter.Type), member, arg1);
            }
            else if (expressions[0].Type == typeof(float?))
            {
                return Expression.Call(QueryableCachedMethodInfo.Average_NullableFloat_TSource(parameter.Type), member, arg1);
            }
            else if (expressions[0].Type == typeof(double?))
            {
                return Expression.Call(QueryableCachedMethodInfo.Average_NullableDouble_TSource(parameter.Type), member, arg1);
            }
            else if (expressions[0].Type == typeof(decimal?))
            {
                return Expression.Call(QueryableCachedMethodInfo.Average_NullableDecimal_TSource(parameter.Type), member, arg1);
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

            return Expression.Call(QueryableCachedMethodInfo.Concat(parameter.Type), member, expressions[0]);
        }

        public MethodCallExpression Contains(Expression member, ParameterExpression parameter, Expression[] expressions)
        {
            if (expressions.Length != 1)
            {
                throw new ArgumentException("Invalid number of parameters");
            }

            if (expressions[0].Type == parameter.Type)
            {
                return Expression.Call(QueryableCachedMethodInfo.Contains(parameter.Type), member, expressions[0]);
            }

            throw new ArgumentException("Invalid parameter type");

        }

        public MethodCallExpression Count(Expression member, ParameterExpression parameter, Expression[] expressions)
        {
            switch (expressions.Length)
            {
                case 0:
                    return Expression.Call(QueryableCachedMethodInfo.Count(parameter.Type), member);
                case 1:
                    var type = typeof(Func<,>).MakeGenericType(parameter.Type, typeof(bool));
                    var arg1 = Expression.Lambda(type, expressions[0], parameter);
                    return Expression.Call(QueryableCachedMethodInfo.Count_Predicate(parameter.Type), member, arg1);
                default:
                    throw new ArgumentException("Invalid number of parameters");
            }
        }

        public MethodCallExpression DefaultIfEmpty(Expression member, ParameterExpression parameter, Expression[] expressions)
        {
            switch (expressions.Length)
            {
                case 0:
                    return Expression.Call(QueryableCachedMethodInfo.DefaultIfEmpty(parameter.Type), member);
                case 1:
                    var arg1 = expressions[0];
                    return Expression.Call(QueryableCachedMethodInfo.DefaultIfEmpty_DefaultValue(parameter.Type), member, arg1);
                default:
                    throw new ArgumentException("Invalid number of parameters");
            }
        }

        public MethodCallExpression Distinct(Expression member, ParameterExpression parameter, Expression[] expressions)
        {
            switch (expressions.Length)
            {
                case 0:
                    return Expression.Call(QueryableCachedMethodInfo.Distinct(parameter.Type), member);
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
            return Expression.Call(QueryableCachedMethodInfo.ElementAt(parameter.Type), member, arg1);
        }

        public MethodCallExpression ElementAtOrDefault(Expression member, ParameterExpression parameter, Expression[] expressions)
        {
            if (expressions.Length != 1)
            {
                throw new ArgumentException("Invalid number of parameters");
            }

            var arg1 = expressions[0];
            return Expression.Call(QueryableCachedMethodInfo.ElementAtOrDefault(parameter.Type), member, arg1);
        }

        public MethodCallExpression Except(Expression member, ParameterExpression parameter, Expression[] expressions)
        {
            if (expressions.Length != 1)
            {
                throw new ArgumentException("Invalid number of parameters");
            }

            var arg1 = expressions[0];
            return Expression.Call(QueryableCachedMethodInfo.Except(parameter.Type), member, arg1);
        }

        public MethodCallExpression First(Expression member, ParameterExpression parameter, Expression[] expressions)
        {
            switch (expressions.Length)
            {
                case 0:
                    return Expression.Call(QueryableCachedMethodInfo.First(parameter.Type), member);
                case 1:
                    var type = typeof(Func<,>).MakeGenericType(parameter.Type, typeof(bool));
                    var arg1 = Expression.Lambda(type, expressions[0], parameter);
                    return Expression.Call(QueryableCachedMethodInfo.First_Predicate(parameter.Type), member, arg1);
                default:
                    throw new ArgumentException("Invalid number of parameters");
            }
        }

        public MethodCallExpression FirstOrDefault(Expression member, ParameterExpression parameter, Expression[] expressions)
        {
            switch (expressions.Length)
            {
                case 0:
                    return Expression.Call(QueryableCachedMethodInfo.FirstOrDefault(parameter.Type), member);
                case 1:
                    var type = typeof(Func<,>).MakeGenericType(parameter.Type, typeof(bool));
                    var arg1 = Expression.Lambda(type, expressions[0], parameter);
                    return Expression.Call(QueryableCachedMethodInfo.FirstOrDefault_Predicate(parameter.Type), member, arg1);
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
                    return Expression.Call(QueryableCachedMethodInfo.GroupBy_KeySelector(parameter.Type, expressions[0].Type), member, arg1);
                default:
                    var type2 = typeof(Func<,>).MakeGenericType(parameter.Type, typeof(bool));
                    var arg2 = Expression.Lambda(type2, expressions[0], parameter);
                    return Expression.Call(QueryableCachedMethodInfo.GroupBy_ValueSelector(parameter.Type, expressions[0].Type, expressions[1].Type), member, arg1, arg2);
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
            return Expression.Call(QueryableCachedMethodInfo.Intersect(parameter.Type), member, arg1);
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
                    return Expression.Call(QueryableCachedMethodInfo.Last(parameter.Type), member);
                case 1:
                    var type = typeof(Func<,>).MakeGenericType(parameter.Type, typeof(bool));
                    var arg1 = Expression.Lambda(type, expressions[0], parameter);
                    return Expression.Call(QueryableCachedMethodInfo.Last_Predicate(parameter.Type), member, arg1);
                default:
                    throw new ArgumentException("Invalid number of parameters");
            }
        }

        public MethodCallExpression LastOrDefault(Expression member, ParameterExpression parameter, Expression[] expressions)
        {
            switch (expressions.Length)
            {
                case 0:
                    return Expression.Call(QueryableCachedMethodInfo.LastOrDefault(parameter.Type), member);
                case 1:
                    var type = typeof(Func<,>).MakeGenericType(parameter.Type, typeof(bool));
                    var arg1 = Expression.Lambda(type, expressions[0], parameter);
                    return Expression.Call(QueryableCachedMethodInfo.LastOrDefault_Predicate(parameter.Type), member, arg1);
                default:
                    throw new ArgumentException("Invalid number of parameters");
            }
        }

        public MethodCallExpression LongCount(Expression member, ParameterExpression parameter, Expression[] expressions)
        {
            switch (expressions.Length)
            {
                case 0:
                    return Expression.Call(QueryableCachedMethodInfo.LongCount(parameter.Type), member);
                case 1:
                    var type = typeof(Func<,>).MakeGenericType(parameter.Type, typeof(bool));
                    var arg1 = Expression.Lambda(type, expressions[0], parameter);
                    return Expression.Call(QueryableCachedMethodInfo.LongCount_Predicate(parameter.Type), member, arg1);
                default:
                    throw new ArgumentException("Invalid number of parameters");
            }
        }

        public MethodCallExpression Max(Expression member, ParameterExpression parameter, Expression[] expressions)
        {
            switch (expressions.Length)
            {
                case 0:
                    return Expression.Call(QueryableCachedMethodInfo.Max(parameter.Type), member);
                case 1:
                    var type = typeof(Func<,>).MakeGenericType(parameter.Type, expressions[0].Type);
                    var arg1 = Expression.Lambda(type, expressions[0], parameter);
                    return Expression.Call(QueryableCachedMethodInfo.Max_Selector(parameter.Type, expressions[0].Type), member, arg1);
                default:
                    throw new ArgumentException("Invalid number of parameters");
            }
        }

        public MethodCallExpression Min(Expression member, ParameterExpression parameter, Expression[] expressions)
        {
            switch (expressions.Length)
            {
                case 0:
                    return Expression.Call(QueryableCachedMethodInfo.Min(parameter.Type), member);
                case 1:
                    var type = typeof(Func<,>).MakeGenericType(parameter.Type, expressions[0].Type);
                    var arg1 = Expression.Lambda(type, expressions[0], parameter);
                    return Expression.Call(QueryableCachedMethodInfo.Min_Selector(parameter.Type, expressions[0].Type), member, arg1);
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
            return Expression.Call(QueryableCachedMethodInfo.OrderBy_KeySelector(parameter.Type, expressions[0].Type), member, arg1);

        }

        public MethodCallExpression OrderByDescending(Expression member, ParameterExpression parameter, Expression[] expressions)
        {
            if (expressions.Length != 1)
            {
                throw new ArgumentException("Invalid number of parameters");
            }

            var type = typeof(Func<,>).MakeGenericType(parameter.Type, expressions[0].Type);
            var arg1 = Expression.Lambda(type, expressions[0], parameter);
            return Expression.Call(QueryableCachedMethodInfo.OrderByDescending_KeySelector(parameter.Type, expressions[0].Type), member, arg1);
        }

        public MethodCallExpression Reverse(Expression member, ParameterExpression parameter, Expression[] expressions)
        {
            if (expressions.Length != 0)
            {
                throw new ArgumentException("Invalid number of parameters");
            }

            return Expression.Call(QueryableCachedMethodInfo.Reverse(parameter.Type), member);
        }

        public MethodCallExpression Select(Expression member, ParameterExpression parameter, Expression[] expressions)
        {
            switch (expressions.Length)
            {
                case 1:
                    var type = typeof(Func<,>).MakeGenericType(parameter.Type, expressions[0].Type);
                    var arg1 = Expression.Lambda(type, expressions[0], parameter);
                    return Expression.Call(QueryableCachedMethodInfo.Select(parameter.Type, expressions[0].Type), member, arg1);
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
                    return Expression.Call(QueryableCachedMethodInfo.SequenceEqual(parameter.Type), member, arg1);
                default:
                    throw new ArgumentException("Invalid number of parameters");
            }
        }

        public MethodCallExpression Single(Expression member, ParameterExpression parameter, Expression[] expressions)
        {
            switch (expressions.Length)
            {
                case 0:
                    return Expression.Call(QueryableCachedMethodInfo.Single(parameter.Type), member);
                case 1:
                    var type = typeof(Func<,>).MakeGenericType(parameter.Type, typeof(bool));
                    var arg1 = Expression.Lambda(type, expressions[0], parameter);
                    return Expression.Call(QueryableCachedMethodInfo.Single_Predicate(parameter.Type), member, arg1);
                default:
                    throw new ArgumentException("Invalid number of parameters");
            }
        }

        public MethodCallExpression SingleOrDefault(Expression member, ParameterExpression parameter, Expression[] expressions)
        {
            switch (expressions.Length)
            {
                case 0:
                    return Expression.Call(QueryableCachedMethodInfo.SingleOrDefault(parameter.Type), member);
                case 1:
                    var type = typeof(Func<,>).MakeGenericType(parameter.Type, typeof(bool));
                    var arg1 = Expression.Lambda(type, expressions[0], parameter);
                    return Expression.Call(QueryableCachedMethodInfo.SingleOrDefault_Predicate(parameter.Type), member, arg1);
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
                    return Expression.Call(QueryableCachedMethodInfo.Skip(parameter.Type), member, arg1);
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
                    return Expression.Call(QueryableCachedMethodInfo.SkipWhile(parameter.Type), member, arg1);
                default:
                    throw new ArgumentException("Invalid number of parameters");
            }
        }

        public MethodCallExpression Sum(Expression member, ParameterExpression parameter, Expression[] expressions)
        {
            if (expressions.Length == 0)
            {
                return Expression.Call(QueryableCachedMethodInfo.Sum(parameter.Type), member);
            }

            if (expressions.Length != 1)
            {
                throw new ArgumentException("Invalid number of parameters");
            }

            var type = typeof(Func<,>).MakeGenericType(parameter.Type, expressions[0].Type);
            var arg1 = Expression.Lambda(type, expressions[0], parameter);

            if (expressions[0].Type == typeof(int))
            {
                return Expression.Call(QueryableCachedMethodInfo.Sum(parameter.Type), member, arg1);
            }
            else if (expressions[0].Type == typeof(long))
            {
                return Expression.Call(QueryableCachedMethodInfo.Sum(parameter.Type), member, arg1);
            }
            else if (expressions[0].Type == typeof(float))
            {
                return Expression.Call(QueryableCachedMethodInfo.Sum(parameter.Type), member, arg1);
            }
            else if (expressions[0].Type == typeof(double))
            {
                return Expression.Call(QueryableCachedMethodInfo.Sum(parameter.Type), member, arg1);
            }
            else if (expressions[0].Type == typeof(decimal))
            {
                return Expression.Call(QueryableCachedMethodInfo.Sum(parameter.Type), member, arg1);
            }
            else if (expressions[0].Type == typeof(int?))
            {
                return Expression.Call(QueryableCachedMethodInfo.Sum(parameter.Type), member, arg1);
            }
            else if (expressions[0].Type == typeof(long?))
            {
                return Expression.Call(QueryableCachedMethodInfo.Sum(parameter.Type), member, arg1);
            }
            else if (expressions[0].Type == typeof(float?))
            {
                return Expression.Call(QueryableCachedMethodInfo.Sum(parameter.Type), member, arg1);
            }
            else if (expressions[0].Type == typeof(double?))
            {
                return Expression.Call(QueryableCachedMethodInfo.Sum(parameter.Type), member, arg1);
            }
            else if (expressions[0].Type == typeof(decimal?))
            {
                return Expression.Call(QueryableCachedMethodInfo.Sum(parameter.Type), member, arg1);
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
                    return Expression.Call(QueryableCachedMethodInfo.Take(parameter.Type), member, arg1);
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
                    return Expression.Call(QueryableCachedMethodInfo.TakeWhile(parameter.Type), member, arg1);
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
            return Expression.Call(QueryableCachedMethodInfo.ThenBy_Selector(parameter.Type, expressions[0].Type), member, arg1);
        }

        public MethodCallExpression ThenByDescending(Expression member, ParameterExpression parameter, Expression[] expressions)
        {
            if (expressions.Length != 1)
            {
                throw new ArgumentException("Invalid number of parameters");
            }

            var type = typeof(Func<,>).MakeGenericType(parameter.Type, expressions[0].Type);
            var arg1 = Expression.Lambda(type, expressions[0], parameter);
            return Expression.Call(QueryableCachedMethodInfo.ThenByDescending_Selector(parameter.Type, expressions[0].Type), member, arg1);
        }

        public MethodCallExpression ToArray(Expression member, ParameterExpression parameter, Expression[] expressions)
        {
            if (expressions.Length != 0)
            {
                throw new ArgumentException("Invalid number of parameters");
            }

            return Expression.Call(QueryableCachedMethodInfo.ToArray(parameter.Type), member);
        }

        public MethodCallExpression ToList(Expression member, ParameterExpression parameter, Expression[] expressions)
        {
            if (expressions.Length != 0)
            {
                throw new ArgumentException("Invalid number of parameters");
            }

            return Expression.Call(QueryableCachedMethodInfo.ToList(parameter.Type), member);
        }

        public MethodCallExpression Union(Expression member, ParameterExpression parameter, Expression[] expressions)
        {
            switch (expressions.Length)
            {
                case 1:
                    var arg1 = expressions[0];
                    return Expression.Call(QueryableCachedMethodInfo.SequenceEqual(parameter.Type), member, arg1);
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
                    return Expression.Call(QueryableCachedMethodInfo.Where(parameter.Type), member, arg1);
                default:
                    throw new ArgumentException("Invalid number of parameters");
            }
        }
    }
}
