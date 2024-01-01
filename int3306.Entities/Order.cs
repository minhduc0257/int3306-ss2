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
        
        [Column("user_payment_method_id")]
        public int UserPaymentMethodId {get;set;}
        
        [JsonProperty("user_payment_method")]
        [ForeignKey("UserPaymentMethodId")]
        public virtual UserPaymentMethod? UserPaymentMethod { get; set; }
        
        [Column("total_price")]
        public int TotalPrice {get;set;}
        
        [JsonProperty("detail")]
        public virtual ICollection<OrderDetail>? OrderDetails { get; set; } = new List<OrderDetail>();
        
        // set this on order creation
        [JsonProperty("cart_id")]
        [NotMapped]
        public List<int>? CartId { get; set; } = new();

        public string Country { get; set; } = "";
        public string Province { get; set; } = "";
        public string City { get; set; } = "";
        public string Ward { get; set; } = "";
        public string Street { get; set; } = "";
        public string Extra { get; set; } = "";
        public string Email { get; set; } = "";

        [Column("phone_number")]
        public string PhoneNumber { get; set; } = "";
        
        [ForeignKey(nameof(UserId))]
        public virtual User? User { get; set; }
    }
}