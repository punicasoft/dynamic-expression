using System.Linq.Expressions;
using Punica.Linq.Dynamic.Tokens;
using Punica.Linq.Dynamic.Tokens.abstractions;

namespace Punica.Linq.Dynamic
{
    /// <summary>
    /// Arguments are external inputs to expressions
    /// VariablesInstance is similar to arguments but used as a parameter for expressions so that sql conversion use it as a parameter //TODO: check whether you can merge Arguments and VariablesInstance together?
    /// <see cref="MethodToken"/> for rest of details. but there could be lambda expression at <see cref="RootToken"/> level
    /// </summary>
    public class TokenContext
    {
       // public string Text { get; }

        private readonly string _txt;
        private int _pos;

        public bool CanRead => _pos < _txt.Length;
        public char Current { get; private set; }

        public int CurrentPosition => _pos;

        /// <summary>
        /// Used to pass parameters for expressions @param where param is a property/field of VariablesInstance
        /// </summary>
        public Expression? VariablesInstance { get; }

        /// <summary>
        /// Arguments
        /// </summary>
        public IExpression[] Argumentss { get; }

        public MethodContext MethodContext { get; }


        public TokenContext(string text, MethodContext? methodContext = null, Expression? variablesInstance = null, params IExpression[] args)
        {
           // Text = text;
            _txt = text;
            SetPosition(0);
            VariablesInstance = variablesInstance;
            Argumentss = args;
            MethodContext = methodContext ?? new MethodContext();
        }

        public void SetPosition(int pos)
        {
            _pos = pos;
            Current = CanRead ? _txt[_pos] : '\0';
        }

        public char PeekNext()
        {
            return _pos + 1 < _txt.Length ? _txt[_pos + 1] : '\0';
        }

        private void NextChar()
        {
            if (CanRead)
            {
                _pos++;
            }

            Current = CanRead ? _txt[_pos] : '\0';
        }

        public void NextToken(bool skipWhiteSpaces = true)
        {
            NextChar();

            while (char.IsWhiteSpace(Current) && skipWhiteSpaces)
            {
                NextChar();
            }
        }

        public string Substring(int startIndex, int length)
        {
            return _txt.Substring(startIndex, length);
        }

        public bool Match(string text)
        {
            int j;
            int k = _pos;
            for (j = 0; j < text.Length; j++)
            {
                k = _pos + j;

                if (k >= _txt.Length || _txt[k] != text[j])
                {
                    return false;
                }
            }

            //_pos = k;
            //NextChar();
            return true;
        }

    }
}
