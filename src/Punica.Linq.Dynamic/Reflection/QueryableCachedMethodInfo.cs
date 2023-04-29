using System.Linq.Expressions;
using System.Reflection;

namespace Punica.Linq.Dynamic.Reflection
{
    public static class QueryableCachedMethodInfo
    {
        private static MethodInfo? _all;

        public static MethodInfo All(Type type) => (_all ??= typeof(Queryable).GetMethod(nameof(Queryable.All))!.GetGenericMethodDefinition())
            .MakeGenericMethod(type);

        private static MethodInfo? _any;

        public static MethodInfo Any(Type type) => (_any ??= new Func<IQueryable<object>, bool>(Queryable.Any).GetMethodInfo().GetGenericMethodDefinition())
            .MakeGenericMethod(type);

        private static MethodInfo? _any_predicate;

        public static MethodInfo Any_Predicate(Type type) => (_any_predicate ??= new Func<IQueryable<object>, Expression<Func<object, bool>>, bool>(Queryable.Any).GetMethodInfo().GetGenericMethodDefinition())
            .MakeGenericMethod(type);

        private static MethodInfo? _average_int_source;

        public static MethodInfo Average_Int_TSource(Type type) => (_average_int_source ??= new Func<IQueryable<object>, Expression<Func<object, int>>, double>(Queryable.Average).GetMethodInfo().GetGenericMethodDefinition())
            .MakeGenericMethod(type);

        private static MethodInfo? _average_long_source;

        public static MethodInfo Average_Long_TSource(Type type) => (_average_long_source ??= new Func<IQueryable<object>, Expression<Func<object, long>>, double>(Queryable.Average).GetMethodInfo().GetGenericMethodDefinition())
            .MakeGenericMethod(type);

        private static MethodInfo? _average_float_source;

        public static MethodInfo Average_Float_TSource(Type type) => (_average_float_source ??= new Func<IQueryable<object>, Expression<Func<object, float>>, float>(Queryable.Average).GetMethodInfo().GetGenericMethodDefinition())
            .MakeGenericMethod(type);

        private static MethodInfo? _average_double_source;

        public static MethodInfo Average_Double_TSource(Type type) => (_average_double_source ??= new Func<IQueryable<object>, Expression<Func<object, double>>, double>(Queryable.Average).GetMethodInfo().GetGenericMethodDefinition())
            .MakeGenericMethod(type);

        private static MethodInfo? _average_decimal_source;

        public static MethodInfo Average_Decimal_TSource(Type type) => (_average_decimal_source ??= new Func<IQueryable<object>, Expression<Func<object, decimal>>, decimal>(Queryable.Average).GetMethodInfo().GetGenericMethodDefinition())
            .MakeGenericMethod(type);

        private static MethodInfo? _average_nullableInt_source;

        public static MethodInfo Average_NullableInt_TSource(Type type) => (_average_nullableInt_source ??= new Func<IQueryable<object>, Expression<Func<object, int?>>, double?>(Queryable.Average).GetMethodInfo().GetGenericMethodDefinition())
            .MakeGenericMethod(type);

        private static MethodInfo? _average_nullableLong_source;

        public static MethodInfo Average_NullableLong_TSource(Type type) => (_average_nullableInt_source ??= new Func<IQueryable<object>, Expression<Func<object, long?>>, double?>(Queryable.Average).GetMethodInfo().GetGenericMethodDefinition())
            .MakeGenericMethod(type);

        private static MethodInfo? _average_nullableFloat_source;

        public static MethodInfo Average_NullableFloat_TSource(Type type) => (_average_nullableFloat_source ??= new Func<IQueryable<object>, Expression<Func<object, float?>>, float?>(Queryable.Average).GetMethodInfo().GetGenericMethodDefinition())
            .MakeGenericMethod(type);

        private static MethodInfo? _average_nullableDouble_source;

        public static MethodInfo Average_NullableDouble_TSource(Type type) => (_average_nullableDouble_source ??= new Func<IQueryable<object>, Expression<Func<object, double?>>, double?>(Queryable.Average).GetMethodInfo().GetGenericMethodDefinition())
            .MakeGenericMethod(type);

