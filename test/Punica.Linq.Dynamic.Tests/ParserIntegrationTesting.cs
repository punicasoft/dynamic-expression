using System.Linq.Expressions;
using System.Text.Json;
using Punica.Linq.Dynamic.Tests.Utils;

namespace Punica.Linq.Dynamic.Tests
{
    public class ParserIntegrationTesting
    {
        private Expression<Func<TResult>> GetExpression<TResult>(string expression)
        {
            var rootToken = Tokenizer.Evaluate(new TokenContext(expression));

            var resultExpression = rootToken.Evaluate();
            return (Expression<Func<TResult>>)resultExpression;
        }

        private Expression<Func<T1, TResult>> GetExpression<T1, TResult>(string expression)
        {
            var context = new TokenContext(expression);
            context.AddStartParameter(typeof(T1));
            var rootToken = Tokenizer.Evaluate(context);

            var resultExpression = rootToken.Evaluate();
            return (Expression<Func<T1, TResult>>)resultExpression;
        }

        private LambdaExpression GetGeneralExpression<T1, TResult>(string expression)
        {
            var context = new TokenContext(expression);
            context.AddStartParameter(typeof(T1));
            var rootToken = Tokenizer.Evaluate(context);

            var resultExpression = rootToken.Evaluate();
            return (LambdaExpression)resultExpression;
        }


        [Fact]
        public void Evaluate_WhenExpressionIsIntegerAdd_ShouldWork()
        {
            string stringExp = "5 + 7";

            var resultExpression = GetExpression<int>(stringExp);
            var actual = resultExpression.Compile()();

            Assert.Equal(5 + 7, actual);
        }

        [Theory]
        [InlineData(5, 7)]
        [InlineData(7, 5)]
        [InlineData(-5, 7)]
        [InlineData(-7, 5)]
        public void Evaluate_WhenExpressionIsIntegerMinus_ShouldWork(int x, int y)
        {

            string stringExp = $"{x} - {y}";

            var resultExpression = GetExpression<int>(stringExp);
            var actual = resultExpression.Compile()();

            Assert.Equal(x - y, actual);
        }


        [Theory]
        [InlineData(5.3, 7.1)]
        [InlineData(-5.3, 7.1)]
        [InlineData(7.1, -5.3)]
        public void Evaluate_WhenExpressionIsRealAdd_ShouldWork(double x, double y)
        {

            string stringExp = $"{x} + {y}";
            var resultExpression = GetExpression<double>(stringExp);
            var actual = resultExpression.Compile()();

            Assert.Equal(x + y, actual);
        }

        [Theory]
        [InlineData(5.3, 7.1)]
        [InlineData(7.1, 5.3)]
        [InlineData(-5.3, 7.1)]
        [InlineData(-7.1, 5.3)]
        public void Evaluate_WhenExpressionIsRealMinus_ShouldWork(double x, double y)
        {

            string stringExp = $"{x} - {y}";
            var resultExpression = GetExpression<double>(stringExp);
            var actual = resultExpression.Compile()();

            Assert.Equal(x - y, actual);
        }


        [Theory]
        [InlineData(5, 7)]
        [InlineData(5, 0)]
        [InlineData(-5, 7)]
        [InlineData(-7, 5)]
        public void Evaluate_WhenExpressionIsMultiply_ShouldWork(int x, int y)
        {

            string stringExp = $"{x} * {y}";
            var resultExpression = GetExpression<int>(stringExp);
            var actual = resultExpression.Compile()();

            Assert.Equal(x * y, actual);
        }

        [Theory]
        [InlineData(5.2, 7.2)]
        [InlineData(5.2, 7)]
        [InlineData(5.7, 0)]
        [InlineData(-5.2, 7)]
        [InlineData(-7.3, 5)]
        public void Evaluate_WhenExpressionIsMultiplyReal_ShouldWork(double x, double y)
        {

            string stringExp = $"{x} * {y}";
            var resultExpression = GetExpression<double>(stringExp);
            var actual = resultExpression.Compile()();

            Assert.Equal(x * y, actual);
        }

