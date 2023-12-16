using System.ComponentModel.DataAnnotations.Schema;
using int3306.Entities.Shared;

namespace int3306.Entities
{
    public class ProductVariantValue : IBaseEntity
    {
        public int Id { get; set; }
        public int Status { get; set; }
        
        [Column("variant_id")]
        public int VariantId { get; set; }

        public string Name { get; set; } = "";
        
        [ForeignKey(nameof(VariantId))]
        public virtual ProductVariant? ProductVariant { get; set; } 
    }
}