using System.Diagnostics;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text;
using Punica.Linq.Dynamic.Abstractions;
using Punica.Linq.Dynamic.Expressions;

namespace Punica.Linq.Dynamic
{
    public class Tokenizer
    {
        internal static RootToken Evaluate(Parser context)
        {
            return new RootToken(context.MethodContext.GetParameters(), Tokenize(context));
        }

        internal static List<IToken> Tokenize(Parser context)
        {
            List<IToken> tokens = new List<IToken>();

            while (context.CurrentToken.Id != TokenId.End)
            {
                var token = GetToken(context);

                if (token != null && context.CurrentToken.Id != TokenId.Lambda)
                {
                    tokens.Add(token);
                }

            }

            return tokens;
        }

        private static IToken? GetToken(Parser context)
        {
            var token = context.CurrentToken;

            switch (token.Id)
            {
                // Person.Name or Person() or Person.Select() or  Person.Select().ToList()
                // @person.Name or @person.Select() or  @person.Select().ToList()\
                // new { Name} 
                // Select( a=> a.Name ) or Select( a=> a ) or Select( a => new {a.Name})
                // GroupBy( @pets, p => p, u => u.Pets, (a,b) => new {a.Name, b.Owner} )
                // new Person {Name }
                case TokenId.Identifier:
                case TokenId.Variable:
                {
                    context.NextToken();
                    var nextToken = context.CurrentToken;

                    if (nextToken.Id == TokenId.Dot)
                    {
                        // Handle property or method access chain
                        var expression = ParseVariableExpression(context, token); //handle initial variable or parameter access.
                        var memberExpression = ParseMemberAccessExpression(context, expression); //handle chaining
                        return memberExpression;
                    }
                    else if (nextToken.Id == TokenId.LeftParenthesis)
                    {
                        // Handle method access chain

                        if (token.Id == TokenId.Variable)
                        {
                            throw new Exception("Expected identifier before method call");
                        }

                        // Handle method call
                        var methodCallExpression = ParseMethodCallExpression(context, context.MethodContext.AddOrGetParameter(), token);
                        var memberExpression = ParseMemberAccessExpression(context, methodCallExpression); //handle chaining

                        return memberExpression;
                    }
                    else if (nextToken.Id == TokenId.Lambda)
                    {
                        // Handle lambda expression
                        return new PropertyToken(null, token.Text);

                    }
                    else
                    {
                        // Handle variable
                        return ParseVariableExpression(context, token);
                    }
                }
                case TokenId.New:
                {
                    context.NextToken();
                    var nextToken = context.CurrentToken;

                    switch (nextToken.Id)
                    {
                        case TokenId.LeftCurlyParen:
                        {
                            // Handle new call new {Field1, Field2}
                            var methodCallExpression = ParseNewCallExpression(context, new NewAnonymousToken());
                            var memberExpression = ParseMemberAccessExpression(context, methodCallExpression);  //handle chaining
                            return memberExpression;
                        }
                        case TokenId.Identifier:
                        {
                            // Handle new call
                            var typeName = ParseTypeName(context); //check until next token
                            var type = context.FindType(typeName);

                            switch (context.CurrentToken.Id)
                            {
                                case TokenId.LeftCurlyParen:
                                {
                                    // Handle new call new ClassA{Field1, Field2}
                                    var methodCallExpression = ParseNewCallExpression(context, new NewToken(type));
                                    var memberExpression = ParseMemberAccessExpression(context, methodCallExpression);  //handle chaining
                                    return memberExpression;
                                }
                                case TokenId.LeftParenthesis:
                                {
                                    var constructorArguments = ParseArguments(context);

                                    // Handle new call new ClassA(Field1, Field2)
                                    INewExpression newConstructorToken = new NewConstructorToken(type, constructorArguments);

                                    if (context.CurrentToken.Id == TokenId.LeftCurlyParen)
                                    {
                                        // Handle new call new ClassA(Field1, Field2){Field3 = Field3}
                                        newConstructorToken = ParseNewCallExpression(context, newConstructorToken);
                                    }
                                    var memberExpression = ParseMemberAccessExpression(context, newConstructorToken);  //handle chaining
                                    return memberExpression;
                                }
                                default:
                                    throw new Exception("Expected curly bracket after new call with type");
                            }

                            break;
                        }
                        default:
                            throw new Exception("Expected curly bracket after new call");
                    }

                    break;
                }
                case TokenId.Unknown:
                    throw new Exception("Unknown token");
                default:
                    context.NextToken();
                    return token.ParsedToken;
            }
        }



        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static IExpression? ParseMemberAccessExpression(Parser context, IExpression expression)
        {
            while (context.CurrentToken.Id is TokenId.Dot)
            {
                context.NextToken(); // consume the dot

                var memberToken = context.CurrentToken;

                if (context.CurrentToken.Id != TokenId.Identifier)
                {
                    throw new Exception("Expected identifier after dot");
                }

                context.NextToken(); // consume the member

                if (context.CurrentToken.Id == TokenId.LeftParenthesis)
                {
                    expression = ParseMethodCallExpression(context, expression, memberToken);
                }
                else
                {
                    expression = new PropertyToken(expression, memberToken.Text);
                }
            }

            return expression;

        }

