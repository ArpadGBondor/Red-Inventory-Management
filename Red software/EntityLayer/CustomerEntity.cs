using System;
using System.Data.Linq.Mapping;

namespace EntityLayer
{
    [Table(Name = "Customers")]
    public class CustomerEntity
    {
        public CustomerEntity() { }

        public CustomerEntity(string _id, string _city)
        { CustomerID = _id; City = _city; }

        [Column(IsPrimaryKey = true)]
        public string CustomerID { get; set; }

        [Column]
        public string City { get; set; }
    }
}
