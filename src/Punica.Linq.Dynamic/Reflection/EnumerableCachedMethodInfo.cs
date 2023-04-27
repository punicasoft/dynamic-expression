using System.Collections;
using System.Reflection;

namespace Punica.Linq.Dynamic.Reflection
{
    public static class EnumerableCachedMethodInfo
    {
        private static MethodInfo? _all;

        public static MethodInfo All(Type type) => (_all ??= typeof(Enumerable).GetMethod(nameof(Enumerable.All))!.GetGenericMethodDefinition())
            .MakeGenericMethod(type);

        private static MethodInfo? _any;

        public static MethodInfo Any(Type type) => (_any ??= new Func<IEnumerable<object>, bool>(Enumerable.Any).GetMethodInfo().GetGenericMethodDefinition())
            .MakeGenericMethod(type);

        private static MethodInfo? _any_predicate;

        public static MethodInfo Any_Predicate(Type type) => (_any_predicate ??= new Func<IEnumerable<object>, Func<object, bool>, bool>(Enumerable.Any).GetMethodInfo().GetGenericMethodDefinition())
            .MakeGenericMethod(type);

        private static MethodInfo? _average_int_source;

        public static MethodInfo Average_Int_TSource(Type type) => (_average_int_source ??= new Func<IEnumerable<object>, Func<object, int>, double>(Enumerable.Average).GetMethodInfo().GetGenericMethodDefinition())
            .MakeGenericMethod(type);

        private static MethodInfo? _average_long_source;

        public static MethodInfo Average_Long_TSource(Type type) => (_average_long_source ??= new Func<IEnumerable<object>, Func<object, long>, double>(Enumerable.Average).GetMethodInfo().GetGenericMethodDefinition())
            .MakeGenericMethod(type);

        private static MethodInfo? _average_float_source;

        public static MethodInfo Average_Float_TSource(Type type) => (_average_float_source ??= new Func<IEnumerable<object>, Func<object, float>, float>(Enumerable.Average).GetMethodInfo().GetGenericMethodDefinition())
            .MakeGenericMethod(type);

        private static MethodInfo? _average_double_source;

        public static MethodInfo Average_Double_TSource(Type type) => (_average_double_source ??= new Func<IEnumerable<object>, Func<object, double>, double>(Enumerable.Average).GetMethodInfo().GetGenericMethodDefinition())
            .MakeGenericMethod(type);

        private static MethodInfo? _average_decimal_source;

        public static MethodInfo Average_Decimal_TSource(Type type) => (_average_decimal_source ??= new Func<IEnumerable<object>, Func<object, decimal>, decimal>(Enumerable.Average).GetMethodInfo().GetGenericMethodDefinition())
            .MakeGenericMethod(type);

        private static MethodInfo? _average_nullableInt_source;

        public static MethodInfo Average_NullableInt_TSource(Type type) => (_average_nullableInt_source ??= new Func<IEnumerable<object>, Func<object, int?>, double?>(Enumerable.Average).GetMethodInfo().GetGenericMethodDefinition())
            .MakeGenericMethod(type);

        private static MethodInfo? _average_nullableLong_source;

        public static MethodInfo Average_NullableLong_TSource(Type type) => (_average_nullableInt_source ??= new Func<IEnumerable<object>, Func<object, long?>, double?>(Enumerable.Average).GetMethodInfo().GetGenericMethodDefinition())
            .MakeGenericMethod(type);

        private static MethodInfo? _average_nullableFloat_source;

        public static MethodInfo Average_NullableFloat_TSource(Type type) => (_average_nullableFloat_source ??= new Func<IEnumerable<object>, Func<object, float?>, float?>(Enumerable.Average).GetMethodInfo().GetGenericMethodDefinition())
            .MakeGenericMethod(type);

        private static MethodInfo? _average_nullableDouble_source;

