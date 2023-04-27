# Dynamic Expression
A Simple text parse which convert expression from a string to Lambda Expression. Features and tolerance for expression are limited since this an early stage of work.

## How to use

Addition
```csharp
string stringExp = "5 + 7";
Evaluator evaluator = new Evaluator((Type)null, null);
var expression1 = TextParser.Evaluate(stringExp, evaluator);
var resultExpression = evaluator.GetFilterExpression<int>(expression1[0]);
Func<bool> func = resultExpression.Compile();
var result = func(); // 12
```
Contains

```csharp     
string stringExp = $"'rl' in 'hello world'";
Evaluator evaluator = new Evaluator((Type)null, null);
var expression1 = TextParser.Evaluate(stringExp, evaluator);
var resultExpression = evaluator.GetFilterExpression<bool>(expression1[0]);
Func<bool> func = resultExpression.Compile();
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
Evaluator evaluator = new Evaluator(typeof(MyList), null);
var expression1 = TextParser.Evaluate(stringExp, evaluator);
var resultExpression = evaluator.GetFilterExpression<MyList, bool>(expression1[0]);
var actual = resultExpression.Compile()(Data.Collection);
Func<bool> func = resultExpression.Compile();
var result = func(data);
```

## Features

 - Support equality operators: **==, !=**
 - Support relational operators: **>,>=,<=, <**
 - Arithmetic Operators: **+, - , / ,*, %** (Does not support negative numbers)
 - New expressions **new**
 - Linq Methods **Any, Contain, Select**
 - Precedence using **(,)**

## Limitation

 - Need spaces between tokens
 - Not every Linq method supported. Only what mentioned above
 - Doesn't works with negative numbers 
 - Lambada not supported

|                         Method |      Mean |     Error |    StdDev |   Gen0 |   Gen1 | Allocated |
|------------------------------- |----------:|----------:|----------:|-------:|-------:|----------:|
| Two_Select_With_New_Expression | 13.469 us | 0.2661 us | 0.3065 us | 1.7395 | 0.0153 |  14.22 KB |
|     Select_With_New_Expression |  6.309 us | 0.0895 us | 0.0794 us | 0.7553 |      - |    6.2 KB |
|    Advanced_Boolean_Expression |  3.714 us | 0.0303 us | 0.0284 us | 0.6409 | 0.0038 |   5.26 KB |

