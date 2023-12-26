using System.ComponentModel.DataAnnotations.Schema;
using int3306.Entities.Shared;
using Newtonsoft.Json;

namespace int3306.Entities
{
    [Table("cart")]
    public class Cart : IBaseEntity
    {
        public int Id { get; set; }
        public int Status { get; set; } = 1;
        
        [Column("user_id")]
        public int UserId { get; set; }
        
        [Column("product_id")]
        public int ProductId { get; set; }
        
        [Column("count")]
        public int Count { get; set; }
        
        [Column("added")]
        public DateTime Added { get; set; }

        [JsonProperty("product")]
        [ForeignKey(nameof(ProductId))]
        public virtual Product? Product { get; set; }

        [JsonProperty("variants")]
        public virtual ICollection<CartVariant> CartVariants { get; set; } = new List<CartVariant>();
    }
}