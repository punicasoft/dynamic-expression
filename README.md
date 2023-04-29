

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
| Two_Select_With_New_Expression | 32.223 us | 0.4045 us | 0.3378 us | 4.0283 | 0.0610 |  33.03 KB |
|     Select_With_New_Expression | 15.513 us | 0.2949 us | 0.2897 us | 1.8616 |      - |  15.42 KB |
|    Advanced_Boolean_Expression |  4.677 us | 0.0913 us | 0.1280 us | 0.5035 |      - |   4.14 KB |
|     Average_Int_With_Predicate | 29.340 us | 0.2213 us | 0.2070 us | 3.8757 | 0.0305 |   31.7 KB |

