using System.ComponentModel.DataAnnotations.Schema;
using int3306.Entities.Shared;

namespace int3306.Entities
{
    [Table("product_variant")]
    public class ProductVariant : IBaseEntity
    {
        public int Id { get; set; }
        public int Status { get; set; }
        
        [Column("product_id")]
        public int ProductId { get; set; }

        public string Name { get; set; } = "";
        
        [ForeignKey(nameof(ProductId))]
        public virtual Product? Product { get; set; }
        
        public virtual List<ProductVariantValue> ProductVariantValues { get; set; } = new();
    }
}