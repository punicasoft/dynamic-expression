using Punica.Linq.Dynamic.Tests.Utils;

namespace Punica.Linq.Dynamic.Tests
{
    public class OperatorAliasAndDetectionTests : ExpressionTestsBase
    {

        [Theory]
        [InlineData(5, 7, " - ")]
        [InlineData(5, 7, "-")]
        [InlineData(7, 5, " sub ")]
        [InlineData(7, 5, "sub ")]
        [InlineData(7, 5, "sub", Skip = "Not supported as sub5 can be identifier as well")]
        public void Evaluate_WhenOperator_IsSubtractOrAlias_ShouldWork(int x, int y, string op)
        {
            string stringExp = $"{x}{op}{y}";

            var resultExpression = GetExpression<int>(stringExp);
            var actual = resultExpression.Compile()();

            Assert.Equal(x - y, actual);
        }


        [Theory]
        [InlineData(5.3, 7.1, " + ")]
        [InlineData(5.3, 7.1, "+")]
        [InlineData(5.3, 7.1, " add ")]
        [InlineData(5.3, 7.1, "add ")]
        [InlineData(7, 5, "add", Skip = "Not supported as add5 can be identifier as well")]
        public void Evaluate_WhenOperator_IsAddOrAlias_ShouldWork(double x, double y, string op)
        {
            string stringExp = $"{x}{op}{y}";
            var resultExpression = GetExpression<double>(stringExp);
            var actual = resultExpression.Compile()();

            Assert.Equal(x + y, actual);
        }

        [Theory]
        [InlineData(5, 7, " * ")]
        [InlineData(5, 7, "*")]
        [InlineData(-5, 7, " mul ")]
        [InlineData(5, -3, "mul ")]
        [InlineData(5.3, -3, "mul ", Skip = "Not supported as 5.3m detect as a number")]
        [InlineData(7, 5, "mul", Skip = "Not supported as mul5 can be identifier as well")]
        public void Evaluate_WhenOperator_IsMultiplyOrAlias_ShouldWork(double x, int y, string op)
        {

            string stringExp = $"{x}{op}{y}";
            var resultExpression = GetExpression<int>(stringExp);
            var actual = resultExpression.Compile()();

            Assert.Equal(x * y, actual);
        }

        [Theory]
        [InlineData(5.3, 7.1, " / ")]
        [InlineData(5.3, 7.1, "/")]
        [InlineData(5.3, 7.1, " div ")]
        [InlineData(5, 7.1, " div ")]
        [InlineData(5.3, 7.1, "div ", Skip = "Not supported as 5.3d detect as a number")]
        [InlineData(7, 5, "div", Skip = "Not supported as div5 can be identifier as well")]
        public void Evaluate_WhenOperator_IsDivideOrAlias_ShouldWork(double x, double y, string op)
        {
            string stringExp = $"{x}{op}{y}";
            var resultExpression = GetExpression<double>(stringExp);
            var actual = resultExpression.Compile()();
            Assert.Equal(x / y, actual);
        }

        [Theory]
        [InlineData(5, 7, " % ")]
        [InlineData(5, 7, "%")]
        [InlineData(5, 7, " mod ")]
        [InlineData(5.3, 7, "mod ", Skip = "Not supported as 5.3m detect as a number")]
        [InlineData(7, 5, "mod", Skip = "Not supported as mod5 can be identifier as well")]
        public void Evaluate_WhenOperator_IsModuloOrAlias_ShouldWork(double x, int y, string op)
        {
            string stringExp = $"{x}{op}{y}";
            var resultExpression = GetExpression<int>(stringExp);
            var actual = resultExpression.Compile()();
            Assert.Equal(x % y, actual);
        }

