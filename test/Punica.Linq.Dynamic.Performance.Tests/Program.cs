// See https://aka.ms/new-console-template for more information

using BenchmarkDotNet.Running;
using Punica.Linq.Dynamic.Performance.Tests;

Console.WriteLine("Hello, World!");


BenchmarkRunner.Run<DynamicTests>();

//var test = new DynamicTests();
//test.Average_Int_With_Predicate();