        private static MethodInfo? _average_nullableDecimal_source;

        public static MethodInfo Average_NullableDecimal_TSource(Type type) => (_average_nullableDecimal_source ??= new Func<IQueryable<object>, Expression<Func<object, decimal?>>, decimal?>(Queryable.Average).GetMethodInfo().GetGenericMethodDefinition())
            .MakeGenericMethod(type);

        private static MethodInfo? _cast;

        public static MethodInfo Cast(Type type) => (_cast ??= new Func<IQueryable, IQueryable<object>>(Queryable.Cast<object>).GetMethodInfo().GetGenericMethodDefinition())
            .MakeGenericMethod(type);

        private static MethodInfo? _concat;

        public static MethodInfo Concat(Type type) => (_concat ??= new Func<IQueryable<object>, IQueryable<object>, IEnumerable<object>>(Queryable.Concat).GetMethodInfo().GetGenericMethodDefinition())
            .MakeGenericMethod(type);

        private static MethodInfo? _contains;

        public static MethodInfo Contains(Type type) => (_contains ??= new Func<IQueryable<object>, object, bool>(Queryable.Contains).GetMethodInfo().GetGenericMethodDefinition())
            .MakeGenericMethod(type);
        
        private static MethodInfo? _count;

        public static MethodInfo Count(Type type) => (_count ??= new Func<IQueryable<object>, int>(Queryable.Count).GetMethodInfo().GetGenericMethodDefinition())
            .MakeGenericMethod(type);

        private static MethodInfo? _count_predicate;

        public static MethodInfo Count_Predicate(Type type) => (_count_predicate ??= new Func<IQueryable<object>, Expression<Func<object, bool>>, int>(Queryable.Count).GetMethodInfo().GetGenericMethodDefinition())
            .MakeGenericMethod(type);

        private static MethodInfo? _defaultIfEmpty;

        public static MethodInfo DefaultIfEmpty(Type type) => (_defaultIfEmpty ??= new Func<IQueryable<object>, IQueryable<object>>(Queryable.DefaultIfEmpty).GetMethodInfo().GetGenericMethodDefinition())
            .MakeGenericMethod(type);

        private static MethodInfo? _defaultIfEmpty_defaultValue;

        public static MethodInfo DefaultIfEmpty_DefaultValue(Type type) => (_defaultIfEmpty_defaultValue ??= new Func<IQueryable<object>, object, IQueryable<object>>(Queryable.DefaultIfEmpty).GetMethodInfo().GetGenericMethodDefinition())
            .MakeGenericMethod(type);

        private static MethodInfo? _distinct;

        public static MethodInfo Distinct(Type type) => (_distinct ??= new Func<IQueryable<object>, IQueryable<object>>(Queryable.Distinct).GetMethodInfo().GetGenericMethodDefinition())
            .MakeGenericMethod(type);

        private static MethodInfo? _distinct_comparer;

        public static MethodInfo Distinct_Comparer(Type type) => (_distinct_comparer ??= new Func<IQueryable<object>, IEqualityComparer<object>, IQueryable<object>>(Queryable.Distinct).GetMethodInfo().GetGenericMethodDefinition())
            .MakeGenericMethod(type);

        private static MethodInfo? _elementAt;

        public static MethodInfo ElementAt(Type type) => (_elementAt ??= new Func<IQueryable<object>, int, object>(Queryable.ElementAt).GetMethodInfo().GetGenericMethodDefinition())
            .MakeGenericMethod(type);

        private static MethodInfo? _elementAtOrDefault;

        public static MethodInfo ElementAtOrDefault(Type type) => (_elementAtOrDefault ??= new Func<IQueryable<object>, int, object>(Queryable.ElementAtOrDefault).GetMethodInfo().GetGenericMethodDefinition())
            .MakeGenericMethod(type);

        private static MethodInfo? _except;

        public static MethodInfo Except(Type type) => (_except ??= new Func<IQueryable<object>, IQueryable<object>, IQueryable<object>>(Queryable.Except).GetMethodInfo().GetGenericMethodDefinition())
            .MakeGenericMethod(type);

