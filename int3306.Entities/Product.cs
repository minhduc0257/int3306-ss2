using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using int3306.Entities.Shared;
using Newtonsoft.Json;

namespace int3306.Entities
{
    [Table("products")]
    public class Product : IBaseEntity
    {
#pragma warning disable CS8618
        [Key]
        public int Id { get; set; }
        public int Status { get; set; } = 1;

        [Column("product_type_id")]
        public int? ProductTypeId { get; set; }
        
        
        public string Name { get; set; }
        public string Description { get; set; }
        
        [Column("climate_description")]
        public string ClimateDescription { get; set; }
        
        public string Yield { get; set; }
        
        [Column("growing_season")]
        public int GrowingSeason { get; set; }
        
        [Column("planting_duration")]
        public int PlantingDuration { get; set; }

        public int Price { get; set; }

        [ForeignKey(nameof(ProductTypeId))]
        public virtual ProductType? ProductType { get; set; }

        [JsonIgnore]
        public virtual List<ProductToTag> ProductToTags { get; set; } = new();

        [NotMapped]
        public virtual List<ProductTag> ProductTags { get; set; } = new();
        
        [JsonProperty("stock")]
        [JsonIgnore]
        public virtual List<Stock> Stocks { get; set; } = new();


        public virtual List<ProductThumbnail> ProductThumbnails { get; set; } = new();
        
        [NotMapped]
        public int Stock { get; set; }

        [NotMapped]
        public float Rating { get; set; }
#pragma warning restore CS8618
    }
}