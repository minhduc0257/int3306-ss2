using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using int3306.Entities.Shared;

namespace int3306.Entities
{
    [Table("product_tags")]
    public class ProductTag : IBaseEntity
    {
#pragma warning disable CS8618
        [Key]
        public int Id { get; set; }
        public int Status { get; set; } = 1;
        
        [Column("name")]
        public string Name { get; set; }
#pragma warning restore CS8618
    }
}