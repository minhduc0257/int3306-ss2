using System.ComponentModel.DataAnnotations.Schema;
using int3306.Entities.Shared;
using Newtonsoft.Json;

namespace int3306.Entities
{
    [Table("product_variant_values")]
    public class ProductVariantValue : IBaseEntity
    {
        public int Id { get; set; }
        public int Status { get; set; }
        
        [Column("variant_id")]
        public int VariantId { get; set; }

        public string Name { get; set; } = "";
        
        [ForeignKey(nameof(VariantId))]
        [JsonIgnore]
        public virtual ProductVariant? ProductVariant { get; set; } 
    }
}