        public static MethodInfo Average_NullableDouble_TSource(Type type) => (_average_nullableDouble_source ??= new Func<IEnumerable<object>, Func<object, double?>, double?>(Enumerable.Average).GetMethodInfo().GetGenericMethodDefinition())
            .MakeGenericMethod(type);

        private static MethodInfo? _average_nullableDecimal_source;

        public static MethodInfo Average_NullableDecimal_TSource(Type type) => (_average_nullableDecimal_source ??= new Func<IEnumerable<object>, Func<object, decimal?>, decimal?>(Enumerable.Average).GetMethodInfo().GetGenericMethodDefinition())
            .MakeGenericMethod(type);

        private static MethodInfo? _cast;

        public static MethodInfo Cast(Type type) => (_cast ??= new Func<IEnumerable, IEnumerable<object>>(Enumerable.Cast<object>).GetMethodInfo().GetGenericMethodDefinition())
            .MakeGenericMethod(type);

        private static MethodInfo? _concat;

        public static MethodInfo Concat(Type type) => (_concat ??= new Func<IEnumerable<object>, IEnumerable<object>, IEnumerable<object>>(Enumerable.Concat).GetMethodInfo().GetGenericMethodDefinition())
            .MakeGenericMethod(type);

        private static MethodInfo? _contains;

        public static MethodInfo Contains(Type type) => (_contains ??= new Func<IEnumerable<object>, object, bool>(Enumerable.Contains).GetMethodInfo().GetGenericMethodDefinition())
            .MakeGenericMethod(type);

        private static MethodInfo? _contains_predicate;

        public static MethodInfo Contains_Predicate(Type type) => (_contains_predicate ??= new Func<IEnumerable<object>, Func<object, bool>, bool>(Enumerable.Contains).GetMethodInfo().GetGenericMethodDefinition())
            .MakeGenericMethod(type);

        private static MethodInfo? _count;

        public static MethodInfo Count(Type type) => (_count ??= new Func<IEnumerable<object>, int>(Enumerable.Count).GetMethodInfo().GetGenericMethodDefinition())
            .MakeGenericMethod(type);

        private static MethodInfo? _count_predicate;

        public static MethodInfo Count_Predicate(Type type) => (_count_predicate ??= new Func<IEnumerable<object>, Func<object, bool>, int>(Enumerable.Count).GetMethodInfo().GetGenericMethodDefinition())
            .MakeGenericMethod(type);

        private static MethodInfo? _defaultIfEmpty;

        public static MethodInfo DefaultIfEmpty(Type type) => (_defaultIfEmpty ??= new Func<IEnumerable<object>, IEnumerable<object>>(Enumerable.DefaultIfEmpty).GetMethodInfo().GetGenericMethodDefinition())
            .MakeGenericMethod(type);

        private static MethodInfo? _defaultIfEmpty_defaultValue;

        public static MethodInfo DefaultIfEmpty_DefaultValue(Type type) => (_defaultIfEmpty_defaultValue ??= new Func<IEnumerable<object>, object, IEnumerable<object>>(Enumerable.DefaultIfEmpty).GetMethodInfo().GetGenericMethodDefinition())
            .MakeGenericMethod(type);

        private static MethodInfo? _distinct;

        public static MethodInfo Distinct(Type type) => (_distinct ??= new Func<IEnumerable<object>, IEnumerable<object>>(Enumerable.Distinct).GetMethodInfo().GetGenericMethodDefinition())
            .MakeGenericMethod(type);

        private static MethodInfo? _distinct_comparer;

        public static MethodInfo Distinct_Comparer(Type type) => (_distinct_comparer ??= new Func<IEnumerable<object>, IEqualityComparer<object>, IEnumerable<object>>(Enumerable.Distinct).GetMethodInfo().GetGenericMethodDefinition())
            .MakeGenericMethod(type);

        private static MethodInfo? _elementAt;

        public static MethodInfo ElementAt(Type type) => (_elementAt ??= new Func<IEnumerable<object>, int, object>(Enumerable.ElementAt).GetMethodInfo().GetGenericMethodDefinition())
            .MakeGenericMethod(type);

