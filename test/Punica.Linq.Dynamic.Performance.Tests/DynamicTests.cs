using System.Linq.Expressions;
using BenchmarkDotNet.Attributes;

namespace Punica.Linq.Dynamic.Performance.Tests
{
    [MemoryDiagnoser]
    public class DynamicTests
    {
        //13ms
        [Benchmark]
        public void Two_Select_With_New_Expression()
        {
            string stringExp = "Select( new { FirstName , Children.Select(new {Name , Gender}).ToList() as 'Kids'} )";
            var rootToken = Tokenizer.Evaluate(new TokenContext(stringExp, new MethodContext(Expression.Parameter(typeof(IQueryable<Person>), "arg"))));
            var resultExpression = rootToken.Evaluate(null);
        }

        [Benchmark]
        public void Select_With_New_Expression()
        {
            string stringExp = "Select( new { FirstName , LastName as 'Kids'} )";
            var rootToken = Tokenizer.Evaluate(new TokenContext(stringExp, new MethodContext(Expression.Parameter(typeof(IQueryable<Person>), "arg"))));
            var resultExpression = rootToken.Evaluate(null);
        }

        [Benchmark]
        public void Advanced_Boolean_Expression()
        {
            string stringExp = $"(5 > 3 && 2 <= 4 || 1 != 1 ) && 2 + 4 > 3 && 's' in 'cro' + 's'";
            var rootToken = Tokenizer.Evaluate(new TokenContext(stringExp));
            var resultExpression = rootToken.Evaluate(null);
        }
    }

    public class Person
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Description { get; set; }
        public DateTime Dob { get; set; }
        public int NoOfChildren { get; set; }
        public bool IsMarried { get; set; }
        public bool IsMale { get; set; }
        public Account Account { get; set; }

        public List<Child> Children { get; set; }
    }

    public class Account
    {
        public string Name { get; set; }
        public decimal Balance { get; set; }
    }

    public class Child
    {
        public string Name { get; set; }

        public string Gender { get; set; }
    }

}
