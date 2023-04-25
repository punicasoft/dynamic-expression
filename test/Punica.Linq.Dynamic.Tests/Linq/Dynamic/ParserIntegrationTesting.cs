using System.Linq.Expressions;
using System.Text.Json;
using Punica.Linq.Dynamic;
using Punica.Tests.Utils;

namespace Punica.Tests.Linq.Dynamic
{
    public class ParserIntegrationTesting
    {

        [Fact]
        public void Evaluate_WhenExpressionIsIntegerAdd_ShouldWork()
        {

            string stringExp = "5 + 7";
            Evaluator evaluator = new Evaluator((Type)null, null);
            var expression1 = TextParser.Evaluate(stringExp, evaluator);
            var resultExpression = evaluator.GetFilterExpression<int>(expression1[0]);
            var actual = resultExpression.Compile()();

            Assert.Equal(5 + 7, actual);
        }

        [Theory]
        [InlineData(5, 7)]
        [InlineData(7, 5)]
        [InlineData(-5, 7, Skip = "Not Supported Yet")]
        [InlineData(-7, 5, Skip = "Not Supported Yet")]
        public void Evaluate_WhenExpressionIsIntegerMinus_ShouldWork(int x, int y)
        {

            string stringExp = $"{x} - {y}";
            Evaluator evaluator = new Evaluator((Type)null, null);
            var expression1 = TextParser.Evaluate(stringExp, evaluator);
            var resultExpression = evaluator.GetFilterExpression<int>(expression1[0]);
            var actual = resultExpression.Compile()();

            Assert.Equal(x - y, actual);
        }


        [Theory]
        [InlineData(5.3, 7.1)]
        [InlineData(-5.3, 7.1, Skip = "Not Supported Yet")]
        [InlineData(-7.1, 5.3, Skip = "Not Supported Yet")]
        public void Evaluate_WhenExpressionIsRealAdd_ShouldWork(double x, double y)
        {

            string stringExp = $"{x} + {y}";
            Evaluator evaluator = new Evaluator((Type)null, null);
            var expression1 = TextParser.Evaluate(stringExp, evaluator);
            var resultExpression = evaluator.GetFilterExpression<double>(expression1[0]);
            var actual = resultExpression.Compile()();

            Assert.Equal(x + y, actual);
        }

        [Theory]
        [InlineData(5.3, 7.1)]
        [InlineData(7.1, 5.3)]
        [InlineData(-5.3, 7.1, Skip = "Not Supported Yet")]
        [InlineData(-7.1, 5.3, Skip = "Not Supported Yet")]
        public void Evaluate_WhenExpressionIsRealMinus_ShouldWork(double x, double y)
        {

            string stringExp = $"{x} - {y}";
            Evaluator evaluator = new Evaluator((Type)null, null);
            var expression1 = TextParser.Evaluate(stringExp, evaluator);
            var resultExpression = evaluator.GetFilterExpression<double>(expression1[0]);
            var actual = resultExpression.Compile()();

            Assert.Equal(x - y, actual);
        }


        [Theory]
        [InlineData(5, 7)]
        [InlineData(5, 0)]
        [InlineData(-5, 7, Skip = "Not Supported Yet")]
        [InlineData(-7, 5, Skip = "Not Supported Yet")]
        public void Evaluate_WhenExpressionIsMultiply_ShouldWork(int x, int y)
        {

            string stringExp = $"{x} * {y}";
            Evaluator evaluator = new Evaluator((Type)null, null);
            var expression1 = TextParser.Evaluate(stringExp, evaluator);
            var resultExpression = evaluator.GetFilterExpression<int>(expression1[0]);
            var actual = resultExpression.Compile()();

            Assert.Equal(x * y, actual);
        }

        [Theory]
        [InlineData(5.2, 7.2)]
        [InlineData(5.2, 7, Skip = "Mismatched Type, Not Supported Yet")]
        [InlineData(5.7, 0, Skip = "Mismatched Type, Not Supported Yet")]
        [InlineData(-5.2, 7, Skip = "Not Supported Yet")]
        [InlineData(-7.3, 5, Skip = "Not Supported Yet")]
        public void Evaluate_WhenExpressionIsMultiplyReal_ShouldWork(double x, double y)
        {

            string stringExp = $"{x} * {y}";
            Evaluator evaluator = new Evaluator((Type)null, null);
            var expression1 = TextParser.Evaluate(stringExp, evaluator);
            var resultExpression = evaluator.GetFilterExpression<double>(expression1[0]);
            var actual = resultExpression.Compile()();

            Assert.Equal(x * y, actual);
        }

