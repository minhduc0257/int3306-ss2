using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using int3306.Entities.Shared;

namespace int3306.Entities
{
    [Table("users")]
    public class User : IBaseEntity
    {
#pragma warning disable CS8618
        [Key]
        [Column("id")]
        public int Id { get; set; }
        
        [Column("status")]
        public int Status { get; set; }
        
        [Column("username")]
        public string Username { get; set; }
        
        [Newtonsoft.Json.JsonIgnore]
        [JsonIgnore]
        [Column("password")]
        public string Password { get; set; }
        
        [Column("name")]
        public string Name { get; set; }
        
        [Column("creation_time")]
        public DateTimeOffset CreationTime { get; set; }

        public UserDetail Detail { get; set; }
#pragma warning restore CS8618
    }
}