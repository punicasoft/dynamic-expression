using Punica.Linq.Dynamic.Tests.Utils;

namespace Punica.Linq.Dynamic.Tests
{
    public class OperatorTests : ExpressionTestsBase
    {
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
        [InlineData(7, -5)]
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
        [InlineData(7.1, -8.3)]
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
        [InlineData(7, -3)]
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
        [InlineData(7.3, -5)]
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
        [InlineData(7, -11)]
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
        [InlineData(7.8, -3.2)]
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
        [InlineData(-3, -3)]
        [InlineData(-6, 6)]
        [InlineData(2, -2)]
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
        [InlineData(-6, 6)]
        [InlineData(2, -2)]
        [InlineData(-4, -4)]
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
        [InlineData(-5, -7)]
        [InlineData(-5, 2)]
        [InlineData(5, -7)]
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
        [InlineData(-5, -3)]
        [InlineData(-5, 2)]
        [InlineData(5, -7)]
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
        [InlineData(-5, -7)]
        [InlineData(-5, 2)]
        [InlineData(5, -7)]
        [InlineData(-3, -3)]
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
        [InlineData(-5, -3)]
        [InlineData(-5, 2)]
        [InlineData(5, -7)]
        [InlineData(-3, -3)]
        public void Evaluate_WhenExpressionIsLessThanEqual_ShouldWork(int x, int y)
        {
            string stringExp = $"{x} <= {y}";
            var resultExpression = GetExpression<bool>(stringExp);
            var actual = resultExpression.Compile()();

            Assert.Equal(x <= y, actual);
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
        [InlineData("true", true)]
        [InlineData("false", false)]
        public void Evaluate_WhenExpressionIsNot_ShouldWork(string exp, bool value)
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

        [Fact]
        public void Evaluate_WhenExpressionIsCondition_ShouldWork_2()
        {
            string stringExp = "IsMale?'Male':'Female'";
            var resultExpression = GetGeneralExpression<Person>(stringExp);
            var actual = resultExpression.Compile().DynamicInvoke(Data.Persons[0]);

            var expected = Data.Persons[0].IsMale ? "Male" : "Female";

            Assert.Equal(expected, actual);
        }

    }
}