        [Theory]
        [InlineData(11, 5)]
        [InlineData(5, 7)]
        [InlineData(-5, 7, Skip = "Not Supported Yet")]
        [InlineData(-7, 5, Skip = "Not Supported Yet")]
        public void Evaluate_WhenExpressionIsDivide_ShouldWork(int x, int y)
        {

            string stringExp = $"{x} / {y}";
            Evaluator evaluator = new Evaluator((Type)null, null);
            var expression1 = TextParser.Evaluate(stringExp, evaluator);
            var resultExpression = evaluator.GetFilterExpression<int>(expression1[0]);
            var actual = resultExpression.Compile()();

            Assert.Equal(x / y, actual);
        }

        [Theory]
        [InlineData(11.3, 5.1)]
        [InlineData(5.4, 7.8)]
        [InlineData(5.4, 3, Skip = "Mismatched Type, Not Supported Yet")]
        [InlineData(-5.6, 7.9, Skip = "Not Supported Yet")]
        [InlineData(-7.8, 3.2, Skip = "Not Supported Yet")]
        public void Evaluate_WhenExpressionIsDivideReal_ShouldWork(double x, double y)
        {

            string stringExp = $"{x} / {y}";
            Evaluator evaluator = new Evaluator((Type)null, null);
            var expression1 = TextParser.Evaluate(stringExp, evaluator);
            var resultExpression = evaluator.GetFilterExpression<double>(expression1[0]);
            var actual = resultExpression.Compile()();

            Assert.Equal(x / y, actual);
        }

        [Theory]
        [InlineData(7, 8)]
        [InlineData(5, 5)]
        public void Evaluate_WhenExpressionIsEqual_ShouldWork(int x, int y)
        {

            string stringExp = $"{x} == {y}";
            Evaluator evaluator = new Evaluator((Type)null, null);
            var expression1 = TextParser.Evaluate(stringExp, evaluator);
            var resultExpression = evaluator.GetFilterExpression<bool>(expression1[0]);
            var actual = resultExpression.Compile()();

            Assert.Equal(x == y, actual);
        }

        [Theory]
        [InlineData(7, 8)]
        [InlineData(5, 5)]
        public void Evaluate_WhenExpressionIsNotEqual_ShouldWork(int x, int y)
        {

            string stringExp = $"{x} != {y}";
            Evaluator evaluator = new Evaluator((Type)null, null);
            var expression1 = TextParser.Evaluate(stringExp, evaluator);
            var resultExpression = evaluator.GetFilterExpression<bool>(expression1[0]);
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
            Evaluator evaluator = new Evaluator((Type)null, null);
            var expression1 = TextParser.Evaluate(stringExp, evaluator);
            var resultExpression = evaluator.GetFilterExpression<bool>(expression1[0]);
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
            Evaluator evaluator = new Evaluator((Type)null, null);
            var expression1 = TextParser.Evaluate(stringExp, evaluator);
            var resultExpression = evaluator.GetFilterExpression<bool>(expression1[0]);
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
            Evaluator evaluator = new Evaluator((Type)null, null);
            var expression1 = TextParser.Evaluate(stringExp, evaluator);
            var resultExpression = evaluator.GetFilterExpression<bool>(expression1[0]);
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
            Evaluator evaluator = new Evaluator((Type)null, null);
            var expression1 = TextParser.Evaluate(stringExp, evaluator);
            var resultExpression = evaluator.GetFilterExpression<bool>(expression1[0]);
            var actual = resultExpression.Compile()();

            Assert.Equal(x <= y, actual);
        }

        [Theory]
        [InlineData("true", true)]
        [InlineData("false", false)]
        public void Evaluate_WhenExpressionIsBoolNot_ShouldWork(string exp, bool value)
        {
            string stringExp = $"!{exp}";
            Evaluator evaluator = new Evaluator((Type)null, null);
            var expression1 = TextParser.Evaluate(stringExp, evaluator);
            var resultExpression = evaluator.GetFilterExpression<bool>(expression1[0]);
            var actual = resultExpression.Compile()();

            Assert.Equal(!value, actual);
        }

