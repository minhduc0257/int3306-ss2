using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using int3306.Entities.Shared;
using Newtonsoft.Json;

namespace int3306.Entities
{
    [Table("product_to_tag")]
    public class ProductToTag : IBaseEntity
    {
#pragma warning disable CS8618
        [Key]
        public int Id { get; set; }
        public int Status { get; set; } = 1;
        
        [Column("product_id")]
        public int ProductId { get; set; }
        
        [Column("product_tag_id")]
        public int ProductTagId { get; set; }
        
        [JsonIgnore]
        [JsonProperty("product")]
        [ForeignKey(nameof(ProductId))]
        public virtual Product Product { get; set; }
        
        [JsonIgnore]
        [JsonProperty("product_tag")]
        [ForeignKey(nameof(ProductTagId))]
        public virtual ProductTag ProductTag { get; set; }
#pragma warning restore CS8618
    }
}