        [Theory]
        [InlineData(11, 5)]
        [InlineData(5, 7)]
        [InlineData(-5, 7)]
        [InlineData(-7, 5)]
        public void Evaluate_WhenExpressionIsDivide_ShouldWork(int x, int y)
        {

            string stringExp = $"{x} / {y}";
            var resultExpression = GetExpression<int>(stringExp);
            var actual = resultExpression.Compile()();

            Assert.Equal(x / y, actual);
        }

        [Theory]
        [InlineData(11.3, 5.1)]
        [InlineData(5.4, 7.8)]
        [InlineData(5.4, 3)]
        [InlineData(-5.6, 7.9)]
        [InlineData(-7.8, 3.2)]
        public void Evaluate_WhenExpressionIsDivideReal_ShouldWork(double x, double y)
        {
            string stringExp = $"{x} / {y}";
            var resultExpression = GetExpression<double>(stringExp);
            var actual = resultExpression.Compile()();

            Assert.Equal(x / y, actual);
        }



        [Theory]
        [InlineData(7, 8)]
        [InlineData(5, 5)]
        public void Evaluate_WhenExpressionIsEqual_ShouldWork(int x, int y)
        {

            string stringExp = $"{x} == {y}";
            var resultExpression = GetExpression<bool>(stringExp);
            var actual = resultExpression.Compile()();

            Assert.Equal(x == y, actual);
        }

        [Theory]
        [InlineData(7, 8)]
        [InlineData(5, 5)]
        public void Evaluate_WhenExpressionIsNotEqual_ShouldWork(int x, int y)
        {

            string stringExp = $"{x} != {y}";
            var resultExpression = GetExpression<bool>(stringExp);
            var actual = resultExpression.Compile()();

            Assert.Equal(x != y, actual);
        }


        [Theory]
        [InlineData(7, 7)]
        [InlineData(3, 5)]
        [InlineData(8, 2)]
        public void Evaluate_WhenExpressionIsGreaterThan_ShouldWork(int x, int y)
        {
            string stringExp = $"{x} > {y}";
            var resultExpression = GetExpression<bool>(stringExp);
            var actual = resultExpression.Compile()();

            Assert.Equal(x > y, actual);
        }


        [Theory]
        [InlineData(7, 7)]
        [InlineData(3, 5)]
        [InlineData(8, 2)]
        public void Evaluate_WhenExpressionIsLessThan_ShouldWork(int x, int y)
        {
            string stringExp = $"{x} < {y}";
            var resultExpression = GetExpression<bool>(stringExp);
            var actual = resultExpression.Compile()();

            Assert.Equal(x < y, actual);
        }

        [Theory]
        [InlineData(7, 7)]
        [InlineData(3, 5)]
        [InlineData(8, 2)]
        public void Evaluate_WhenExpressionIsGreaterThanEqual_ShouldWork(int x, int y)
        {
            string stringExp = $"{x} >= {y}";
            var resultExpression = GetExpression<bool>(stringExp);
            var actual = resultExpression.Compile()();

            Assert.Equal(x >= y, actual);
        }


        [Theory]
        [InlineData(7, 7)]
        [InlineData(3, 5)]
        [InlineData(8, 2)]
        public void Evaluate_WhenExpressionIsLessThanEqual_ShouldWork(int x, int y)
        {
            string stringExp = $"{x} <= {y}";
            var resultExpression = GetExpression<bool>(stringExp);
            var actual = resultExpression.Compile()();

            Assert.Equal(x <= y, actual);
        }

        [Theory]
        [InlineData("true", true)]
        [InlineData("false", false)]
        public void Evaluate_WhenExpressionIsBoolNot_ShouldWork(string exp, bool value)
        {
            string stringExp = $"!{exp}";
            var resultExpression = GetExpression<bool>(stringExp);
            var actual = resultExpression.Compile()();

            Assert.Equal(!value, actual);
        }