        private static MethodInfo? _elementAtOrDefault;

        public static MethodInfo ElementAtOrDefault(Type type) => (_elementAtOrDefault ??= new Func<IEnumerable<object>, int, object>(Enumerable.ElementAtOrDefault).GetMethodInfo().GetGenericMethodDefinition())
            .MakeGenericMethod(type);

        private static MethodInfo? _except;

        public static MethodInfo Except(Type type) => (_except ??= new Func<IEnumerable<object>, IEnumerable<object>, IEnumerable<object>>(Enumerable.Except).GetMethodInfo().GetGenericMethodDefinition())
            .MakeGenericMethod(type);

        private static MethodInfo? _except_comparer;

        public static MethodInfo Except_Comparer(Type type) => (_except_comparer ??= new Func<IEnumerable<object>, IEnumerable<object>, IEqualityComparer<object>, IEnumerable<object>>(Enumerable.Except).GetMethodInfo().GetGenericMethodDefinition())
            .MakeGenericMethod(type);

        private static MethodInfo? _first;

        public static MethodInfo First(Type type) => (_first ??= new Func<IEnumerable<object>, object>(Enumerable.First).GetMethodInfo().GetGenericMethodDefinition())
            .MakeGenericMethod(type);

        private static MethodInfo? _first_predicate;

        public static MethodInfo First_Predicate(Type type) => (_first_predicate ??= new Func<IEnumerable<object>, Func<object, bool>, object>(Enumerable.First).GetMethodInfo().GetGenericMethodDefinition())
            .MakeGenericMethod(type);

        private static MethodInfo? _firstOrDefault;

        public static MethodInfo FirstOrDefault(Type type) => (_firstOrDefault ??= new Func<IEnumerable<object>, object>(Enumerable.FirstOrDefault).GetMethodInfo().GetGenericMethodDefinition())
            .MakeGenericMethod(type);

        private static MethodInfo? _firstOrDefault_predicate;

        public static MethodInfo FirstOrDefault_Predicate(Type type) => (_firstOrDefault_predicate ??= new Func<IEnumerable<object>, Func<object, bool>, object>(Enumerable.FirstOrDefault).GetMethodInfo().GetGenericMethodDefinition())
            .MakeGenericMethod(type);

        private static MethodInfo? _groupBy_keySelector;

        public static MethodInfo GroupBy_KeySelector(Type source, Type key) => (_groupBy_keySelector ??= new Func<IEnumerable<object>, Func<object, object>, IEnumerable<IGrouping<object, object>>>(Enumerable.GroupBy).GetMethodInfo().GetGenericMethodDefinition())
            .MakeGenericMethod(source, key);

        private static MethodInfo? _groupBy_valueSelector;

        public static MethodInfo GroupBy_ValueSelector(Type source, Type key, Type element) => (_groupBy_valueSelector ??= new Func<IEnumerable<object>, Func<object, object>, Func<object, object>, IEnumerable<IGrouping<object, object>>>(Enumerable.GroupBy).GetMethodInfo().GetGenericMethodDefinition())
            .MakeGenericMethod(source, key, element);

        //private static MethodInfo? _groupBy_keySelector_resultSelector;

        //public static MethodInfo GroupBy_KeySelector_ResultSelector(Type type) => (_groupBy_keySelector_resultSelector ??= new Func<IEnumerable<object>, Func<object, object>, Func<object, IEnumerable<object>, object>, IEnumerable<object>>(Enumerable.GroupBy).GetMethodInfo().GetGenericMethodDefinition())
        //    .MakeGenericMethod(type, type, type);

        //private static MethodInfo? _groupBy_keySelector_elementSelector_resultSelector;

