namespace Punica.Linq.Dynamic.Tests.Utils
{
    public static class Data
    {
        public static List<Person> Persons = new List<Person>()
        {
            new Person()
            {
                FirstName = "John",
                LastName = "Doe",
                Children = new List<Child>()
                {
                    new Child()
                    {
                        Name = "Child OneDoe",
                        Gender = "Male"
                    },
                    new Child()
                    {
                        Name = "Child TwoDoe",
                        Gender = "Female"
                    }
                },
                Account = new Account()
                {
                    Balance = 5000,
                    Name = "Premium"
                }
            },
            new Person()
            {
                FirstName = "Jane",
                LastName = "Doe",
                Children = new List<Child>()
                {
                    new Child()
                    {
                        Name = "Sweet OneDoe",
                        Gender = "Male"
                    },
                    new Child()
                    {
                        Name = "Sweet TwoDoe",
                        Gender = "Female"
                    }
                },
                Account = new Account()
                {
                    Balance = 2000,
                    Name = "Standard"
                }
            }
        };

        public static List<Numbers> Num = new List<Numbers>()
        {
            new Numbers()
            {
                Marks = 5,
                Prices = 5.2m,
                Area = 5.2f,
                Length = 5.2d,
                LongNumbers = 5L,
                MarksN = 5,
                PricesN = 5.2m,
                AreaN = 5.2f,
                LengthN = 5.2d,
                LongNumbersN = 5L,

            },
            new Numbers()
            {
                Marks = 232,
                Prices = 232.3m,
                Area = 232.3f,
                Length = 232.3d,
                LongNumbers = 232L,
                MarksN = 232,
                PricesN = 232.3m,
                AreaN = 232.3f,
                LengthN = 232.3d,
                LongNumbersN = 232L,

            },
            new Numbers()
            {
                Marks = 34534,
                Prices = 34534.7m,
                Area = 34534.7f,
                Length = 34534.7d,
                LongNumbers = 34534L,
                MarksN = null,
                PricesN = null,
                AreaN = null,
                LengthN = null,
                LongNumbersN = null,
            }
        };


        public static MyList Collection = new MyList()
        {
            Numbers = new[] { 5, 232, 23, 1, 4, 34534 },
            Prices = new[] { 5.2m, 232.3m, 23.4m, 1.5m, 4.6m, 34534.7m },
            Area = new[] { 5.2f, 232.3f, 23.4f, 1.5f, 4.6f, 34534.7f },
            Length = new[] { 5.2d, 232.3d, 23.4d, 1.5d, 4.6d, 34534.7d },
            LongNumbers = new[] { 5L, 232L, 23L, 1L, 4L, 34534L },
            NumbersN = new int?[] { 5, 232, 23, 1, 4, 34534, null },
            PricesN = new decimal?[] { 5.2m, 232.3m, 23.4m, 1.5m, 4.6m, 34534.7m, null },
            AreaN = new float?[] { 5.2f, 232.3f, 23.4f, 1.5f, 4.6f, 34534.7f, null },
            LengthN = new double?[] { 5.2d, 232.3d, 23.4d, 1.5d, 4.6d, 34534.7d, null },
            LongNumbersN = new long?[] { 5L, 232L, 23L, 1L, 4L, 34534L, null },
            Words = new[] { "Hi", "girl", "world", "Today", "TREE" },
            Months = new List<string>() { "Jan", "Feb", "March", "April" },
            Statuses = new List<Status>()
            {
                Status.Active,
                Status.Inactive,
                Status.Online,
                Status.Paused
            }
        };


    }

    public class Numbers
    {
        public int Marks { get; set; }
        public decimal Prices { get; set; }
        public float Area { get; set; }
        public double Length { get; set; }
        public long LongNumbers { get; set; }
        public int? MarksN { get; set; }
        public decimal? PricesN { get; set; }
        public float? AreaN { get; set; }
        public double? LengthN { get; set; }
        public long? LongNumbersN { get; set; }
    }

    public class MyList
    {
        public int[] Numbers { get; set; }
        public decimal[] Prices { get; set; }
        public float[] Area { get; set; }
        public double[] Length { get; set; }
        public long[] LongNumbers { get; set; }
        public int?[] NumbersN { get; set; }
        public decimal?[] PricesN { get; set; }
        public float?[] AreaN { get; set; }
        public double?[] LengthN { get; set; }
        public long?[] LongNumbersN { get; set; }
        public string[] Words { get; set; }
        public List<string> Months { get; set; }
        public List<Status> Statuses { get; set; }
    }

    public class Person
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Description { get; set; }
        public DateTime Dob { get; set; }
        public int NoOfChildren { get; set; }
        public bool IsMarried { get; set; }
        public bool IsMale { get; set; }
        public Account Account { get; set; }

        public List<Child> Children { get; set; }
    }

    public class Account
    {
        public string Name { get; set; }
        public decimal Balance { get; set; }
    }

    public class Child
    {
        public string Name { get; set; }

        public string Gender { get; set; }
    }

    public class Pet
    {
        public string Name { get; set; }
        public Person Owner { get; set; }
    }

    class PetOwner
    {
        public string Name { get; set; }
        public List<string> Pets { get; set; }
    }

    public class JoinPara
    {
        public List<Pet> pets;

        public List<Person> persons;
    }

    public enum Status
    {
        Active,
        Inactive,
        Online,
        Running,
        Paused,
        Waiting,
        Processing
    }
}
