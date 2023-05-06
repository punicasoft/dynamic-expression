using System.Diagnostics;
using System.Linq.Expressions;
using Punica.Linq.Dynamic.Expressions;

namespace Punica.Linq.Dynamic
{
    public class MethodContext
    {
        private const string _arg = "_arg";
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
                var para = _parameters[key].FirstOrDefault(p => p.Name == name);

                if (para != null)
                {
                    return para; //if there is lambda this should populated before this method get invoked
                }
            }



            return null;
        }

        public ParameterToken? AddOrGetParameter()
        {
            if (_depth == 0)
            {
                return _parameters[0].First(p => p.Name == _arg); //should 
            }

            if (!_parameters.ContainsKey(_depth))
            {
                return AddParameter(new ParameterToken(_arg + _depth));
            }
            else
            {
                return GetParameter();
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

        public ParameterToken[] MoveToNextArgument()
        {
            if (_parameters.ContainsKey(_depth))
            {
                var parameterTokens = _parameters[_depth].ToArray();
                _parameters[_depth].Clear();
                return parameterTokens;
            }

            return Array.Empty<ParameterToken>();
        }

        public ParameterToken AddParameter(ParameterToken parameter)
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

            return parameter;
        }

        /// <summary>
        /// Call this to clear temp arguments added during lambda variable detection
        /// Call after detecting => in a argument. 
        /// </summary>
        /// <exception cref="UnreachableException"></exception>
        public void ClearDepthArgs()
        {
            if (_parameters.ContainsKey(_depth))
            {
                if (_parameters[_depth].Count > 1)
                {
                    throw new UnreachableException(
                        "There can not be any other arg than first arg added during lambda variable detection");
                }
                _parameters[_depth].Clear();
            }

        }

        public void AddParameters(IReadOnlyList<string> paraNames)
        {
            ClearDepthArgs();
            foreach (var name in paraNames)
            {
                AddParameter(new ParameterToken(name));
            }
        }
    }
}