        //public static MethodInfo GroupBy_KeySelector_ElementSelector_ResultSelector(Type type) => (_groupBy_keySelector_elementSelector_resultSelector ??= new Func<IEnumerable<object>, Func<object, object>, Func<object, object>, Func<object, IEnumerable<object>, object>, IEnumerable<object>>(Enumerable.GroupBy).GetMethodInfo().GetGenericMethodDefinition())
        //    .MakeGenericMethod(type, type, type, type);

        //private static MethodInfo? _groupBy_keySelector_comparer;

        //public static MethodInfo GroupBy_KeySelector_Comparer(Type type) => (_groupBy_keySelector_comparer ??= new Func<IEnumerable<object>, Func<object, object>, IEqualityComparer<object>, IEnumerable<IGrouping<object, object>>>(Enumerable.GroupBy).GetMethodInfo().GetGenericMethodDefinition())
        //    .MakeGenericMethod(type, type);

        private static MethodInfo? _groupJoin;

        public static MethodInfo GroupJoin(Type outer, Type inner, Type key, Type result) => (_groupJoin ??= new Func<IEnumerable<object>, IEnumerable<object>, Func<object, object>, Func<object, object>, Func<object, IEnumerable<object>, object>, IEnumerable<object>>(Enumerable.GroupJoin).GetMethodInfo().GetGenericMethodDefinition())
            .MakeGenericMethod(outer, inner, key, result);

        private static MethodInfo? _intersect;

        public static MethodInfo Intersect(Type type) => (_intersect ??= new Func<IEnumerable<object>, IEnumerable<object>, IEnumerable<object>>(Enumerable.Intersect).GetMethodInfo().GetGenericMethodDefinition())
            .MakeGenericMethod(type);


        private static MethodInfo? _intersect_comparer;

        public static MethodInfo Intersect_Comparer(Type type) => (_intersect_comparer ??= new Func<IEnumerable<object>, IEnumerable<object>, IEqualityComparer<object>, IEnumerable<object>>(Enumerable.Intersect).GetMethodInfo().GetGenericMethodDefinition())
            .MakeGenericMethod(type);


        private static MethodInfo? _join;

        public static MethodInfo Join(Type outer, Type inner, Type key, Type result) => (_join ??= new Func<IEnumerable<object>, IEnumerable<object>, Func<object, object>, Func<object, object>, Func<object, object, object>, IEnumerable<object>>(Enumerable.Join).GetMethodInfo().GetGenericMethodDefinition())
            .MakeGenericMethod(outer, inner, key, result);

        private static MethodInfo? _last;

        public static MethodInfo Last(Type type) => (_last ??= new Func<IEnumerable<object>, object>(Enumerable.Last).GetMethodInfo().GetGenericMethodDefinition())
            .MakeGenericMethod(type);

        private static MethodInfo? _last_predicate;

        public static MethodInfo Last_Predicate(Type type) => (_last_predicate ??= new Func<IEnumerable<object>, Func<object, bool>, object>(Enumerable.Last).GetMethodInfo().GetGenericMethodDefinition())
            .MakeGenericMethod(type);

        private static MethodInfo? _lastOrDefault;

        public static MethodInfo LastOrDefault(Type type) => (_lastOrDefault ??= new Func<IEnumerable<object>, object>(Enumerable.LastOrDefault).GetMethodInfo().GetGenericMethodDefinition())
            .MakeGenericMethod(type);

        private static MethodInfo? _lastOrDefault_predicate;

        public static MethodInfo LastOrDefault_Predicate(Type type) => (_lastOrDefault_predicate ??= new Func<IEnumerable<object>, Func<object, bool>, object>(Enumerable.LastOrDefault).GetMethodInfo().GetGenericMethodDefinition())
            .MakeGenericMethod(type);

        private static MethodInfo? _longCount;

        public static MethodInfo LongCount(Type type) => (_longCount ??= new Func<IEnumerable<object>, long>(Enumerable.LongCount).GetMethodInfo().GetGenericMethodDefinition())
            .MakeGenericMethod(type);

        private static MethodInfo? _longCount_predicate;

