
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
 
 

|                         Method |      Mean |     Error |    StdDev |   Gen0 |   Gen1 | Allocated |
|------------------------------- |----------:|----------:|----------:|-------:|-------:|----------:|
| Two_Select_With_New_Expression | 32.223 us | 0.4045 us | 0.3378 us | 4.0283 | 0.0610 |  33.03 KB |
|     Select_With_New_Expression | 15.513 us | 0.2949 us | 0.2897 us | 1.8616 |      - |  15.42 KB |
|    Advanced_Boolean_Expression |  4.677 us | 0.0913 us | 0.1280 us | 0.5035 |      - |   4.14 KB |
|     Average_Int_With_Predicate | 29.340 us | 0.2213 us | 0.2070 us | 3.8757 | 0.0305 |   31.7 KB |

