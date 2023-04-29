using System.Linq.Expressions;
using Punica.Linq.Dynamic.Tokens.abstractions;

namespace Punica.Linq.Dynamic
{
    public static class ExpressionEvaluator
    {
        /// <summary>
        /// Shunting Yard algorithm
        /// </summary>
        /// <param name="tokens"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public static Expression Evaluate(List<IToken> tokens)
        {
            //If there is no tokens
            if (tokens.Count == 0)
            {
                throw new ArgumentException("No tokens");
            }

            // Evaluate the expression using the shunting-yard algorithm
            Stack<IToken> outputQueue = new Stack<IToken>();
            Stack<IToken> operatorStack = new Stack<IToken>();

            for (var i = 0; i < tokens.Count; i++)
            {
                var token = tokens[i];

                switch (token.TokenType)
                {
                    case TokenType.Operator:

                        // Check if the '-' operator is a unary minus
                        if (token.ExpressionType == ExpressionType.Subtract && (i == 0 || tokens[i - 1].TokenType == TokenType.Operator || tokens[i - 1].TokenType == TokenType.OpenParen))
                        {
                            token = TokenCache.NegateToken;
                            tokens[i] = token;
                        }

                        // Pop operators from the stack until a lower-precedence or left-associative operator is found
                        while (operatorStack.Count > 0 &&
                               (token.Precedence < operatorStack.Peek().Precedence ||
                                token.Precedence == operatorStack.Peek().Precedence && token.IsLeftAssociative))
                        {
                            outputQueue.Push(operatorStack.Pop());
                        }

                        // Push the new operator onto the stack
                        operatorStack.Push(token);
                        break;

                    case TokenType.OpenParen:
                        // Push left parentheses onto the stack
                        operatorStack.Push(token);
                        break;

                    case TokenType.CloseParen:
                        // Pop operators from the stack and add them to the output queue until a left parenthesis is found
                        while (operatorStack.Count > 0 && operatorStack.Peek().TokenType != TokenType.OpenParen)
                        {
                            outputQueue.Push(operatorStack.Pop());
                        }

                        // If a left parenthesis was not found, the expression is invalid
                        if (operatorStack.Count == 0)
                        {
                            throw new ArgumentException("Mismatched parentheses");
                        }

                        // Pop the left parenthesis from the stack
                        operatorStack.Pop();
                        break;

                    case TokenType.Unknown:
                        throw new ArgumentException("Should not be available");
                        break;

                    default:
                        // Push operands onto the output queue
                        outputQueue.Push(token);
                        break;
                }
            }

            // Pop any remaining operators from the stack and add them to the output queue
            while (operatorStack.Count > 0)
            {
                if (operatorStack.Peek().TokenType == TokenType.OpenParen)
                {
                    throw new ArgumentException("Mismatched parentheses");
                }

                outputQueue.Push(operatorStack.Pop());
            }


            // Evaluate Operators

            Stack<Expression> evaluationStack = new Stack<Expression>();

            var reverse = outputQueue.Reverse();


            foreach (var token in reverse)
            {
                if (token is IOperation ot)
                {
                    evaluationStack.Push(ot.Evaluate(evaluationStack)); 
                }
                else if (token is IExpression vt)
                {
                    evaluationStack.Push(vt.Evaluate());
                }
                else
                {
                    throw new ArgumentException("Invalid token");
                }

            }

            outputQueue.Clear();

            return evaluationStack.Pop();
        }

        public static (Expression left, Expression right) ConvertExpressions(Expression left, Expression right)
        {
            if (left.Type == right.Type)
            {
                return (left, right);
            }

            if (left.Type == typeof(double) || right.Type == typeof(double))
            {
                return (Convert(left, typeof(double)), Convert(right, typeof(double)));
            }

            if (left.Type == typeof(float) || right.Type == typeof(float))
            {
                return (Convert(left, typeof(float)), Convert(right, typeof(float)));
            }

            if (left.Type == typeof(long) || right.Type == typeof(long))
            {
                return (Convert(left, typeof(long)), Convert(right, typeof(long)));
            }

            if (left.Type == typeof(int) || right.Type == typeof(int))
            {
                return (Convert(left, typeof(int)), Convert(right, typeof(int)));
            }

            if (left.Type == typeof(short) || right.Type == typeof(short))
            {
                return (Convert(left, typeof(short)), Convert(right, typeof(short)));
            }

            if (left.Type == typeof(byte) || right.Type == typeof(byte))
            {
                return (Convert(left, typeof(byte)), Convert(right, typeof(byte)));
            }

            throw new InvalidOperationException("Cannot add types " + left.Type + " and " + right.Type);

        }

        private static Expression Convert(Expression expression, Type type)
        {
            if (expression.Type == type)
            {
                return expression;
            }

            return Expression.Convert(expression, type);
        }
    }
}
