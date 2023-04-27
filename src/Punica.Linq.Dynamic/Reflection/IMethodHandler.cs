using System.Linq.Expressions;

namespace Punica.Linq.Dynamic.Reflection
{
    public interface IMethodHandler
    {
        MethodCallExpression CallMethod(string methodName, Expression member, ParameterExpression parameter, Expression[] expressions);
    }
}
