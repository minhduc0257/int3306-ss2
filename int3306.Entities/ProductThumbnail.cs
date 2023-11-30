using System.ComponentModel.DataAnnotations.Schema;
using int3306.Entities.Shared;

namespace int3306.Entities
{
    [Table("product_thumbnail")]
    public class ProductThumbnail : IBaseEntity
    {
        public int Id { get; set; }
        public int Status { get; set; } = 1;

        [Column("product_id")] 
        public int ProductId { get; set; }

        [Column("url")] 
        public string Url { get; set; } = ""; // Chen anh j vo bay giooo
    }
}