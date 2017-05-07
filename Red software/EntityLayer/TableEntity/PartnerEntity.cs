using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer
{

    [Table("Partners")]
    public class PartnerEntity : IComparable<PartnerEntity>
    {
        [Key]
        public int Id { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }

        public bool Customer { get; set; }

        public bool Dealer { get; set; }

        public string Address { get; set; }

        public string AccountNumber { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }

        public int CompareTo(PartnerEntity other)
        {
            var compare1 = Name ?? string.Empty;
            var compare2 = other.Name ?? string.Empty;
            return compare1.CompareTo(compare2);
        }
    }
}
