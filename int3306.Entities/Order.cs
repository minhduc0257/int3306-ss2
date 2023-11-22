using System.ComponentModel.DataAnnotations.Schema;
using int3306.Entities.Shared;
using Newtonsoft.Json;

namespace int3306.Entities
{
    [Table("order")]
    public class Order : IBaseEntity
    {
        public int Id { get; set; }
        public int Status { get; set; } = 1;
        
        [Column("user_id")]
        public int UserId {get;set;}
        
        [Column("timestamp")]
        public DateTime? Timestamp {get;set;}
        
        [Column("user_address_id")]
        public int UserAddressId {get;set;}
        
        [Column("user_payment_method_id")]
        public int UserPaymentMethodId {get;set;}
        
        [Column("total_price")]
        public int TotalPrice {get;set;}
        
        [JsonProperty("detail")]
        public virtual List<OrderDetail> OrderDetails { get; set; } = new();
        
        // set this on order creation
        [JsonProperty("cart_id")]
        [NotMapped]
        public List<int>? CartId { get; set; } = new();
    }
}