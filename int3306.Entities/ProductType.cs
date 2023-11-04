using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using int3306.Entities.Shared;

namespace int3306.Entities
{
    [Table("product_type")]
    public class ProductType : IBaseEntity
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("status")]
        public int Status { get; set; }

        [Column("name")]
        public string Name { get; set; }
        
        [Column("description")]
        public string Description { get; set; }
    }
}