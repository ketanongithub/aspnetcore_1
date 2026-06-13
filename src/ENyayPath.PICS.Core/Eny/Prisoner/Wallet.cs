using ENyayPath.PICS.Core.Entities;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ENyayPath.PICS.Core.Eny.Prisoner
{
    [Table("Wallet")]
    public class Wallet : Entity<Guid>
    {
        [Key]
        [Column("WalletId")]
        public override Guid Id { get; set; }

        public Guid PrisonerId { get; set; }

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal BalanceAmount { get; set; }
    }
}
