using Punica.Linq.Dynamic.abstractions;

namespace Punica.Linq.Dynamic
{
    public struct Alias
    {
        public TokenId Id { get; set; }
        public IToken Token { get; set; }
    }
}
