using System.ComponentModel.DataAnnotations.Schema;
using int3306.Entities.Shared;

namespace int3306.Entities
{
    [Table("user_addresses")]
    public class UserAddress : IBaseEntity
    {
        public int Id { get; set; }
        public int Status { get; set; }

        [Column("phone_number")]
        public string PhoneNumber { get; set; } = "";
        
        [Column("province")]
        public string Province { get; set; } = "";
        
        [Column("city")]
        public string City { get; set; } = "";
        
        [Column("ward")]
        public string Ward { get; set; } = "";
        
        [Column("street")]
        public string Street { get; set; } = "";
        
        [Column("extra")]
        public string Extra { get; set; } = "";
        
        [Column("user_id")]
        public int UserId {get;set;}
    }
}