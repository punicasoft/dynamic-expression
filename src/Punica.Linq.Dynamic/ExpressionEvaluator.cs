using System.Linq.Expressions;
using Punica.Linq.Dynamic.Abstractions;

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
        public static Expression Evaluate(IList<IToken> tokens)
        {
            if (tokens.Count == 0)
            {
                throw new ArgumentException("No tokens");
            }

            // Evaluate the expression using the shunting-yard algorithm
            LinkedList<IToken> outputQueue = new LinkedList<IToken>();
            Stack<IOperation> operatorStack = new Stack<IOperation>();

            for (var i = 0; i < tokens.Count; i++)
            {
                var token = tokens[i];

                switch (token.TokenType)
                {
                    case TokenType.Operator:

                        // Check if the '-'/'+' operator is a unary minus or unary plus
                        if (token.ExpressionType == ExpressionType.Subtract && (i == 0 || tokens[i - 1].TokenType == TokenType.Operator || tokens[i - 1].TokenType == TokenType.OpenParen))
                        {
                            token = TokenCache.NegateToken;
                            tokens[i] = token;
                        }
                        else if(token.ExpressionType == ExpressionType.Add && (i == 0 || tokens[i - 1].TokenType == TokenType.Operator || tokens[i - 1].TokenType == TokenType.OpenParen))
                        {
                            token = TokenCache.UnaryPlusToken;
                            tokens[i] = token;
                        }

                        var operation = (IOperation)token;

                        // Pop operators from the stack until a lower-precedence or left-associative operator is found
                        while (operatorStack.TryPeek(out var topOperator) &&
                               (operation.Precedence < topOperator.Precedence ||
                                operation.Precedence == topOperator.Precedence && operation.IsLeftAssociative))
                        {
                            outputQueue.AddLast(operatorStack.Pop());
                        }

                        // Push the new operator onto the stack
                        operatorStack.Push(operation);
                        break;

                    case TokenType.OpenParen:
                        // Push left parentheses onto the stack
                        operatorStack.Push((IOperation)token);
                        break;

                    case TokenType.CloseParen:
                        // Pop operators from the stack and add them to the output queue until a left parenthesis is found
                        while (operatorStack.TryPeek(out var op) && op.TokenType != TokenType.OpenParen)
                        {
                            outputQueue.AddLast(operatorStack.Pop());
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

                    default:
                        // Push operands onto the output queue
                        outputQueue.AddLast(token);
                        break;
                }
            }

            // Pop any remaining operators from the stack and add them to the output queue
            while (operatorStack.TryPop(out var remainingOperator))
            {
                if (remainingOperator.TokenType == TokenType.OpenParen)
                {
                    throw new ArgumentException("Mismatched parentheses");
                }

                outputQueue.AddLast(remainingOperator);
            }
            
            // Evaluate the expression using a reverse polish notation evaluator
            Stack<Expression> evaluationStack = new Stack<Expression>();

            foreach (var token in outputQueue)
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

            return evaluationStack.Pop();
        }

    }
}