        [Theory]
        [InlineData(true, false, " && ")]
        [InlineData(true, false, "&&")]
        [InlineData(true, false, " and ")]
        [InlineData(true, false, "and ", Skip = "true,and identifiers detect as one trueand")]
        [InlineData(true, false, "and", Skip = "Not supported as and5 can be identifier as well")]
        public void Evaluate_WhenOperator_IsAndOrAlias_ShouldWork(bool x, bool y, string op)
        {
            string stringExp = $"{x.ToString().ToLower()}{op}{y.ToString().ToLower()}";
            var resultExpression = GetExpression<bool>(stringExp);
            var actual = resultExpression.Compile()();
            Assert.Equal(x && y, actual);
        }

        [Theory]
        [InlineData(true, false, " || ")]
        [InlineData(true, false, "||")]
        [InlineData(true, false, " or ")]
        [InlineData(true, false, "or ", Skip = "true,and identifiers detect as one trueand")]
        [InlineData(true, false, "or", Skip = "Not supported as and5 can be identifier as well")]
        public void Evaluate_WhenOperator_IsOrOrAlias_ShouldWork(bool x, bool y, string op)
        {
            string stringExp = $"{x.ToString().ToLower()}{op}{y.ToString().ToLower()}";
            var resultExpression = GetExpression<bool>(stringExp);
            var actual = resultExpression.Compile()();
            Assert.Equal(x || y, actual);
        }

        [Theory]
        [InlineData(true, " ! ")]
        [InlineData(true, "!")]
        [InlineData(true, " not ")]
        [InlineData(true, "not ", Skip = "true,not identifiers detect as one nottrue")]
        [InlineData(true, "not", Skip = "Not supported as and5 can be identifier as well")]
        public void Evaluate_WhenOperator_IsNotOrAlias_ShouldWork(bool x, string op)
        {
            string stringExp = $"{op}{x.ToString().ToLower()}";
            var resultExpression = GetExpression<bool>(stringExp);
            var actual = resultExpression.Compile()();
            Assert.Equal(!x, actual);
        }

        [Theory]
        [InlineData(5, 7, " > ")]
        [InlineData(5, 7, ">")]
        [InlineData(5, 7, " gt ")]
        [InlineData(5, 7, "gt ")]
        [InlineData(5, 7, "gt", Skip = "Not supported as gt5 can be identifier as well")]
        public void Evaluate_WhenOperator_IsGreaterThanOrAlias_ShouldWork(int x, int y, string op)
        {
            string stringExp = $"{x}{op}{y}";
            var resultExpression = GetExpression<bool>(stringExp);
            var actual = resultExpression.Compile()();
            Assert.Equal(x > y, actual);
        }

        [Theory]
        [InlineData(5, 7, " < ")]
        [InlineData(5, 7, "<")]
        [InlineData(5, 7, " lt ")]
        [InlineData(5.3, 7, "lt ")]
        [InlineData(5, 7, "lt", Skip = "Not supported as lt5 can be identifier as well")]
        public void Evaluate_WhenOperator_IsLessThanOrAlias_ShouldWork(double x, int y, string op)
        {
            string stringExp = $"{x}{op}{y}";
            var resultExpression = GetExpression<bool>(stringExp);
            var actual = resultExpression.Compile()();
            Assert.Equal(x < y, actual);
        }

        [Theory]
        [InlineData(5, 7, " >= ")]
        [InlineData(5, 7, ">=")]
        [InlineData(5, 7, " ge ")]
        [InlineData(5, 7, "ge ")]
        [InlineData(5, 7, "ge", Skip = "Not supported as ge5 can be identifier as well")]
        public void Evaluate_WhenOperator_IsGreaterThanOrEqualOrAlias_ShouldWork(int x, int y, string op)
        {
            string stringExp = $"{x}{op}{y}";
            var resultExpression = GetExpression<bool>(stringExp);
            var actual = resultExpression.Compile()();
            Assert.Equal(x >= y, actual);
        }