        private static MethodInfo? _except_comparer;

        public static MethodInfo Except_Comparer(Type type) => (_except_comparer ??= new Func<IQueryable<object>, IQueryable<object>, IEqualityComparer<object>, IQueryable<object>>(Queryable.Except).GetMethodInfo().GetGenericMethodDefinition())
            .MakeGenericMethod(type);

        private static MethodInfo? _first;

        public static MethodInfo First(Type type) => (_first ??= new Func<IQueryable<object>, object>(Queryable.First).GetMethodInfo().GetGenericMethodDefinition())
            .MakeGenericMethod(type);

        private static MethodInfo? _first_predicate;

        public static MethodInfo First_Predicate(Type type) => (_first_predicate ??= new Func<IQueryable<object>, Expression<Func<object, bool>>, object>(Queryable.First).GetMethodInfo().GetGenericMethodDefinition())
            .MakeGenericMethod(type);

        private static MethodInfo? _firstOrDefault;

        public static MethodInfo FirstOrDefault(Type type) => (_firstOrDefault ??= new Func<IQueryable<object>, object>(Queryable.FirstOrDefault).GetMethodInfo().GetGenericMethodDefinition())
            .MakeGenericMethod(type);

        private static MethodInfo? _firstOrDefault_predicate;

        public static MethodInfo FirstOrDefault_Predicate(Type type) => (_firstOrDefault_predicate ??= new Func<IQueryable<object>, Func<object, bool>, object>(Queryable.FirstOrDefault).GetMethodInfo().GetGenericMethodDefinition())
            .MakeGenericMethod(type);

        private static MethodInfo? _groupBy_keySelector;

        public static MethodInfo GroupBy_KeySelector(Type source, Type key) => (_groupBy_keySelector ??= new Func<IQueryable<object>, Expression<Func<object, object>>, IQueryable<IGrouping<object, object>>>(Queryable.GroupBy).GetMethodInfo().GetGenericMethodDefinition())
            .MakeGenericMethod(source, key);

        private static MethodInfo? _groupBy_valueSelector;

        public static MethodInfo GroupBy_ValueSelector(Type source, Type key, Type element) => (_groupBy_valueSelector ??= new Func<IQueryable<object>, Expression<Func<object, object>>, Expression<Func<object, object>>, IQueryable<IGrouping<object, object>>>(Queryable.GroupBy).GetMethodInfo().GetGenericMethodDefinition())
            .MakeGenericMethod(source, key, element);

        private static MethodInfo? _groupJoin;

        public static MethodInfo GroupJoin(Type outer, Type inner, Type key, Type result) => (_groupJoin ??= new Func<IQueryable<object>, IEnumerable<object>, Expression<Func<object, object>>, Expression<Func<object, object>>, Expression<Func<object, IEnumerable<object>, object>>, IQueryable<object>>(Queryable.GroupJoin).GetMethodInfo().GetGenericMethodDefinition())
            .MakeGenericMethod(outer, inner, key, result);

        private static MethodInfo? _intersect;

        public static MethodInfo Intersect(Type type) => (_intersect ??= new Func<IQueryable<object>, IQueryable<object>, IQueryable<object>>(Queryable.Intersect).GetMethodInfo().GetGenericMethodDefinition())
            .MakeGenericMethod(type);


        private static MethodInfo? _intersect_comparer;

        public static MethodInfo Intersect_Comparer(Type type) => (_intersect_comparer ??= new Func<IQueryable<object>, IQueryable<object>, IEqualityComparer<object>, IQueryable<object>>(Queryable.Intersect).GetMethodInfo().GetGenericMethodDefinition())
            .MakeGenericMethod(type);


        private static MethodInfo? _join;

        public static MethodInfo Join(Type outer, Type inner, Type key, Type result) => (_join ??= new Func<IQueryable<object>, IQueryable<object>, Expression<Func<object, object>>, Expression<Func<object, object>>, Expression<Func<object, object, object>>, IQueryable<object>>(Queryable.Join).GetMethodInfo().GetGenericMethodDefinition())
            .MakeGenericMethod(outer, inner, key, result);

