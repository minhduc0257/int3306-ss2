using System.ComponentModel.DataAnnotations.Schema;
using int3306.Entities.Shared;

namespace int3306.Entities
{
    [Table("roles")]
    public class Role : IBaseEntity
    {
        public int Id { get; set; }
        public int Status { get; set; } = 1;

        public string Name { get; set; } = "";
        
        [Column("is_admin")]
        public bool IsAdmin { get; set; }
        
        [Column("is_stock_manager")]
        public bool IsStockManager { get; set; }
    }
}