        //var expressionString = "Person.Select(p => new { p.Name, Age = DateTime.Now.Year - p.BirthYear }).Where(x => x.Age > 30)";
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static MethodToken ParseMethodCallExpression(Parser context, IExpression targetExpression, Token methodToken)
        {
            context.MethodContext.NextDepth();
            var method = new MethodToken(methodToken.Text, targetExpression);

            var argument = new Argument();
            context.NextToken(); // consume parenthesis
            int depth = 1;

            while (context.CurrentToken.Id != TokenId.End && depth > 0)
            {
                switch (context.CurrentToken.Id)
                {
                    // Math.Add((3*5)+1,4)
                    case TokenId.LeftParenthesis:
                        depth++;
                        argument.AddToken(context.CurrentToken.ParsedToken!);
                        context.NextToken();
                        break;
                    case TokenId.RightParenthesis:
                        depth--;
                        if (argument.Tokens.Count > 0)
                        {
                            if (depth == 0)  // only add if there any tokens and we are at the end of the method
                            {
                                var lambdas = context.MethodContext.MoveToNextArgument();
                                argument.AddParameters(lambdas);
                                method.AddArgument(argument);
                            }
                            else
                            {
                                argument.AddToken(context.CurrentToken.ParsedToken!);
                            }
                        }
                        context.NextToken();
                        break;

                    case TokenId.Comma:
                        // (person, petCollection) => new { OwnerName = person.FirstName, Pets = petCollection.Select(pet => pet.Name) }
                        if (argument.IsFirstOpenParenthesis())
                        {
                            argument.AddToken(context.CurrentToken.ParsedToken!);
                        }
                        else
                        {   // Add(FirstName, LastName) }
                            var lambdas = context.MethodContext.MoveToNextArgument();
                            argument.AddParameters(lambdas);
                            method.AddArgument(argument);
                            argument = new Argument();
                        }
                        context.NextToken();
                        break;
                    case TokenId.Lambda:
                        var paraNames = argument.ProcessLambda();
                        context.MethodContext.AddParameters(paraNames);
                        context.NextToken();// consume lambda
                        break;
                    default:
                        {
                            var token = GetToken(context);

                            if (token != null)
                            {
                                argument.AddToken(token);
                            }

                            break;
                        }
                }
            }

            if (depth != 0)
            {
                throw new ArgumentException("Input contains mismatched parentheses.");
            }

            context.MethodContext.PreviousDepth();

            return method;
        }


