using System;
using System.Data.Linq.Mapping;

namespace Red_software
{
    [Table(Name = "Customers")]
    public class Customer
    {
        public Customer() { }
        public Customer(string _id, string _city)
        { CustomerID = _id; City = _city; }
        [Column(IsPrimaryKey = true)]
        public string CustomerID;
        [Column]
        public string City;
    }
}
