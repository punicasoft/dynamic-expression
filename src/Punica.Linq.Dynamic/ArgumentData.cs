namespace Punica.Linq.Dynamic
{
    public struct ArgumentData
    {
        public bool IsFunction { get; init; }
        public int FunctionInputsCount { get; init; }
        public Type? FuncType { get; init; }
        public Type? Type { get; init; }

        public ArgumentData(bool isFunction, int functionInputsCount, Type? type)
        {
            IsFunction = isFunction;
            FunctionInputsCount = functionInputsCount;
            Type = type;

            if (IsFunction)
            {
                switch (FunctionInputsCount)
                {
                    case 0:
                        FuncType = typeof(Func<>);
                        break;
                    case 1:
                        FuncType = typeof(Func<,>);
                        break;
                    case 2:
                        FuncType = typeof(Func<,,>);
                        break;
                    case 3:
                        FuncType = typeof(Func<,,,>);
                        break;
                    case 4:
                        FuncType = typeof(Func<,,,,>);
                        break;
                    case 5:
                        FuncType = typeof(Func<,,,,,>);
                        break;
                    case 6:
                        FuncType = typeof(Func<,,,,,,>);
                        break;
                    case 7:
                        FuncType = typeof(Func<,,,,,,,>);
                        break;
                    case 8:
                        FuncType = typeof(Func<,,,,,,,,>);
                        break;
                    case 9:
                        FuncType = typeof(Func<,,,,,,,,,>);
                        break;
                    case 10:
                        FuncType = typeof(Func<,,,,,,,,,,>);
                        break;
                    case 11:
                        FuncType = typeof(Func<,,,,,,,,,,,>);
                        break;
                    case 12:
                        FuncType = typeof(Func<,,,,,,,,,,,,>);
                        break;
                    case 13:
                        FuncType = typeof(Func<,,,,,,,,,,,,,>);
                        break;
                    case 14:
                        FuncType = typeof(Func<,,,,,,,,,,,,,,>);
                        break;
                    case 15:
                        FuncType = typeof(Func<,,,,,,,,,,,,,,,>);
                        break;
                    case 16:
                        FuncType = typeof(Func<,,,,,,,,,,,,,,,,>);
                        break;
                    default:
                        throw new ArgumentException(
                            $"Invalid Number of args in Func delegate. Max support is 16 and but found {FunctionInputsCount}");
                }
            }
        }
    }
}
