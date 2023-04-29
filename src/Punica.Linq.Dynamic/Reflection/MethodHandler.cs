namespace Punica.Linq.Dynamic.Reflection
{
 
    public class MethodHandler
    {
        private static readonly IMethodHandler EnumerableHandler  =new EnumerableMethodHandler();
        private static readonly IMethodHandler QueryableHandler = new QueryableMethodHandler();

        public static MethodHandler Instance { get; } = new MethodHandler();

        private MethodHandler()
        {
        }

        public IMethodHandler GetHandler(Type type)
        {
            if (typeof(IQueryable).IsAssignableFrom(type))
            {
                return QueryableHandler;
            }
            else
            {
                return EnumerableHandler;
            }
        }

    }
}
