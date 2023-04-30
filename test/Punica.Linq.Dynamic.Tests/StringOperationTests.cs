using Punica.Linq.Dynamic.Tests.Utils;

namespace Punica.Linq.Dynamic.Tests
{
    public class StringOperationTests : ExpressionTestsBase
    {

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
    }
}