        [Theory]
        [InlineData(true, 5, 7)]
        [InlineData(false, 8, 3)]
        public void Evaluate_WhenExpressionIsCondition_ShouldWork(bool value, int ifTrue, int ifFalse)
        {
            string stringExp = $"{value.ToString().ToLower()} ? {ifTrue} : {ifFalse}";
            var resultExpression = GetExpression<int>(stringExp);
            var actual = resultExpression.Compile()();

            Assert.Equal(value ? ifTrue : ifFalse, actual);
        }


        [Theory]
        [InlineData("hello", "world")]
        [InlineData("hel", "lo")]
        [InlineData("hell", "o")]
        public void Evaluate_WhenExpressionIsStringAdd_ShouldWork(string x, string y)
        {

            string stringExp = $"'{x}' + '{y}'";
            var resultExpression = GetExpression<string>(stringExp);
            var actual = resultExpression.Compile()();

            Assert.Equal(x + y, actual);
        }

        [Theory]
        [InlineData("he", "hello world")]
        [InlineData("ll", "hello world")]
        [InlineData("o", "hello world")]
        [InlineData("so", "hello world")]
        public void Evaluate_WhenExpressionIsInString_ShouldWork(string x, string y)
        {
            string stringExp = $"'{x}' in '{y}'";
            var resultExpression = GetExpression<bool>(stringExp);
            var actual = resultExpression.Compile()();

            Assert.Equal(y.Contains(x), actual);
        }

        [Fact]
        public void Evaluate_WhenExpressionIsComplexPrimitiveExpression_ShouldWork()
        {
            string stringExp = $"(5 > 3 && 2 <= 4 || 1 != 1 ) && 2 + 4 > 3 && 's' in 'cro' + 's'";
            var resultExpression = GetExpression<bool>(stringExp);
            var actual = resultExpression.Compile()();

            Assert.Equal((5 > 3 && 2 <= 4 || 1 != 1) && 2 + 4 > 3 && ("cro" + "s").Contains("s"), actual);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(3)]
        [InlineData(5)]
        [InlineData(232)]
        public void Evaluate_WhenExpressionIsInArrayInt_ShouldWork(int x)
        {
            string stringExp = $"{x} in Numbers";
            var resultExpression = GetExpression<MyList, bool>(stringExp);
            var actual = resultExpression.Compile()(Data.Collection);
            Assert.Equal(Data.Collection.Numbers.Contains(x), actual);
        }

        [Theory]
        [InlineData("Jan")]
        [InlineData("3")]
        public void Evaluate_WhenExpressionIsInListString_ShouldWork(string x)
        {
            string stringExp = $"'{x}' in Months";
            var resultExpression = GetExpression<MyList, bool>(stringExp);
            var actual = resultExpression.Compile()(Data.Collection);

            Assert.Equal(Data.Collection.Months.Contains(x), actual);
        }

        [Theory]
        [InlineData(Status.Active)]
        [InlineData(Status.Running)]
        public void Evaluate_WhenExpressionIsInListEnum_ShouldWork(Status x)
        {
            string stringExp = $"'{x}' in Statuses";
            var resultExpression = GetExpression<MyList, bool>(stringExp);
            var actual = resultExpression.Compile()(Data.Collection);

            Assert.Equal(Data.Collection.Statuses.Contains(x), actual);
        }



        [Fact]
        public void Evaluate_WhenExpressionIsNewSimpleExpression_ShouldWork()
        {
            string stringExp = "new { FirstName , LastName }";
            var resultExpression = GetGeneralExpression<Person, dynamic>(stringExp);
            var actual = resultExpression.Compile().DynamicInvoke(Data.Persons[0]);
            var a = new { Data.Persons[0].FirstName, Data.Persons[0].LastName }.ToString();
            Assert.Equal(a, actual.ToString());
        }

        [Fact]
        public void Evaluate_WhenExpressionIsNewSimpleExpressionWithAs_ShouldWork()
        {
            string stringExp = "new { FirstName as 'First' , LastName as 'Last' }";
            var resultExpression = GetGeneralExpression<Person, object>(stringExp);
            var actual = resultExpression.Compile().DynamicInvoke(Data.Persons[0]);
            var a = new { First = Data.Persons[0].FirstName, Last = Data.Persons[0].LastName }.ToString();
            Assert.Equal(a, actual.ToString());
        }

