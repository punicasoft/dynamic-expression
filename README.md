
# Dynamic Expression
A Simple text parse which convert expression from a string to Lambda Expression. Features and tolerance for expression are limited since this an early stage of work.

## How to use

Addition
```csharp
string stringExp = "5 + 7";
var rootToken = Tokenizer.Evaluate(new TokenContext(stringExp ));
var resultExpression = rootToken.Evaluate(null);
var exp = (Expression<Func<int>>)resultExpression;
Func<bool> func = exp.Compile();
var result = func(); // 12

```
Contains

```csharp     
string stringExp = $"'rl' in 'hello world'";
var rootToken = Tokenizer.Evaluate(new TokenContext(stringExp ));
var resultExpression = rootToken.Evaluate(null);
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

string stringExp = $"'Status.Active' in Statuses";
var methodContext = new MethodContext(Expression.Parameter(typeof(MyList), "arg"));
var rootToken = Tokenizer.Evaluate(new TokenContext(stringExp, methodContext));
var resultExpression = rootToken.Evaluate(null);
var exp = (Expression<Func<MyList, bool>>)resultExpression;
Func<bool> func = exp.Compile();
var result = func(data);
```

## Features

 - Support equality operators: **==, !=**
 - Support relational operators: **>,>=,<=, <**
 - Arithmetic Operators: **+, - , / ,*, %** (Does not support negative numbers)
 - New expressions **new**
 - String Contains and Concatenation (Previously supported as well) 
 - Linq Methods **Most common Linq methods. Exclusion are linq methods with multi lambda such as GroupJoin,   	SelectMany and certain overloads**
 - Precedence using **(,)**

## Limitation

 - Some tokens has better detections compared to before but some may still need spaces between tokens
 - Some Linq method aren't supported.
 - Not all string operation supported
 - Dates and GUID operation has very little support
 - Doesn't works with negative numbers very well
 - Lambada not supported
 
 ## Improvements

 - Some tokens has better detections compared to before but some may still need spaces between tokens
 - Much More Linq method support
 - Method chaining support but not perfect
 - Internal algorithm change support future changes
 
 

|                         Method |      Mean |     Error |    StdDev |   Gen0 | Allocated |
|------------------------------- |----------:|----------:|----------:|-------:|----------:|
| Two_Select_With_New_Expression | 19.018 us | 0.3765 us | 0.4482 us | 2.1057 |  17.24 KB |
|     Select_With_New_Expression |  7.272 us | 0.0857 us | 0.0802 us | 0.8087 |   6.63 KB |
|    Advanced_Boolean_Expression |  3.970 us | 0.0739 us | 0.0655 us | 0.4883 |   4.01 KB |

