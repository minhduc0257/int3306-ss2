using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using int3306.Entities.Shared;

namespace int3306.Entities
{
    [Table("user_detail")]
    public class UserDetail : IBaseEntity
    {
#pragma warning disable CS8618
        
        [Key]
        [Column("id")]
        public int Id { get; set; }
        
        [Column("status")]
        public int Status { get; set; }
        
        [Column("user_id")]
        public int UserId { get; set; }
        
        [Column("email")]
        public string Email { get; set; } = "";
        
        [Column("dob")]
        public DateTimeOffset DateOfBirth { get; set; }
        
        [Column("phone_number")]
        public string PhoneNumber { get; set; } = "";
#pragma warning restore CS8618
    }
}