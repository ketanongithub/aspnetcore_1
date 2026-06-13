using ENyayPath.PICS.Core.Entities;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ENyayPath.PICS.Core.Eny.Prisoner
{
    [Table("Recharge")]
    public class Recharge : Entity<Guid>
    {
        [Key]
        [Column("TransactionId")]
        public override Guid Id { get; set; }

        public Guid PrisonerId { get; set; }

        public int RechargeType { get; set; }

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal Amount { get; set; }

        [StringLength(50)]
        public string? ReferenceNumber { get; set; }

        public DateTime TransactionDate { get; set; } = DateTime.UtcNow;
    }
}
