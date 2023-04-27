using System.Linq.Expressions;
using Punica.Linq.Dynamic.Tokens;
using Punica.Linq.Dynamic.Tokens.abstractions;

namespace Punica.Linq.Dynamic
{
    public class MethodContext
    {
        private readonly Dictionary<int, List<ParameterToken>> _parameters = new Dictionary<int, List<ParameterToken>>();
        private int _depth = 0;

        public int Depth => _depth;

        public MethodContext()
        {
        }

        public MethodContext(ParameterExpression parameter) : this(new ParameterToken(parameter))
        {
        }

        public MethodContext(ParameterToken parameter)
        {
            AddParameter(parameter);
        }

        public ParameterToken? GetParameter(string name)
        {
            foreach (var key in _parameters.Keys)
            {
                var para = _parameters[_depth].FirstOrDefault(p => p.Name == name);

                if (para != null)
                {
                    return para;
                }
            }

            return null;
        }

        public ParameterToken? GetParameter()
        {
            return _parameters[_depth].FirstOrDefault();
        }

        public List<ParameterToken> GetParameters()
        {
            if (_parameters.ContainsKey(_depth))
            {
                return _parameters[_depth];
            }

            return new List<ParameterToken>();
        }

        public void NextDepth()
        {
            _depth++;
        }

        public void PreviousDepth()
        {
            _parameters.Remove(_depth);
            _depth--;
        }

        public void MoveToNextArgument()
        {
            if (_parameters.ContainsKey(_depth))
            {
                _parameters[_depth].Clear();
            }
        }

        public void AddParameter(IExpression expression)
        {
            AddParameter(new ParameterToken(expression, "arg" + _depth));
        }

        public void AddParameter(ParameterToken parameter)
        {
            foreach (var key in _parameters.Keys)
            {
                if (key == _depth)
                {
                    continue;
                }

                var para = _parameters[key].FirstOrDefault(p => p.Name == parameter.Name);

                if (para != null)
                {
                    throw new Exception("Parameter with name " + parameter.Name + " already exists");
                }
            }

            if (!_parameters.ContainsKey(_depth))
            {
                _parameters[_depth] = new List<ParameterToken>();
            }

            _parameters[_depth].Add(parameter);
        }
    }
}
