using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace int3306.Entities
{
    [Table("products_1")] // non-existent table
    [Keyless]
    public class ProductExtended
    {
        [Column("product_id")]
        public int ProductId { get; set; }
        
        [Column("rating")]  
        public float Rating { get; set; }
        
        [Column("count")]
        public int Count { get; set; }
    }
}