using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using int3306.Entities.Shared;

namespace int3306.Entities
{
    public class UserPaymentMethod : IBaseEntity
    {
#pragma warning disable CS8618
        [Key]
        public int Id { get; set; }
        public int Status { get; set; } = 1;
        
        [Column("card_expiry")]
        public DateTimeOffset Expiry { get; set; }
        
        [Column("card_number")]
        public string CardNumber { get; set; }
        
        [Column("card_owner")]
        public string CardOwner { get; set; }
        
        [Column("card_verification")]
        public string CardVerification { get; set; }
#pragma warning restore CS8618
    }
}