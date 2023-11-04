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
        public int Status { get; set; }
        
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
        
        [JsonProperty("product_type")]
        [ForeignKey(nameof(ProductTypeId))]
        public virtual ProductType? ProductType { get; set; }
        
        [JsonProperty("product_tags")]
        [JsonIgnore]
        public virtual List<ProductToTag> ProductToTags { get; set; } = new();

        [NotMapped]
        public virtual List<ProductTag> ProductTags { get; set; } = new();
#pragma warning restore CS8618
    }
}