using System.ComponentModel.DataAnnotations.Schema;
using int3306.Entities.Shared;
using Newtonsoft.Json;

namespace int3306.Entities
{
    [Table("stock")]
    public class Stock : IBaseEntity
    {
        public int Id { get; set; }
        public int Status { get; set; } = 1;
        
        [Column("product_id")]
        public int ProductId { get; set; }
        
        [Column("last_modified")]
        public DateTime LastModified { get; set; }
        public int Count { get; set; }
        public string Description { get; set; } = "";
        

        [JsonProperty("product")]
        [ForeignKey(nameof(ProductId))]
        public virtual Product? Product { get; set; }
    }
}
