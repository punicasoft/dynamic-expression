

# Dynamic Expression
A Simple text parse which convert expression from a string to Lambda Expression.

### Installing Dynamic Expression

You should install [Punica.Linq.Dynamic](https://www.nuget.org/packages/Punica.Linq.Dynamic/)

    Install-Package Punica.Linq.Dynamic
    
Or via the .NET Core command line interface:

    dotnet add package Punica.Linq.Dynamic

Either commands, from Package Manager Console or .NET Core CLI, will download and install Punica.Linq.Dynamic and all required dependencies.

## How to use

Common Methods Used in Samples
```csharp
public Expression<Func<TResult>> GetExpression<TResult>(string expression)
{
    var eval = new Evaluator();
    return eval.Parse<Expression<Func<TResult>>>(expression);
}

public Expression<Func<T1, TResult>> GetExpression<T1, TResult>(string expression)
{
    var eval = new Evaluator()
        .AddStartParameter(typeof(T1));
    return eval.Parse<Expression<Func<T1, TResult>>>(expression);
}

public LambdaExpression GetGeneralExpression<T1>(string expression)
{
    var eval = new Evaluator()
        .AddStartParameter(typeof(T1));
    return eval.Parse(expression);
}

```

Addition
```csharp
 string stringExp = "5 + 7";

 var resultExpression = GetExpression<int>(stringExp);
 var result = resultExpression.Compile()(); // 12

```
Contains

```csharp     
string stringExp = $"'rl' in 'hello world'";
var resultExpression = GetExpression<bool>(stringExp);
var result = resultExpression.Compile()(); // true
```

```csharp    
public class MyList
{    
    public List<Status> Statuses { get; set; }
}

var data = new MyList(){
     Statuses = new List<Status>()
     {
         Status.Active,
         Status.Inactive,
         Status.Online,
         Status.Paused
     }
}  
string expression= $"'Status.Active' in Statuses";

var resultExpression = GetExpression<MyList, bool>(expression);
Func<MyList, bool> func = resultExpression.Compile();

var result = func(data); // true
```

```csharp    
public class Person
{
   public Guid Id { get; set; }
   public string FirstName { get; set; }
   public string LastName { get; set; }
   public List<Child> Children { get; set; }
}
public class Child
{
   public string Name { get; set; }
   public string Gender { get; set; }
}

public class MyList
{    
    public List<Person> Persons { get; set; }
}

var data = new MyList(){
    Persons = new List<Person>(){
    --- Your Data --
    }
}  

string expression= $"Select(new{ FirstName,Children.Select(new{Name , Gender}).ToList() as 'Kids'}).Where(Kids.Any(Gender == 'Female'))";
var resultExpression = GetGeneralExpression<IQueryable<Person>>(stringExp);
var function = resultExpression.Compile();
var result = function(data.Persons.AsQueryable());

// data.Persons.AsQueryable() can be _context.Persons in EF Core DB context, in that case code equivalant to above text to code is

  _context.Persons..Select(x => new
    {
        FirstName = x.FirstName,
        Kids = x.Children.Select(c => new
        {
            c.Name,
            Gender = c.Gender
        }).ToList(),
    }).Where(p => p.Kids.Any(k => k.Gender == "Female"));
	
```

## Features

 - Support equality operators: **==, !=**
 - Support relational operators: **>,>=,<=, <**
 - Support Conditional operators **??, ?**
 - Arithmetic Operators: **+, - , / ,*, %** (Does not support negative numbers)
 - New expressions **new**
 - String Contains and Concatenation (Previously supported as well) 
 - Linq Methods **Linq methods. (WIP)**
 - Precedence using **(,)**
 - Lambda calls

## Limitation (To be Validated)

(With dynamic method support added in most should work as long as method can be inferred on runtime from the string and inputs outputs)
 - Linq methods are WIP (Enumerable methods are all added in haven't test out).
 - Not all string operation supported
 - Dates and GUID operation has very little support if they are inputs
 
 ## Improvements

 - Overall tokens detections
 - Better number type support
 - Most Linq limitation removed 
 - Dynamic method detection support compared to previous static definition but slower compared previous implementation.
 - Will interpret types based on expression, will fail if it can't determine the overload
 - Method chaining support.
 
 

| Method                         | Mean      | Error     | StdDev    | Gen0   | Gen1   | Allocated |
|------------------------------- |----------:|----------:|----------:|-------:|-------:|----------:|
| Two_Select_With_New_Expression | 18.931 μs | 0.3485 μs | 0.3260 μs | 2.8076 |      - |  23.25 KB |
| Select_With_New_Expression     |  8.746 μs | 0.0788 μs | 0.0737 μs | 1.1597 |      - |   9.86 KB |
| Advanced_Boolean_Expression    |  3.129 μs | 0.0532 μs | 0.0498 μs | 0.5035 | 0.0038 |   4.13 KB |
| Average_Int_With_Predicate     |  9.644 μs | 0.1402 μs | 0.1312 μs | 1.2512 |      - |  10.37 KB |

