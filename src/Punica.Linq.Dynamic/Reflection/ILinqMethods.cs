using System.Linq.Expressions;

namespace Punica.Linq.Dynamic.Reflection
{
    public interface ILinqMethods
    {
        MethodCallExpression All(Expression member, ParameterExpression parameter, Expression[] expressions);
        MethodCallExpression Any(Expression member, ParameterExpression parameter, Expression[] expressions);
        MethodCallExpression Average(Expression member, ParameterExpression parameter, Expression[] expressions);
        MethodCallExpression Cast(Expression member, ParameterExpression parameter, Expression[] expressions);
        MethodCallExpression Concat(Expression member, ParameterExpression parameter, Expression[] expressions);
        MethodCallExpression Contains(Expression member, ParameterExpression parameter, Expression[] expressions);
        MethodCallExpression Count(Expression member, ParameterExpression parameter, Expression[] expressions);
        MethodCallExpression DefaultIfEmpty(Expression member, ParameterExpression parameter, Expression[] expressions);
        MethodCallExpression Distinct(Expression member, ParameterExpression parameter, Expression[] expressions);
        MethodCallExpression ElementAt(Expression member, ParameterExpression parameter, Expression[] expressions);
        MethodCallExpression ElementAtOrDefault(Expression member, ParameterExpression parameter, Expression[] expressions);
        MethodCallExpression Except(Expression member, ParameterExpression parameter, Expression[] expressions);
        MethodCallExpression First(Expression member, ParameterExpression parameter, Expression[] expressions);
        MethodCallExpression FirstOrDefault(Expression member, ParameterExpression parameter, Expression[] expressions);
        MethodCallExpression GroupBy(Expression member, ParameterExpression parameter, Expression[] expressions);
        MethodCallExpression GroupJoin(Expression member, ParameterExpression parameter, Expression[] expressions);
        MethodCallExpression Intersect(Expression member, ParameterExpression parameter, Expression[] expressions);
        MethodCallExpression Join(Expression member, ParameterExpression parameter, Expression[] expressions);
        MethodCallExpression Last(Expression member, ParameterExpression parameter, Expression[] expressions);
        MethodCallExpression LastOrDefault(Expression member, ParameterExpression parameter, Expression[] expressions);
        MethodCallExpression LongCount(Expression member, ParameterExpression parameter, Expression[] expressions);
        MethodCallExpression Max(Expression member, ParameterExpression parameter, Expression[] expressions);
        MethodCallExpression Min(Expression member, ParameterExpression parameter, Expression[] expressions);
        MethodCallExpression OrderBy(Expression member, ParameterExpression parameter, Expression[] expressions);
        MethodCallExpression OrderByDescending(Expression member, ParameterExpression parameter, Expression[] expressions);
        MethodCallExpression Reverse(Expression member, ParameterExpression parameter, Expression[] expressions);
        MethodCallExpression Select(Expression member, ParameterExpression parameter, Expression[] expressions);
        MethodCallExpression SelectMany(Expression member, ParameterExpression parameter, Expression[] expressions);
        MethodCallExpression SequenceEqual(Expression member, ParameterExpression parameter, Expression[] expressions);
        MethodCallExpression Single(Expression member, ParameterExpression parameter, Expression[] expressions);
        MethodCallExpression SingleOrDefault(Expression member, ParameterExpression parameter, Expression[] expressions);
        MethodCallExpression Skip(Expression member, ParameterExpression parameter, Expression[] expressions);
        MethodCallExpression SkipWhile(Expression member, ParameterExpression parameter, Expression[] expressions);
        MethodCallExpression Sum(Expression member, ParameterExpression parameter, Expression[] expressions);
        MethodCallExpression Take(Expression member, ParameterExpression parameter, Expression[] expressions);
        MethodCallExpression TakeWhile(Expression member, ParameterExpression parameter, Expression[] expressions);
        MethodCallExpression ThenBy(Expression member, ParameterExpression parameter, Expression[] expressions);
        MethodCallExpression ThenByDescending(Expression member, ParameterExpression parameter, Expression[] expressions);
        MethodCallExpression ToArray(Expression member, ParameterExpression parameter, Expression[] expressions);
        MethodCallExpression ToList(Expression member, ParameterExpression parameter, Expression[] expressions);
        MethodCallExpression Union(Expression member, ParameterExpression parameter, Expression[] expressions);
        MethodCallExpression Where(Expression member, ParameterExpression parameter, Expression[] expressions);
    }
}
