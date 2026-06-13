using ENyayPath.PICS.Core.Entities;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ENyayPath.PICS.Core.Eny.Common
{
    [Table("Document")]
    public class DocumentMaster : Entity<Guid>
    {
        [Key]
        [Column("DocumentId")]
        public override Guid Id { get; set; }

        [Required]
        [StringLength(100)]
        public string DocumentName { get; set; } = null!;

        [StringLength(200)]
        public string? Description { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    }
}
