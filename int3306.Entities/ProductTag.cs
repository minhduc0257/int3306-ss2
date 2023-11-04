using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using int3306.Entities.Shared;

namespace int3306.Entities
{
    [Table("product_tags")]
    public class ProductTag : IBaseEntity
    {
        [Key]
        public int Id { get; set; }
        public int Status { get; set; }
        
        [Column("name")]
        public string Name { get; set; }
    }
}