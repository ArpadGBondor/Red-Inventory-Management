using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer
{

    [Table(Name = "Partners")]
    public class PartnerEntity : IComparable<PartnerEntity>
    {
        [Column(IsPrimaryKey = true, IsDbGenerated = true, AutoSync = AutoSync.OnInsert)]
        public int Id { get; set; }

        [Column]
        public string Code { get; set; }

        [Column]
        public string Name { get; set; }

        [Column]
        public bool Customer { get; set; }

        [Column]
        public bool Dealer { get; set; }

        [Column]
        public string Address { get; set; }

        [Column]
        public string AccountNumber { get; set; }

        [Column]
        public string Phone { get; set; }

        [Column]
        public string Email { get; set; }

        public int CompareTo(PartnerEntity other)
        {
            return Name.CompareTo(other.Name);
        }
    }
}
