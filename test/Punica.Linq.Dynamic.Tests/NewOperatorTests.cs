using System.Linq.Expressions;
using System.Text.Json;
using Punica.Linq.Dynamic.Tests.Utils;

namespace Punica.Linq.Dynamic.Tests
{
    public class NewOperatorTests : ExpressionTestsBase
    {
        [Theory]
        [InlineData("new { FirstName , LastName }")]
        [InlineData("new{FirstName,LastName}")]
        public void Evaluate_WhenExpressionIsNewSimpleExpression_ShouldWork(string stringExp)
        {
            var resultExpression = GetGeneralExpression<Person>(stringExp);
            var actual = resultExpression.Compile().DynamicInvoke(Data.Persons[0]);
            var a = new { Data.Persons[0].FirstName, Data.Persons[0].LastName }.ToString();
            Assert.Equal(a, actual.ToString());
        }

        [Fact]
        public void Evaluate_WhenExpressionIsNewSimpleExpressionWithAs_ShouldWork()
        {
            string stringExp = "new { FirstName as 'First' , LastName as 'Last' }";
            var resultExpression = GetGeneralExpression<Person>(stringExp);
            var actual = resultExpression.Compile().DynamicInvoke(Data.Persons[0]);
            var a = new { First = Data.Persons[0].FirstName, Last = Data.Persons[0].LastName }.ToString();
            Assert.Equal(a, actual.ToString());
        }

        [Fact]
        public void Evaluate_WhenExpressionIsNewExpressionWithDotAndAs_ShouldWork()
        {
            string stringExp = "new { Account.Name as 'Name' , Account.Balance as 'Balance' }";
            var resultExpression = GetGeneralExpression<Person>(stringExp);
            var actual = resultExpression.Compile().DynamicInvoke(Data.Persons[0]);
            var a = new { Name = Data.Persons[0].Account.Name, Balance = Data.Persons[0].Account.Balance }.ToString();
            Assert.Equal(a, actual.ToString());
        }

        [Fact(Skip = "Assignment Not Supported yet")]
        public void Evaluate_WhenExpressionIsNewExpressionWithDotAndAssign_ShouldWork()
        {
            string stringExp = "new { Name = Account.Name , Bala = Account.Balance  }";
            var resultExpression = GetGeneralExpression<Person>(stringExp);
            var actual = resultExpression.Compile().DynamicInvoke(Data.Persons[0]);
            var a = new { Name = Data.Persons[0].Account.Name, Bala = Data.Persons[0].Account.Balance }.ToString();
            Assert.Equal(a, actual.ToString());
        }

        [Fact]
        public void Evaluate_WhenExpressionIsNewExpressionWithDot_ShouldWork()
        {
            string stringExp = "new { Account.Name , Account.Balance }";
            var resultExpression = GetGeneralExpression<Person>(stringExp);
            var actual = resultExpression.Compile().DynamicInvoke(Data.Persons[0]);
            var a = new { AccountName = Data.Persons[0].Account.Name, AccountBalance = Data.Persons[0].Account.Balance }.ToString();
            Assert.Equal(a, actual.ToString());
        }


        [Fact]
        public void Evaluate_WhenExpressionIsNewExpressionWithOperators_ShouldWork()
        {
            string stringExp = "new { FirstName + LastName as 'FullName' , Account.Balance + 10 as 'Balance' }";
            var resultExpression = GetGeneralExpression<Person>(stringExp);
            var actual = resultExpression.Compile().DynamicInvoke(Data.Persons[0]);
            var a = new { FullName = Data.Persons[0].FirstName + Data.Persons[0].LastName, Balance = Data.Persons[0].Account.Balance + 10 }.ToString();
            Assert.Equal(a, actual.ToString());
        }

        [Fact]
        public void Evaluate_WhenExpressionIsNewExpressionWithNew_ShouldWork()
        {
            string stringExp = "new { new { Account.Name, Account.Balance } as 'Bank' }";
            var resultExpression = GetGeneralExpression<Person>(stringExp);
            var actual = resultExpression.Compile().DynamicInvoke(Data.Persons[0]);
            var expected = JsonSerializer.Serialize(new { Bank = new { AccountName = Data.Persons[0].Account.Name, AccountBalance = Data.Persons[0].Account.Balance } });
            var actualJson = JsonSerializer.Serialize(actual);
            Assert.Equal(expected, actualJson);
        }


