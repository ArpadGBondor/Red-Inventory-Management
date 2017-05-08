using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EntityLayer
{
    [Table("ProdCategory")]
    public class ProductCategoryEntity
    {
        [Key]
        public int Id { get; set; }

        public string Category { get; set; }
    }
}