        private static MethodInfo? _last;

        public static MethodInfo Last(Type type) => (_last ??= new Func<IQueryable<object>, object>(Queryable.Last).GetMethodInfo().GetGenericMethodDefinition())
            .MakeGenericMethod(type);

        private static MethodInfo? _last_predicate;

        public static MethodInfo Last_Predicate(Type type) => (_last_predicate ??= new Func<IQueryable<object>, Expression<Func<object, bool>>, object>(Queryable.Last).GetMethodInfo().GetGenericMethodDefinition())
            .MakeGenericMethod(type);

        private static MethodInfo? _lastOrDefault;

        public static MethodInfo LastOrDefault(Type type) => (_lastOrDefault ??= new Func<IQueryable<object>, object>(Queryable.LastOrDefault).GetMethodInfo().GetGenericMethodDefinition())
            .MakeGenericMethod(type);

        private static MethodInfo? _lastOrDefault_predicate;

        public static MethodInfo LastOrDefault_Predicate(Type type) => (_lastOrDefault_predicate ??= new Func<IQueryable<object>, Func<object, bool>, object>(Queryable.LastOrDefault).GetMethodInfo().GetGenericMethodDefinition())
            .MakeGenericMethod(type);

        private static MethodInfo? _longCount;

        public static MethodInfo LongCount(Type type) => (_longCount ??= new Func<IQueryable<object>, long>(Queryable.LongCount).GetMethodInfo().GetGenericMethodDefinition())
            .MakeGenericMethod(type);

        private static MethodInfo? _longCount_predicate;

        public static MethodInfo LongCount_Predicate(Type type) => (_longCount_predicate ??= new Func<IQueryable<object>, Expression<Func<object, bool>>, long>(Queryable.LongCount).GetMethodInfo().GetGenericMethodDefinition())
            .MakeGenericMethod(type);

        private static MethodInfo? _max;

        public static MethodInfo Max(Type type) => (_max ??= new Func<IQueryable<object>, object?>(Queryable.Max).GetMethodInfo().GetGenericMethodDefinition())
            .MakeGenericMethod(type);

        private static MethodInfo? _max_selector;

        public static MethodInfo Max_Selector(Type source, Type result) => (_max_selector ??= new Func<IQueryable<object>, Expression<Func<object, object>>, object>(Queryable.Max).GetMethodInfo().GetGenericMethodDefinition())
            .MakeGenericMethod(source, result);

        private static MethodInfo? _min;

        public static MethodInfo Min(Type type) => (_min ??= new Func<IQueryable<object>, object?>(Queryable.Min).GetMethodInfo().GetGenericMethodDefinition())
            .MakeGenericMethod(type);

        private static MethodInfo? _min_selector;

        public static MethodInfo Min_Selector(Type source, Type result) => (_min_selector ??= new Func<IQueryable<object>, Expression<Func<object, object>>, object>(Queryable.Min).GetMethodInfo().GetGenericMethodDefinition())
            .MakeGenericMethod(source, result);


        private static MethodInfo? _orderBy_keySelector;

        public static MethodInfo OrderBy_KeySelector(Type source, Type key) => (_orderBy_keySelector ??= new Func<IQueryable<object>, Expression<Func<object, object>>, IOrderedQueryable<object>>(Queryable.OrderBy).GetMethodInfo().GetGenericMethodDefinition())
            .MakeGenericMethod(source, key);

        private static MethodInfo? _orderByDescending_keySelector;

        public static MethodInfo OrderByDescending_KeySelector(Type source, Type key) => (_orderByDescending_keySelector ??= new Func<IQueryable<object>, Expression<Func<object, object>>, IOrderedQueryable<object>>(Queryable.OrderByDescending).GetMethodInfo().GetGenericMethodDefinition())
            .MakeGenericMethod(source, key);

        private static MethodInfo? _reverse;

        public static MethodInfo Reverse(Type type) => (_reverse ??= new Func<IQueryable<object>, IQueryable<object>>(Queryable.Reverse).GetMethodInfo().GetGenericMethodDefinition())
            .MakeGenericMethod(type);