        [Theory]
        [InlineData(5, 7, " <= ")]
        [InlineData(5, 7, "<=")]
        [InlineData(5, 7, " le ")]
        [InlineData(5.45, 7, "le ")]
        [InlineData(5, 7, "le", Skip = "Not supported as le5 can be identifier as well")]
        public void Evaluate_WhenOperator_IsLessThanOrEqualOrAlias_ShouldWork(double x, int y, string op)
        {
            string stringExp = $"{x}{op}{y}";
            var resultExpression = GetExpression<bool>(stringExp);
            var actual = resultExpression.Compile()();
            Assert.Equal(x <= y, actual);
        }

        [Theory]
        [InlineData(5, 7, " == ")]
        [InlineData(5, 7, "==")]
        [InlineData(5, 7, " eq ")]
        [InlineData(5.3, 7, "eq ", Skip = "Not supported as 5.3e will be scientific notation")]
        [InlineData(5, 7, "eq", Skip = "Not supported as eq5 can be identifier as well")]
        public void Evaluate_WhenOperator_IsEqualOrAlias_ShouldWork(double x, int y, string op)
        {
            string stringExp = $"{x}{op}{y}";
            var resultExpression = GetExpression<bool>(stringExp);
            var actual = resultExpression.Compile()();
            Assert.Equal(x == y, actual);
        }

        [Theory]
        [InlineData(5, 7, " != ")]
        [InlineData(5, 7, "!=")]
        [InlineData(5, 7, " ne ")]
        [InlineData(5.3, 7, "ne ")]
        [InlineData(5, 7, "ne", Skip = "Not supported as ne5 can be identifier as well")]
        public void Evaluate_WhenOperator_IsNotEqualOrAlias_ShouldWork(double x, int y, string op)
        {
            string stringExp = $"{x}{op}{y}";
            var resultExpression = GetExpression<bool>(stringExp);
            var actual = resultExpression.Compile()();
            Assert.Equal(x != y, actual);
        }

        [Theory]
        [InlineData("he", "hello world", " in ")]
        [InlineData("he", "hello world", "in")]
        public void Evaluate_WhenOperator_IsAliasIn_ShouldWork(string x, string y, string op)
        {
            string stringExp = $"'{x}'{op}'{y}'";
            var resultExpression = GetExpression<bool>(stringExp);
            var actual = resultExpression.Compile()();
            Assert.Equal(y.Contains(x), actual);
        }

        [Theory]
        [InlineData(7, " in ")]
        [InlineData(7, "in ")]
        [InlineData(7, "in", Skip = "Not supported as inNumbers can be identifier as well")]
        public void Evaluate_WhenExpressionIsInArrayInt_ShouldWork(int x, string op)
        {
            string stringExp = $"{x}{op}Numbers";
            var resultExpression = GetExpression<MyList, bool>(stringExp);
            var actual = resultExpression.Compile()(Data.Collection);
            Assert.Equal(Data.Collection.Numbers.Contains(x), actual);
        }

        [Theory]
        [InlineData("Jan", " in ")]
        [InlineData("Jan", "in ")]
        [InlineData("Jan", "in", Skip = "Not supported as inMonths can be identifier as well")]
        public void Evaluate_WhenExpressionIsInListString_ShouldWork(string x, string op)
        {
            string stringExp = $"'{x}'{op}Months";
            var resultExpression = GetExpression<MyList, bool>(stringExp);
            var actual = resultExpression.Compile()(Data.Collection);

            Assert.Equal(Data.Collection.Months.Contains(x), actual);
        }

        [Theory]
        [InlineData(" as ")]
        [InlineData("as ", Skip = "Not supported as FirstNameas can be identifier as well")]
        [InlineData(" as")]
        [InlineData("as", Skip = "Not supported as FirstNameas can be identifier as well")]
        public void Evaluate_WhenOperator_IsAssignOrAlias_ShouldWork(string op)
        {
            string stringExp = $"new {{ FirstName{op}'Name' }}";
            var resultExpression = GetGeneralExpression<Person>(stringExp);
            var actual = resultExpression.Compile().DynamicInvoke(Data.Persons[0]);
            var a = new { Name = Data.Persons[0].FirstName }.ToString();
            Assert.Equal(a, actual.ToString());
        }

    }
}
