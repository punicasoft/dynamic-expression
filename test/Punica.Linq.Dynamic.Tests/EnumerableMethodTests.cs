using System.Linq.Expressions;
using System.Text.Json;
using Punica.Linq.Dynamic.Tests.Utils;

namespace Punica.Linq.Dynamic.Tests
{
    //https://learn.microsoft.com/en-us/dotnet/api/system.linq.enumerable.contains?view=net-8.0
    public class EnumerableMethodTests : ExpressionTestsBase
    {
        [Fact]
        public void Evaluate_All_ShouldWork()
        {
            var expression = GetExpression<Pet[], bool>("All(Age > 5)");
            var actual = expression.Compile()(Data.Pets);
            var expected = Data.Pets.All(p => p.Age > 5);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Evaluate_Any_ShouldWork()
        {
            var expression = GetExpression<Pet[], bool>("Any()");
            var actual = expression.Compile()(Data.Pets);
            var expected = Data.Pets.Any();
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Evaluate_Any_Predicate_ShouldWork()
        {
            var expression = GetExpression<Pet[], bool>("Any(Age > 5)");
            var actual = expression.Compile()(Data.Pets);
            var expected = Data.Pets.Any(p => p.Age > 5);
            Assert.Equal(expected, actual);
        }

        [Theory]
        [MemberData(nameof(Average))]
        public void Evaluate_Average_ShouldWork(string property, object expected)
        {
            string stringExp = $"{property}.Average()";
            var resultExpression = GetGeneralExpression<MyList>(stringExp);
            var actual = resultExpression.Compile().DynamicInvoke(Data.Collection);
            Assert.Equal(expected, actual);
        }

        [Theory]
        [MemberData(nameof(AveragePredicate))]
        public void Evaluate_Average_Predicate_ShouldWork(string property, object expected)
        {
            string stringExp = $"Average({property})";
            var resultExpression = GetGeneralExpression<List<Numbers>>(stringExp);
            var actual = resultExpression.Compile().DynamicInvoke(Data.Num);
            Assert.Equal(expected, actual);
        }


        [Fact]
        public void Evaluate_Concat_ShouldWork()
        {
            Pet[] cats = { new Pet { Name="Barley", Age=8 },
                new Pet { Name="Boots", Age=4 },
                new Pet { Name="Whiskers", Age=1 } };

            Pet[] dogs = { new Pet { Name="Bounder", Age=3 },
                new Pet { Name="Snoopy", Age=14 },
                new Pet { Name="Fido", Age=9 } };

            var expression = GetGeneralExpression("cats.Concat(dogs)", null,Expression.Parameter(cats.GetType(), "cats"), Expression.Parameter(dogs.GetType(), "dogs"));
            var actual = expression.Compile().DynamicInvoke(cats,dogs);
            var expected = cats.Concat(dogs);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Evaluate_Contains_ShouldWork()
        {
            var expression = GetExpression<string[], bool>("Contains(\"mango\")");
            var actual = expression.Compile()(Data.Fruits);
            var expected = Data.Fruits.Contains("mango"); 
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Evaluate_Contains_Comparer_ShouldWork()
        {
            Product[] fruits = { new Product { Name = "apple", Code = 9 },
                new Product { Name = "orange", Code = 4 },
                new Product { Name = "lemon", Code = 12 } };

            Product apple = new Product { Name = "apple", Code = 9 };

            ProductComparer prodc = new ProductComparer();

            var expression = "fruits.Contains(@apple, @prodc)";

            var context = new Evaluator()
                .AddVariable("apple", apple)
                .AddVariable("prodc", prodc)
                .AddParameter(fruits.GetType(), "fruits");

            var func = context.Evaluate<Func<Product[], bool>>(expression);

            var actual = func(fruits);
            var expected = fruits.Contains(apple, prodc);
            Assert.Equal(expected, actual);
        }

        // Test whether Count working
        [Fact]
        public void Evaluate_Count_ShouldWork()
        {
            var expression = GetExpression<Pet[], int>("Count()");
            var actual = expression.Compile()(Data.Pets);
            var expected = Data.Pets.Count();
            Assert.Equal(expected, actual);
        }

        // Test whether Count working with predicate
        [Fact]
        public void Evaluate_Count_Predicate_ShouldWork()
        {
            var expression = GetExpression<Pet[], int>("Count(Age > 5)");
            var actual = expression.Compile()(Data.Pets);
            var expected = Data.Pets.Count(p => p.Age > 5);
            Assert.Equal(expected, actual);
        }

        // Test DefaultIfEmpty working
        [Fact]
        public void Evaluate_DefaultIfEmpty_ShouldWork()
        {
            var empty = new List<int>();
            var expression = GetExpression<List<int>, IEnumerable<int>>("DefaultIfEmpty()");
            var actual = expression.Compile()(empty);
            var expected = empty.DefaultIfEmpty();
            Assert.Equal(expected.First(), actual.First());
        }

        // Test DefaultIfEmpty working with default value
        [Fact]
        public void Evaluate_DefaultIfEmpty_DefaultValue_ShouldWork()
        {
            var empty = new List<int>();
            var expression = GetExpression<List<int>, IEnumerable<int>>("DefaultIfEmpty(5)");
            var actual = expression.Compile()(empty);
            var expected = empty.DefaultIfEmpty(5);
            Assert.Equal(expected.First(), actual.First());
        }

        // Test Distinct working
        [Fact]
        public void Evaluate_Distinct_ShouldWork()
        {
            var expression = GetExpression<string[], IEnumerable<string>>("Distinct()");
            var actual = expression.Compile()(Data.Fruits);
            var expected = Data.Fruits.Distinct();
            Assert.Equal(expected, actual);
        }

        // Test ElementAt working
        [Fact]
        public void Evaluate_ElementAt_ShouldWork()
        {
            var expression = GetExpression<string[], string>("ElementAt(2)");
            var actual = expression.Compile()(Data.Fruits);
            var expected = Data.Fruits.ElementAt(2);
            Assert.Equal(expected, actual);
        }


        //  Test Select with index working
        [Fact]
        public void Evaluate_Select_Index_ShouldWork()
        {
            List<int> numbers = new List<int> { 1, 2, 3, 4, 5 };

            var expression = GetGeneralExpression<List<int>>("Select((x, index) => new { x, index})");
            var actual = expression.Compile().DynamicInvoke(numbers);
            var expected = numbers.Select((x, index) => new {x, index});
            var expectedJson = JsonSerializer.Serialize(expected);
            var actualJson = JsonSerializer.Serialize(actual);
            Assert.Equal(expectedJson, actualJson);
        }

        // Test Join should work
        [Fact]
        public void Evaluate_Join_ShouldWork()
        {
            Person magnus = new Person { FirstName = "Magnus" };
            Person terry = new Person { FirstName = "Terry" };
            Person charlotte = new Person { FirstName = "Charlotte" };

            Pet barley = new Pet { Name = "Barley", Owner = terry };
            Pet boots = new Pet { Name = "Boots", Owner = terry };
            Pet whiskers = new Pet { Name = "Whiskers", Owner = charlotte };
            Pet daisy = new Pet { Name = "Daisy", Owner = magnus };

            List<Person> people = new List<Person> { magnus, terry, charlotte };
            List<Pet> pets = new List<Pet> { barley, boots, whiskers, daisy };

            var expression = GetGeneralExpression("people.Join(pets, person => person, pet => pet.Owner, (person, pet) => new { person.FirstName as 'OwnerName', pet.Name as 'Pet' })", null,Expression.Parameter(people.GetType(), "people"), Expression.Parameter(pets.GetType(), "pets"));
            var actual = expression.Compile().DynamicInvoke(people, pets);
            var expected = people.Join(pets, person => person, pet => pet.Owner, (person, pet) => new { OwnerName = person.FirstName, Pet = pet.Name });
            var expectedJson = JsonSerializer.Serialize(expected);
            var actualJson = JsonSerializer.Serialize(actual);
            Assert.Equal(expectedJson, actualJson);
           
        }

        [Fact]
        public void Evaluate_WhenExpressionIsQueryable_ShouldWork()
        {
            string stringExp = "Select( new { FirstName , Children.Select(new {Name , Gender}).ToList() as 'Kids'} )";
            var resultExpression = GetGeneralExpression<IQueryable<Person>>(stringExp);
            var actual = resultExpression.Compile().DynamicInvoke(Data.Persons.AsQueryable());
            var expected = JsonSerializer.Serialize(Data.Persons.AsQueryable().Select(p => new { p.FirstName, Kids = p.Children.Select(c => new { c.Name, c.Gender }) }));
            var actualJson = JsonSerializer.Serialize(actual);
            Assert.Equal(expected, actualJson);
        }

        [Fact]
        public void Evaluate_WhenExpressionIsQueryableWithWhere_ShouldWork()
        {
            string stringExp = "Select(new{ FirstName,Children.Select(new{Name , Gender}).ToList() as 'Kids'}).Where(Kids.Any(Gender == 'Female'))";
            var resultExpression = GetGeneralExpression<IQueryable<Person>>(stringExp);
            var actual = resultExpression.Compile().DynamicInvoke(Data.Persons.AsQueryable());
            var expected = JsonSerializer.Serialize(Data.Persons.AsQueryable()
                .Select(p => new { p.FirstName, Kids = p.Children.Select(c => new { c.Name, c.Gender }) })
                .Where(o => o.Kids.Any(k => k.Gender == "Female")));

            var actualJson = JsonSerializer.Serialize(actual);
            Assert.Equal(expected, actualJson);
        }
        

        public static IEnumerable<object[]> Average =>
            new List<object[]>
            {
                new object[] { nameof(MyList.Numbers), Data.Collection.Numbers.Average() },
                new object[] { nameof(MyList.Prices),  Data.Collection.Prices.Average() },
                new object[] { nameof(MyList.Area), Data.Collection.Area.Average() },
                new object[] { nameof(MyList.Length), Data.Collection.Length.Average() },
                new object[] { nameof(MyList.LongNumbers), Data.Collection.LongNumbers.Average() },

                new object[] { nameof(MyList.NumbersN), Data.Collection.NumbersN.Average() },
                new object[] { nameof(MyList.PricesN), Data.Collection.PricesN.Average() },
                new object[] { nameof(MyList.AreaN), Data.Collection.AreaN.Average() },
                new object[] { nameof(MyList.LengthN), Data.Collection.LengthN.Average() },
                new object[] { nameof(MyList.LongNumbersN), Data.Collection.LongNumbersN.Average() },
            };

        public static IEnumerable<object[]> AveragePredicate =>
            new List<object[]>
            {
                new object[] { nameof(Numbers.Marks), Data.Num.Average(n=> n.Marks) },
                new object[] { nameof(Numbers.Prices), Data.Num.Average(n=> n.Prices) },
                new object[] { nameof(Numbers.Area), Data.Num.Average(n=> n.Area) },
                new object[] { nameof(Numbers.Length), Data.Num.Average(n=> n.Length) },
                new object[] { nameof(Numbers.LongNumbers), Data.Num.Average(n=> n.LongNumbers) },

                new object[] { nameof(Numbers.MarksN), Data.Num.Average(n=> n.MarksN) },
                new object[] { nameof(Numbers.PricesN), Data.Num.Average(n=> n.PricesN) },
                new object[] { nameof(Numbers.AreaN), Data.Num.Average(n=> n.AreaN) },
                new object[] { nameof(Numbers.LengthN), Data.Num.Average(n=> n.LengthN) },
                new object[] { nameof(Numbers.LongNumbersN), Data.Num.Average(n=> n.LongNumbersN) },
            };


    }
}
