

# Dynamic Expression
A Simple text parse which convert expression from a string to Lambda Expression. Features and tolerance for expression are limited since this an early stage of work.

## How to use

Addition
```csharp
string expression= "5 + 7";
var rootToken = Tokenizer.Evaluate(new TokenContext(expression));
var resultExpression = rootToken.Evaluate();

var exp = (Expression<Func<int>>)resultExpression;
Func<bool> func = exp.Compile();

var result = func(); // 12

```
Contains

```csharp     
string stringExp = $"'rl' in 'hello world'";
var rootToken = Tokenizer.Evaluate(new TokenContext(stringExp ));
var resultExpression = rootToken.Evaluate();

var exp = (Expression<Func<bool>>)resultExpression;
Func<bool> func = exp.Compile();

var result = func(); // true

```

```csharp    
public class MyList
{
    public int[] Numbers { get; set; }
    public string[] Words { get; set; }
    public List<string> Months { get; set; }
    public List<Status> Statuses { get; set; }
}

var data= new MyList(){
---
}  
string expression= $"'Status.Active' in Statuses";

var context = new TokenContext(expression);
context.AddStartParameter(typeof(T1));
var rootToken = Tokenizer.Evaluate(context);
var resultExpression = rootToken.Evaluate();

var exp = (Expression<Func<MyList, bool>>)resultExpression;
Func<bool> func = exp.Compile();

var result = func(data);
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

var data= new MyList(){
---
}  

string expression= $"Select(new{ FirstName,Children.Select(new{Name , Gender}).ToList() as 'Kids'}).Where(Kids.Any(Gender == 'Female'))";

var context = new TokenContext(expression);
context.AddStartParameter(typeof(MyList));
var rootToken = Tokenizer.Evaluate(context);

var resultExpression = rootToken.Evaluate();
var exp = (LambdaExpression)resultExpression;
var result = exp.Compile().DynamicInvoke(Data.Persons.AsQueryable());	
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

## Limitation

 - Linq methods are WIP (Enumerable methods are all added in haven't test out).
 - Not all string operation supported
 - Dates and GUID operation has very little support if they are inputs
 - Doesn't works with negative numbers very well
 
 ## Improvements

 - Overall tokens detections
 - Better number type support
 - Most Linq limitation removed 
 - Dynamic method detection support compared to previous static definition but slower compared previous implementation.
 - Will interpret types based on expression, will fail if it can't determine the overload
 - Method chaining support.
 
 

|                         Method |      Mean |     Error |    StdDev |   Gen0 |   Gen1 | Allocated |
|------------------------------- |----------:|----------:|----------:|-------:|-------:|----------:|
| Two_Select_With_New_Expression | 19.070 us | 0.3691 us | 0.4394 us | 2.0752 | 0.0305 |  17.05 KB |
|     Select_With_New_Expression |  9.224 us | 0.1651 us | 0.1545 us | 0.9918 |      - |   8.15 KB |
|    Advanced_Boolean_Expression |  4.147 us | 0.0784 us | 0.0872 us | 0.4883 |      - |   4.02 KB |
|     Average_Int_With_Predicate | 11.084 us | 0.2194 us | 0.2052 us | 1.0223 |      - |   8.47 KB |

