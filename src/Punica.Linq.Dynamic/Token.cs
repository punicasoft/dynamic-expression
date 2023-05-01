using Punica.Linq.Dynamic.abstractions;

namespace Punica.Linq.Dynamic
{
    public struct Token
    {
        public TokenId Id { get; set; }
        public string Text { get; set; }
        public IToken? ParsedToken { get; set; }
    }
}