        [Theory]
        [InlineData(true, 5, 7)]
        [InlineData(false, 8, 3)]
        public void Evaluate_WhenExpressionIsCondition_ShouldWork(bool value, int ifTrue, int ifFalse)
        {
            string stringExp = $"{value.ToString().ToLower()} ? {ifTrue} : {ifFalse}";
            Evaluator evaluator = new Evaluator((Type)null, null);
            var expression1 = TextParser.Evaluate(stringExp, evaluator);
            var resultExpression = evaluator.GetFilterExpression<int>(expression1[0]);
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
            Evaluator evaluator = new Evaluator((Type)null, null);
            var expression1 = TextParser.Evaluate(stringExp, evaluator);
            var resultExpression = evaluator.GetFilterExpression<string>(expression1[0]);
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
            Evaluator evaluator = new Evaluator((Type)null, null);
            var expression1 = TextParser.Evaluate(stringExp, evaluator);
            var resultExpression = evaluator.GetFilterExpression<bool>(expression1[0]);
            var actual = resultExpression.Compile()();

            Assert.Equal(y.Contains(x), actual);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(3)]
        [InlineData(5)]
        [InlineData(232)]
        public void Evaluate_WhenExpressionIsInArrayInt_ShouldWork(int x)
        {
            string stringExp = $"{x} in Numbers";
            Evaluator evaluator = new Evaluator(typeof(MyList), null);
            var expression1 = TextParser.Evaluate(stringExp, evaluator);
            var resultExpression = evaluator.GetFilterExpression<MyList, bool>(expression1[0]);
            var actual = resultExpression.Compile()(Data.Collection);

            Assert.Equal(Data.Collection.Numbers.Contains(x), actual);
        }

        [Theory]
        [InlineData("Jan")]
        [InlineData("3")]
        public void Evaluate_WhenExpressionIsInListString_ShouldWork(string x)
        {
            string stringExp = $"'{x}' in Months";
            Evaluator evaluator = new Evaluator(typeof(MyList), null);
            var expression1 = TextParser.Evaluate(stringExp, evaluator);
            var resultExpression = evaluator.GetFilterExpression<MyList, bool>(expression1[0]);
            var actual = resultExpression.Compile()(Data.Collection);

            Assert.Equal(Data.Collection.Months.Contains(x), actual);
        }

        [Theory]
        [InlineData(Status.Active)]
        [InlineData(Status.Running)]
        public void Evaluate_WhenExpressionIsInListEnum_ShouldWork(Status x)
        {
            string stringExp = $"'{x}' in Statuses";
            Evaluator evaluator = new Evaluator(typeof(MyList), null);
            var expression1 = TextParser.Evaluate(stringExp, evaluator);
            var resultExpression = evaluator.GetFilterExpression<MyList, bool>(expression1[0]);
            var actual = resultExpression.Compile()(Data.Collection);

            Assert.Equal(Data.Collection.Statuses.Contains(x), actual);
        }

        [Fact]
        public void Evaluate_WhenExpressionIsComplexPrimitiveExpression_ShouldWork()
        {
            string stringExp = $"(5 > 3 && 2 <= 4 || 1 != 1 ) && 2 + 4 > 3 && 's' in 'cro' + 's'";
            Evaluator evaluator = new Evaluator((Type)null, null);
            var expression1 = TextParser.Evaluate(stringExp, evaluator);
            var resultExpression = evaluator.GetFilterExpression<bool>(expression1[0]);
            var actual = resultExpression.Compile()();

            Assert.Equal((5 > 3 && 2 <= 4 || 1 != 1) && 2 + 4 > 3 &&  ("cro" + "s").Contains("s"), actual);
        }


        [Fact]
        public void Evaluate_WhenExpressionIsNewSimpleExpression_ShouldWork()
        {
            string stringExp = "new { FirstName , LastName }";
            Evaluator evaluator = new Evaluator(typeof(Person), null);
            var expression1 = TextParser.Evaluate(stringExp, evaluator);
            var resultExpression = evaluator.GetFilterExpression<Person, object>(expression1[0]);
            var actual = resultExpression.Compile()(Data.Persons[0]);
            var a = new { Data.Persons[0].FirstName, Data.Persons[0].LastName }.ToString();
            Assert.Equal(a, actual.ToString());
        }

        [Fact]
        public void Evaluate_WhenExpressionIsNewSimpleExpressionWithAs_ShouldWork()
        {
            string stringExp = "new { FirstName as 'First' , LastName as 'Last' }";
            Evaluator evaluator = new Evaluator(typeof(Person), null);
            var expression1 = TextParser.Evaluate(stringExp, evaluator);
            var resultExpression = evaluator.GetFilterExpression<Person, object>(expression1[0]);
            var actual = resultExpression.Compile()(Data.Persons[0]);
            var a = new { First = Data.Persons[0].FirstName, Last = Data.Persons[0].LastName }.ToString();
            Assert.Equal(a, actual.ToString());
        }

        [Fact]
        public void Evaluate_WhenExpressionIsNewExpressionWithDotAndAs_ShouldWork()
        {
            string stringExp = "new { Account.Name as 'Name' , Account.Balance as 'Balance' }";
            Evaluator evaluator = new Evaluator(typeof(Person), null);
            var expression1 = TextParser.Evaluate(stringExp, evaluator);
            var resultExpression = evaluator.GetFilterExpression<Person, object>(expression1[0]);
            var actual = resultExpression.Compile()(Data.Persons[0]);
            var a = new { Name = Data.Persons[0].Account.Name, Balance = Data.Persons[0].Account.Balance }.ToString();
            Assert.Equal(a, actual.ToString());
        }

