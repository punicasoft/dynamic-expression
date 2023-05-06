using Punica.Linq.Dynamic.Tests.Utils;
using System.Linq.Expressions;
using System;

namespace Punica.Linq.Dynamic.Tests
{
    public class StringOperationTests : ExpressionTestsBase
    {

        [Theory]
        [InlineData("hello", "world")]
        [InlineData("hel", "lo")]
        [InlineData("hell", "o")]
        public void Evaluate_WhenExpressionIsAdd_ShouldWork(string x, string y)
        {

            string expression = $"'{x}' + '{y}'";
            var resultExpression = GetExpression<string>(expression);
            var actual = resultExpression.Compile()();

            Assert.Equal(x + y, actual);
        }


        [Theory]
        [InlineData("hello world", "world")]
        [InlineData("hello", "lo")]
        [InlineData("hell", "o")]
        public void Evaluate_WhenExpressionIsEndsWith_ShouldWork(string x, string y)
        {
            string expression = $"@x.EndsWith('{y}')";

            var func = new Evaluator()
                .AddIdentifier("x", Expression.Constant(x))
                .Evaluate<Func<bool>>(expression);

            var actual = func();

            Assert.Equal(x.EndsWith(y) , actual);
        }

        [Theory]
        [InlineData("hello world", "hello")]
        [InlineData("hello", "he")]
        [InlineData("hell", "o")]
        public void Evaluate_WhenExpressionIsStartsWith_ShouldWork(string x, string y)
        {
            string expression = $"@x.StartsWith('{y}')";

            var func = new Evaluator()
                .AddIdentifier("x", Expression.Constant(x))
                .Evaluate<Func<bool>>(expression);

            var actual = func();

            Assert.Equal(x.StartsWith(y), actual);
        }

        [Theory]
        [InlineData("hello world", "llo")]
        [InlineData("hello", "el")]
        [InlineData("hell", "o")]
        public void Evaluate_WhenExpressionIsContains_ShouldWork(string x, string y)
        {
            string expression = $"@x.Contains('{y}')";

            var func = new Evaluator()
                .AddIdentifier("x", Expression.Constant(x))
                .Evaluate<Func<bool>>(expression);

            var actual = func();

            Assert.Equal(x.Contains(y), actual);
        }

        [Theory]
        [InlineData("  hello world  ")]
        [InlineData("  hello  ")]
        [InlineData(" hell")]
        public void Evaluate_WhenExpressionIsTrim_ShouldWork(string x)
        {
            string expression = $"@x.Trim()";

            var func = new Evaluator()
                .AddIdentifier("x", Expression.Constant(x))
                .Evaluate<Func<string>>(expression);

            var actual = func();

            Assert.Equal(x.Trim(), actual);
        }

        [Fact]
        public void Evaluate_WhenExpressionIsSubstring_ShouldWork()
        {
            var x = "Retrieves a substring from this instance";
            string expression = $"@x.Substring(10)";

            var func = new Evaluator()
                .AddIdentifier("x", Expression.Constant(x))
                .Evaluate<Func<string>>(expression);

            var actual = func();

            Assert.Equal(x.Substring(10), actual);
        }

        [Fact]
        public void Evaluate_WhenExpressionIsSubstring2_ShouldWork()
        {
            var x = "Retrieves a substring from this instance";
            string expression = $"@x.Substring(5 , 10)";

            var func = new Evaluator()
                .AddIdentifier("x", Expression.Constant(x))
                .Evaluate<Func<string>>(expression);

            var actual = func();

            Assert.Equal(x.Substring(5, 10), actual);
        }
    }
}