        [Fact]
        public void Evaluate_WhenExpressionIsNewExpressionWithDotAndAs_ShouldWork()
        {
            string stringExp = "new { Account.Name as 'Name' , Account.Balance as 'Balance' }";
            var resultExpression = GetGeneralExpression<Person, object>(stringExp);
            var actual = resultExpression.Compile().DynamicInvoke(Data.Persons[0]);
            var a = new { Name = Data.Persons[0].Account.Name, Balance = Data.Persons[0].Account.Balance }.ToString();
            Assert.Equal(a, actual.ToString());
        }

        [Fact(Skip = "Assignment Not Supported yet")]
        public void Evaluate_WhenExpressionIsNewExpressionWithDotAndAssign_ShouldWork()
        {
            string stringExp = "new { Name = Account.Name , Bala = Account.Balance  }";
            var resultExpression = GetGeneralExpression<Person, object>(stringExp);
            var actual = resultExpression.Compile().DynamicInvoke(Data.Persons[0]);
            var a = new { Name = Data.Persons[0].Account.Name, Bala = Data.Persons[0].Account.Balance }.ToString();
            Assert.Equal(a, actual.ToString());
        }

        [Fact]
        public void Evaluate_WhenExpressionIsNewExpressionWithDot_ShouldWork()
        {
            string stringExp = "new { Account.Name , Account.Balance }";
            var resultExpression = GetGeneralExpression<Person, object>(stringExp);
            var actual = resultExpression.Compile().DynamicInvoke(Data.Persons[0]);
            var a = new { AccountName = Data.Persons[0].Account.Name, AccountBalance = Data.Persons[0].Account.Balance }.ToString();
            Assert.Equal(a, actual.ToString());
        }


        [Fact]
        public void Evaluate_WhenExpressionIsNewExpressionWithOperators_ShouldWork()
        {
            string stringExp = "new { FirstName + LastName as 'FullName' , Account.Balance + 10 as 'Balance' }";
            var resultExpression = GetGeneralExpression<Person, object>(stringExp);
            var actual = resultExpression.Compile().DynamicInvoke(Data.Persons[0]);
            var a = new { FullName = Data.Persons[0].FirstName + Data.Persons[0].LastName, Balance = Data.Persons[0].Account.Balance + 10 }.ToString();
            Assert.Equal(a, actual.ToString());
        }

        [Fact]
        public void Evaluate_WhenExpressionIsNewExpressionWithNew_ShouldWork()
        {
            string stringExp = "new { new { Account.Name, Account.Balance } as 'Bank' }";
            var resultExpression = GetGeneralExpression<Person, object>(stringExp);
            var actual = resultExpression.Compile().DynamicInvoke(Data.Persons[0]);
            var expected = JsonSerializer.Serialize(new { Bank = new { AccountName = Data.Persons[0].Account.Name, AccountBalance = Data.Persons[0].Account.Balance } });
            var actualJson = JsonSerializer.Serialize(actual);
            Assert.Equal(expected, actualJson);
        }


        [Fact]
        public void Evaluate_WhenExpressionIsNewExpressionWithSelect_ShouldWork()
        {
            string stringExp = "new{FirstName,Children.Select(new{Name,Gender}).ToList() as 'Kids'}";
            var resultExpression = GetGeneralExpression<Person, object>(stringExp);
            var actual = resultExpression.Compile().DynamicInvoke(Data.Persons[0]);

            var expected = JsonSerializer.Serialize(new { Data.Persons[0].FirstName, Kids = Data.Persons[0].Children.Select(c => new { c.Name, c.Gender }).ToList() });
            var actualJson = JsonSerializer.Serialize(actual);
            Assert.Equal(expected, actualJson);
        }