        public static MethodInfo LongCount_Predicate(Type type) => (_longCount_predicate ??= new Func<IEnumerable<object>, Func<object, bool>, long>(Enumerable.LongCount).GetMethodInfo().GetGenericMethodDefinition())
            .MakeGenericMethod(type);

        private static MethodInfo? _max;

        public static MethodInfo Max(Type type) => (_max ??= new Func<IEnumerable<object>, object?>(Enumerable.Max).GetMethodInfo().GetGenericMethodDefinition())
            .MakeGenericMethod(type);

        private static MethodInfo? _max_selector;

        public static MethodInfo Max_Selector(Type source, Type result) => (_max_selector ??= new Func<IEnumerable<object>, Func<object, object>, object>(Enumerable.Max).GetMethodInfo().GetGenericMethodDefinition())
            .MakeGenericMethod(source, result);

        private static MethodInfo? _min;

        public static MethodInfo Min(Type type) => (_min ??= new Func<IEnumerable<object>, object?>(Enumerable.Min).GetMethodInfo().GetGenericMethodDefinition())
            .MakeGenericMethod(type);

        private static MethodInfo? _min_selector;

        public static MethodInfo Min_Selector(Type source, Type result) => (_min_selector ??= new Func<IEnumerable<object>, Func<object, object>, object>(Enumerable.Min).GetMethodInfo().GetGenericMethodDefinition())
            .MakeGenericMethod(source, result);


        private static MethodInfo? _orderBy_keySelector;

        public static MethodInfo OrderBy_KeySelector(Type source, Type key) => (_orderBy_keySelector ??= new Func<IEnumerable<object>, Func<object, object>, IOrderedEnumerable<object>>(Enumerable.OrderBy).GetMethodInfo().GetGenericMethodDefinition())
            .MakeGenericMethod(source, key);

        private static MethodInfo? _orderByDescending_keySelector;

        public static MethodInfo OrderByDescending_KeySelector(Type source, Type key) => (_orderByDescending_keySelector ??= new Func<IEnumerable<object>, Func<object, object>, IOrderedEnumerable<object>>(Enumerable.OrderByDescending).GetMethodInfo().GetGenericMethodDefinition())
            .MakeGenericMethod(source, key);

        private static MethodInfo? _reverse;

        public static MethodInfo Reverse(Type type) => (_reverse ??= new Func<IEnumerable<object>, IEnumerable<object>>(Enumerable.Reverse).GetMethodInfo().GetGenericMethodDefinition())
            .MakeGenericMethod(type);

        private static MethodInfo? _select;

        public static MethodInfo Select(Type source, Type result) => (_select ??= new Func<IEnumerable<object>, Func<object, object>, IEnumerable<object>>(Enumerable.Select).GetMethodInfo().GetGenericMethodDefinition())
            .MakeGenericMethod(source, result);


        private static MethodInfo? _selectMany;

        public static MethodInfo SelectMany(Type source, Type result) => (_selectMany ??= new Func<IEnumerable<object>, Func<object, IEnumerable<object>>, IEnumerable<object>>(Enumerable.SelectMany).GetMethodInfo().GetGenericMethodDefinition())
            .MakeGenericMethod(source, result);

        private static MethodInfo? _selectMany_resultSelector;

        public static MethodInfo SelectMany_ResultSelector(Type source, Type collection, Type result) => (_selectMany_resultSelector ??= new Func<IEnumerable<object>, Func<object, IEnumerable<object>>, Func<object, object, object>, IEnumerable<object>>(Enumerable.SelectMany).GetMethodInfo().GetGenericMethodDefinition())
            .MakeGenericMethod(source, collection, result);

        private static MethodInfo? _sequenceEqual;

        public static MethodInfo SequenceEqual(Type type) => (_sequenceEqual ??= new Func<IEnumerable<object>, IEnumerable<object>, bool>(Enumerable.SequenceEqual).GetMethodInfo().GetGenericMethodDefinition())
            .MakeGenericMethod(type);

