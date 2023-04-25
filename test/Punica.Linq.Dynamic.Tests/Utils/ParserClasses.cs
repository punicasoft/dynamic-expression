using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Punica.Tests.Utils
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

        public static MyList Collection = new MyList()
        {
            Numbers = new[] { 5, 232, 23, 1, 4, 34534 },
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

    public class MyList
    {
        public int[] Numbers { get; set; }
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