        private static MethodInfo? _select;

        public static MethodInfo Select(Type source, Type result) => (_select ??= new Func<IQueryable<object>, Expression<Func<object, object>>, IQueryable<object>>(Queryable.Select).GetMethodInfo().GetGenericMethodDefinition())
            .MakeGenericMethod(source, result);


        private static MethodInfo? _selectMany;

        public static MethodInfo SelectMany(Type source, Type result) => (_selectMany ??= new Func<IQueryable<object>, Expression<Func<object, IEnumerable<object>>>, IQueryable<object>>(Queryable.SelectMany).GetMethodInfo().GetGenericMethodDefinition())
            .MakeGenericMethod(source, result);

        private static MethodInfo? _selectMany_resultSelector;

        public static MethodInfo SelectMany_ResultSelector(Type source, Type collection, Type result) => (_selectMany_resultSelector ??= new Func<IQueryable<object>, Expression<Func<object, IEnumerable<object>>>, Expression<Func<object, object, object>>, IQueryable<object>>(Queryable.SelectMany).GetMethodInfo().GetGenericMethodDefinition())
            .MakeGenericMethod(source, collection, result);

        private static MethodInfo? _sequenceEqual;

        public static MethodInfo SequenceEqual(Type type) => (_sequenceEqual ??= new Func<IQueryable<object>, IQueryable<object>, bool>(Queryable.SequenceEqual).GetMethodInfo().GetGenericMethodDefinition())
            .MakeGenericMethod(type);

        private static MethodInfo? _single;

        public static MethodInfo Single(Type type) => (_single ??= new Func<IQueryable<object>, object>(Queryable.Single).GetMethodInfo().GetGenericMethodDefinition())
            .MakeGenericMethod(type);

        private static MethodInfo? _single_predicate;

        public static MethodInfo Single_Predicate(Type type) => (_single_predicate ??= new Func<IQueryable<object>, Expression<Func<object, bool>>, object>(Queryable.Single).GetMethodInfo().GetGenericMethodDefinition())
            .MakeGenericMethod(type);

        private static MethodInfo? _singleOrDefault;

        public static MethodInfo SingleOrDefault(Type type) => (_singleOrDefault ??= new Func<IQueryable<object>, object?>(Queryable.SingleOrDefault).GetMethodInfo().GetGenericMethodDefinition())
            .MakeGenericMethod(type);

        private static MethodInfo? _singleOrDefault_predicate;

        public static MethodInfo SingleOrDefault_Predicate(Type type) => (_singleOrDefault_predicate ??= new Func<IQueryable<object>, Expression<Func<object, bool>>, object?>(Queryable.SingleOrDefault).GetMethodInfo().GetGenericMethodDefinition())
            .MakeGenericMethod(type);


        private static MethodInfo? _skip;

        public static MethodInfo Skip(Type type) => (_skip ??= new Func<IQueryable<object>, int, IQueryable<object>>(Queryable.Skip).GetMethodInfo().GetGenericMethodDefinition())
            .MakeGenericMethod(type);

        private static MethodInfo? _skipWhile;

        public static MethodInfo SkipWhile(Type type) => (_skipWhile ??= new Func<IQueryable<object>, Expression<Func<object, bool>>, IQueryable<object>>(Queryable.SkipWhile).GetMethodInfo().GetGenericMethodDefinition())
            .MakeGenericMethod(type);

        private static MethodInfo? _skipWhile_index;

        public static MethodInfo SkipWhile_Index(Type type) => (_skipWhile_index ??= new Func<IQueryable<object>, Expression<Func<object, int, bool>>, IQueryable<object>>(Queryable.SkipWhile).GetMethodInfo().GetGenericMethodDefinition())
            .MakeGenericMethod(type);

        public static MethodInfo Sum(Type type) => typeof(Queryable).GetMethod(nameof(Queryable.Sum), new[] { type })!;

        private static MethodInfo? _sum_selector_int;

        public static MethodInfo Sum_Selector_Int(Type source, Type result) => (_sum_selector_int ??= new Func<IQueryable<object>, Expression<Func<object, int>>, int>(Queryable.Sum).GetMethodInfo().GetGenericMethodDefinition())
            .MakeGenericMethod(source, result);