        private static MethodInfo? _single;

        public static MethodInfo Single(Type type) => (_single ??= new Func<IEnumerable<object>, object>(Enumerable.Single).GetMethodInfo().GetGenericMethodDefinition())
            .MakeGenericMethod(type);

        private static MethodInfo? _single_predicate;

        public static MethodInfo Single_Predicate(Type type) => (_single_predicate ??= new Func<IEnumerable<object>, Func<object, bool>, object>(Enumerable.Single).GetMethodInfo().GetGenericMethodDefinition())
            .MakeGenericMethod(type);

        private static MethodInfo? _singleOrDefault;

        public static MethodInfo SingleOrDefault(Type type) => (_singleOrDefault ??= new Func<IEnumerable<object>, object?>(Enumerable.SingleOrDefault).GetMethodInfo().GetGenericMethodDefinition())
            .MakeGenericMethod(type);

        private static MethodInfo? _singleOrDefault_predicate;

        public static MethodInfo SingleOrDefault_Predicate(Type type) => (_singleOrDefault_predicate ??= new Func<IEnumerable<object>, Func<object, bool>, object?>(Enumerable.SingleOrDefault).GetMethodInfo().GetGenericMethodDefinition())
            .MakeGenericMethod(type);


        private static MethodInfo? _skip;

        public static MethodInfo Skip(Type type) => (_skip ??= new Func<IEnumerable<object>, int, IEnumerable<object>>(Enumerable.Skip).GetMethodInfo().GetGenericMethodDefinition())
            .MakeGenericMethod(type);

        private static MethodInfo? _skipWhile;

        public static MethodInfo SkipWhile(Type type) => (_skipWhile ??= new Func<IEnumerable<object>, Func<object, bool>, IEnumerable<object>>(Enumerable.SkipWhile).GetMethodInfo().GetGenericMethodDefinition())
            .MakeGenericMethod(type);

        private static MethodInfo? _skipWhile_index;

        public static MethodInfo SkipWhile_Index(Type type) => (_skipWhile_index ??= new Func<IEnumerable<object>, Func<object, int, bool>, IEnumerable<object>>(Enumerable.SkipWhile).GetMethodInfo().GetGenericMethodDefinition())
            .MakeGenericMethod(type);
        
        public static MethodInfo Sum(Type type) => typeof(Enumerable).GetMethod(nameof(Enumerable.Sum), new []{type})!;

        //private static MethodInfo? _sum_long;

        //public static MethodInfo Sum_Long(Type type) => (_sum_long ??= new Func<IEnumerable<long>, long>(Enumerable.Sum).GetMethodInfo().GetGenericMethodDefinition())
        //    .MakeGenericMethod(type);

        //private static MethodInfo? _sum_float;

        //public static MethodInfo Sum_Float(Type type) => (_sum_float ??= new Func<IEnumerable<float>, float>(Enumerable.Sum).GetMethodInfo().GetGenericMethodDefinition())
        //    .MakeGenericMethod(type);

        //private static MethodInfo? _sum_double;

        //public static MethodInfo Sum_Double(Type type) => (_sum_double ??= new Func<IEnumerable<double>, double>(Enumerable.Sum).GetMethodInfo().GetGenericMethodDefinition())
        //    .MakeGenericMethod(type);

        //private static MethodInfo? _sum_decimal;

        //public static MethodInfo Sum_Decimal(Type type) => (_sum_decimal ??= new Func<IEnumerable<decimal>, decimal>(Enumerable.Sum).GetMethodInfo().GetGenericMethodDefinition())
        //    .MakeGenericMethod(type);

        private static MethodInfo? _sum_selector_int;

        public static MethodInfo Sum_Selector_Int(Type source, Type result) => (_sum_selector_int ??= new Func<IEnumerable<object>, Func<object, int>, int>(Enumerable.Sum).GetMethodInfo().GetGenericMethodDefinition())
            .MakeGenericMethod(source, result);

        private static MethodInfo? _sum_selector_long;