        private static List<Argument> ParseArguments(Parser context)
        {
            context.MethodContext.NextDepth();

            var arguments = new List<Argument>();
            var argument = new Argument();
            context.NextToken(); // consume parenthesis
            int depth = 1;

            while (context.CurrentToken.Id != TokenId.End && depth > 0)
            {
                switch (context.CurrentToken.Id)
                {
                    // Math.Add((3*5)+1,4)
                    case TokenId.LeftParenthesis:
                        depth++;
                        argument.AddToken(context.CurrentToken.ParsedToken!);
                        context.NextToken();
                        break;
                    case TokenId.RightParenthesis:
                        depth--;
                        if (argument.Tokens.Count > 0)
                        {
                            if (depth == 0)  // only add if there any tokens and we are at the end of the method
                            {
                                var lambdas = context.MethodContext.MoveToNextArgument();
                                argument.AddParameters(lambdas);
                                arguments.Add(argument);
                            }
                            else
                            {
                                argument.AddToken(context.CurrentToken.ParsedToken!);
                            }
                        }
                        context.NextToken();
                        break;

                    case TokenId.Comma:
                        // (person, petCollection) => new { OwnerName = person.FirstName, Pets = petCollection.Select(pet => pet.Name) }
                        if (argument.IsFirstOpenParenthesis())
                        {
                            argument.AddToken(context.CurrentToken.ParsedToken!);
                        }
                        else
                        {   // Add(FirstName, LastName) }
                            var lambdas = context.MethodContext.MoveToNextArgument();
                            argument.AddParameters(lambdas);
                            arguments.Add(argument);
                            argument = new Argument();
                        }
                        context.NextToken();
                        break;
                    case TokenId.Lambda:
                        var paraNames = argument.ProcessLambda();
                        context.MethodContext.AddParameters(paraNames);
                        context.NextToken();// consume lambda
                        break;
                    default:
                        {
                            var token = GetToken(context);

                            if (token != null)
                            {
                                argument.AddToken(token);
                            }

                            break;
                        }
                }
            }

            if (depth != 0)
            {
                throw new ArgumentException("Input contains mismatched parentheses.");
            }

            context.MethodContext.PreviousDepth();

            return arguments;
        }


        // (person, petCollection) => new CustomObject{ OwnerName = person.FirstName, Pets = petCollection.Select(pet => pet.Name) }
        // person => new Person{ person.FirstName }
        // new Person{ person.FirstName }
        // new { person.FirstName }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static INewExpression ParseNewCallExpression(Parser context, INewExpression newToken)
        {
            var argument = new Argument();
            context.NextToken(); // consume parenthesis
            int depth = 1;

            while (context.CurrentToken.Id != TokenId.End && depth > 0)
            {
                switch (context.CurrentToken.Id)
                {
                    case TokenId.LeftCurlyParen:
                        throw new UnreachableException("Invalid Case Check algorithm"); // possibly not a option
                    case TokenId.RightCurlyParen:
                        depth--;
                        if (argument.Tokens.Count > 0)
                        {
                            newToken.AddArgument(argument);
                        }
                        context.NextToken();
                        break;
                    case TokenId.Comma:
                        newToken.AddArgument(argument);
                        context.NextToken();
                        argument = new Argument();
                        break;
                    default:
                        {
                            var token = GetToken(context);

                            if (token != null)
                            {
                                argument.AddToken(token);
                            }

                            break;
                        }
                }
            }

            if (depth != 0)
            {
                throw new ArgumentException("Input contains mismatched parentheses.");
            }

            return newToken;
        }


        private static string ParseTypeName(Parser context)
        {
            var sb = new StringBuilder();
            sb.Append(context.CurrentToken.Text);

            context.NextToken(); //consume current type or namespace

            while (context.CurrentToken.Id is TokenId.Dot)
            {
                context.NextToken(); // consume the dot

                var memberToken = context.CurrentToken;

                if (context.CurrentToken.Id != TokenId.Identifier)
                {
                    throw new Exception("Expected identifier after dot");
                }

                sb.Append(".");
                sb.Append(context.CurrentToken.Text);

                context.NextToken(); // consume the member
            }

            return sb.ToString();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static IExpression ParseVariableExpression(Parser context, Token token)
        {
            if (token.Id == TokenId.Variable)
            {
                if (token.ParsedToken == null)
                {
                    throw new ArgumentNullException($"Missing Variable {token.Text}");
                }

                return (IExpression)token.ParsedToken;
            }
            else if (token.Id == TokenId.Identifier)
            {
                // FirstName
                // person => person.FirstName
                var parameter = context.MethodContext.GetParameter(token.Text);
                if (parameter != null)
                {
                    return parameter;
                }
                else
                {
                    return new PropertyToken(context.MethodContext.AddOrGetParameter(), token.Text);
                }
            }
            else
            {
                throw new Exception($"Unexpected token {token.Id} when parsing variable expression");
            }

            //TODO: use  var variableExpression = Expression.Variable(variableType, variableName);
        }


    }
}
