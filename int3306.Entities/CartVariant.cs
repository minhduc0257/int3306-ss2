using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using int3306.Entities.Shared;
using Newtonsoft.Json;

namespace int3306.Entities
{
    [Table("cart_variant")]
    public class CartVariant : IBaseEntity
    {
        [Key]
        public int Id { get; set; }
        public int Status { get; set; } = 1;

        [Column("cart_id")]
        public int CartId { get; set; }
        
        [Column("variant_id")]
        public int VariantId { get; set; }
        
        [Column("variant_value_id")]
        public int VariantValueId { get; set; }
        
        [JsonProperty("variant")]
        [ForeignKey(nameof(VariantId))]
        public virtual ProductVariant? Variant { get; set; }
        
        [JsonProperty("variant_value")]
        [ForeignKey(nameof(VariantValueId))]
        public virtual ProductVariantValue? VariantValue { get; set; }
    }
}