        [Fact]
        public void Evaluate_WhenExpressionIsNewExpressionWithSelect_ShouldWork()
        {
            string stringExp = "new{FirstName,Children.Select(new{Name,Gender}).ToList() as 'Kids'}";
            var resultExpression = GetGeneralExpression<Person>(stringExp);
            var actual = resultExpression.Compile().DynamicInvoke(Data.Persons[0]);

            var expected = JsonSerializer.Serialize(new { Data.Persons[0].FirstName, Kids = Data.Persons[0].Children.Select(c => new { c.Name, c.Gender }).ToList() });
            var actualJson = JsonSerializer.Serialize(actual);
            Assert.Equal(expected, actualJson);
        }


        [Theory]
        [InlineData("person => new { person.FirstName , person.LastName }")]
        [InlineData("person=>new{person.FirstName,person.LastName}")]
        [InlineData("new { person.FirstName , person.LastName }")]
        public void Evaluate_WhenExpressionIsNewExpressionWithLambda_ShouldWork(string stringExp)
        {
            var resultExpression = GetGeneralExpression(stringExp, null, Expression.Parameter(typeof(Person), "person"));
            var actual = resultExpression.Compile().DynamicInvoke(Data.Persons[0]);
            var a = new { Data.Persons[0].FirstName, Data.Persons[0].LastName }.ToString();
            Assert.Equal(a, actual.ToString());
        }

        [Theory]
        [InlineData("new { @person.FirstName , @person.LastName }")]
        [InlineData("new{@person.FirstName,@person.LastName}")]
        public void Evaluate_WhenExpressionIsNewExpressionWithParameters_ShouldWork(string stringExp)
        {
            var resultExpression = GetGeneralExpression(stringExp, Expression.Constant(Data.MyVariables));
            var actual = resultExpression.Compile().DynamicInvoke();
            var a = new { personFirstName = Data.Persons[0].FirstName, personLastName = Data.Persons[0].LastName }.ToString();
            Assert.Equal(a, actual.ToString());
        }


        [Theory]
        [InlineData("person => new Person{ person.FirstName , person.LastName }")]
        [InlineData("person=>new Person{person.FirstName,person.LastName}")]
        [InlineData("new Person{ person.FirstName , person.LastName }")]
        public void Evaluate_WhenExpressionIsNewInstanceExpressionWithLambda_ShouldWork(string stringExp)
        {
            var resultExpression = GetGeneralExpression(stringExp, null, Expression.Parameter(typeof(Person), "person"));
            var actual = resultExpression.Compile().DynamicInvoke(Data.Persons[0]);
            var a = new Person() { FirstName = Data.Persons[0].FirstName, LastName = Data.Persons[0].LastName }.ToString();
            Assert.Equal(a, actual.ToString());
        }


        [Theory]
        [InlineData("person => new Utils.Person{ person.FirstName , person.LastName }")]
        [InlineData("person => new Punica.Linq.Dynamic.Tests.Utils.Person{ person.FirstName , person.LastName }")]
        public void Evaluate_WhenExpressionIsNewInstanceExpressionWithNameSpace_ShouldWork(string stringExp)
        {
            var resultExpression = GetGeneralExpression(stringExp, null, Expression.Parameter(typeof(Person), "person"));
            var actual = resultExpression.Compile().DynamicInvoke(Data.Persons[0]);
            var a = new Person() { FirstName = Data.Persons[0].FirstName, LastName = Data.Persons[0].LastName }.ToString();
            Assert.Equal(a, actual.ToString());
        }

        [Theory]
        [InlineData("person => new Utils2.Person{ person.FirstName , person.LastName }", "Unsupported type Utils2.Person")]
        [InlineData("person => new Punica.Dynamic.Tests.Utils.Person{ person.FirstName , person.LastName }", "Unsupported type Punica.Dynamic.Tests.Utils.Person")]
        public void Evaluate_WhenExpressionIsNewInstanceExpressionWithInvalidNameSpace_ShouldThrowError(string stringExp, string error)
        {
            var exception = Assert.Throws<ArgumentException>(() => GetGeneralExpression(stringExp, null, Expression.Parameter(typeof(Person), "person")));
            Assert.Equal(error, exception.Message);
        }


        [Theory]
        [InlineData("person => new User(person.FirstName , person.LastName)")]
        [InlineData("person=>new User(person.FirstName){person.LastName}")]
        public void Evaluate_WhenExpressionIsNewConstructorExpressionWithLambda_ShouldWork(string stringExp)
        {
            var eval = new Evaluator()
                .AddParameter<Person>("person")
                .AddType<User>();

            var resultExpression = eval.Parse(stringExp);
            var actual = resultExpression.Compile().DynamicInvoke(Data.Persons[0]);
            var a = new User(Data.Persons[0].FirstName, Data.Persons[0].LastName).ToString();
            Assert.Equal(a, actual.ToString());
        }

    }
}
