using System.ComponentModel.DataAnnotations.Schema;
using int3306.Entities.Shared;
using Newtonsoft.Json;

namespace int3306.Entities
{
    [Table("user_to_role")]
    public class UserToRole : IBaseEntity
    {
        public int Id { get; set; }
        public int Status { get; set; } = 1;

        [Column("user_id")]
        public int UserId { get; set; }
        
        [Column("role_id")]
        public int RoleId { get; set; }
        

        [JsonProperty("user")]
        [ForeignKey(nameof(UserId))]
        public virtual User? User { get; set; }

        [JsonProperty("role")]
        [ForeignKey(nameof(RoleId))]
        public virtual Role? Role { get; set; }
    }
}