        private static MethodInfo? _sum_selector_long;

        public static MethodInfo Sum_Selector_Long(Type source, Type result) => (_sum_selector_long ??= new Func<IQueryable<object>, Expression<Func<object, long>>, long>(Queryable.Sum).GetMethodInfo().GetGenericMethodDefinition())
            .MakeGenericMethod(source, result);

        private static MethodInfo? _sum_selector_float;

        public static MethodInfo Sum_Selector_Float(Type source, Type result) => (_sum_selector_float ??= new Func<IQueryable<object>, Expression<Func<object, float>>, float>(Queryable.Sum).GetMethodInfo().GetGenericMethodDefinition())
            .MakeGenericMethod(source, result);

        private static MethodInfo? _sum_selector_double;

        public static MethodInfo Sum_Selector_Double(Type source, Type result) => (_sum_selector_double ??= new Func<IQueryable<object>, Expression<Func<object, double>>, double>(Queryable.Sum).GetMethodInfo().GetGenericMethodDefinition())
            .MakeGenericMethod(source, result);

        private static MethodInfo? _sum_selector_decimal;

        public static MethodInfo Sum_Selector_Decimal(Type source, Type result) => (_sum_selector_decimal ??= new Func<IQueryable<object>, Expression<Func<object, decimal>>, decimal>(Queryable.Sum).GetMethodInfo().GetGenericMethodDefinition())
            .MakeGenericMethod(source, result);

        private static MethodInfo? _take;

        public static MethodInfo Take(Type type) => (_take ??= new Func<IQueryable<object>, int, IQueryable<object>>(Queryable.Take).GetMethodInfo().GetGenericMethodDefinition())
            .MakeGenericMethod(type);

        private static MethodInfo? _takeWhile;

        public static MethodInfo TakeWhile(Type type) => (_takeWhile ??= new Func<IQueryable<object>, Expression<Func<object, bool>>, IQueryable<object>>(Queryable.TakeWhile).GetMethodInfo().GetGenericMethodDefinition())
            .MakeGenericMethod(type);

        private static MethodInfo? _thenBy_selector;

        public static MethodInfo ThenBy_Selector(Type source, Type key) => (_thenBy_selector ??= new Func<IOrderedQueryable<object>, Expression<Func<object, object>>, IOrderedQueryable<object>>(Queryable.ThenBy).GetMethodInfo().GetGenericMethodDefinition())
            .MakeGenericMethod(source, key);

        private static MethodInfo? _thenByDescending_selector;

        public static MethodInfo ThenByDescending_Selector(Type source, Type key) => (_thenByDescending_selector ??= new Func<IOrderedQueryable<object>, Expression<Func<object, object>>, IOrderedQueryable<object>>(Queryable.ThenByDescending).GetMethodInfo().GetGenericMethodDefinition())
            .MakeGenericMethod(source, key);

        private static MethodInfo? _toArray;

        public static MethodInfo ToArray(Type type) => (_toArray ??= new Func<IEnumerable<object>, object[]>(Enumerable.ToArray).GetMethodInfo().GetGenericMethodDefinition())
            .MakeGenericMethod(type);

        private static MethodInfo? _toList;

        public static MethodInfo ToList(Type type) => (_toList ??= new Func<IEnumerable<object>, List<object>>(Enumerable.ToList).GetMethodInfo().GetGenericMethodDefinition())
            .MakeGenericMethod(type);

        private static MethodInfo? _union;

        public static MethodInfo Union(Type type) => (_union ??= new Func<IQueryable<object>, IEnumerable<object>, IQueryable<object>>(Queryable.Union).GetMethodInfo().GetGenericMethodDefinition())
            .MakeGenericMethod(type);

        private static MethodInfo? _where;

        public static MethodInfo Where(Type type) => (_where ??= new Func<IQueryable<object>, Expression<Func<object, bool>>, IQueryable<object>>(Queryable.Where).GetMethodInfo().GetGenericMethodDefinition())
            .MakeGenericMethod(type);

    }
}
