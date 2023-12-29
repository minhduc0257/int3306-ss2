using System.ComponentModel.DataAnnotations.Schema;
using int3306.Entities.Shared;
using Newtonsoft.Json;

namespace int3306.Entities
{
    [Table("asset")]
    public class Asset : IBaseEntity
    {
        public int Id { get; set; }
        public int Status { get; set; }

        public string Name { get; set; } = "";

        [JsonIgnore]
        [NotMapped]
        public byte[] File { get; set; } = Array.Empty<byte>();
    }
}