        public static MethodInfo Sum_Selector_Long(Type source, Type result) => (_sum_selector_long ??= new Func<IEnumerable<object>, Func<object, long>, long>(Enumerable.Sum).GetMethodInfo().GetGenericMethodDefinition())
            .MakeGenericMethod(source, result);

        private static MethodInfo? _sum_selector_float;

        public static MethodInfo Sum_Selector_Float(Type source, Type result) => (_sum_selector_float ??= new Func<IEnumerable<object>, Func<object, float>, float>(Enumerable.Sum).GetMethodInfo().GetGenericMethodDefinition())
            .MakeGenericMethod(source, result);

        private static MethodInfo? _sum_selector_double;

        public static MethodInfo Sum_Selector_Double(Type source, Type result) => (_sum_selector_double ??= new Func<IEnumerable<object>, Func<object, double>, double>(Enumerable.Sum).GetMethodInfo().GetGenericMethodDefinition())
            .MakeGenericMethod(source, result);

        private static MethodInfo? _sum_selector_decimal;

        public static MethodInfo Sum_Selector_Decimal(Type source, Type result) => (_sum_selector_decimal ??= new Func<IEnumerable<object>, Func<object, decimal>, decimal>(Enumerable.Sum).GetMethodInfo().GetGenericMethodDefinition())
            .MakeGenericMethod(source, result);

        private static MethodInfo? _take;

        public static MethodInfo Take(Type type) => (_take ??= new Func<IEnumerable<object>, int, IEnumerable<object>>(Enumerable.Take).GetMethodInfo().GetGenericMethodDefinition())
            .MakeGenericMethod(type);

        private static MethodInfo? _takeWhile;

        public static MethodInfo TakeWhile(Type type) => (_takeWhile ??= new Func<IEnumerable<object>, Func<object, bool>, IEnumerable<object>>(Enumerable.TakeWhile).GetMethodInfo().GetGenericMethodDefinition())
            .MakeGenericMethod(type);

        private static MethodInfo? _thenBy_selector;

        public static MethodInfo ThenBy_Selector(Type source, Type key) => (_thenBy_selector ??= new Func<IOrderedEnumerable<object>, Func<object, object>, IOrderedEnumerable<object>>(Enumerable.ThenBy).GetMethodInfo().GetGenericMethodDefinition())
            .MakeGenericMethod(source, key);

        private static MethodInfo? _thenByDescending_selector;

        public static MethodInfo ThenByDescending_Selector(Type source, Type key) => (_thenByDescending_selector ??= new Func<IOrderedEnumerable<object>, Func<object, object>, IOrderedEnumerable<object>>(Enumerable.ThenByDescending).GetMethodInfo().GetGenericMethodDefinition())
            .MakeGenericMethod(source, key);
   
        private static MethodInfo? _toArray;

        public static MethodInfo ToArray(Type type) => (_toArray ??= new Func<IEnumerable<object>, object[]>(Enumerable.ToArray).GetMethodInfo().GetGenericMethodDefinition())
            .MakeGenericMethod(type);

        private static MethodInfo? _toList;

        public static MethodInfo ToList(Type type) => (_toList ??= new Func<IEnumerable<object>, List<object>>(Enumerable.ToList).GetMethodInfo().GetGenericMethodDefinition())
            .MakeGenericMethod(type);

        private static MethodInfo? _union;

        public static MethodInfo Union(Type type) => (_union ??= new Func<IEnumerable<object>, IEnumerable<object>, IEnumerable<object>>(Enumerable.Union).GetMethodInfo().GetGenericMethodDefinition())
            .MakeGenericMethod(type);

        private static MethodInfo? _where;

        public static MethodInfo Where(Type type) => (_where ??= new Func<IEnumerable<object>, Func<object, bool>, IEnumerable<object>>(Enumerable.Where).GetMethodInfo().GetGenericMethodDefinition())
            .MakeGenericMethod(type);

    }
}