        [Fact]
        public void Evaluate_WhenExpressionIsQueryable_ShouldWork()
        {
            string stringExp = "Select( new { FirstName , Children.Select(new {Name , Gender}).ToList() as 'Kids'} )";
            var resultExpression = GetGeneralExpression<IQueryable<Person>, object>(stringExp);
            var actual = resultExpression.Compile().DynamicInvoke(Data.Persons.AsQueryable());
            var expected = JsonSerializer.Serialize(Data.Persons.AsQueryable().Select(p => new { p.FirstName, Kids = p.Children.Select(c => new { c.Name, c.Gender }) }));
            var actualJson = JsonSerializer.Serialize(actual);
            Assert.Equal(expected, actualJson);
        }

        [Fact]
        public void Evaluate_WhenExpressionIsCondition_ShouldWork_2()
        {
            string stringExp = "IsMale?'Male':'Female'";
            var resultExpression = GetGeneralExpression<Person, object>(stringExp);
            var actual = resultExpression.Compile().DynamicInvoke(Data.Persons[0]);

            var expected = Data.Persons[0].IsMale ? "Male" : "Female";

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Evaluate_WhenExpressionIsQueryableWithWhere_ShouldWork()
        {
            string stringExp = "Select(new{ FirstName,Children.Select(new{Name , Gender}).ToList() as 'Kids'}).Where(Kids.Any(Gender == 'Female'))";
            var resultExpression = GetGeneralExpression<IQueryable<Person>, object>(stringExp);
            var actual = resultExpression.Compile().DynamicInvoke(Data.Persons.AsQueryable());
            var expected = JsonSerializer.Serialize(Data.Persons.AsQueryable()
                .Select(p => new { p.FirstName, Kids = p.Children.Select(c => new { c.Name, c.Gender }) })
                .Where(o => o.Kids.Any(k => k.Gender == "Female")));

            var actualJson = JsonSerializer.Serialize(actual);
            Assert.Equal(expected, actualJson);
        }

        [Theory]
        [MemberData(nameof(TestData))]
        public void Evaluate_WhenExpressionIsAverage_ShouldWork(string methodName, object expected)
        {
            string stringExp = $"Average({methodName})";
            var resultExpression = GetGeneralExpression<List<Numbers>, object>(stringExp);
            var actual = resultExpression.Compile().DynamicInvoke(Data.Num);
            Assert.Equal(expected, actual);
        }

        public static IEnumerable<object[]> TestData =>
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

        ///// TODO add date time add, and other operations, string operations, guid


        //[Fact]
        //public void Evaluate_Select2_Should_Work()
        //{
        //    List<int> numbers = new List<int> { 1, 2, 3, 4, 5 };

        //    // Using the first Select overload: Func<TSource, TResult>
        //    // Doubles each number in the list
        //    IEnumerable<int> doubledNumbers = numbers.Select(x => x * 2);
        //    Console.WriteLine("Doubled numbers:");
        //    foreach (int number in doubledNumbers)
        //    {
        //        Console.WriteLine(number);
        //    }

        //    // Using the second Select overload: Func<TSource, int, TResult>
        //    // Creates a tuple with the number and its index in the list
        //    IEnumerable<(int Number, int Index)> numbersWithIndex = numbers.Select((x, index) => (Number: x, Index: index));
        //    Console.WriteLine("\nNumbers with index:");
        //    foreach (var numberWithIndex in numbersWithIndex)
        //    {
        //        Console.WriteLine($"Number: {numberWithIndex.Number}, Index: {numberWithIndex.Index}");
        //    }
        //}

        //[Fact]
        //public void Evaluate_GroupJoin_ShouldWork()
        //{
        //    Person magnus = new Person { FirstName = "Magnus" };
        //    Person terry = new Person { FirstName = "Terry" };
        //    Person charlotte = new Person { FirstName = "Charlotte" };

        //    Pet barley = new Pet { Name = "Barley", Owner = terry };
        //    Pet boots = new Pet { Name = "Boots", Owner = terry };
        //    Pet whiskers = new Pet { Name = "Whiskers", Owner = charlotte };
        //    Pet daisy = new Pet { Name = "Daisy", Owner = magnus };

        //    List<Person> people = new List<Person> { magnus, terry, charlotte };
        //    List<Pet> pets = new List<Pet> { barley, boots, whiskers, daisy };

        //    // Create a list where each element is an anonymous
        //    // type that contains a person's name and
        //    // a collection of names of the pets they own.
        //    var expected = people.GroupJoin(pets,
        //        person => person,
        //        pet => pet.Owner,
        //        (person, petCollection) =>
        //            new
        //            {
        //                OwnerName = person.FirstName,
        //                Pets = petCollection.Select(pet => pet.Name)
        //            });


        //    var para = new JoinPara()
        //    {
        //        pets = pets
        //    };

        //    // string stringExp = "people.GroupJoin(pets, person => person, pet => pet.Owner, (person, petCollection) => new { OwnerName = person.FirstName, Pets = petCollection.Select(pet => pet.Name) } )";

        //    string stringExp = "GroupJoin(@pets, person, pet.Owner,  new { person.FirstName as 'OwnerName', petCollection.Select(Name) } as 'Pets' )";
        //    //TODO: may be use _ but the issue with two types of parameters

        //    // Genral expression
        //    // var resultExpression = GetGeneralExpression<List<Person>, object>(stringExp);
        //    var rootToken = Tokenizer2.Evaluate(new TokenContext(stringExp, new MethodContext(Expression.Parameter(typeof(List<Person>), "arg")), Expression.Constant(para)));

        //    var resultExpression = rootToken.Evaluate(null);
        //    var lambdaExpression = (LambdaExpression)resultExpression;
        //    var actual = lambdaExpression.Compile().DynamicInvoke(people);
        //}

        //[Fact]
        //public void Evaluate_Join_ShouldWork()
        //{
        //    Person magnus = new Person { Name = "Hedlund, Magnus" };
        //    Person terry = new Person { Name = "Adams, Terry" };
        //    Person charlotte = new Person { Name = "Weiss, Charlotte" };

        //    Pet barley = new Pet { Name = "Barley", Owner = terry };
        //    Pet boots = new Pet { Name = "Boots", Owner = terry };
        //    Pet whiskers = new Pet { Name = "Whiskers", Owner = charlotte };
        //    Pet daisy = new Pet { Name = "Daisy", Owner = magnus };

        //    List<Person> people = new List<Person> { magnus, terry, charlotte };
        //    List<Pet> pets = new List<Pet> { barley, boots, whiskers, daisy };

        //    // Create a list of Person-Pet pairs where
        //    // each element is an anonymous type that contains a
        //    // Pet's name and the name of the Person that owns the Pet.
        //    var query =
        //        people.Join(pets,
        //            person => person,
        //            pet => pet.Owner,
        //            (person, pet) =>
        //                new { OwnerName = person.FirstName, Pet = pet.Name });

        //}

        //[Fact]
        //public void Evaluate_SelectMany2_ShouldWork()
        //{
        //    PetOwner[] petOwners =
        //    { new PetOwner { Name="Higa",
        //            Pets = new List<string>{ "Scruffy", "Sam" } },
        //        new PetOwner { Name="Ashkenazi",
        //            Pets = new List<string>{ "Walker", "Sugar" } },
        //        new PetOwner { Name="Price",
        //            Pets = new List<string>{ "Scratches", "Diesel" } },
        //        new PetOwner { Name="Hines",
        //            Pets = new List<string>{ "Dusty" } } };

        //    // Project the pet owner's name and the pet's name.
        //    var query =
        //        petOwners
        //            .SelectMany(petOwner => petOwner.Pets,
        //                (petOwner, petName) => new { petOwner, petName })
        //            .Where(ownerAndPet => ownerAndPet.petName.StartsWith("S"))
        //            .Select(ownerAndPet =>
        //                new
        //                {
        //                    Owner = ownerAndPet.petOwner.Name,
        //                    Pet = ownerAndPet.petName
        //                }
        //            );
        //}

    }
}
