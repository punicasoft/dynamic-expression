namespace Punica.Tests.Utils
{

    internal interface IA
    {

    }

    internal interface IB
    {
    }

    internal interface IB<T> : IB
    {
        T Id { get; }
    }

    internal interface IC<T>
    {
        T Id { get; }
    }

    internal class A : IA
    {

    }
    internal class Aa : A
    {

    }

    internal class B : IB<Guid>
    {
        public Guid Id { get; }
    }

    internal class C : IC<B>
    {
        public B Id { get; }
    }

    internal class Ca : IC<Aa>
    {
        public Aa Id { get; }
    }

    internal class D : C
    {
    }

    internal class Collections
    {
        public int Count { get; }

        public string Name { get; }

        public int[] Days { get; set; }

        public List<string> DayNames { get; set; }

        public IList<string> Weeks { get; set; }

        public ICollection<string> WeekDays { get; set; }

        public IEnumerable<A> Items { get; set; }

        public IQueryable<int> DaysQueryable { get; set; }

    }

    
}
