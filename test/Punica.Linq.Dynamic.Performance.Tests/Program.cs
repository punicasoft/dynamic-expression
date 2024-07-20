// See https://aka.ms/new-console-template for more information

using BenchmarkDotNet.Running;
using Punica.Linq.Dynamic.Performance.Tests;


BenchmarkRunner.Run<DynamicTests>();
