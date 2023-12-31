using System.ComponentModel.DataAnnotations.Schema;
using int3306.Entities.Shared;
using Newtonsoft.Json;

namespace int3306.Entities
{
    [Table("order_detail")]
    public class OrderDetail : IBaseEntity
    {
        public int Id { get; set; }
        public int Status { get; set; } = 1;
        
        [Column("order_id")]
        public int OrderId { get; set; }
        
        [Column("product_id")]
        public int ProductId { get; set; }
        
        public int Count { get; set; }
        
        [Column("total_price")]
        public int TotalPrice { get; set; }

        [Column("rating")]
        public int Rating {get;set;}

        [JsonProperty("product")]
        [ForeignKey(nameof(ProductId))]
        public virtual Product? Product { get; set; }
        
        [JsonIgnore]
        [JsonProperty("order")]
        [ForeignKey(nameof(OrderId))]
        public virtual Order? Order { get; set; }


        [JsonProperty("variants")]
        public virtual ICollection<OrderDetailVariant> OrderDetailVariant { get; set; } = new List<OrderDetailVariant>();
    }
}