        [Fact(Skip = "Assignment Not Supported yet")]
        public void Evaluate_WhenExpressionIsNewExpressionWithDotAndAssign_ShouldWork()
        {
            string stringExp = "new { Name = Account.Name , Bala = Account.Balance  }";
            Evaluator evaluator = new Evaluator(typeof(Person), null);
            var expression1 = TextParser.Evaluate(stringExp, evaluator);
            var resultExpression = evaluator.GetFilterExpression<Person, object>(expression1[0]);
            var actual = resultExpression.Compile()(Data.Persons[0]);
            var a = new { Name = Data.Persons[0].Account.Name, Bala = Data.Persons[0].Account.Balance }.ToString();
            Assert.Equal(a, actual.ToString());
        }

        [Fact]
        public void Evaluate_WhenExpressionIsNewExpressionWithDot_ShouldWork()
        {
            string stringExp = "new { Account.Name , Account.Balance }";
            Evaluator evaluator = new Evaluator(typeof(Person), null);
            var expression1 = TextParser.Evaluate(stringExp, evaluator);
            var resultExpression = evaluator.GetFilterExpression<Person, object>(expression1[0]);
            var actual = resultExpression.Compile()(Data.Persons[0]);
            var a = new { AccountName =Data.Persons[0].Account.Name, AccountBalance =Data.Persons[0].Account.Balance }.ToString();
            Assert.Equal(a, actual.ToString());
        }


        [Fact]
        public void Evaluate_WhenExpressionIsNewExpressionWithOperators_ShouldWork()
        {
            string stringExp = "new { FirstName + LastName as 'FullName' , Account.Balance + 10 as 'Balance' }";
            Evaluator evaluator = new Evaluator(typeof(Person), null);
            var expression1 = TextParser.Evaluate(stringExp, evaluator);
            var resultExpression = evaluator.GetFilterExpression<Person, object>(expression1[0]);
            var actual = resultExpression.Compile()(Data.Persons[0]);
            var a = new { FullName = Data.Persons[0].FirstName + Data.Persons[0].LastName, Balance = Data.Persons[0].Account.Balance + 10 }.ToString();
            Assert.Equal(a, actual.ToString());
        }

        [Fact]
        public void Evaluate_WhenExpressionIsNewExpressionWithNew_ShouldWork()
        {
            string stringExp = "new { new { Account.Name, Account.Balance } as 'Bank' }";
            Evaluator evaluator = new Evaluator(typeof(Person), null);
            var expression1 = TextParser.Evaluate(stringExp, evaluator);
            var resultExpression = evaluator.GetFilterExpression<Person, object>(expression1[0]);
            var actual = resultExpression.Compile()(Data.Persons[0]);
            var expected = JsonSerializer.Serialize(new { Bank = new { AccountName = Data.Persons[0].Account.Name, AccountBalance = Data.Persons[0].Account.Balance } });
            var actualJson = JsonSerializer.Serialize(actual);
            Assert.Equal(expected, actualJson);
        }


        [Fact]
        public void Evaluate_WhenExpressionIsNewExpressionWithSelect_ShouldWork()
        {
            string stringExp = "new { FirstName , Children.Select(new {Name , Gender}).ToList() as Kids}";
            Evaluator evaluator = new Evaluator(typeof(Person), null);
            var expression1 = TextParser.Evaluate(stringExp, evaluator);
            var resultExpression = evaluator.GetFilterExpression<Person, object>(expression1[0]);
            var actual = resultExpression.Compile()(Data.Persons[0]);
            var expected = JsonSerializer.Serialize(new { Data.Persons[0].FirstName, Kids = Data.Persons[0].Children.Select(c => new { c.Name, c.Gender }) }) ;
            var actualJson = JsonSerializer.Serialize(actual);
            Assert.Equal(expected, actualJson);
        }

        [Fact]
        public void Evaluate_WhenExpressionIsQueryable_ShouldWork()
        {
            string stringExp = "this.Select( new { FirstName , Children.Select(new {Name , Gender}).ToList() as Kids} )";
            Evaluator evaluator = new Evaluator(typeof(IQueryable<Person>), null);
            var expression1 = TextParser.Evaluate(stringExp, evaluator);
            var resultExpression = evaluator.GetFilterExpression<IQueryable<Person>, object>(expression1[0]);
            var actual = resultExpression.Compile()(Data.Persons.AsQueryable());
            var expected = JsonSerializer.Serialize(Data.Persons.AsQueryable().Select(  p=> new {p.FirstName, Kids = p.Children.Select(c => new { c.Name, c.Gender }) }));
            var actualJson = JsonSerializer.Serialize(actual);
            Assert.Equal(expected, actualJson);
        }

    }
}
