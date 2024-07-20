using Punica.Linq.Dynamic.Tests.Utils;

namespace Punica.Linq.Dynamic.Tests
{
    public class ConversionTests :ExpressionTestsBase
    {

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


        [Theory]
        [InlineData("Name == null ")]
        [InlineData("null == Name ")]
        public void Evaluate_WhenExpressionIsHasNullParameter_ShouldWork(string expression)
        {
            var pet = new Pet
            {
                Name = null
            };
            
            var resultExpression = GetExpression<Pet, bool>(expression);
            var actual = resultExpression.Compile()(pet);
            Assert.True(actual);
        }
    }
}
