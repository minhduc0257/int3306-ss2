using System.ComponentModel.DataAnnotations.Schema;
using int3306.Entities.Shared;

namespace int3306.Entities
{
    public class OrderDetailVariant : IBaseEntity
    {
        public int Id { get; set; }
        public int Status { get; set; }
        
        [Column("order_detail_id")]
        public int OrderDetailId { get; set; }
        
        [Column("variant_id")]
        public int VariantId { get; set; }
        
        [Column("variant_value_id")]
        public int VariantValueId { get; set; }
        
        [ForeignKey(nameof(OrderDetailId))]
        public virtual OrderDetail? OrderDetail { get; set; }
        
        [ForeignKey(nameof(VariantId))]
        public virtual ProductVariant? Variant { get; set; }
        
        [ForeignKey(nameof(VariantValueId))]
        public virtual ProductVariantValue? VariantValue { get; set; }
    }
}